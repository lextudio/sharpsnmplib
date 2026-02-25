using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;

namespace DotNetSnmp.Transport.Targets
{
    public interface ISnmpTarget
    {
        int Retries { get; }

        int MaxMessageSize { get; }

        int Timeout { get; }

        OctetString SecurityName { get; }

        VersionCode ProtocolVersion { get; }
    }
}
