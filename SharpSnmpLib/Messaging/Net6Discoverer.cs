// Discoverer classes.
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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

#if NET6_0_OR_GREATER
namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Discoverer class to discover SNMP agents in the same network.
    /// </summary>
    public sealed partial class Discoverer
    {
        /// <summary>
        /// Discovers agents of the specified version in a specific time interval.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="broadcastAddress">The broadcast address.</param>
        /// <param name="community">The community.</param>
        /// <param name="token">The cancellation token.</param>
        /// <param name="contextName">The optional context name.</param>
        /// <remarks><paramref name="broadcastAddress"/> must be an IPv4 address. IPv6 is not yet supported here.</remarks>
        public void Discover(VersionCode version, IPEndPoint broadcastAddress, OctetString? community, CancellationToken token, OctetString? contextName = null)
        {
            if (broadcastAddress == null)
            {
                throw new ArgumentNullException(nameof(broadcastAddress));
            }

            if (version != VersionCode.V3 && community == null)
            {
                throw new ArgumentNullException(nameof(community));
            }

            var addressFamily = broadcastAddress.AddressFamily;
            if (addressFamily == AddressFamily.InterNetworkV6)
            {
                throw new ArgumentException("IP v6 is not yet supported.", nameof(broadcastAddress));
            }

            byte[] bytes;
            _requestId = Messenger.NextRequestId;
            if (version == VersionCode.V3)
            {
                var discovery = new Discovery(Messenger.NextMessageId, _requestId, Messenger.MaxMessageSize, contextName);
                bytes = discovery.ToBytes();
            }
            else
            {
                var message = new GetRequestMessage(_requestId, version, community!, _defaultVariables);
                bytes = message.ToBytes();
            }

            using var udp = new UdpClient(addressFamily);
            udp.EnableBroadcast = true;

            AsyncHelper.RunSync(async () => await udp.SendAsync(bytes, broadcastAddress, token));
            var activeBefore = Interlocked.CompareExchange(ref _active, Active, Inactive);
            if (activeBefore == Active)
            {
                // If already started, we've nothing to do.
                return;
            }

            udp.Client.ReceiveBufferSize = Messenger.MaxMessageSize;

#if ASYNC
            Task.Factory.StartNew(() => AsyncBeginReceive(udp.Client, token));
#else
            Task.Factory.StartNew(() => AsyncReceive(udp), token);
#endif
            token.WaitHandle.WaitOne();
            Interlocked.CompareExchange(ref _active, Inactive, Active);
        }

#if ASYNC
        private void AsyncBeginReceive(Socket socket, CancellationToken token)
        {
            while (true)
            {
                // If no more active, then stop.
                if (Interlocked.Exchange(ref _active, _active) == Inactive)
                {
                    return;
                }

                if (token.IsCancellationRequested)
                {
                    return;
                }

                byte[] buffer = new byte[_bufferSize];
                EndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                try
                {
                    var iar = socket.BeginReceiveFrom(buffer, 0, _bufferSize, SocketFlags.None, ref remote, AsyncEndReceive, new Tuple<Socket, byte[]>(socket, buffer));
                    WaitHandle.WaitAny(new []{iar.AsyncWaitHandle, token.WaitHandle});
                }
                catch (SocketException ex)
                {
                    // ignore WSAECONNRESET, http://bytes.com/topic/c-sharp/answers/237558-strange-udp-socket-problem
                    if (ex.SocketErrorCode != SocketError.ConnectionReset)
                    {
                        // If the SnmpTrapListener was active, marks it as stopped and call HandleException.
                        // If it was inactive, the exception is likely to result from this, and we raise nothing.
                        long activeBefore = Interlocked.CompareExchange(ref _active, Inactive, Active);
                        if (activeBefore == Active)
                        {
                            HandleException(ex);
                        }
                    }
                }
            }
        }
#endif

        /// <summary>
        /// Discovers agents of the specified version in a specific time interval.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="broadcastAddress">The broadcast address.</param>
        /// <param name="community">The community.</param>
        /// <param name="token">The cancellation token.</param>
        /// <remarks><paramref name="broadcastAddress"/> must be an IPv4 address. IPv6 is not yet supported here.</remarks>
        public async Task DiscoverAsync(VersionCode version, IPEndPoint broadcastAddress, OctetString community, CancellationToken token, OctetString? contextName = null)
        {
            if (broadcastAddress == null)
            {
                throw new ArgumentNullException(nameof(broadcastAddress));
            }

            if (version != VersionCode.V3 && community == null)
            {
                throw new ArgumentNullException(nameof(community));
            }

            var addressFamily = broadcastAddress.AddressFamily;
            byte[] bytes;
            _requestId = Messenger.NextRequestId;
            if (version == VersionCode.V3)
            {
                var discovery = new Discovery(Messenger.NextMessageId, _requestId, Messenger.MaxMessageSize, contextName);
                bytes = discovery.ToBytes();
            }
            else
            {
                var message = new GetRequestMessage(_requestId, version, community, _defaultVariables);
                bytes = message.ToBytes();
            }

            using var udp = new UdpClient(addressFamily);
            if (addressFamily == AddressFamily.InterNetworkV6)
            {
                udp.MulticastLoopback = false;
            }
            else if (addressFamily == AddressFamily.InterNetwork)
            {
                udp.EnableBroadcast = true;
            }

            await udp.Client.SendToAsync(bytes, SocketFlags.None, broadcastAddress, token);
            var activeBefore = Interlocked.CompareExchange(ref _active, Active, Inactive);
            if (activeBefore == Active)
            {
                // If already started, we've nothing to do.
                return;
            }

            await ReceiveAsync(udp.Client, token);

            Interlocked.CompareExchange(ref _active, Inactive, Active);
            udp.Close();
        }

        private async Task ReceiveAsync(Socket socket, CancellationToken token)
        {
            while (true)
            {
                // If no more active, then stop.
                if (Interlocked.Exchange(ref _active, _active) == Inactive)
                {
                    return;
                }

                if (token.IsCancellationRequested)
                {
                    return;
                }

                try
                {
                    EndPoint remote = new IPEndPoint(socket.AddressFamily == AddressFamily.InterNetwork ? IPAddress.Any : IPAddress.IPv6Any, 0);

                    var buffer = new byte[socket.ReceiveBufferSize];
                    var result = await socket.ReceiveFromAsync(new ArraySegment<byte>(buffer), SocketFlags.None, remote, token);
                    await Task.Factory.StartNew(() => HandleMessage(buffer, result.ReceivedBytes, (IPEndPoint)result.RemoteEndPoint), token)
                        .ConfigureAwait(false);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.ConnectionReset)
                    {
                        continue;
                    }

                    // If the SnmpTrapListener was active, marks it as stopped and call HandleException.
                    // If it was inactive, the exception is likely to result from this, and we raise nothing.
                    var activeBefore = Interlocked.CompareExchange(ref _active, Inactive, Active);
                    if (activeBefore == Active)
                    {
                        HandleException(ex);
                    }
                }
            }
        }
    }
}
#endif
