// Version 1 membership provider class.
// Copyright (C) 2009-2010 Lex Li
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

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// SNMP version 1 membership provider, who checks community names for security.
    /// </summary>    
    public sealed class Version1MembershipProvider : IMembershipProvider
    {
        private const VersionCode Version = VersionCode.V1;
        private readonly OctetString _get;
        private readonly OctetString _set;

        /// <summary>
        /// Initializes a new instance of the <see cref="Version1MembershipProvider"/> class.
        /// </summary>
        /// <param name="getCommunity">The get community.</param>
        /// <param name="setCommunity">The set community.</param>
        public Version1MembershipProvider(OctetString getCommunity, OctetString setCommunity)
        {
            _get = getCommunity;
            _set = setCommunity;
        }

        /// <summary>
        /// Authenticates the request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public bool AuthenticateRequest(ISnmpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            
            if (context.Request.Version != Version)
            {
                return false;
            }

            var parameters = context.Request.Parameters;
            if (context.Request.Pdu().TypeCode == SnmpType.SetRequestPdu)
            {
                return parameters.UserName == _set;
            }

            return parameters.UserName == _get;
        }
    }
}
