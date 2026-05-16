using Lextm.SharpSnmpLib;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// SNMP message.
    /// </summary>
    public interface ISnmpMessage : ISnmpData
    {
        /// <summary>
        /// Gets the SNMP protocol version.
        /// </summary>
        VersionCode ProtocolVersion { get; }

        /// <summary>
        /// Gets the SNMP protocol version (legacy compatibility alias).
        /// </summary>
        VersionCode Version => ProtocolVersion;

        /// <summary>
        /// Gets the message scope.
        /// </summary>
        IScope? Scope { get; }

        /// <summary>
        /// Gets message header information.
        /// </summary>
        global::Lextm.SharpSnmpLib.Header Header => global::Lextm.SharpSnmpLib.Header.FromMessage(this);

        /// <summary>
        /// Gets security parameters.
        /// </summary>
        global::Lextm.SharpSnmpLib.SecurityParameters Parameters => global::Lextm.SharpSnmpLib.SecurityParameters.FromMessage(this);

        /// <summary>
        /// Gets privacy provider (legacy compatibility).
        /// </summary>
        global::Lextm.SharpSnmpLib.Security.IPrivacyProvider Privacy => global::Lextm.SharpSnmpLib.Security.DefaultPrivacyProvider.DefaultPair;

        /// <summary>
        /// Returns the original wire bytes when the message was received from the network,
        /// or null when the message was constructed from scratch.
        /// Useful for forwarding/proxying without re-serialization.
        /// </summary>
        byte[]? RawBytes => null;

        /// <summary>
        /// Serializes the message to a byte array. Returns cached wire bytes when available.
        /// </summary>
        byte[] ToBytes() => RawBytes ?? AsnSerializableExtensions.Encode(this);
    }
}
