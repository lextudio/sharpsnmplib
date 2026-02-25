using System.Security.Cryptography;
using DotNetSnmp.Asn1;

namespace DotNetSnmp.Protocol.V3.Security.Authentication
{
    /// <summary>
    /// Base abstract class for SNMP v3 authentication providers that implements common functionality.
    /// </summary>
    /// <remarks>
    /// This class provides a common implementation for different HMAC-based authentication algorithms.
    /// Derived classes should specify the digest size, truncated digest size, hash algorithm name, and
    /// a function to create the appropriate HMAC implementation.
    /// </remarks>
    public abstract class AuthenticationProviderBase : IAuthenticationProvider
    {
        /// <inheritdoc/>
        public int TruncatedDigestSize { get; init; }

        /// <inheritdoc/>
        public int DigestSize { get; init; }

        private readonly Func<byte[], HMAC> _create;
        private readonly ReadOnlyMemory<byte> _passcode;
        private readonly HashAlgorithmName _name;

        /// <summary>
        /// Initializes a new instance of AuthenticationProviderBase.
        /// </summary>
        /// <param name="digestSize">The full size of the digest in bytes produced by the hash algorithm.</param>
        /// <param name="truncatedDigestSize">The size of the truncated digest in bytes used for authentication parameters.</param>
        /// <param name="name">The hash algorithm name.</param>
        /// <param name="passcode">The password or passphrase to be used for authentication.</param>
        /// <param name="create">A function that creates an HMAC algorithm instance given a key.</param>
        protected AuthenticationProviderBase(int digestSize, int truncatedDigestSize, HashAlgorithmName name, ReadOnlyMemory<byte> passcode, Func<byte[], HMAC> create)
        {
            DigestSize = digestSize;
            TruncatedDigestSize = truncatedDigestSize;
            _name = name;
            _create = create;
            _passcode = passcode;
        }

        /// <summary>
        /// Creates hmac.
        /// </summary>
        /// <param name="engineId">The engine ID used for key localization.</param>
        /// <returns>An HMAC algorithm instance initialized with the localized key.</returns>
        private HMAC CreateHmac(ReadOnlyMemory<byte> engineId)
        {
            // Use stackalloc for small keys, typically digest sizes are reasonable for stack allocation
            Span<byte> key = stackalloc byte[DigestSize];
            PasswordToKey(_passcode, engineId, key);

            // Unfortunately, this allocation is unavoidable due to the HMAC API requiring a byte[]
            return _create(key.ToArray());
        }

        /// <inheritdoc/>
        public void AuthenticateOutgoingMsg(SnmpV3Message message, Memory<byte> newAuthParams)
        {
            using var hmac = CreateHmac(message.SecurityParameters.EngineId);

            // Ensure auth parameters buffer is properly initialized before encoding
            newAuthParams.Span.Fill(0);
            message.SecurityParameters.AuthParams = newAuthParams;

            // This will encode the message with zeroed auth parameters
            // Note: message.Encode() creates a new byte array - this allocation is inherent to the encoding process
            ReadOnlyMemory<byte> wholeMsg = message.Encode();

            // Use stack allocation for digest to avoid heap allocation
            Span<byte> digest = stackalloc byte[DigestSize];

            if (hmac.TryComputeHash(wholeMsg.Span, digest, out _))
            {
                // Copy only the truncated portion of the digest to the auth parameters
                digest[..TruncatedDigestSize].CopyTo(message.SecurityParameters.AuthParams.Span);
            }
        }

        /// <inheritdoc/>
        public bool AuthenticateIncomingMsg(SnmpV3Message message)
        {
            using var hmac = CreateHmac(message.SecurityParameters.EngineId);

            // Get a reference to the auth params to avoid unnecessary copying
            var authParams = message.SecurityParameters.AuthParams.Span;

            // Use stackalloc for messageDigest to avoid heap allocation
            Span<byte> messageDigest = stackalloc byte[TruncatedDigestSize];
            authParams.CopyTo(messageDigest);
            authParams.Fill(0);

            // Note: message.Encode() creates a new byte array - this allocation is inherent to the encoding process
            ReadOnlyMemory<byte> wholeMsg = message.Encode();

            // Use stack allocation for digest to avoid heap allocation
            Span<byte> digest = stackalloc byte[DigestSize];
            if (hmac.TryComputeHash(wholeMsg.Span, digest, out _))
            {
                // Use SequenceEqual on spans for efficient memory comparison
                return digest[..TruncatedDigestSize].SequenceEqual(messageDigest);
            }

            return false;
        }

        /// <inheritdoc/>
        public void PasswordToKey(in ReadOnlyMemory<byte> secret, in ReadOnlyMemory<byte> engineId, Span<byte> destination)
        {
            using var hash = IncrementalHash.CreateHash(_name);
            KeyUtils.GenerateLocalizedKey(secret.Span, engineId.Span, hash, destination);
        }
    }
}
