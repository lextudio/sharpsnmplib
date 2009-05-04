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
            if (this == SecurityRecord.Default)
            {
                flags = SecurityLevel.Reportable;
            }
            else if (_privacy == DefaultPrivacyProvider.Instance)
            {
                flags = SecurityLevel.Reportable | SecurityLevel.Authentication;
            }
            else
            {
                flags = SecurityLevel.Reportable | SecurityLevel.Authentication | SecurityLevel.Privacy;
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
