using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Listener binding interface.
    /// </summary>
    public interface IListenerBinding
    {
        /// <summary>
        /// Sends a response message.
        /// </summary>
        /// <param name="response">
        /// A <see cref="ISnmpMessage"/>.
        /// </param>
        /// <param name="receiver">Receiver.</param>
        void SendResponse(ISnmpMessage response, EndPoint receiver);

        /// <summary>
        /// Endpoint.
        /// </summary>
        IPEndPoint Endpoint { get; }
    }
}
