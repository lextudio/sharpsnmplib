using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;
using System.Buffers;
using System.Security.Cryptography;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Implementation of Triple DES (3DES) privacy protocol for SNMPv3.
    /// </summary>
    public class TripleDESPrivacyProvider : PrivacyProviderBase
    {
        private const int KeyLength = 32;

        /// <summary>
        /// Extends a key that is shorter than the required length using a key localization procedure.
        /// </summary>
        /// <param name="privKey">The source key to extend</param>
        /// <param name="keyLength">The required key length in bytes</param>
        /// <param name="authenticationService">The authentication service to use for key derivation</param>
        /// <param name="engineId">The engine ID to use for localization</param>
        /// <param name="destination">The destination buffer where the extended key will be stored</param>
        public static void ExtendShortKey(
            ReadOnlySpan<byte> privKey,
            int keyLength,
            IAuthenticationProvider authenticationService,
            ReadOnlyMemory<byte> engineId,
            Span<byte> destination)
        {
            // Copy as many bytes as we have from the key, up to the required keyLength
            int bytesToCopy = Math.Min(privKey.Length, keyLength);
            privKey[..bytesToCopy].CopyTo(destination);

            // If the auth key is smaller than the required length, extend it using localized keys
            if (bytesToCopy < keyLength)
            {
                // For small keys, create a new array directly
                // We can't use stackalloc here due to scoping rules with Span
                byte[] shortKeyArray = new byte[bytesToCopy];
                privKey[..bytesToCopy].CopyTo(shortKeyArray);

                Span<byte> tempKey = stackalloc byte[authenticationService.DigestSize];
                int length = bytesToCopy;

                while (length < keyLength)
                {
                    // Create a ReadOnlyMemory from the array
                    ReadOnlyMemory<byte> shortKeyMemory = shortKeyArray;
                    authenticationService.PasswordToKey(shortKeyMemory, engineId, tempKey);
                    int bytesToCopyNow = Math.Min(keyLength - length, authenticationService.TruncatedDigestSize);
                    tempKey[..bytesToCopyNow].CopyTo(destination[length..]);
                    length += bytesToCopyNow;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of TripleDESPrivacyProvider.
        /// </summary>
        public TripleDESPrivacyProvider(
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
            // Always use stackalloc for the key buffer since KeyLength is only 32 bytes
            Span<byte> privacyKeySpan = stackalloc byte[KeyLength];
            byte[]? paddingBuffer = null;

            try
            {
                Span<byte> privKey = stackalloc byte[AuthenticationProvider.DigestSize];
                AuthenticationProvider.PasswordToKey(Passcode, parameters.EngineId, privKey);

                using var tripleDes = TripleDES.Create();

                // Initialize the privacy key using the static helper method
                ExtendShortKey(privKey, KeyLength, AuthenticationProvider, parameters.EngineId, privacyKeySpan);

                // Set the key for TripleDES encryption/decryption
                // This allocation is unavoidable due to the TripleDES API requirements
                tripleDes.Key = privacyKeySpan[..24].ToArray();

                // Generate an 8-octet salt value
                var saltValue = GetNextSalt();

                // Copy engine boots to the first part of privParams
                EngineBootsMemory.Span.CopyTo(parameters.PrivParams.Span);

                // Copy salt value to the second part of privParams
                // Use precise slice to avoid memory copying
                BinaryHelpers.CopyBytesMostSignificantFirst(
                    saltValue,
                    parameters.PrivParams.Span[EngineBootsMemory.Length..]);

                var salt = parameters.PrivParams.Span;

                // preIV = last 8 octets of privacyKey
                Span<byte> preIv = privacyKeySpan.Slice(24, 8);

                // final IV - use stackalloc for small fixed-size buffer
                Span<byte> iv = stackalloc byte[8];

                // XOR pre-iv with salt
                for (int i = 0; i < 8; i++)
                {
                    iv[i] = (byte)(salt[i] ^ preIv[i]);
                }

                // Calculate the required ciphertext length with padding
                var paddedCiphertextLength = tripleDes.GetCiphertextLengthCbc(
                    scopedPdu.Length,
                    PaddingMode.Zeros);

                ReadOnlySpan<byte> cleartext = scopedPdu.Span;

                // Only allocate padding buffer if actually needed
                if (scopedPdu.Length != paddedCiphertextLength)
                {
                    // Precisely size the buffer rental
                    paddingBuffer = ArrayPool<byte>.Shared.Rent(paddedCiphertextLength);

                    // Copy data and apply padding
                    scopedPdu.Span.CopyTo(paddingBuffer);
                    paddingBuffer.AsSpan(scopedPdu.Length, paddedCiphertextLength - scopedPdu.Length).Fill(1);

                    cleartext = paddingBuffer.AsSpan(0, paddedCiphertextLength);
                }

                // Perform the actual encryption
                return tripleDes.EncryptCbc(
                    cleartext,
                    iv,
                    encryptedScopedPdu,
                    PaddingMode.None);
            }
            finally
            {
                // Return padding buffer to the pool if one was rented
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
            // Use stackalloc for the key buffer since KeyLength is only 32 bytes
            Span<byte> privacyKeySpan = stackalloc byte[KeyLength];

            Span<byte> privKey = stackalloc byte[AuthenticationProvider.DigestSize];
            AuthenticationProvider.PasswordToKey(Passcode, parameters.EngineId, privKey);

            using var tripleDes = TripleDES.Create();

            // Initialize the privacy key using the static helper method
            ExtendShortKey(privKey, KeyLength, AuthenticationProvider, parameters.EngineId, privacyKeySpan);

            // Set the key for TripleDES encryption/decryption
            // This allocation is unavoidable due to the TripleDES API requirements
            tripleDes.Key = privacyKeySpan[..24].ToArray();

            // preIV = last 8 octets of privacyKey
            Span<byte> preIv = privacyKeySpan.Slice(24, 8);

            ReadOnlySpan<byte> salt = parameters.PrivParams.Span;

            // final IV - use stackalloc for small fixed-size buffer
            Span<byte> iv = stackalloc byte[8];

            // XOR pre-iv with salt
            for (int i = 0; i < 8; i++)
            {
                iv[i] = (byte)(salt[i] ^ preIv[i]);
            }

            // Perform the actual decryption
            tripleDes.DecryptCbc(
                encryptedPdu.Span,
                iv,
                decryptedPdu,
                PaddingMode.Zeros);
        }

        /// <summary>
        /// Initializes a new instance of TripleDESPrivacyProvider with v12-compatible signature.
        /// </summary>
        public TripleDESPrivacyProvider(OctetString? passphrase, IAuthenticationProvider authenticationProvider)
            : this(
                authenticationProvider ?? throw new ArgumentNullException(nameof(authenticationProvider)),
                (passphrase ?? throw new ArgumentNullException(nameof(passphrase))).Octets)
        {
        }

    }
}
