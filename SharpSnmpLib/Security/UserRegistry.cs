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
        private readonly IDictionary<OctetString, User> _users = new Dictionary<OctetString, User>();
        private static readonly UserRegistry DefaultRegistry = new UserRegistry(null);

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRegistry"/> class.
        /// </summary>
        /// <param name="users">The users.</param>
        public UserRegistry(User[] users)
        {
            if (users != null)
            {
                foreach (User user in users)
                {
                    Add(user);
                }
            }
        }

        /// <summary>
        /// Gets the default registry.
        /// </summary>
        /// <value>The default registry.</value>
        public static UserRegistry Default
        {
            get { return DefaultRegistry; }
        }

        /// <summary>
        /// Adds the specified user name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="providers">The providers.</param>
        public void Add(OctetString userName, ProviderPair providers)
        {
            Add(new User(userName, providers));
        }

        /// <summary>
        /// Adds the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void Add(User user)
        {
            if (!_users.ContainsKey(user.Name))
            {
                _users.Add(user.Name, user);
            }
        }

        /// <summary>
        /// Finds the specified user name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public ProviderPair Find(OctetString userName)
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
                return _users[userName].Providers;
            }

            throw new ArgumentException("no such user: " + userName);
        }
    }
}
