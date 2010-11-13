// Normal SNMP context class.
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

using System.Collections.Generic;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Nomral SNMP context class. It is v1 and v2c specific.
    /// </summary>
    internal sealed class NormalSnmpContext : SnmpContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NormalSnmpContext"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="users">The users.</param>
        /// <param name="binding">The binding.</param>
        public NormalSnmpContext(ISnmpMessage request, IPEndPoint sender, UserRegistry users, IListenerBinding binding) 
            : base(request, sender, users, null, binding)
        {
        }

        /// <summary>
        /// Handles the authentication failure.
        /// </summary>
        internal override void HandleAuthenticationFailure()
        {
            // TODO: implement this later according to v1 and v2c RFC.
            Response = null;
        }

        /// <summary>
        /// Copies the request variable bindings to response.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="index">The index.</param>
        internal override void CopyRequest(ErrorCode status, int index)
        {
            Response = new ResponseMessage(
                Request.RequestId(),
                Request.Version,
                Request.Parameters.UserName,
                status,
                index,
                Request.Pdu().Variables);
            if (TooBig)
            {
                GenerateTooBig();
            }
        }

        /// <summary>
        /// Generates too big message.
        /// </summary>
        public override void GenerateTooBig()
        {
            Response = new ResponseMessage(
                Request.RequestId(),
                Request.Version,
                Request.Parameters.UserName,
                ErrorCode.TooBig,
                0,
                Request.Pdu().Variables);
        }

        /// <summary>
        /// Handles the membership.
        /// </summary>
        /// <returns>Always returns <code>false</code>.</returns>
        public override bool HandleMembership()
        {
            return false;
        }

        /// <summary>
        /// Generates the response.
        /// </summary>
        /// <param name="variables">The variables.</param>
        internal override void GenerateResponse(IList<Variable> variables)
        {
            Response = new ResponseMessage(
                Request.RequestId(),
                Request.Version,
                Request.Parameters.UserName,
                ErrorCode.NoError,
                0,
                variables);
            if (TooBig)
            {
                GenerateTooBig();
            }
        }
    }
}