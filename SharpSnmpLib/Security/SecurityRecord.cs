using System;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// A pair of providers. One is for authentication, and the other is for privacy.
    /// </summary>
    public class ProviderPair
    {
        private IAuthenticationProvider _authentication;
        private static Lextm.SharpSnmpLib.Security.ProviderPair _default = new ProviderPair(DefaultAuthenticationProvider.Instance, DefaultPrivacyProvider.Instance);
        private IPrivacyProvider _privacy;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderPair"/> class.
        /// </summary>
        /// <param name="authentication">The authentication.</param>
        /// <param name="privacy">The privacy.</param>
        public ProviderPair(IAuthenticationProvider authentication, IPrivacyProvider privacy)
        {
            if (authentication == DefaultAuthenticationProvider.Instance)
            {
                // FIXME: in this way privacy cannot be non-default.
                if (privacy != DefaultPrivacyProvider.Instance)
                {
                    throw new ArgumentException("if authentication is off, then privacy cannot be used");
                }
            }

            _authentication = authentication;
            _privacy = privacy;
        }

        /// <summary>
        /// Gets the authentication.
        /// </summary>
        /// <value>The authentication.</value>
        public IAuthenticationProvider Authentication
        {
            get { return _authentication; }
        }

        /// <summary>
        /// Gets the privacy.
        /// </summary>
        /// <value>The privacy.</value>
        public IPrivacyProvider Privacy
        {
            get { return _privacy; }
        }

        /// <summary>
        /// Toes the security level.
        /// </summary>
        /// <returns></returns>
        public SecurityLevel ToSecurityLevel()
        {
            SecurityLevel flags;
            if (_authentication == DefaultAuthenticationProvider.Instance)
            {
                flags = SecurityLevel.None;
            }
            else if (_privacy == DefaultPrivacyProvider.Instance)
            {
                flags = SecurityLevel.Authentication;
            }
            else
            {
                flags = SecurityLevel.Authentication | SecurityLevel.Privacy;
            }

            return flags;
        }

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>The default.</value>
        public static Lextm.SharpSnmpLib.Security.ProviderPair Default
        {
            get
            {
                return _default;
            }
        }
    }
}
