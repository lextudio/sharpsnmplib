using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Lextm.SharpSnmpLib.Security;
using System.IO;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Helper class.
    /// </summary>
    public static class Helper
    {
        private static readonly Socket udp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private static Socket udpV6;
        private const string STR_IPV6NotSupport = "the OS does not support it";
        private static object root = new object();

        /// <summary>
        /// Authenticates this message.
        /// </summary>
        public static void Authenticate(ISnmpMessage message, ProviderPair pair)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            
            if (pair == null)
            {
                throw new ArgumentNullException("pair");
            }
            
            message.Parameters.AuthenticationParameters = pair.Authentication.ComputeHash(message);
        }

        internal static Sequence PackMessage(VersionCode version, IPrivacyProvider privacy, ISegment header, SecurityParameters parameters, ISegment scope)
        {
            List<ISnmpData> collection = new List<ISnmpData>(4);
            collection.Add(new Integer32((int)version));
            collection.Add(header.GetData(version));
            collection.Add(parameters.GetData(version));
            collection.Add(privacy.Encrypt(scope.GetData(version), parameters));
            return new Sequence(collection);
        }

        /// <summary>
        /// Packs the message.
        /// </summary>
        /// <param name="version">The protocol version.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static Sequence PackMessage(VersionCode version, params ISnmpData[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            
            List<ISnmpData> collection = new List<ISnmpData>(1 + data.Length);
            collection.Add(new Integer32((int)version));
            collection.AddRange(data);
            return new Sequence(collection);
        }
        /// <summary>
        /// Gets the socket.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns></returns>
        public static Socket GetSocket(EndPoint endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }
            
            if (endpoint.AddressFamily == AddressFamily.InterNetwork)
            {
                return udp;
            }
            else
            {
                return UdpV6;
            }
        }

        private static Socket UdpV6
        {
            get
            {
#if !(CF)
                if (!Socket.OSSupportsIPv6)
                {
                    throw new InvalidOperationException(STR_IPV6NotSupport);
                }
#endif
                if (udpV6 == null)
                {
                    lock (root)
                    {
                        if (udpV6 == null)
                        {
                            udpV6 = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
                        }
                    }
                }

                return udpV6;
            }
        }
    }
}
