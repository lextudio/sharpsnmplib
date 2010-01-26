using System;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SNMP context.
    /// </summary>
    internal class SnmpContext
    {
        private readonly ISnmpMessage _request;
        private readonly DateTime _createdTime;
        private readonly IPEndPoint _sender;
        private ISnmpMessage _response;
        private readonly Listener _listener;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnmpContext"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="listener">The listener.</param>
        public SnmpContext(ISnmpMessage request, ISnmpMessage response, IPEndPoint sender, Listener listener)
        {
            _request = request;
            _listener = listener;
            _sender = sender;
            _response = response;
            _createdTime = DateTime.Now;
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
        }

        /// <summary>
        /// Gets the sender.
        /// </summary>
        /// <value>The sender.</value>
        public IPEndPoint Sender
        {
            get { return _sender; }
        }

        /// <summary>
        /// Responds the specified response.
        /// </summary>
        /// <param name="response">The response.</param>
        public void Respond(GetResponseMessage response)
        {
            _response = response;
            Listener.SendResponse(response, Sender);
        }
    }
}