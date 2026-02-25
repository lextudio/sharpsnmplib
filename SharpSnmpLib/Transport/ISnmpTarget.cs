using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;

namespace DotNetSnmp.Transport.Targets
{
    /// <summary>
    /// Defines the contract for ISnmpTarget.
    /// </summary>
    public interface ISnmpTarget
    {
        /// <summary>
        /// Gets retries.
        /// </summary>
        int Retries { get; }

        /// <summary>
        /// Gets max Message Size.
        /// </summary>
        int MaxMessageSize { get; }

        /// <summary>
        /// Gets timeout.
        /// </summary>
        int Timeout { get; }

        /// <summary>
        /// Gets security Name.
        /// </summary>
        OctetString SecurityName { get; }

        /// <summary>
        /// Gets the SNMP protocol version.
        /// </summary>
        VersionCode ProtocolVersion { get; }
    }
}
