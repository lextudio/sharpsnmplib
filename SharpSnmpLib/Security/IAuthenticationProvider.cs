// Authentication provider interface.
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

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Authentication provider interface.
    /// </summary>
    public interface IAuthenticationProvider
    {
        /// <summary>
        /// Gets the clean digest.
        /// </summary>
        /// <value>The clean digest.</value>
        OctetString CleanDigest { get; }

        /// <summary>
        /// Converts password to key.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="engineId">The engine id.</param>
        /// <returns></returns>
        byte[] PasswordToKey(byte[] password, byte[] engineId);

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="header">The header.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <returns></returns>
        OctetString ComputeHash(VersionCode version, Header header, SecurityParameters parameters, Scope scope, IPrivacyProvider privacy);

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="header">The header.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="scopeBytes">The scope bytes.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <returns></returns>
        OctetString ComputeHash(VersionCode version, Header header, SecurityParameters parameters, ISnmpData scopeBytes, IPrivacyProvider privacy);
    }
}
