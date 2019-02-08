// Privacy provider interface.
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
    /// Privacy provider interface.
    /// </summary>
    public interface IPrivacyProvider
    {
        /// <summary>
        /// Encrypts the specified scope.
        /// </summary>
        /// <param name="data">The scope data.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        ISnmpData Encrypt(ISnmpData data, SecurityParameters parameters);

        /// <summary>
        /// Gets the salt.
        /// </summary>
        /// <value>The salt.</value>
        OctetString Salt { get; }

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        ISnmpData Decrypt(ISnmpData data, SecurityParameters parameters);
        
        /// <summary>
        /// Corresponding <see cref="IAuthenticationProvider"/>.
        /// </summary>
        IAuthenticationProvider AuthenticationProvider { get; }

        [Obsolete("Use EngineIds instead.")]
        OctetString EngineId { get; }

        /// <summary>
        /// Engine IDs.
        /// </summary>
        /// <remarks>This is an optional field, and only used by TRAP v2 authentication.</remarks>
        ICollection<OctetString> EngineIds { get; }

        /// <summary>
        /// Passwords to key.
        /// </summary>
        /// <param name="secret">The secret.</param>
        /// <param name="engineId">The engine identifier.</param>
        /// <returns></returns>
        byte[] PasswordToKey(byte[] secret, byte[] engineId);
    }
}
