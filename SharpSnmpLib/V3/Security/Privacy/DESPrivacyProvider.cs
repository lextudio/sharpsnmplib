using System.Collections.Generic;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;
using System.Buffers;
using System.Security.Cryptography;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Privacy provider for DES.
    /// </summary>
    public class DESPrivacyProvider : PrivacyProviderBase
    {
        /// <summary>
        /// Initializes a new instance of DESPrivacyProvider.
        /// </summary>
        public DESPrivacyProvider(
            in IAuthenticationProvider authenticationService,
            in ReadOnlyMemory<byte> passcode)
            : base(8, authenticationService, passcode)
        {
        }

        /// <inheritdoc/>
        public override int EncryptScopedPdu(
            in ReadOnlyMemory<byte> scopedPdu,
            in UsmSecurityParameters parameters,
            Span<byte> encryptedScopedPdu)
        {
            // We need to use Array for the buffers that will be converted to Memory<byte>
            // This is more efficient than using a larger, fixed-size buffer from the pool
            byte[] bufferBackArray = ArrayPool<byte>.Shared.Rent(20);

            // Only allocate padding buffer when needed
            byte[]? paddingBuffer = null;

            try
            {
                // Use stack allocation for the private key
                Span<byte> privKey = stackalloc byte[AuthenticationProvider.DigestSize];
                AuthenticationProvider.PasswordToKey(Passcode, parameters.EngineId, privKey);

                // Create memory slices from the rented array
                var privacyKey = (Memory<byte>)bufferBackArray[..16];
                EngineBootsMemory = (Memory<byte>)bufferBackArray[16..20];

                // Update engine boots info
                EngineBoots = parameters.EngineBoots;
                UpdateEngineBoots(parameters.EngineBoots);

                // Copy key into privacy key buffer
                privKey.CopyTo(privacyKey.Span);

                using var des = DES.Create();
                // This allocation is unavoidable due to DES API requirements
                des.Key = privacyKey.Span[..8].ToArray();

                // Generate an 8-octet salt value
                var saltValue = GetNextSalt();

                // Copy engine boots to the first part of the privacy parameters
                EngineBootsMemory.Span.CopyTo(parameters.PrivParams.Span);

                // Copy salt value to the second part of the privacy parameters
                BinaryHelpers.CopyBytesMostSignificantFirst(
                    saltValue,
                    parameters.PrivParams.Span[EngineBootsMemory.Length..]); // More precise slicing

                var salt = parameters.PrivParams.Span;

                // preIV = last 8 octets of privacyKey
                Span<byte> preIv = privacyKey.Span[8..16];

                // Use stack allocation for the IV buffer (small, fixed size)
                Span<byte> iv = stackalloc byte[8];

                // XOR pre-iv with salt
                for (int i = 0; i < 8; i++)
                {
                    iv[i] = (byte)(salt[i] ^ preIv[i]);
                }

                // Perform the encryption
                return des.EncryptCbc(
                    scopedPdu.Span,
                    iv,
                    encryptedScopedPdu,
                    PaddingMode.Zeros);
            }
            finally
            {
                // Always return the buffer to the pool
                ArrayPool<byte>.Shared.Return(bufferBackArray, clearArray: true);

                // Only return padding buffer if it was actually allocated
                if (paddingBuffer != null)
                {
                    ArrayPool<byte>.Shared.Return(paddingBuffer, clearArray: true);
                }
            }
        }

        /// <inheritdoc/>
        public override void DecryptScopedPdu(
            in ReadOnlyMemory<byte> encryptedPdu,
            in UsmSecurityParameters parameters,
            Span<byte> decryptedPdu)
        {
            // Use appropriate buffer size and only allocate what we need
            byte[] bufferBackArray = ArrayPool<byte>.Shared.Rent(20);

            // Only allocate padding buffer if needed
            byte[]? paddingBuffer = null;

            try
            {
                // Use stack allocation for the private key
                Span<byte> privKey = stackalloc byte[AuthenticationProvider.DigestSize];
                AuthenticationProvider.PasswordToKey(Passcode, parameters.EngineId, privKey);

                // Create memory slices from the rented array
                var privacyKey = (Memory<byte>)bufferBackArray[..16];
                EngineBootsMemory = (Memory<byte>)bufferBackArray[16..20];

                // Update engine boots info
                EngineBoots = parameters.EngineBoots;
                UpdateEngineBoots(parameters.EngineBoots);

                // Copy key into privacy key buffer
                privKey.CopyTo(privacyKey.Span);

                using var des = DES.Create();
                // This allocation is unavoidable due to DES API requirements
                des.Key = privacyKey.Span[..8].ToArray();

                // Get salt from privacy parameters
                ReadOnlySpan<byte> salt = parameters.PrivParams.Span;

                // preIV = last 8 octets of privacyKey
                Span<byte> preIv = privacyKey.Span[8..16];

                // Use stack allocation for the IV buffer (small, fixed size)
                Span<byte> iv = stackalloc byte[8];

                // XOR pre-iv with salt
                for (int i = 0; i < 8; i++)
                {
                    iv[i] = (byte)(salt[i] ^ preIv[i]);
                }

                // Get required padded length
                var paddedCiphertextLength = des.GetCiphertextLengthCbc(
                    encryptedPdu.Length,
                    PaddingMode.Zeros);

                // Only allocate padding buffer if needed
                if (encryptedPdu.Length != paddedCiphertextLength)
                {
                    paddingBuffer = ArrayPool<byte>.Shared.Rent(paddedCiphertextLength);
                    encryptedPdu.Span.CopyTo(paddingBuffer);
                    var paddedText = paddingBuffer.AsSpan(0, paddedCiphertextLength);

                    // Decrypt to a temporary buffer
                    var tempBuffer = ArrayPool<byte>.Shared.Rent(paddedCiphertextLength);
                    try
                    {
                        des.DecryptCbc(paddedText, iv, tempBuffer, PaddingMode.Zeros);
                        tempBuffer.AsSpan(0, encryptedPdu.Length).CopyTo(decryptedPdu);
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(tempBuffer, clearArray: true);
                    }
                }
                else
                {
                    // Decrypt directly to the destination buffer
                    des.DecryptCbc(
                        encryptedPdu.Span,
                        iv,
                        decryptedPdu,
                        PaddingMode.Zeros);
                }
            }
            finally
            {
                // Always return the buffer to the pool
                ArrayPool<byte>.Shared.Return(bufferBackArray, clearArray: true);

                // Only return padding buffer if it was actually allocated
                if (paddingBuffer != null)
                {
                    ArrayPool<byte>.Shared.Return(paddingBuffer, clearArray: true);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of DESPrivacyProvider with v12-compatible signature.
        /// </summary>
        public DESPrivacyProvider(OctetString? passphrase, IAuthenticationProvider authenticationProvider)
            : this(
                authenticationProvider ?? throw new ArgumentNullException(nameof(authenticationProvider)),
                (passphrase ?? throw new ArgumentNullException(nameof(passphrase))).Octets)
        {
        }

        /// <summary>
        /// Gets a value indicating whether DES is supported on current runtime.
        /// </summary>
        public static bool IsSupported
        {
            get
            {
                try { using var _ = DES.Create(); return true; }
                catch { return false; }
            }
        }

        private const int LegacyPrivacyParametersLength = 8;
        private const int LegacyMinimumKeyLength = 16;

        /// <summary>
        /// Encrypts scoped PDU payload bytes using legacy DES helper signature.
        /// </summary>
        public static byte[] Encrypt(byte[] unencryptedData, byte[] key, byte[] privacyParameters)
        {
            if (!IsSupported) throw new PlatformNotSupportedException();
            if (unencryptedData == null) throw new ArgumentNullException(nameof(unencryptedData));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (privacyParameters == null) throw new ArgumentNullException(nameof(privacyParameters));
            if (privacyParameters.Length != LegacyPrivacyParametersLength)
                throw new ArgumentOutOfRangeException(nameof(privacyParameters), "Privacy parameters argument has to be 8 bytes long.");
            if (key.Length < LegacyMinimumKeyLength)
                throw new ArgumentException($"Encryption key length has to 16 bytes or more.", nameof(key));

            var iv = GetLegacyIv(key, privacyParameters);
            var outKey = GetLegacyKey(key);
            if ((unencryptedData.Length % 8) != 0)
            {
                var tmp = new byte[8 * ((unencryptedData.Length / 8) + 1)];
                Buffer.BlockCopy(unencryptedData, 0, tmp, 0, unencryptedData.Length);
                unencryptedData = tmp;
            }
            using var des = DES.Create();
            des.Key = outKey;
            return des.EncryptCbc(unencryptedData, iv, PaddingMode.None);
        }

        /// <summary>
        /// Decrypts scoped PDU payload bytes using legacy DES helper signature.
        /// </summary>
        public static byte[] Decrypt(byte[] encryptedData, byte[] key, byte[] privacyParameters)
        {
            if (!IsSupported) throw new PlatformNotSupportedException();
            if (encryptedData == null) throw new ArgumentNullException(nameof(encryptedData));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (privacyParameters == null) throw new ArgumentNullException(nameof(privacyParameters));
            if (encryptedData.Length == 0) throw new ArgumentException("Empty encrypted data.", nameof(encryptedData));
            if ((encryptedData.Length % 8) != 0) throw new ArgumentException("Encrypted data buffer has to be divisible by 8.", nameof(encryptedData));
            if (privacyParameters.Length != LegacyPrivacyParametersLength)
                throw new ArgumentOutOfRangeException(nameof(privacyParameters), "Privacy parameters argument has to be 8 bytes long.");
            if (key.Length < LegacyMinimumKeyLength)
                throw new ArgumentOutOfRangeException(nameof(key), "Decryption key has to be at least 16 bytes long.");

            var iv = GetLegacyIv(key, privacyParameters);
            var outKey = GetLegacyKey(key);
            using var des = DES.Create();
            des.Key = outKey;
            return des.DecryptCbc(encryptedData, iv, PaddingMode.Zeros);
        }

        private static byte[] GetLegacyIv(IReadOnlyList<byte> key, IReadOnlyList<byte> privacyParameters)
        {
            var iv = new byte[LegacyPrivacyParametersLength];
            for (var i = 0; i < iv.Length; i++) iv[i] = (byte)(key[8 + i] ^ privacyParameters[i]);
            return iv;
        }

        private static byte[] GetLegacyKey(IReadOnlyList<byte> privacyPassword)
        {
            var outKey = new byte[8];
            for (var i = 0; i < outKey.Length; i++) outKey[i] = privacyPassword[i];
            return outKey;
        }

    }
}
