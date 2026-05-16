using System.Net;
using System.Net.Sockets;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Extension methods for <see cref="EndPoint"/>.
    /// </summary>
    public static class EndPointExtension
    {
        /// <summary>
        /// Gets a UDP socket for the endpoint.
        /// </summary>
        public static Socket GetSocket(this EndPoint endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            return new Socket(endpoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
        }
    }
}
