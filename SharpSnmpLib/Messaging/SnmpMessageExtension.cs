// SNMP message extension class.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
#if NETSTANDARD1_3
using System.Runtime.InteropServices;
#endif
using System.Threading;
using System.Threading.Tasks;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Extension methods for <see cref="ISnmpMessage"/>.
    /// </summary>
    public static class SnmpMessageExtension
    {
        /// <summary>
        /// Gets the <see cref="SnmpType"/>.
        /// </summary>
        /// <param name="message">The <see cref="ISnmpMessage"/>.</param>
        /// <returns></returns>
        public static SnmpType TypeCode(this ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return message.Pdu().TypeCode;
        }

        /// <summary>
        /// Variables.
        /// </summary>
        /// <param name="message">The <see cref="ISnmpMessage"/>.</param>
        public static IList<Variable> Variables(this ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var code = message.TypeCode();
            return code == SnmpType.Unknown ? new List<Variable>(0) : message.Scope.Pdu.Variables;
        }

        /// <summary>
        /// Request ID.
        /// </summary>
        /// <param name="message">The <see cref="ISnmpMessage"/>.</param>
        public static int RequestId(this ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return message.Scope.Pdu.RequestId.ToInt32();
        }

        /// <summary>
        /// Gets the message ID.
        /// </summary>
        /// <value>The message ID.</value>
        /// <param name="message">The <see cref="ISnmpMessage"/>.</param>
        /// <remarks>For v3, message ID is different from request ID. For v1 and v2c, they are the same.</remarks>
        public static int MessageId(this ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return message.Header == Header.Empty ? message.RequestId() : message.Header.MessageId;
        }

        /// <summary>
        /// PDU.
        /// </summary>
        /// <param name="message">The <see cref="ISnmpMessage"/>.</param>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pdu")]
        public static ISnmpPdu Pdu(this ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return message.Scope.Pdu;
        }

        /// <summary>
        /// Community name.
        /// </summary>
        /// <param name="message">The <see cref="ISnmpMessage"/>.</param>
        public static OctetString Community(this ISnmpMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return message.Parameters.UserName;
        }

#region sync methods

        /// <summary>
        /// Sends an <see cref="ISnmpMessage"/>.
        /// </summary>
        /// <param name="message">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="manager">Manager</param>
        public static void Send(this ISnmpMessage message, EndPoint manager)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            var code = message.TypeCode();
            if ((code != SnmpType.TrapV1Pdu && code != SnmpType.TrapV2Pdu) && code != SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.InvariantCulture,
                    "not a trap message: {0}",
                    code));
            }

            using (var socket = manager.GetSocket())
            {
                message.Send(manager, socket);
            }
        }

        /// <summary>
        /// Sends an <see cref="ISnmpMessage"/>.
        /// </summary>
        /// <param name="message">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="manager">Manager</param>
        /// <param name="socket">The socket.</param>
        public static void Send(this ISnmpMessage message, EndPoint manager, Socket socket)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (socket == null)
            {
                throw new ArgumentNullException(nameof(socket));
            }

            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            var code = message.TypeCode();
            if ((code != SnmpType.TrapV1Pdu && code != SnmpType.TrapV2Pdu) && code != SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.InvariantCulture,
                    "not a trap message: {0}",
                    code));
            }

            var bytes = message.ToBytes();
            socket.SendTo(bytes, 0, bytes.Length, SocketFlags.None, manager);
        }

        /// <summary>
        /// Sends this <see cref="ISnmpMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="request">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="timeout">The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</param>
        /// <param name="receiver">Port number.</param>
        /// <param name="registry">User registry.</param>
        /// <returns></returns>
        public static ISnmpMessage GetResponse(this ISnmpMessage request, int timeout, IPEndPoint receiver, UserRegistry registry)
        {
            // TODO: make more usage of UserRegistry.
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            var code = request.TypeCode();
            if (code == SnmpType.TrapV1Pdu || code == SnmpType.TrapV2Pdu || code == SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "not a request message: {0}", code));
            }

            using (var socket = receiver.GetSocket())
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
                throw new ArgumentNullException(nameof(request));
            }

            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            var code = request.TypeCode();
            if (code == SnmpType.TrapV1Pdu || code == SnmpType.TrapV2Pdu || code == SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "not a request message: {0}", code));
            }

            using (var socket = receiver.GetSocket())
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
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            if (udpSocket == null)
            {
                throw new ArgumentNullException(nameof(udpSocket));
            }

            var registry = new UserRegistry();
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
                throw new ArgumentNullException(nameof(request));
            }

            if (udpSocket == null)
            {
                throw new ArgumentNullException(nameof(udpSocket));
            }

            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            var requestCode = request.TypeCode();
            if (requestCode == SnmpType.TrapV1Pdu || requestCode == SnmpType.TrapV2Pdu || requestCode == SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "not a request message: {0}", requestCode));
            }

            var bytes = request.ToBytes();
            var bufSize = udpSocket.ReceiveBufferSize = Messenger.MaxMessageSize;
            var reply = new byte[bufSize];

            // Whatever you change, try to keep the Send and the Receive close to each other.
            udpSocket.SendTo(bytes, receiver);
            udpSocket.ReceiveTimeout = timeout;
            int count;
            try
            {
                count = udpSocket.Receive(reply, 0, bufSize, SocketFlags.None);
            }
            catch (SocketException ex)
            {
                // IMPORTANT: Mono behavior.
                if (IsRunningOnMono && ex.SocketErrorCode == SocketError.WouldBlock)
                {
                    throw TimeoutException.Create(receiver.Address, timeout);
                }


                if (ex.SocketErrorCode == SocketError.TimedOut)
                {
                    throw TimeoutException.Create(receiver.Address, timeout);
                }

                throw;
            }

            // Passing 'count' is not necessary because ParseMessages should ignore it, but it offer extra safety (and would avoid an issue if parsing >1 response).
            var response = MessageFactory.ParseMessages(reply, 0, count, registry)[0];
            var responseCode = response.TypeCode();
            if (responseCode == SnmpType.ResponsePdu || responseCode == SnmpType.ReportPdu)
            {
                var requestId = request.MessageId();
                var responseId = response.MessageId();
                if (responseId != requestId)
                {
                    throw OperationException.Create(string.Format(CultureInfo.InvariantCulture, "wrong response sequence: expected {0}, received {1}", requestId, responseId), receiver.Address);
                }

                return response;
            }

            throw OperationException.Create(string.Format(CultureInfo.InvariantCulture, "wrong response type: {0}", responseCode), receiver.Address);
        }

#endregion

#region async methods
#if NET452
        /// <summary>
        /// Ends a pending asynchronous read.
        /// </summary>
        /// <param name="request">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="asyncResult">An <see cref="IAsyncResult"/> that stores state information and any user defined data for this asynchronous operation.</param>
        /// <returns></returns>
        [Obsolete("Please use GetResponseAsync and await on it.")]
        public static ISnmpMessage EndGetResponse(this ISnmpMessage request, IAsyncResult asyncResult)
        {
            if (asyncResult == null)
            {
                throw new ArgumentNullException(nameof(asyncResult));
            }
            
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            
            var ar = (SnmpMessageAsyncResult)asyncResult;
            var s = ar.WorkSocket;
            var count = s.EndReceive(ar.Inner);
            
            // Passing 'count' is not necessary because ParseMessages should ignore it, but it offer extra safety (and would avoid an issue if parsing >1 response).
            var response = MessageFactory.ParseMessages(ar.GetBuffer(), 0, count, ar.Users)[0];
            var responseCode = response.TypeCode();
            if (responseCode == SnmpType.ResponsePdu || responseCode == SnmpType.ReportPdu)
            {
                var requestId = request.MessageId();
                var responseId = response.MessageId();
                if (responseId != requestId)
                {
                    throw OperationException.Create(string.Format(CultureInfo.InvariantCulture, "wrong response sequence: expected {0}, received {1}", requestId, responseId), ar.Receiver.Address);
                }

                return response;
            }

            throw OperationException.Create(string.Format(CultureInfo.InvariantCulture, "wrong response type: {0}", responseCode), ar.Receiver.Address);
        }

        /// <summary>
        /// Begins to asynchronously send an <see cref="ISnmpMessage"/> to an <see cref="IPEndPoint"/>.
        /// </summary>
        /// <param name="request">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="receiver">Agent.</param>
        /// <param name="registry">The user registry.</param>
        /// <param name="udpSocket">The UDP <see cref="Socket"/> to use to send/receive.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="state">The state object.</param>
        /// <returns></returns>
        [Obsolete("Please use GetResponseAsync and await on it.")]
        public static IAsyncResult BeginGetResponse(this ISnmpMessage request, IPEndPoint receiver, UserRegistry registry, Socket udpSocket, AsyncCallback callback, object state)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (udpSocket == null)
            {
                throw new ArgumentNullException(nameof(udpSocket));
            }

            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            var requestCode = request.TypeCode();
            if (requestCode == SnmpType.TrapV1Pdu || requestCode == SnmpType.TrapV2Pdu || requestCode == SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "not a request message: {0}", requestCode));
            }

            // Whatever you change, try to keep the Send and the Receive close to each other.
            udpSocket.SendTo(request.ToBytes(), receiver);
            var bufferSize = udpSocket.ReceiveBufferSize = Messenger.MaxMessageSize;
            var buffer = new byte[bufferSize];

            // https://sharpsnmplib.codeplex.com/workitem/7234
            if (callback != null)
            {
                AsyncCallback wrapped = callback;
                callback = asyncResult =>
                {
                    var result = new SnmpMessageAsyncResult(asyncResult, udpSocket, registry, receiver, buffer);
                    wrapped(result);
                };
            }

            var ar = udpSocket.BeginReceive(buffer, 0, bufferSize, SocketFlags.None, callback, state);
            return new SnmpMessageAsyncResult(ar, udpSocket, registry, receiver, buffer);
        }
#endif
        /// <summary>
        /// Sends an <see cref="ISnmpMessage"/>.
        /// </summary>
        /// <param name="message">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="manager">Manager</param>
        public static async Task SendAsync(this ISnmpMessage message, EndPoint manager)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            var code = message.TypeCode();
            if ((code != SnmpType.TrapV1Pdu && code != SnmpType.TrapV2Pdu) && code != SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.InvariantCulture,
                    "not a trap message: {0}",
                    code));
            }

            using (var socket = manager.GetSocket())
            {
                await message.SendAsync(manager, socket).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Sends an <see cref="ISnmpMessage"/>.
        /// </summary>
        /// <param name="message">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="manager">Manager</param>
        /// <param name="socket">The socket.</param>
        public static async Task SendAsync(this ISnmpMessage message, EndPoint manager, Socket socket)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (socket == null)
            {
                throw new ArgumentNullException(nameof(socket));
            }

            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            var code = message.TypeCode();
            if ((code != SnmpType.TrapV1Pdu && code != SnmpType.TrapV2Pdu) && code != SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.InvariantCulture,
                    "not a trap message: {0}",
                    code));
            }

            var bytes = message.ToBytes();
            var info = SocketExtension.EventArgsFactory.Create();
            info.RemoteEndPoint = manager;
            info.SetBuffer(bytes, 0, bytes.Length);
            using (var awaitable1 = new SocketAwaitable(info))
            {
                await socket.SendToAsync(awaitable1);
            }
        }

        /// <summary>
        /// Sends this <see cref="ISnmpMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="request">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="receiver">Port number.</param>
        /// <param name="registry">User registry.</param>
        /// <returns></returns>
        public static async Task<ISnmpMessage> GetResponseAsync(this ISnmpMessage request, IPEndPoint receiver, UserRegistry registry)
        {
            // TODO: make more usage of UserRegistry.
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            var code = request.TypeCode();
            if (code == SnmpType.TrapV1Pdu || code == SnmpType.TrapV2Pdu || code == SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "not a request message: {0}", code));
            }

            using (var socket = receiver.GetSocket())
            {
                return await request.GetResponseAsync(receiver, registry, socket).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Sends this <see cref="ISnmpMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="request">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="receiver">Port number.</param>
        /// <returns></returns>
        public static async Task<ISnmpMessage> GetResponseAsync(this ISnmpMessage request, IPEndPoint receiver)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            var code = request.TypeCode();
            if (code == SnmpType.TrapV1Pdu || code == SnmpType.TrapV2Pdu || code == SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "not a request message: {0}", code));
            }

            using (var socket = receiver.GetSocket())
            {
                return await request.GetResponseAsync(receiver, socket).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Sends this <see cref="ISnmpMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="request">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="receiver">Agent.</param>
        /// <param name="udpSocket">The UDP <see cref="Socket"/> to use to send/receive.</param>
        /// <returns></returns>
        public static async Task<ISnmpMessage> GetResponseAsync(this ISnmpMessage request, IPEndPoint receiver, Socket udpSocket)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            if (udpSocket == null)
            {
                throw new ArgumentNullException(nameof(udpSocket));
            }

            var registry = new UserRegistry();
            if (request.Version == VersionCode.V3)
            {
                registry.Add(request.Parameters.UserName, request.Privacy);
            }

            return await request.GetResponseAsync(receiver, registry, udpSocket).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends an <see cref="ISnmpMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="request">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="receiver">Agent.</param>
        /// <param name="udpSocket">The UDP <see cref="Socket"/> to use to send/receive.</param>
        /// <param name="registry">The user registry.</param>
        /// <returns></returns>
        public static async Task<ISnmpMessage> GetResponseAsync(this ISnmpMessage request, IPEndPoint receiver, UserRegistry registry, Socket udpSocket)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (udpSocket == null)
            {
                throw new ArgumentNullException(nameof(udpSocket));
            }

            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            var requestCode = request.TypeCode();
            if (requestCode == SnmpType.TrapV1Pdu || requestCode == SnmpType.TrapV2Pdu || requestCode == SnmpType.ReportPdu)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "not a request message: {0}", requestCode));
            }

            var bytes = request.ToBytes();
            var bufSize = udpSocket.ReceiveBufferSize = Messenger.MaxMessageSize;

            // Whatever you change, try to keep the Send and the Receive close to each other.
            var info = SocketExtension.EventArgsFactory.Create();
            info.RemoteEndPoint = receiver;
            info.SetBuffer(bytes, 0, bytes.Length);
            using (var awaitable1 = new SocketAwaitable(info))
            {
                await udpSocket.SendToAsync(awaitable1);
            }

            int count;
            var reply = new byte[bufSize];

            // IMPORTANT: follow http://blogs.msdn.com/b/pfxteam/archive/2011/12/15/10248293.aspx
            var args = SocketExtension.EventArgsFactory.Create();
            EndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                args.RemoteEndPoint = remote;
                args.SetBuffer(reply, 0, bufSize);
                using (var awaitable = new SocketAwaitable(args))
                {
                    count = await udpSocket.ReceiveAsync(awaitable);
                }
            }
            catch (SocketException ex)
            {
                // IMPORTANT: Mono behavior (https://bugzilla.novell.com/show_bug.cgi?id=599488)
                if (IsRunningOnMono && ex.SocketErrorCode == SocketError.WouldBlock)
                {
                    throw TimeoutException.Create(receiver.Address, 0);
                }


                if (ex.SocketErrorCode == SocketError.TimedOut)
                {
                    throw TimeoutException.Create(receiver.Address, 0);
                }

                throw;
            }

            // Passing 'count' is not necessary because ParseMessages should ignore it, but it offer extra safety (and would avoid an issue if parsing >1 response).
            var response = MessageFactory.ParseMessages(reply, 0, count, registry)[0];
            var responseCode = response.TypeCode();
            if (responseCode == SnmpType.ResponsePdu || responseCode == SnmpType.ReportPdu)
            {
                var requestId = request.MessageId();
                var responseId = response.MessageId();
                if (responseId != requestId)
                {
                    throw OperationException.Create(string.Format(CultureInfo.InvariantCulture, "wrong response sequence: expected {0}, received {1}", requestId, responseId), receiver.Address);
                }

                return response;
            }

            throw OperationException.Create(string.Format(CultureInfo.InvariantCulture, "wrong response type: {0}", responseCode), receiver.Address);
        }

#endregion

        /// <summary>
        /// Tests if running on Mono.
        /// </summary>
        /// <returns></returns>
        public static bool IsRunningOnMono
        {
            get { return Type.GetType("Mono.Runtime") != null; }
        }

        /// <summary>
        /// Gets a value indicating whether it is
        /// running on Windows.
        /// </summary>
        /// <value><c>true</c> if is running on Windows; otherwise, <c>false</c>.</value>
        public static bool IsRunningOnWindows
        {
            get
            {
#if NET452
                return !IsRunningOnMono;
#elif NETSTANDARD1_3
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Gets a value indicating whether it is running on macOS.
        /// </summary>
        /// <value><c>true</c> if is running on macOS; otherwise, <c>false</c>.</value>
        public static bool IsRunningOnMac
        {
            get
            {
#if NET452
                return IsRunningOnMono;
#elif NETSTANDARD1_3
                return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Gets a value indicating whether it is running on iOS.
        /// </summary>
        /// <value><c>true</c> if is running on iOS; otherwise, <c>false</c>.</value>

        public static bool IsRunningOnIOS
        {
            get
            {
#if NET452
                return false;
#elif NETSTANDARD1_3
                return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#elif XAMARINIOS1_0
                return true;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Packs up the <see cref="ISnmpMessage"/>.
        /// </summary>
        /// <param name="message">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="length">The length bytes.</param>
        /// <returns></returns>
        internal static Sequence PackMessage(this ISnmpMessage message, byte[] length)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return ByteTool.PackMessage(
                length,
                message.Version,
                message.Header,
                message.Parameters,
                message.Privacy.GetScopeData(message.Header, message.Parameters, message.Scope.GetData(message.Version)));
        }

        private sealed class SnmpMessageAsyncResult : IAsyncResult
        {
            private readonly byte[] _buffer;

            public SnmpMessageAsyncResult(IAsyncResult inner, Socket socket, UserRegistry users, IPEndPoint receiver, byte[] buffer)
            {
                _buffer = buffer;
                WorkSocket = socket;
                Users = users;
                Receiver = receiver;
                Inner = inner;
            }

            public IAsyncResult Inner { get; private set; }

            public Socket WorkSocket { get; private set; }

            public UserRegistry Users { get; private set; }

            public byte[] GetBuffer()
            {
                return _buffer;
            }

            public IPEndPoint Receiver { get; private set; }

            public bool IsCompleted
            {
                get { return Inner.IsCompleted; }
            }

            public WaitHandle AsyncWaitHandle
            {
                get { return Inner.AsyncWaitHandle; }
            }

            public object AsyncState
            {
                get { return Inner.AsyncState; }
            }

            public bool CompletedSynchronously
            {
                get { return Inner.CompletedSynchronously; }
            }
        }
    }
}
