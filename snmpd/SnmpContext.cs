using System;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SNMP context.
    /// </summary>
    internal abstract class SnmpContext
    {
        /// <summary>
        /// Max response size.
        /// </summary>
        protected const int MaxResponseSize = 1500;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnmpContext"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="users">The users.</param>
        /// <param name="objects">The agent core objects.</param>
        /// <param name="binding">The binding.</param>
        protected SnmpContext(ISnmpMessage request, IPEndPoint sender, UserRegistry users, AgentObjects objects, ListenerBinding binding)
        {
            Request = request;
            Binding = binding;
            Users = users;
            Sender = sender;
            CreatedTime = DateTime.Now;
            Objects = objects;
        }

        /// <summary>
        /// Gets or sets the binding.
        /// </summary>
        /// <value>The binding.</value>
        public ListenerBinding Binding { get; private set; }

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
        public UserRegistry Users { get; private set; }

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
        protected AgentObjects Objects { get; private set; }

        /// <summary>
        /// Sends out response message.
        /// </summary>
        public void SendResponse()
        {
            if (Response == null)
            {
                return;
            }

            AuthenticateMessage();
            Binding.SendResponse(Response, Sender);
        }

        protected abstract void AuthenticateMessage();

        internal abstract void HandleAuthenticationFailure();

        internal abstract void GenerateResponse(ResponseData data);

        public abstract bool HandleMembership();
    }
}