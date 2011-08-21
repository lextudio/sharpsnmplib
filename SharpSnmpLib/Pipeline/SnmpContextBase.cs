// SNMP context class.
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
using System.Collections.Generic;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// SNMP context.
    /// </summary>
    internal abstract class SnmpContextBase : ISnmpContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SnmpContextBase"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="users">The users.</param>
        /// <param name="group">The engine core group.</param>
        /// <param name="binding">The binding.</param>
        protected SnmpContextBase(ISnmpMessage request, IPEndPoint sender, UserRegistry users, EngineGroup group, IListenerBinding binding)
        {
            Request = request;
            Binding = binding;
            Users = users;
            Sender = sender;
            CreatedTime = DateTime.Now;
            Group = group;
        }

        /// <summary>
        /// Gets or sets the binding.
        /// </summary>
        /// <value>The binding.</value>
        public IListenerBinding Binding { get; private set; }

        /// <summary>
        /// Gets the created time.
        /// </summary>
        /// <value>The created time.</value>
        public DateTime CreatedTime { get; private set; }

        /// <summary>
        /// Gets the request.
        /// </summary>
        /// <value>The request.</value>
        public ISnmpMessage Request { get; private set; }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <value>The users.</value>
        protected UserRegistry Users { get; private set; }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <value>The response.</value>
        public ISnmpMessage Response { get; protected set; }

        /// <summary>
        /// Gets the sender.
        /// </summary>
        /// <value>The sender.</value>
        public IPEndPoint Sender { get; private set; }

        /// <summary>
        /// Gets or sets the objects.
        /// </summary>
        /// <value>The objects.</value>
        protected EngineGroup Group { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [too big].
        /// </summary>
        /// <value><c>true</c> if the response is too big; otherwise, <c>false</c>.</value>
        public bool TooBig
        {
            get
            {
                var length = Response.ToBytes().Length;
                return length > Request.Header.MaxSize || length > Messenger.MaxMessageSize;
            }
        }

        /// <summary>
        /// Sends out response message.
        /// </summary>
        public void SendResponse()
        {
            if (Response == null)
            {
                return;
            }

            Binding.SendResponse(Response, Sender);
        }

        /// <summary>
        /// Handles the authentication failure.
        /// </summary>
        public abstract void HandleAuthenticationFailure();

        /// <summary>
        /// Generates the response.
        /// </summary>
        /// <param name="variables">The variables.</param>
        public abstract void GenerateResponse(IList<Variable> variables);

        /// <summary>
        /// Copies the request variable bindings to response.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="index">The index.</param>
        public abstract void CopyRequest(ErrorCode status, int index);

        /// <summary>
        /// Handles the membership authentication.
        /// </summary>
        /// <returns></returns>
        public abstract bool HandleMembership();

        /// <summary>
        /// Generates too big message.
        /// </summary>
        public abstract void GenerateTooBig();
    }
}