using DotNetSnmp.Asn1.Serialization;
using Lextm.SharpSnmpLib;

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
    }
}
