using DotNetSnmp.Common.Helpers;
using DotNetSnmp.Protocol.V3.Security.Authentication;
using System.Buffers;
using System.Security.Cryptography;

namespace DotNetSnmp.Protocol.V3.Security.Privacy
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
    }
}
