using DotNetSnmp.Asn1.Serialization;

namespace DotNetSnmp.Common.Definitions
{
    /// <summary>
    /// SNMP message.
    /// </summary>
    public interface ISnmpMessage : IAsnSerializable
    {
        /// <summary>
        /// Gets the SNMP protocol version.
        /// </summary>
        VersionCode ProtocolVersion { get; }

        /// <summary>
        /// Gets the message scope.
        /// </summary>
        IScope? Scope { get; }
    }
}
