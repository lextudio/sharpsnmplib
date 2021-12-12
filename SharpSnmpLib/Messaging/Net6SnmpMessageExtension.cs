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
#if NETSTANDARD2_0
using System.Runtime.InteropServices;
#endif
using System.Threading;
using System.Threading.Tasks;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
#if NET6_0
    /// <summary>
    /// Extension methods for <see cref="ISnmpMessage"/>.
    /// </summary>
    public static partial class SnmpMessageExtension
    {
        #region async methods

        /// <summary>
        /// Sends an <see cref="ISnmpMessage"/>.
        /// </summary>
        /// <param name="message">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="manager">Manager</param>
        /// <param name="token">The cancellation token.</param>
        public static async Task SendAsync(this ISnmpMessage message, EndPoint manager, CancellationToken token)
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

            using var socket = manager.GetSocket();
            await message.SendAsync(manager, socket, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends an <see cref="ISnmpMessage"/>.
        /// </summary>
        /// <param name="message">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="manager">Manager</param>
        /// <param name="socket">The socket.</param>
        /// <param name="token">The cancellation token.</param>
        public static async Task SendAsync(this ISnmpMessage message, EndPoint manager, Socket socket, CancellationToken token)
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

            var buffer = new ArraySegment<byte>(message.ToBytes());
            await socket.SendToAsync(buffer, SocketFlags.None, manager, token);
        }

        /// <summary>
        /// Sends this <see cref="ISnmpMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="request">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="receiver">Port number.</param>
        /// <param name="registry">User registry.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<ISnmpMessage> GetResponseAsync(this ISnmpMessage request, IPEndPoint receiver, UserRegistry registry, CancellationToken token)
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

            using var socket = receiver.GetSocket();
            return await request.GetResponseAsync(receiver, registry, socket, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends this <see cref="ISnmpMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="request">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="receiver">Port number.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<ISnmpMessage> GetResponseAsync(this ISnmpMessage request, IPEndPoint receiver, CancellationToken token)
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

            using var socket = receiver.GetSocket();
            return await request.GetResponseAsync(receiver, socket, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends this <see cref="ISnmpMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="request">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="receiver">Agent.</param>
        /// <param name="udpSocket">The UDP <see cref="Socket"/> to use to send/receive.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<ISnmpMessage> GetResponseAsync(this ISnmpMessage request, IPEndPoint receiver, Socket udpSocket, CancellationToken token)
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

            return await request.GetResponseAsync(receiver, registry, udpSocket, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends an <see cref="ISnmpMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="request">The <see cref="ISnmpMessage"/>.</param>
        /// <param name="receiver">Agent.</param>
        /// <param name="udpSocket">The UDP <see cref="Socket"/> to use to send/receive.</param>
        /// <param name="registry">The user registry.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<ISnmpMessage> GetResponseAsync(this ISnmpMessage request, IPEndPoint receiver, UserRegistry registry, Socket udpSocket, CancellationToken token)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (udpSocket == null)
            {
                throw new ArgumentNullException(nameof(udpSocket));
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
            var buffer = new ArraySegment<byte>(bytes);
            await udpSocket.SendToAsync(buffer, SocketFlags.None, receiver ?? throw new ArgumentNullException(nameof(receiver)), token);

            int count;
            byte[] reply = new byte[bufSize];

            // IMPORTANT: follow http://blogs.msdn.com/b/pfxteam/archive/2011/12/15/10248293.aspx
            var remoteAddress = udpSocket.AddressFamily == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Any : IPAddress.Any;
            EndPoint remote = new IPEndPoint(remoteAddress, 0);

            try
            {
                var result = await udpSocket.ReceiveMessageFromAsync(new ArraySegment<byte>(reply), SocketFlags.None, remote, token);
                count = result.ReceivedBytes;
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

    }
#endif
}
