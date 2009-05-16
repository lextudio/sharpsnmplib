using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// A repository to store user information for providers.
    /// </summary>
    public class UserRegistry
    {
        private IDictionary<OctetString, ProviderPair> _users = new Dictionary<OctetString, ProviderPair>();

        /// <summary>
        /// Adds the specified user name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="pair">The pair.</param>
        public void Add(OctetString userName, ProviderPair pair)
        {
            _users.Add(userName, pair);
        }

        internal ProviderPair Find(OctetString userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }

            if (userName == OctetString.Empty)
            {
                return ProviderPair.Default;
            }

            if (_users.ContainsKey(userName))
            {
                return _users[userName];
            }

            throw new ArgumentException("no such user: " + userName);
        }
    }
}
