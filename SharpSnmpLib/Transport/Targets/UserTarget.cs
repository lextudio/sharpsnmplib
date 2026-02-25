using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V3.Security.Authentication;
using DotNetSnmp.Protocol.V3.Security.Privacy;

namespace DotNetSnmp.Transport.Targets
{
    /// <summary>
    /// Represents the UserTarget type.
    /// </summary>
    public record UserTarget : AbstractTarget
    {
        /// <summary>
        /// Represents this member.
        /// </summary>
        public IAuthenticationProvider AuthenticationService
        {
            get { return PrivacyService.AuthenticationProvider; }
        }

        /// <summary>
        /// Gets privacy Service.
        /// </summary>
        public IPrivacyProvider PrivacyService { get; }

        /// <summary>
        /// Gets engine Id.
        /// </summary>
        public ReadOnlyMemory<byte> EngineId { get; set; } = ReadOnlyMemory<byte>.Empty;

        /// <summary>
        /// Gets engine Boots.
        /// </summary>
        public int EngineBoots { get; set; }

        /// <summary>
        /// Gets engine Time.
        /// </summary>
        public int EngineTime { get; set; }

        /// <summary>
        /// Initializes a new instance of UserTarget.
        /// </summary>
        public UserTarget(OctetString securityName, IPrivacyProvider privacy, int maxMessageSize = 65507)
            : base(VersionCode.V3, securityName)
        {
            MaxMessageSize = maxMessageSize;
            PrivacyService = privacy;
        }

        /// <summary>
        /// Represents this member.
        /// </summary>
        public Levels SecurityLevel
        {
            get
            {
                var isAuth = AuthenticationService is not DefaultAuthenticationProvider;
                var isPriv = PrivacyService is not DefaultPrivacyProvider;
                if (isAuth && isPriv)
                {
                    return Levels.Authentication
                        | Levels.Privacy;
                }
                else if (isAuth)
                {
                    return Levels.Authentication;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Represents usm.
        /// </summary>
        public SecurityModel SecurityModel => SecurityModel.Usm;
    }
}
