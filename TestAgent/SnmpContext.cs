using System;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SNMP context.
    /// </summary>
    internal abstract class SnmpContext
    {
        private readonly ISnmpMessage _request;
        private readonly DateTime _createdTime;
        private readonly IPEndPoint _sender;
        private ISnmpMessage _response;
        private readonly Listener _listener;
        private readonly AgentObjects _objects;
        protected const int MaxResponseSize = 1500;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnmpContext"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="listener">The listener.</param>
        /// <param name="objects">The agent core objects.</param>
        protected SnmpContext(ISnmpMessage request, IPEndPoint sender, Listener listener, AgentObjects objects)
        {
            _request = request;
            _listener = listener;
            _sender = sender;
            _createdTime = DateTime.Now;
            _objects = objects;
        }

        /// <summary>
        /// Gets the created time.
        /// </summary>
        /// <value>The created time.</value>
        public DateTime CreatedTime
        {
            get { return _createdTime; }
        }

        /// <summary>
        /// Gets the request.
        /// </summary>
        /// <value>The request.</value>
        public ISnmpMessage Request
        {
            get { return _request; }
        }

        /// <summary>
        /// Gets the listener.
        /// </summary>
        /// <value>The listener.</value>
        public Listener Listener
        {
            get { return _listener; }
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <value>The response.</value>
        public ISnmpMessage Response
        {
            get { return _response; }
            protected set { _response = value; }
        }

        /// <summary>
        /// Gets the sender.
        /// </summary>
        /// <value>The sender.</value>
        public IPEndPoint Sender
        {
            get { return _sender; }
        }

        protected AgentObjects Objects
        {
            get { return _objects; }
        }

        /// <summary>
        /// Sends out response message.
        /// </summary>
        public void SendResponse()
        {
            if (_response == null)
            {
                return;
            }

            AuthenticateMessage();
            Listener.SendResponse(_response, Sender);
        }

        protected abstract void AuthenticateMessage();

        internal abstract void HandleAuthenticationFailure();

        internal abstract void GenerateResponse(ResponseData data);

        public abstract bool HandleMembership();
    }
}