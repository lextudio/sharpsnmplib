using System;
using System.Net;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Provides data for a GET request received event.
    /// </summary>
    public sealed class GetRequestReceivedEventArgs : EventArgs
    {
        private GetRequestMessage _request;
        private IPEndPoint _agent;

        /// <summary>
        /// Creates a <see cref="GetRequestReceivedEventArgs"/> 
        /// </summary>
        /// <param name="agent">Agent</param>
        /// <param name="response">GET request message</param>
        public GetRequestReceivedEventArgs(IPEndPoint agent, GetRequestMessage request)
        {
            _agent = agent;
            _request = request;
        }

        /// <summary>
        /// Trap message.
        /// </summary>
        public GetRequestMessage Request
        {
            get
            {
                return _request;
            }
        }

        /// <summary>
        /// Agent.
        /// </summary>
        public IPEndPoint Agent
        {
            get { return _agent; }
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="GetRequestReceivedEventArgs"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Trap received event args: trap message: " + _request + "; agent: " + _agent;
        }
    }
}


