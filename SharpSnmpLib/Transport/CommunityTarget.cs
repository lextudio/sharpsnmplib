using DotNetSnmp.Asn1.SyntaxObjects;

namespace DotNetSnmp.Transport.Targets
{
    public record CommunityTarget : AbstractTarget
    {
        public CommunityTarget(Common.Definitions.VersionCode version, OctetString community)
            : base(version, community)
        {
        }

        public OctetString Community
        {
            get { return SecurityName; }
        }
    }
}
