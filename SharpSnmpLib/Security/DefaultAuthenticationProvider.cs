// Default authentication provider (empty class).
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
using Lextm.SharpSnmpLib.Messaging;

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

        private static readonly object Root = new object();
        private static IAuthenticationProvider _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static IAuthenticationProvider Instance
        {
            get
            {
                lock (Root)
                {
                    if (_instance == null)
                    {
                        _instance = new DefaultAuthenticationProvider();
                    }
                }

                return _instance;
            }
        }

        #region IAuthenticationProvider Members

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public OctetString ComputeHash(ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
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
            if (engineId == null)
            {
                throw new ArgumentNullException("engineId");
            }
            
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            
            // IMPORTANT: this is not needed in this class.
            return null;
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
            return "Default authentication provider";
        }
    }
}
