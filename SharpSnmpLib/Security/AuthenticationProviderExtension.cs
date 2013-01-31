// IAuthenticationProvider extension class.
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

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Authentication provider extension.
    /// </summary>
    public static class AuthenticationProviderExtension
    {
        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="provider">The authentication provider.</param>
        /// <param name="version">The version.</param>
        /// <param name="header">The header.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="privacy">The privacy provider.</param>
        public static void ComputeHash(this IAuthenticationProvider provider, VersionCode version, Header header, SecurityParameters parameters, ISegment scope, IPrivacyProvider privacy)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
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

            if (provider is DefaultAuthenticationProvider)
            {
                return;
            }

            if (0 == (header.SecurityLevel & Levels.Authentication))
            {
                return;
            }

            var scopeData = privacy.GetScopeData(header, parameters, scope.GetData(version));
            parameters.AuthenticationParameters = provider.ComputeHash(version, header, parameters, scopeData, privacy, null); // replace the hash.
        }

        /// <summary>
        /// Verifies the hash.
        /// </summary>
        /// <param name="provider">The authentication provider.</param>
        /// <param name="version">The version.</param>
        /// <param name="header">The header.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="scopeBytes">The scope bytes.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <param name="length">The length bytes.</param>
        /// <returns>
        /// Returns <code>true</code> if hash matches. Otherwise, returns <code>false</code>.
        /// </returns>
        public static bool VerifyHash(this IAuthenticationProvider provider, VersionCode version, Header header, SecurityParameters parameters, ISnmpData scopeBytes, IPrivacyProvider privacy, byte[] length)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
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

            if (provider is DefaultAuthenticationProvider)
            {
                return true;
            }

            if (0 == (header.SecurityLevel & Levels.Authentication))
            {
                return true;
            }

            var expected = parameters.AuthenticationParameters;
            parameters.AuthenticationParameters = provider.CleanDigest; // clean the hash first.
            var newHash = provider.ComputeHash(version, header, parameters, scopeBytes, privacy, length);
            parameters.AuthenticationParameters = expected; // restore the hash.
            return newHash == expected;
        }
    }
}
