// Provider pair.
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

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// A pair of providers. One is for authentication, and the other is for privacy.
    /// </summary>
    public sealed class ProviderPair
    {
        private readonly IAuthenticationProvider _authentication;
        private static readonly ProviderPair DefaultPair = new ProviderPair(DefaultAuthenticationProvider.Instance, DefaultPrivacyProvider.Instance);
        private readonly IPrivacyProvider _privacy;

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
        public Levels ToSecurityLevel()
        {
            Levels flags;
            if (_authentication == DefaultAuthenticationProvider.Instance)
            {
                flags = Levels.None;
            }
            else if (_privacy == DefaultPrivacyProvider.Instance)
            {
                flags = Levels.Authentication;
            }
            else
            {
                flags = Levels.Authentication | Levels.Privacy;
            }

            return flags;
        }

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>The default.</value>
        public static ProviderPair Default
        {
            get
            {
                return DefaultPair;
            }
        }
    }
}
