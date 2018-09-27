// Default authentication provider (empty class).
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
    /// Default authentication provider.
    /// </summary>
    public sealed class DefaultAuthenticationProvider : IAuthenticationProvider
    {
        private DefaultAuthenticationProvider()
        {
        }

        private static IAuthenticationProvider _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static IAuthenticationProvider Instance
        {
            get { return _instance ?? (_instance = new DefaultAuthenticationProvider()); }
        }

        #region IAuthenticationProvider Members

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="header">The header.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="data">The scope data.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <param name="length">The length bytes.</param>
        /// <returns></returns>
        public OctetString ComputeHash(VersionCode version, ISegment header, SecurityParameters parameters, ISnmpData data, IPrivacyProvider privacy, byte[] length)
        {
            if (header == null)
            {
                throw new ArgumentNullException(nameof(header));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (privacy == null)
            {
                throw new ArgumentNullException(nameof(privacy));
            }

            return OctetString.Empty;
        }

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public OctetString ComputeHash(byte[] buffer, OctetString engineId)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (engineId == null)
            {
                throw new ArgumentNullException(nameof(engineId));
            }

            return OctetString.Empty;
        }

        /// <summary>
        /// Gets the clean digest.
        /// </summary>
        /// <value>The clean digest.</value>
        public OctetString CleanDigest
        {
            get { return OctetString.Empty; }
        }

        /// <summary>
        /// Converts password to key.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="engineId"></param>
        /// <returns></returns>
        public byte[] PasswordToKey(byte[] password, byte[] engineId)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }
            
            if (engineId == null)
            {
                throw new ArgumentNullException(nameof(engineId));
            }
            
            // IMPORTANT: this function is not used.
            return new byte[0];
        }

        /// <summary>
        /// Gets the length of the digest.
        /// </summary>
        /// <value>The length of the digest.</value>
        public int DigestLength => 0;

        #endregion

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Default authentication provider";
        }
    }
}
