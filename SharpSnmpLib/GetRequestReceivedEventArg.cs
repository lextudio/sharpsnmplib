using System;
using System.Net;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Provides data for a GET request received event.
    /// </summary>
    [Obsolete("Use MessageReceivedEventArgs instead.")]
    public sealed class GetRequestReceivedEventArgs : EventArgs
    {
        private readonly GetRequestMessage _request;
        private readonly IPEndPoint _sender;

        /// <summary>
        /// Creates a <see cref="GetRequestReceivedEventArgs"/>.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="request">GET request message.</param>
        public GetRequestReceivedEventArgs(IPEndPoint sender, GetRequestMessage request)
        {
            _sender = sender;
            _request = request;
        }

        /// <summary>
        /// GET request message.
        /// </summary>
        public GetRequestMessage Request
        {
            get
            {
                return _request;
            }
        }

        /// <summary>
        /// Sender.
        /// </summary>
        public IPEndPoint Sender
        {
            get { return _sender; }
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="GetRequestReceivedEventArgs"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "GET request received event args: GET request message: " + _request + "; sender: " + _sender;
        }
    }
}