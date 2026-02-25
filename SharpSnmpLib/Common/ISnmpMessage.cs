using DotNetSnmp.Asn1.Serialization;

namespace DotNetSnmp.Common.Definitions
{
    public interface ISnmpMessage : IAsnSerializable
    {
        VersionCode ProtocolVersion { get; }

        IScope? Scope { get; }
    }
}
