using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;

namespace DotNetSnmp.Transport.Targets
{
    public abstract record AbstractTarget : ISnmpTarget
    {
        protected AbstractTarget(VersionCode version, OctetString securityName)
        {
            ProtocolVersion = version;
            SecurityName = securityName;
        }

        public OctetString SecurityName { get; } = OctetString.Empty;

        public int Retries { get; init; } = 0;

        public int Timeout { get; init; } = 60000;

        public int MaxMessageSize { get; init; } = 0xFFE3;

        public VersionCode ProtocolVersion
        {
            get;
        }
    }
}
