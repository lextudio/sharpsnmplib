using System.Buffers;
using System.Security.Cryptography;
using DotNetSnmp.Common.Helpers;
using DotNetSnmp.Protocol.V3.Security.Authentication;

namespace DotNetSnmp.Protocol.V3.Security.Privacy
{
    /// <summary>
    /// Base class for AES-based privacy protocols (AES-128, AES-192, AES-256).
    /// </summary>
    public abstract class AESPrivacyProviderBase : PrivacyProviderBase
    {
        /// <summary>
        /// Minimum AES block size in bytes.
        /// </summary>
        protected const int MinimalBlockSize = 16;

        private readonly int _keyLength;

        /// <summary>
        /// Initializes a new instance of AESPrivacyProviderBase.
        /// </summary>
        protected AESPrivacyProviderBase(
            IAuthenticationProvider authenticationService,
            ReadOnlyMemory<byte> passcode,
            int keyLength)
            : base(8, authenticationService, passcode)
        {
            _keyLength = keyLength;
        }

        /// <inheritdoc/>
        public override int EncryptScopedPdu(
            in ReadOnlyMemory<byte> scopedPdu,
            in UsmSecurityParameters parameters,
            Span<byte> encryptedScopedPdu)
        {
            var _paddingBuffer = ArrayPool<byte>.Shared.Rent(65507);

            // Allocate memory for the key and engine data
            // keyLength bytes for the key, plus 8 bytes for engine data
            var _bufferBackArray = ArrayPool<byte>.Shared.Rent(_keyLength + 8);
            try
            {
                Span<byte> privKey = stackalloc byte[AuthenticationProvider.DigestSize];
                AuthenticationProvider.PasswordToKey(Passcode, parameters.EngineId, privKey);

                var _aes = Aes.Create();

                EngineBoots = parameters.EngineBoots;
                EngineTime = parameters.EngineTime;

                var _privacyKey = (Memory<byte>)_bufferBackArray[.._keyLength];

                EngineBootsMemory = (Memory<byte>)_bufferBackArray[_keyLength..(_keyLength + 4)];
                EngineTimeMemory = (Memory<byte>)_bufferBackArray[(_keyLength + 4)..(_keyLength + 8)];

                // Cache engineBoots and engineTime because a change
                // in these values equates to a 'reboot' or similar of the
                // authoritative SNMP engine, a resync is needed
                UpdateEngineBoots(parameters.EngineBoots);
                UpdateEngineTime(parameters.EngineTime);

                TripleDESPrivacyProvider.ExtendShortKey(privKey, _keyLength, AuthenticationProvider, parameters.EngineId, _privacyKey.Span);

                // Set the key for AES encryption/decryption
                _aes.Key = _privacyKey.ToArray();

                // Generate an 8-octet salt value
                var saltValue = GetNextSalt();

                Span<byte> iv = stackalloc byte[16];

                // First 4 octets (Most Significant Byte first)
                EngineBootsMemory.Span.CopyTo(iv[..4]);

                // Next 4 octets (Most Significant Byte first)
                EngineTimeMemory.Span.CopyTo(iv[4..8]);

                BinaryHelpers.CopyBytesMostSignificantFirst(
                    saltValue, iv[8..]);

                // Copy the 64-bit salt to privParameters
                iv[8..16].CopyTo(parameters.PrivParams.Span);

                return _aes.EncryptCfb(
                    scopedPdu.Span,
                    iv,
                    encryptedScopedPdu,
                    PaddingMode.Zeros,
                    128);
            }
            finally
            {
                // Always return the original rented array, not a new one
                ArrayPool<byte>.Shared.Return(_bufferBackArray, clearArray: true);
                ArrayPool<byte>.Shared.Return(_paddingBuffer, clearArray: true);
            }
        }

        /// <inheritdoc/>
        public override void DecryptScopedPdu(
            in ReadOnlyMemory<byte> encryptedPdu,
            in UsmSecurityParameters parameters,
            Span<byte> decryptedPdu)
        {
            var _paddingBuffer = ArrayPool<byte>.Shared.Rent(65507);

            // Allocate memory for the key and engine data
            // keyLength bytes for the key, plus 8 bytes for engine data
            var _bufferBackArray = ArrayPool<byte>.Shared.Rent(_keyLength + 8);

            try
            {
                Span<byte> privKey = stackalloc byte[AuthenticationProvider.DigestSize];
                AuthenticationProvider.PasswordToKey(Passcode, parameters.EngineId, privKey);

                using var _aes = Aes.Create();

                EngineBoots = parameters.EngineBoots;
                EngineTime = parameters.EngineTime;

                var _privacyKey = (Memory<byte>)_bufferBackArray[.._keyLength];

                EngineBootsMemory = (Memory<byte>)_bufferBackArray[_keyLength..(_keyLength + 4)];
                EngineTimeMemory = (Memory<byte>)_bufferBackArray[(_keyLength + 4)..(_keyLength + 8)];

                // Cache engineBoots and engineTime because a change
                // in these values equates to a 'reboot' or similar of the
                // authoritative SNMP engine, a resync is needed
                UpdateEngineBoots(parameters.EngineBoots);
                UpdateEngineTime(parameters.EngineTime);

                TripleDESPrivacyProvider.ExtendShortKey(privKey, _keyLength, AuthenticationProvider, parameters.EngineId, _privacyKey.Span);

                // Set the key for AES encryption/decryption
                _aes.Key = _privacyKey.ToArray();

                ReadOnlySpan<byte> salt = parameters.PrivParams.Span;

                // Rebuild the IV used to encrypt the PDU
                Span<byte> iv = stackalloc byte[16];

                // First 4 octets (Most Significant Byte first)
                EngineBootsMemory.Span.CopyTo(iv[..4]);

                // Next 4 octets (Most Significant Byte first)
                EngineTimeMemory.Span.CopyTo(iv[4..8]);

                // Last 8 octets (Most Significant Byte first)
                salt.CopyTo(iv[8..16]);

                var paddedCiphertextLength = _aes.GetCiphertextLengthCfb(
                    encryptedPdu.Length,
                    PaddingMode.Zeros,
                    128);

                ReadOnlySpan<byte> ciphertext = encryptedPdu.Span;

                if (encryptedPdu.Length != paddedCiphertextLength)
                {
                    encryptedPdu.Span.CopyTo(_paddingBuffer);
                    ciphertext = _paddingBuffer[..paddedCiphertextLength];
                    var buffer = ArrayPool<byte>.Shared.Rent(paddedCiphertextLength);
                    try
                    {
                        _aes.DecryptCfb(ciphertext, iv, buffer, PaddingMode.Zeros, 128);
                        buffer[..encryptedPdu.Length].CopyTo(decryptedPdu);
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buffer, clearArray: true);
                    }
                }
                else
                {
                    _aes.DecryptCfb(
                        ciphertext,
                        iv,
                        decryptedPdu,
                        PaddingMode.Zeros,
                        128);
                }
            }
            finally
            {
                // Always return the original rented array, not a new one
                ArrayPool<byte>.Shared.Return(_bufferBackArray, clearArray: true);
                ArrayPool<byte>.Shared.Return(_paddingBuffer, clearArray: true);
            }
        }

        /// <inheritdoc/>
        public override byte[] PrepareBuffer(int encodedLength)
        {
            var length = (encodedLength % MinimalBlockSize == 0)
                ? encodedLength
                : ((encodedLength / MinimalBlockSize) + 1) * MinimalBlockSize;

            return ArrayPool<byte>.Shared.Rent(length);
        }
    }
}
