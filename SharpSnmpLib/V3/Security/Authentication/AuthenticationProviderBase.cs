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
        private readonly object _localizedKeyCacheLock = new();
        private readonly List<LocalizedKeyCacheEntry> _localizedKeyCache = [];
        private const int LocalizedKeyCacheCapacity = 64;

        private sealed class LocalizedKeyCacheEntry
        {
            public required byte[] Secret { get; init; }

            public required byte[] EngineId { get; init; }

            public required byte[] LocalizedKey { get; init; }
        }

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
            // Localized keys are expensive to derive (RFC 3414 1MB passphrase expansion),
            // so cache them for this provider instance.
            var localizedKey = GetLocalizedKey(engineId);
            return _create(localizedKey);
        }

        private byte[] GetLocalizedKey(ReadOnlyMemory<byte> engineId)
            => GetLocalizedKey(_passcode, engineId);

        private byte[] GetLocalizedKey(ReadOnlyMemory<byte> secret, ReadOnlyMemory<byte> engineId)
        {
            lock (_localizedKeyCacheLock)
            {
                foreach (var entry in _localizedKeyCache)
                {
                    if (secret.Span.SequenceEqual(entry.Secret)
                        && engineId.Span.SequenceEqual(entry.EngineId))
                    {
                        return entry.LocalizedKey;
                    }
                }
            }

            var computed = new byte[DigestSize];
            ComputeLocalizedKey(secret, engineId, computed);
            var copiedSecret = secret.ToArray();
            var copiedEngineId = engineId.ToArray();

            lock (_localizedKeyCacheLock)
            {
                foreach (var entry in _localizedKeyCache)
                {
                    if (copiedSecret.AsSpan().SequenceEqual(entry.Secret)
                        && copiedEngineId.AsSpan().SequenceEqual(entry.EngineId))
                    {
                        return entry.LocalizedKey;
                    }
                }

                if (_localizedKeyCache.Count >= LocalizedKeyCacheCapacity)
                {
                    _localizedKeyCache.RemoveAt(0);
                }

                _localizedKeyCache.Add(new LocalizedKeyCacheEntry
                {
                    Secret = copiedSecret,
                    EngineId = copiedEngineId,
                    LocalizedKey = computed,
                });
            }

            return computed;
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
            if (destination.Length < DigestSize)
            {
                throw new ArgumentException($"Destination is too small. Must be >= {DigestSize}. Current: {destination.Length}.", nameof(destination));
            }

            // Common hot path: key localization into digest-sized buffer (auth/priv providers).
            // Reuse cached localized key instead of repeating RFC 3414 1MB expansion every call.
            if (destination.Length == DigestSize)
            {
                var localizedKey = GetLocalizedKey(secret, engineId);
                localizedKey.CopyTo(destination);
                return;
            }

            ComputeLocalizedKey(secret, engineId, destination);
        }

        private void ComputeLocalizedKey(in ReadOnlyMemory<byte> secret, in ReadOnlyMemory<byte> engineId, Span<byte> destination)
        {
            using var hash = IncrementalHash.CreateHash(_name);
            KeyUtils.GenerateLocalizedKey(secret.Span, engineId.Span, hash, destination);
        }
    }
}
