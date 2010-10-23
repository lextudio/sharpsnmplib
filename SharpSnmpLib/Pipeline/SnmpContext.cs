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
    public abstract class SnmpContext
    {
        /// <summary>
        /// Max response size.
        /// </summary>
        protected const int MaxResponseSize = Messenger.MaxMessageSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnmpContext"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="users">The users.</param>
        /// <param name="group">The engine core group.</param>
        /// <param name="binding">The binding.</param>
        protected SnmpContext(ISnmpMessage request, IPEndPoint sender, UserRegistry users, EngineGroup group, IListenerBinding binding)
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
        protected UserRegistry Users { get; set; }

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
        /// Sends out response message.
        /// </summary>
        public void SendResponse()
        {
            if (Response == null)
            {
                return;
            }

            AuthenticateResponse();
            Binding.SendResponse(Response, Sender);
        }

        /// <summary>
        /// Authenticates the message.
        /// </summary>
        protected abstract void AuthenticateResponse();

        /// <summary>
        /// Handles the authentication failure.
        /// </summary>
        internal abstract void HandleAuthenticationFailure();

        /// <summary>
        /// Generates the response.
        /// </summary>
        /// <param name="variables">The variables.</param>
        internal abstract void GenerateResponse(IList<Variable> variables);

        /// <summary>
        /// Copies the request variable bindings to response.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="index">The index.</param>
        internal abstract void CopyRequest(ErrorCode status, int index);

        /// <summary>
        /// Handles the membership authentication.
        /// </summary>
        /// <returns></returns>
        public abstract bool HandleMembership();
    }
}