using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V3.Security.Authentication;
using DotNetSnmp.Protocol.V3.Security.Privacy;

namespace DotNetSnmp.Transport.Targets
{
    /// <summary>
    /// Represents a target for SNMPv3 operations requiring a username and security parameters.
    /// </summary>
    public record UserTarget : AbstractTarget
    {
        public IAuthenticationProvider AuthenticationService
        {
            get { return PrivacyService.AuthenticationProvider; }
        }

        public IPrivacyProvider PrivacyService { get; }

        /// <summary>
        /// Gets or sets the engine ID of the remote SNMP agent.
        /// </summary>
        public ReadOnlyMemory<byte> EngineId { get; set; } = ReadOnlyMemory<byte>.Empty;

        /// <summary>
        /// Gets or sets the engine boots value of the remote SNMP agent.
        /// </summary>
        public int EngineBoots { get; set; }

        /// <summary>
        /// Gets or sets the engine time value of the remote SNMP agent.
        /// </summary>
        public int EngineTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserTarget"/> class with default values.
        /// </summary>
        public UserTarget(OctetString securityName, IPrivacyProvider privacy, int maxMessageSize = 65507)
            : base(VersionCode.V3, securityName)
        {
            MaxMessageSize = maxMessageSize;
            PrivacyService = privacy;
        }

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

        public SecurityModel SecurityModel => SecurityModel.Usm;
    }
}
