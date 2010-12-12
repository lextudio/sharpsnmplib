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
    /// Extension methods for <seealso cref="ISnmpMessage"/>.
    /// </summary>
    public static class SnmpMessageExtension
    {
        /// <summary>
        /// Gets the <seealso cref="SnmpType"/>.
        /// </summary>
        /// <param name="message">The <seealso cref="ISnmpMessage"/>.</param>
        /// <returns></returns>
        public static SnmpType TypeCode(this ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            
            return message.Pdu().TypeCode;
        }
        
        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <param name="message">The <seealso cref="ISnmpMessage"/>.</param>
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
        /// <param name="message">The <seealso cref="ISnmpMessage"/>.</param>
        public static IList<Variable> Variables(this ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var code = message.TypeCode();
            return code == SnmpType.Unknown ? new List<Variable>(0) : message.Scope.Pdu.Variables;
        }

        /// <summary>
        /// Request ID.
        /// </summary>
        /// <param name="message">The <seealso cref="ISnmpMessage"/>.</param>
        public static int RequestId(this ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            return message.Scope.Pdu.RequestId.ToInt32(); 
        }

        /// <summary>
        /// Gets the message ID.
        /// </summary>
        /// <value>The message ID.</value>
        /// <param name="message">The <seealso cref="ISnmpMessage"/>.</param>
        /// <remarks>For v3, message ID is different from request ID. For v1 and v2c, they are the same.</remarks>
        public static int MessageId(this ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            return message.Header == Header.Empty ? message.RequestId() : message.Header.MessageId;
        }

        /// <summary>
        /// PDU.
        /// </summary>
        /// <param name="message">The <seealso cref="ISnmpMessage"/>.</param>
        public static ISnmpPdu Pdu(this ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            return message.Scope.Pdu;
        }

        /// <summary>
        /// Community name.
        /// </summary>
        /// <param name="message">The <seealso cref="ISnmpMessage"/>.</param>
        public static OctetString Community(this ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            return message.Parameters.UserName;
        }

        /// <summary>
        /// Sends an <see cref="ISnmpMessage"/>.
        /// </summary>
        /// <param name="message">The <seealso cref="ISnmpMessage"/>.</param>
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

            var code = message.TypeCode();
            if ((code != SnmpType.TrapV1Pdu && code != SnmpType.TrapV2Pdu) && code != SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture,
                                                                  "not a trap message: {0}",
                                                                  code));
            }

            using (Socket socket = manager.GetSocket())
            {
                message.Send(manager, socket);
            }
        }

        /// <summary>
        /// Sends an <see cref="ISnmpMessage"/>.
        /// </summary>
        /// <param name="message">The <seealso cref="ISnmpMessage"/>.</param>
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

            var code = message.TypeCode();
            if ((code != SnmpType.TrapV1Pdu && code != SnmpType.TrapV2Pdu) && code != SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture,
                                                                  "not a trap message: {0}",
                                                                  code));
            }

            byte[] bytes = message.ToBytes();
            socket.SendTo(bytes, 0, bytes.Length, SocketFlags.None, manager);
        }

        /// <summary>
        /// Sends an <see cref="ISnmpMessage"/>.
        /// </summary>
        /// <param name="message">The <seealso cref="ISnmpMessage"/>.</param>
        /// <param name="manager">Manager</param>
        public static void SendAsync(this ISnmpMessage message, EndPoint manager)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            var code = message.TypeCode();
            if ((code != SnmpType.TrapV1Pdu && code != SnmpType.TrapV2Pdu) && code != SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture,
                                                                  "not a trap message: {0}",
                                                                  code));
            }

            using (Socket socket = manager.GetSocket())
            {
                message.SendAsync(manager, socket);
            }
        }
        
        /// <summary>
        /// Sends an <see cref="ISnmpMessage"/>.
        /// </summary>
        /// <param name="message">The <seealso cref="ISnmpMessage"/>.</param>
        /// <param name="manager">Manager</param>
        /// <param name="socket">The socket.</param>
        public static void SendAsync(this ISnmpMessage message, EndPoint manager, Socket socket)
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

            var code = message.TypeCode();
            if ((code != SnmpType.TrapV1Pdu && code != SnmpType.TrapV2Pdu) && code != SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture,
                                                                  "not a trap message: {0}",
                                                                  code));
            }

            byte[] bytes = message.ToBytes();
            socket.BeginSendTo(bytes, 0, bytes.Length, SocketFlags.None, manager, ar => socket.EndSendTo(ar), null);
        }
        
        /// <summary>
        /// Sends this <see cref="ISnmpMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="request">The <seealso cref="ISnmpMessage"/>.</param>
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

            var code = request.TypeCode();
            if (code == SnmpType.TrapV1Pdu || code == SnmpType.TrapV2Pdu || code == SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "not a request message: {0}", code));
            } 
  
            using (Socket socket = receiver.GetSocket())
            {
                return request.GetResponse(timeout, receiver, registry, socket);
            }
        }

        /// <summary>
        /// Sends this <see cref="ISnmpMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="request">The <see cref="ISnmpMessage"/>.</param>
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

            var code = request.TypeCode();
            if (code == SnmpType.TrapV1Pdu || code == SnmpType.TrapV2Pdu || code == SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "not a request message: {0}", code));
            } 
  
            using (Socket socket = receiver.GetSocket())
            {
                return request.GetResponse(timeout, receiver, socket);
            }
        }

        /// <summary>
        /// Sends this <see cref="ISnmpMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="request">The <see cref="ISnmpMessage"/>.</param>
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
        /// Sends an  <see cref="ISnmpMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="request">The <see cref="ISnmpMessage"/>.</param>
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

            var requestCode = request.TypeCode();
            if (requestCode == SnmpType.TrapV1Pdu || requestCode == SnmpType.TrapV2Pdu || requestCode == SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "not a request message: {0}", requestCode));
            }

            byte[] bytes = request.ToBytes();
#if CF
            int bufSize = 8192;
#else
            int bufSize = udpSocket.ReceiveBufferSize;
#endif
            byte[] reply = new byte[bufSize];

            // Whatever you change, try to keep the Send and the Receive close to each other.
            udpSocket.SendTo(bytes, receiver);
#if !(CF)
            udpSocket.ReceiveTimeout = timeout;
#endif
            int count;
            try
            {
                count = udpSocket.Receive(reply, 0, bufSize, SocketFlags.None);
            }
            catch (SocketException ex)
            {
                // FIXME: If you use a Mono build without the fix for this issue (https://bugzilla.novell.com/show_bug.cgi?id=599488), please uncomment this code.
                //if (SnmpMessageExtension.IsRunningOnMono && ex.ErrorCode == 10035)
                //{
                //    throw TimeoutException.Create(receiver.Address, timeout);
                //}

                if (ex.ErrorCode == WSAETIMEDOUT)
                {
                    throw TimeoutException.Create(receiver.Address, timeout);
                }

                throw;
            }

            // Passing 'count' is not necessary because ParseMessages should ignore it, but it offer extra safety (and would avoid an issue if parsing >1 response).
            ISnmpMessage response = MessageFactory.ParseMessages(reply, 0, count, registry)[0];
            var responseCode = response.TypeCode();
            if (responseCode == SnmpType.ResponsePdu || responseCode == SnmpType.ReportPdu)
            {
                int requestId = request.MessageId();
                var responseId = response.MessageId();
                if (responseId != requestId)
                {
                    throw OperationException.Create(String.Format(CultureInfo.InvariantCulture, "wrong response sequence: expected {0}, received {1}", requestId, responseId), receiver.Address);
                }

                return response;
            }

            throw OperationException.Create(String.Format(CultureInfo.InvariantCulture, "wrong response type: {0}", responseCode), receiver.Address);
        }  
                
        /// <summary>
        /// Ends a pending asynchronous read.
        /// </summary>
        /// <param name="request">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="asyncResult">An <seealso cref="IAsyncResult"/> that stores state information and any user defined data for this asynchronous operation.</param>
        /// <returns></returns>
        public static ISnmpMessage EndGetResponse(this ISnmpMessage request, IAsyncResult asyncResult)
        {
            if (asyncResult == null)
            {
                throw new ArgumentNullException("asyncResult");
            }
            
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            
            StateObject so = (StateObject)asyncResult.AsyncState;
            Socket s = so.WorkSocket;
            int count = s.EndReceive(asyncResult);
            // Passing 'count' is not necessary because ParseMessages should ignore it, but it offer extra safety (and would avoid an issue if parsing >1 response).
            ISnmpMessage response = MessageFactory.ParseMessages(so.Buffer, 0, count, so.Users)[0];
            var responseCode = response.TypeCode();
            if (responseCode == SnmpType.ResponsePdu || responseCode == SnmpType.ReportPdu)
            {
                int requestId = request.MessageId();
                var responseId = response.MessageId();
                if (responseId != requestId)
                {
                    throw OperationException.Create(String.Format(CultureInfo.InvariantCulture, "wrong response sequence: expected {0}, received {1}", requestId, responseId), so.Receiver.Address);
                }

                return response;
            }

            throw OperationException.Create(String.Format(CultureInfo.InvariantCulture, "wrong response type: {0}", responseCode), so.Receiver.Address);
        }

        /// <summary>
        /// Begins to asynchronously send an <see cref="ISnmpMessage"/> to an <seealso cref="IPEndPoint"/>.
        /// </summary>
        /// <param name="request">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="receiver">Agent.</param>
        /// <param name="registry">The user registry.</param>
        /// <param name="udpSocket">The UDP <see cref="Socket"/> to use to send/receive.</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public static IAsyncResult BeginGetResponse(this ISnmpMessage request, IPEndPoint receiver, UserRegistry registry, Socket udpSocket, AsyncCallback callback)
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

            var requestCode = request.TypeCode();
            if (requestCode == SnmpType.TrapV1Pdu || requestCode == SnmpType.TrapV2Pdu || requestCode == SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "not a request message: {0}", requestCode));
            }

            // Whatever you change, try to keep the Send and the Receive close to each other.
            StateObject state = new StateObject(udpSocket, registry, receiver);
            state.WorkSocket.SendTo(request.ToBytes(), receiver);
            return state.WorkSocket.BeginReceive(state.Buffer, 0, state.BufferSize, SocketFlags.None, callback, state);
        }

        /// <summary>
        /// Tests if runnning on Mono. 
        /// </summary>
        /// <returns></returns>
        public static bool IsRunningOnMono
        {
            get { return Type.GetType("Mono.Runtime") != null; }
        }

        /// <summary>
        /// Packs up the <see cref="ISnmpMessage"/>.
        /// </summary>
        /// <param name="message">The <see cref="ISnmpMessage"/>.</param>
        /// <returns></returns>
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

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/ms740668(VS.85).aspx
        /// </summary>
        private const int WSAETIMEDOUT = 10060;

        // State object for reading client data asynchronously
        private class StateObject
        {
            public Socket WorkSocket { get; private set; }
            public UserRegistry Users { get; private set; }
            public int BufferSize { get; private set; }
            public byte[] Buffer { get; private set; }
            public IPEndPoint Receiver { get; private set; }

            public StateObject(Socket socket, UserRegistry users, IPEndPoint receiver)
            {
#if CF
                BufferSize = 8192;
#else
                BufferSize = socket.ReceiveBufferSize;
#endif
                Buffer = new byte[BufferSize];
                WorkSocket = socket;
                Users = users;
                Receiver = receiver;
            }
        }
    }
}
