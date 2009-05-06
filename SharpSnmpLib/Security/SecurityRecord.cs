using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Security
{
    public class SecurityRecord
    {
        private IAuthenticationProvider _authentication;
        private static Lextm.SharpSnmpLib.Security.SecurityRecord _default = new SecurityRecord(DefaultAuthenticationProvider.Instance, DefaultPrivacyProvider.Instance);
        private IPrivacyProvider _privacy;

        public SecurityRecord(IAuthenticationProvider authentication, IPrivacyProvider privacy)
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

        public IAuthenticationProvider Authentication
        {
            get { return _authentication; }
        }

        public IPrivacyProvider Privacy
        {
            get { return _privacy; }
        }

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

        public static Lextm.SharpSnmpLib.Security.SecurityRecord Default
        {
            get
            {
                return _default;
            }
        }
    }
}
