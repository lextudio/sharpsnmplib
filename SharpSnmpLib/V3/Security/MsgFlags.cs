namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Defines the message flags for SNMPv3 message processing (RFC 3414).
    /// </summary>
    /// <remarks>
    /// <para>
    /// Message flags indicate which security services (authentication and privacy) are applied to an SNMPv3 message.
    /// These flags are encoded in the msgFlags field of the SNMPv3 message header and control:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Whether the message is authenticated (Auth flag)</description></item>
    /// <item><description>Whether the message is encrypted (Priv flag)</description></item>
    /// <item><description>Whether the message expects a response (Reportable flag)</description></item>
    /// </list>
    /// <para>
    /// The combination of Auth and Priv flags defines the overall security level:
    /// </para>
    /// <list type="table">
    /// <listheader><term>Flags</term><description>Security Level</description></listheader>
    /// <item><term>NoAuthNoPriv (0)</term><description>No Authentication, No Privacy</description></item>
    /// <item><term>Auth (1)</term><description>Authentication Only</description></item>
    /// <item><term>Auth | Priv (3)</term><description>Authentication and Privacy (Encryption)</description></item>
    /// </list>
    /// </remarks>
    [Flags]
    public enum MsgFlag : byte
    {
        /// <summary>
        /// No authentication and no privacy encryption (security level: noAuthNoPriv).
        /// </summary>
        /// <remarks>
        /// This flag value (0x00) indicates that the message is neither authenticated nor encrypted.
        /// This is the lowest security level in SNMPv3 and is typically used only in controlled environments.
        /// </remarks>
        NoAuthNoPriv = 0,

        /// <summary>
        /// Authentication enabled (bit 0 of msgFlags, value 0x01).
        /// </summary>
        /// <remarks>
        /// When set, indicates that the message has been authenticated using an authentication protocol
        /// (e.g., HMAC-MD5-96, HMAC-SHA-96, HMAC-192-SHA-256, HMAC-256-SHA-384, HMAC-384-SHA-512).
        /// Authentication provides integrity and non-repudiation guarantees but not confidentiality.
        /// </remarks>
        Auth = 1,

        /// <summary>
        /// Privacy (encryption) enabled (bit 1 of msgFlags, value 0x02).
        /// </summary>
        /// <remarks>
        /// When set, indicates that the message scopedPDU portion has been encrypted using a privacy protocol
        /// (e.g., DES, 3DES, AES). Privacy requires authentication to also be enabled per RFC 3414.
        /// When combined with Auth flag (value 0x03), provides both integrity and confidentiality.
        /// </remarks>
        Priv = 2,

        /// <summary>
        /// Reportable message (bit 2 of msgFlags, value 0x04).
        /// </summary>
        /// <remarks>
        /// When set, indicates that the message is a Report message that might be sent without security processing
        /// if an error occurs during incoming message processing. Typically used for engine ID discovery and synchronization.
        /// This flag is independent of the Auth and Priv flags.
        /// </remarks>
        Reportable = 4,
    }
}
