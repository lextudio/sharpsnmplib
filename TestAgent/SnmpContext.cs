using System;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Agent
{
    internal class SnmpContext
    {
        private readonly ISnmpMessage _request;
        private readonly DateTime _createdTime;
        private readonly IPEndPoint _sender;
        private ISnmpMessage _response;
        private readonly Listener _listener;

        public SnmpContext(ISnmpMessage request, ISnmpMessage response, IPEndPoint sender, Listener listener)
        {
            _request = request;
            _listener = listener;
            _sender = sender;
            _response = response;
            _createdTime = DateTime.Now;
        }

        public DateTime CreatedTime
        {
            get { return _createdTime; }
        }

        public ISnmpMessage Request
        {
            get { return _request; }
        }

        public Listener Listener
        {
            get { return _listener; }
        }

        public ISnmpMessage Response
        {
            get { return _response; }
        }

        public IPEndPoint Sender
        {
            get { return _sender; }
        }

        public void Respond(GetResponseMessage response)
        {
            _response = response;
            Listener.SendResponse(response, Sender);
        }
    }
}