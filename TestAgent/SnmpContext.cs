using System;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;

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
            set { _response = value; }
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
        /// Sends out response message.
        /// </summary>
        public void SendResponse()
        {
            if (_response != null)
            {
                if (_response.Version == VersionCode.V3)
                {
                    ProviderPair providers = _listener.Users.Find(_request.Parameters.UserName);
                    providers = providers ?? ProviderPair.Default;
                    Helper.Authenticate(_response, providers);
                }

                Listener.SendResponse(_response, Sender);
            }
        }
    }
}