using Lextm.SharpSnmpLib;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Authentication provider interface.
    /// </summary>
    /// <remarks>
    /// Authentication providers are responsible for message authentication and key generation
    /// in SNMP v3. Different implementations of this interface can provide various authentication
    /// mechanisms such as MD5, SHA-1, SHA-256, etc.
    /// </remarks>
    public interface IAuthenticationProvider
    {
        /// <summary>
        /// Gets truncated Digest Size.
        /// </summary>
        /// <value>
        /// The size of the truncated digest in bytes.
        /// </value>
        public int TruncatedDigestSize { get; }

        /// <summary>
        /// Gets digest Size.
        /// </summary>
        /// <value>
        /// The full size of the digest in bytes.
        /// </value>
        public int DigestSize { get; }

        /// <summary>
        /// Localizes a user secret into an engine-specific authentication key.
        /// </summary>
        /// <param name="secret">The password or secret to convert.</param>
        /// <param name="engineId">The engine ID to use in the key generation process.</param>
        /// <param name="destination">The destination span to write the key into.</param>
        /// <remarks>
        /// The localization process produces a key that is unique to a specific SNMP engine,
        /// providing better security properties.
        /// </remarks>
        void PasswordToKey(in ReadOnlyMemory<byte> secret, in ReadOnlyMemory<byte> engineId, Span<byte> destination);

        /// <summary>
        /// Authenticates an outgoing SNMP v3 message by computing and setting its authentication parameters.
        /// </summary>
        /// <param name="message">The SNMP v3 message to authenticate.</param>
        /// <param name="newAuthParams">A buffer for storing the authentication parameters.</param>
        /// <remarks>
        /// The implementation should compute a message digest, truncate it if necessary, 
        /// and place the result in the newAuthParams buffer.
        /// </remarks>
        void AuthenticateOutgoingMsg(
            SnmpV3Message message,
            Memory<byte> newAuthParams);

        /// <summary>
        /// Authenticates an incoming SNMP v3 message by verifying its authentication parameters.
        /// </summary>
        /// <param name="message">The SNMP v3 message to authenticate.</param>
        /// <returns>
        /// <c>true</c> if the message is authenticated successfully; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The implementation should recompute the message digest and compare it with
        /// the received authentication parameters.
        /// </remarks>
        bool AuthenticateIncomingMsg(
            SnmpV3Message message);

        /// <summary>
        /// Gets a zeroed digest with provider-specific truncated size (legacy compatibility member).
        /// </summary>
        OctetString CleanDigest => new(new byte[TruncatedDigestSize]);
    }
}
