// Default privacy provider (empty class).
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

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Default privacy provider.
    /// </summary>
    public sealed class DefaultPrivacyProvider : IPrivacyProvider
    {
        private static IPrivacyProvider _defaultInstance;
        
        /// <summary>
        /// Default privacy provider with default authentication provider.
        /// </summary>
        public static IPrivacyProvider DefaultPair
        {
            get
            {
                return _defaultInstance ?? (_defaultInstance = new DefaultPrivacyProvider(DefaultAuthenticationProvider.Instance));
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPrivacyProvider"/> class.
        /// </summary>
        /// <param name="authentication">Authentication provider.</param>
        public DefaultPrivacyProvider(IAuthenticationProvider authentication)
        {
            AuthenticationProvider = authentication;
        }

        #region IPrivacyProvider Members

        /// <summary>
        /// Corresponding <see cref="IAuthenticationProvider"/>.
        /// </summary>
        public IAuthenticationProvider AuthenticationProvider { get; private set; }

        [Obsolete("Use EngineIds instead.")]
        public OctetString EngineId { get; set; }

        /// <summary>
        /// Engine IDs.
        /// </summary>
        /// <remarks>This is an optional field, and only used by TRAP v2 authentication.</remarks>
        public ICollection<OctetString> EngineIds { get; set; }

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public ISnmpData Decrypt(ISnmpData data, SecurityParameters parameters)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }            
            
            if (data.TypeCode != SnmpType.Sequence)
            {
                var newException = new DecryptionException("Default decryption failed");
                throw newException;
            }
            
            return data;
        }

        /// <summary>
        /// Encrypts the specified scope.
        /// </summary>
        /// <param name="data">The scope data.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public ISnmpData Encrypt(ISnmpData data, SecurityParameters parameters)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
                        
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }            
          
            if (data.TypeCode == SnmpType.Sequence || data is ISnmpPdu)
            {
                return data;
            }
            
            throw new ArgumentException("Invaild data type.", nameof(data));
        }

        /// <summary>
        /// Gets the salt.
        /// </summary>
        /// <value>The salt.</value>
        public OctetString Salt
        {
            get { return OctetString.Empty; }
        }

        /// <summary>
        /// Passwords to key.
        /// </summary>
        /// <param name="secret">The secret.</param>
        /// <param name="engineId">The engine identifier.</param>
        /// <returns></returns>
        public byte[] PasswordToKey(byte[] secret, byte[] engineId)
        {
            return AuthenticationProvider.PasswordToKey(secret, engineId);
        }

        #endregion

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Default privacy provider";
        }
    }
}
