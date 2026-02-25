using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;

namespace DotNetSnmp.Transport.Targets
{
    /// <summary>
    /// Represents the AbstractTarget type.
    /// </summary>
    public abstract record AbstractTarget : ISnmpTarget
    {
        /// <summary>
        /// Initializes a new instance of AbstractTarget.
        /// </summary>
        protected AbstractTarget(VersionCode version, OctetString securityName)
        {
            ProtocolVersion = version;
            SecurityName = securityName;
        }

        /// <summary>
        /// Gets security Name.
        /// </summary>
        public OctetString SecurityName { get; } = OctetString.Empty;

        /// <summary>
        /// Gets retries.
        /// </summary>
        public int Retries { get; init; } = 0;

        /// <summary>
        /// Gets timeout.
        /// </summary>
        public int Timeout { get; init; } = 60000;

        /// <summary>
        /// Gets max Message Size.
        /// </summary>
        public int MaxMessageSize { get; init; } = 0xFFE3;

        /// <summary>
        /// Represents this member.
        /// </summary>
        public VersionCode ProtocolVersion
        {
            get;
        }
    }
}
