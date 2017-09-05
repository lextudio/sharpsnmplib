// IPrivacyProvider extension methods.
// Copyright (C) 2010 Lex Li
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

/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 10/10/2010
 * Time: 6:59 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Extension class for <see cref="IPrivacyProvider"/>.
    /// </summary>
    public static class PrivacyProviderExtension
    {
        /// <summary>
        /// Converts to <see cref="Levels"/>.
        /// </summary>
        /// <returns>Levels.</returns>
        public static Levels ToSecurityLevel(this IPrivacyProvider privacy)
        {
            if (privacy == null)
            {
                throw new ArgumentNullException(nameof(privacy));
            }
                
            Levels flags;
            if (privacy.AuthenticationProvider == DefaultAuthenticationProvider.Instance)
            {
                flags = 0;
            }
            else if (privacy is DefaultPrivacyProvider)
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
        /// Gets the scope data.
        /// </summary>
        /// <param name="privacy">The privacy provider.</param>
        /// <param name="header">The header.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="rawScopeData">The raw scope data.</param>
        /// <returns>ISnmpData.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// privacy
        /// or
        /// header
        /// </exception>
        public static ISnmpData GetScopeData(this IPrivacyProvider privacy, Header header, SecurityParameters parameters, ISnmpData rawScopeData)
        {
            if (privacy == null)
            {
                throw new ArgumentNullException(nameof(privacy));
            }

            if (header == null)
            {
                throw new ArgumentNullException(nameof(header));
            }

            return Levels.Privacy == (header.SecurityLevel & Levels.Privacy)
                       ? privacy.Encrypt(rawScopeData, parameters)
                       : rawScopeData;
        }

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="header">The header.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="privacy">The privacy provider.</param>
        public static void ComputeHash(this IPrivacyProvider privacy, VersionCode version, Header header, SecurityParameters parameters, ISegment scope)
        {
            if (header == null)
            {
                throw new ArgumentNullException(nameof(header));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }

            if (privacy == null)
            {
                throw new ArgumentNullException(nameof(privacy));
            }

            var provider = privacy.AuthenticationProvider;
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
        /// <param name="version">The version.</param>
        /// <param name="header">The header.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="scopeBytes">The scope bytes.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <param name="length">The length bytes.</param>
        /// <returns>
        /// Returns <c>true</c> if hash matches. Otherwise, returns <c>false</c>.
        /// </returns>
        public static bool VerifyHash(this IPrivacyProvider privacy, VersionCode version, Header header, SecurityParameters parameters, ISnmpData scopeBytes, byte[] length)
        {
            if (header == null)
            {
                throw new ArgumentNullException(nameof(header));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (scopeBytes == null)
            {
                throw new ArgumentNullException(nameof(scopeBytes));
            }

            if (privacy == null)
            {
                throw new ArgumentNullException(nameof(privacy));
            }

            var provider = privacy.AuthenticationProvider;
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
