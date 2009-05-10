using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Security
{
    public class SecurityRegistry
    {
        private IDictionary<OctetString, SecurityRecord> _users = new Dictionary<OctetString, SecurityRecord>();

        public void Add(OctetString userName, SecurityRecord record)
        {
            _users.Add(userName, record);
        }

        internal SecurityRecord Find(OctetString userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }

            if (userName == OctetString.Empty)
            {
                return SecurityRecord.Default;
            }

            if (_users.ContainsKey(userName))
            {
                return _users[userName];
            }

            throw new ArgumentException("no such user: " + userName);
        }
    }
}
