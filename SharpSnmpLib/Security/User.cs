// User class.
// Copyright (C) 2010 Lex Li.
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

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// User class.
    /// </summary>
    public class User
    {
        private readonly ProviderPair _providers;
        private readonly OctetString _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="authen">The authen.</param>
        /// <param name="authenPhrase">The authen phrase.</param>
        /// <param name="privacy">The privacy.</param>
        /// <param name="privacyPhrase">The privacy phrase.</param>
        public User(OctetString name, string authen, OctetString authenPhrase, string privacy, OctetString privacyPhrase)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (authen == null)
            {
                throw new ArgumentNullException("authen");
            }

            if (authenPhrase == null)
            {
                throw new ArgumentNullException("authenPhrase");
            }

            if (privacy == null)
            {
                throw new ArgumentNullException("privacy");
            }

            if (privacyPhrase == null)
            {
                throw new ArgumentNullException("privacyPhrase");
            }

            IAuthenticationProvider authenticationProvider;
            if (string.IsNullOrEmpty(authen))
            {
                authenticationProvider = DefaultAuthenticationProvider.Instance;
            }
            else if (authen.ToUpperInvariant() == "MD5")
            {
                authenticationProvider = new MD5AuthenticationProvider(authenPhrase);
            }
            else if (authen.ToUpperInvariant() == "SHA")
            {
                authenticationProvider = new SHA1AuthenticationProvider(authenPhrase);
            }
            else
            {
                throw new ArgumentException("Unknown authentication method: " + authen, "authen");
            }

            IPrivacyProvider privacyProvider;
            if (string.IsNullOrEmpty(privacy))
            {
                privacyProvider = DefaultPrivacyProvider.Instance;
            }
            else if (privacy.ToUpperInvariant() == "DES")
            {
                privacyProvider = new DESPrivacyProvider(privacyPhrase, authenticationProvider);
            }
            else
            {
                throw new ArgumentException("Unknown privacy method: " + privacy, "privacy");
            }

            _name = name;
            _providers = new ProviderPair(authenticationProvider, privacyProvider);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="providers">The providers.</param>
        public User(OctetString name, ProviderPair providers)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (providers == null)
            {
                throw new ArgumentNullException("providers");
            }

            _name = name;
            _providers = providers;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public OctetString Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <value>The providers.</value>
        public ProviderPair Providers
        {
            get { return _providers; }
        }
    }
}