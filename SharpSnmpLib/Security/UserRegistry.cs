// User registry.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// A repository to store user information for providers.
    /// </summary>
    public sealed class UserRegistry
    {
        private readonly IDictionary<OctetString, ProviderPair> _users = new Dictionary<OctetString, ProviderPair>();
        private static readonly UserRegistry EmptyRegistry = new UserRegistry();

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>The empty.</value>
        public static UserRegistry Empty
        {
            get { return EmptyRegistry; }
        }

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
