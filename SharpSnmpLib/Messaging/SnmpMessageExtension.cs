// Helper class.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// ISnmpMessage extension class.
    /// </summary>
    public static class SnmpMessageExtension
    {
        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <value>The level.</value>
        internal static Levels Level(this ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            
            if (message.Version != VersionCode.V3)
            {
                throw new ArgumentException("this method only applies to v3 messages", "message");
            }
            
            return message.Privacy.ToSecurityLevel(); 
        }
        
        /// <summary>
        /// Variables.
        /// </summary>
        public static IList<Variable> Variables(this ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var code = message.Pdu().TypeCode;
            if (code == SnmpType.Unknown)
            {
                return new List<Variable>(0); // malformed message.
            }
            
            return message.Scope.Pdu.Variables; 
        }

        /// <summary>
        /// Request ID.
        /// </summary>
        public static int RequestId(this ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var code = message.Pdu().TypeCode;
            if (code == SnmpType.Unknown)
            {
                return -1; // malformed message.
            }

            if (code == SnmpType.TrapV1Pdu)
            {
                throw new NotSupportedException();
            }

            return message.Scope.Pdu.RequestId.ToInt32(); 
        }

        /// <summary>
        /// Gets the message ID.
        /// </summary>
        /// <value>The message ID.</value>
        /// <remarks>For v3, message ID is different from request ID. For v1 and v2c, they are the same.</remarks>
        public static int MessageId(this ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var code = message.Pdu().TypeCode;
            if (code == SnmpType.TrapV1Pdu)
            {
                throw new NotSupportedException();
            }

            return message.Header == Header.Empty ? message.RequestId() : message.Header.MessageId;
        }

        /// <summary>
        /// PDU.
        /// </summary>
        public static ISnmpPdu Pdu(this ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var pdu = message.Scope.Pdu;
            if (pdu.TypeCode == SnmpType.Unknown)
            {
                return MalformedPdu.Instance; // malformed message.
            }

            return pdu; 
        }

        /// <summary>
        /// Community name.
        /// </summary>
        public static OctetString Community(this ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            return message.Parameters.UserName;
        }

        /// <summary>
        /// Sends this <see cref="TrapV1Message"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="manager">Manager</param>
        public static void Send(this ISnmpMessage message, EndPoint manager)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            var code = message.Pdu().TypeCode;
            if (code == SnmpType.TrapV1Pdu || code == SnmpType.TrapV2Pdu || code == SnmpType.ReportPdu)
            {
                using (Socket socket = manager.GetSocket())
                {
                    message.Send(manager, socket);
                }
            }
            else
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                                                                  "not a trap message: {0}", code));
            }
        }

        /// <summary>
        /// Sends this <see cref="TrapV1Message"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="manager">Manager</param>
        /// <param name="socket">The socket.</param>
        public static void Send(this ISnmpMessage message, EndPoint manager, Socket socket)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (socket == null)
            {
                throw new ArgumentNullException("socket");
            }

            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            var code = message.Pdu().TypeCode;
            if (code == SnmpType.TrapV1Pdu || code == SnmpType.TrapV2Pdu || code == SnmpType.ReportPdu)
            {
                byte[] bytes = message.ToBytes();
                socket.BeginSendTo(bytes, 0, bytes.Length, SocketFlags.None, manager, null, null);
            }
            else
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                                                                  "not a trap message: {0}", code));
            }
        }
        
        /// <summary>
        /// Sends this <see cref="GetNextRequestMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="timeout">The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</param>
        /// <param name="receiver">Port number.</param>
        /// <param name="registry">User registry.</param>
        /// <returns></returns>
        public static ISnmpMessage GetResponse(this ISnmpMessage request, int timeout, IPEndPoint receiver, UserRegistry registry)
        {
            // TODO: make more usage of UserRegistry.
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (receiver == null)
            {
                throw new ArgumentNullException("receiver");
            }

            var code = request.Pdu().TypeCode;
            if (code == SnmpType.TrapV1Pdu || code == SnmpType.TrapV2Pdu || code == SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "not a request message: {0}", code));
            } 
  
            using (Socket socket = receiver.GetSocket())
            {
                return request.GetResponse(timeout, receiver, registry, socket);
            }
        }

        /// <summary>
        /// Sends this <see cref="GetNextRequestMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="timeout">The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</param>
        /// <param name="receiver">Port number.</param>
        /// <returns></returns>
        public static ISnmpMessage GetResponse(this ISnmpMessage request, int timeout, IPEndPoint receiver)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (receiver == null)
            {
                throw new ArgumentNullException("receiver");
            }

            var code = request.Pdu().TypeCode;
            if (code == SnmpType.TrapV1Pdu || code == SnmpType.TrapV2Pdu || code == SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "not a request message: {0}", code));
            } 
  
            using (Socket socket = receiver.GetSocket())
            {
                return request.GetResponse(timeout, receiver, socket);
            }
        }

        /// <summary>
        /// Sends this <see cref="GetNextRequestMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="timeout">The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</param>
        /// <param name="receiver">Agent.</param>
        /// <param name="udpSocket">The UDP <see cref="Socket"/> to use to send/receive.</param>
        /// <returns></returns>
        public static ISnmpMessage GetResponse(this ISnmpMessage request, int timeout, IPEndPoint receiver, Socket udpSocket)
        {  
            UserRegistry registry = new UserRegistry();
            if (request.Version == VersionCode.V3)
            {
                registry.Add(request.Parameters.UserName, request.Privacy);
            }

            return request.GetResponse(timeout, receiver, registry, udpSocket);
        }

        /// <summary>
        /// Sends this <see cref="GetNextRequestMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="timeout">The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</param>
        /// <param name="receiver">Agent.</param>
        /// <param name="udpSocket">The UDP <see cref="Socket"/> to use to send/receive.</param>
        /// <param name="registry">The user registry.</param>
        /// <returns></returns>
        public static ISnmpMessage GetResponse(this ISnmpMessage request, int timeout, IPEndPoint receiver, UserRegistry registry, Socket udpSocket)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (udpSocket == null)
            {
                throw new ArgumentNullException("udpSocket");
            }

            if (receiver == null)
            {
                throw new ArgumentNullException("receiver");
            }
            
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            var code = request.Pdu().TypeCode;
            if (code == SnmpType.TrapV1Pdu || code == SnmpType.TrapV2Pdu || code == SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "not a request message: {0}", code));
            }  

            return MessageFactory.GetResponse(receiver, request.ToBytes(), request.MessageId(), timeout, registry, udpSocket);
        }
        
        /// <summary>
        /// Tests if runnning on Mono. 
        /// </summary>
        /// <returns></returns>
        public static bool IsRunningOnMono
        {
            get
            {
                return Type.GetType("Mono.Runtime") != null;
            }
        }

        internal static Sequence PackMessage(this ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            return PackMessage(message.Version,
                               message.Header,
                               message.Parameters,
                               message.Privacy.Encrypt(message.Scope.GetData(message.Version), message.Parameters));
        }

        /// <summary>
        /// Packs the message.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        internal static Sequence PackMessage(VersionCode version, params ISnmpData[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            
            List<ISnmpData> collection = new List<ISnmpData>(1 + data.Length) { new Integer32((int)version) };
            collection.AddRange(data);
            return new Sequence(collection);
        }

        internal static Sequence PackMessage(VersionCode version, ISegment header, SecurityParameters parameters, ISnmpData data)
        {
            if (header == null)
            {
                throw new ArgumentNullException("header");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            ISnmpData[] items = new[]
                                    {
                                        new Integer32((int)version),
                                        header.GetData(version),
                                        parameters.GetData(version),
                                        data
                                    };
            return new Sequence(items);
        }
    }
}
