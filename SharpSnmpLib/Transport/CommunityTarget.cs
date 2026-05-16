using Lextm.SharpSnmpLib;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Represents the CommunityTarget type.
    /// </summary>
    public record CommunityTarget : AbstractTarget
    {
        /// <summary>
        /// Initializes a new instance of CommunityTarget.
        /// </summary>
        public CommunityTarget(VersionCode version, OctetString community)
            : base(version, community)
        {
        }

        /// <summary>
        /// Represents this member.
        /// </summary>
        public OctetString Community
        {
            get { return SecurityName; }
        }
    }
}
