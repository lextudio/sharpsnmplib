// IAuthenticationProvider extension class.
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
    /// Authentication provider extension.
    /// </summary>
    public static class AuthenticationProviderExtension
    {
        /// <summary>
        /// Verifies the hash.
        /// </summary>
        /// <param name="authen">The authentication provider.</param>
        /// <param name="version">The version.</param>
        /// <param name="header">The header.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="scopeBytes">The scope bytes.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <returns>
        /// Returns <code>true</code> if hash matches. Otherwise, returns <code>false</code>.
        /// </returns>
        public static bool VerifyHash(this IAuthenticationProvider authen, VersionCode version, Header header, SecurityParameters parameters, ISnmpData scopeBytes, IPrivacyProvider privacy)
        {
            if (authen == null)
            {
                throw new ArgumentNullException("authen");
            }
            
            if (header == null)
            {
                throw new ArgumentNullException("header");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            if (scopeBytes == null)
            {
                throw new ArgumentNullException("scopeBytes");
            }

            if (privacy == null)
            {
                throw new ArgumentNullException("privacy");
            }

            if (authen is DefaultAuthenticationProvider)
            {
                return true;
            }

            if (0 == (header.SecurityLevel & Levels.Authentication))
            {
                return true;
            }

            var expected = parameters.AuthenticationParameters;
            parameters.AuthenticationParameters = authen.CleanDigest; // clean the hash first.
            bool result = authen.ComputeHash(version, header, parameters, scopeBytes, privacy) == expected;
            parameters.AuthenticationParameters = expected; // restore the hash.
            return result;
        }

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="authen">The authentication provider.</param>
        /// <param name="version">The version.</param>
        /// <param name="header">The header.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="privacy">The privacy provider.</param>
        public static void ComputeHash(this IAuthenticationProvider authen, VersionCode version, Header header, SecurityParameters parameters, ISegment scope, IPrivacyProvider privacy)
        {
            if (authen == null)
            {
                throw new ArgumentNullException("authen");
            }
            
            if (header == null)
            {
                throw new ArgumentNullException("header");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            if (scope == null)
            {
                throw new ArgumentNullException("scope");
            }

            if (privacy == null)
            {
                throw new ArgumentNullException("privacy");
            }

            if (authen is DefaultAuthenticationProvider)
            {
                return;
            }

            if (0 == (header.SecurityLevel & Levels.Authentication))
            {
                return;
            }

            var scopeData = privacy.GetScopeData(header, parameters, scope.GetData(version));
            // replace the hash.
            parameters.AuthenticationParameters = authen.ComputeHash(version, header, parameters, scopeData, privacy);
        }
    }
}