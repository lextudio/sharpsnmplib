// User registry.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// A repository to store user information for providers.
    /// </summary>
    public sealed class UserRegistry
    {
        private readonly IDictionary<OctetString, User> _users = new Dictionary<OctetString, User>();

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRegistry"/> class.
        /// </summary>
        /// <param name="users">The users.</param>
// ReSharper disable ParameterTypeCanBeEnumerable.Local
        public UserRegistry(User[] users)
// ReSharper restore ParameterTypeCanBeEnumerable.Local
        {
            if (users == null)
            {
                return;
            }

            foreach (var user in users)
            {
                Add(user);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRegistry"/> class.
        /// </summary>
        public UserRegistry() : this(null)
        {
        }
        
        /// <summary>
        /// Returns the user count.
        /// </summary>
        public int Count
        {
            get { return _users.Count; }
        }

        /// <summary>
        /// Adds the specified user name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="privacy">The privacy provider.</param>
        public UserRegistry Add(OctetString userName, IPrivacyProvider privacy)
        {
            return Add(new User(userName, privacy));
        }

        /// <summary>
        /// Adds the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        public UserRegistry Add(User user)
        {
            if (user == null)
            {
                return this;
            }
            
            if (_users.ContainsKey(user.Name))
            {
                _users.Remove(user.Name);
            }

            _users.Add(user.Name, user);
            return this;
        }

        /// <summary>
        /// Finds the specified user name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public IPrivacyProvider Find(OctetString userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (userName == OctetString.Empty)
            {
                // IMPORTANT: used in messagefactory.cs to decrypt discovery messages.
                return DefaultPrivacyProvider.DefaultPair;
            }

            return _users.ContainsKey(userName) ? _users[userName].Privacy : null;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "User registry: count: {0}", Count);
        }
    }
}
