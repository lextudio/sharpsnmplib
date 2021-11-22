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
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Lextm.SharpSnmpLib.Security;
using System.Threading.Tasks;

namespace Lextm.SharpSnmpLib.Messaging
{
    #if NET6_0
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
        /// <param name="interval">The discovering time interval, in milliseconds.</param>
        /// <remarks><paramref name="broadcastAddress"/> must be an IPv4 address. IPv6 is not yet supported here.</remarks>
        public void Discover(VersionCode version, IPEndPoint broadcastAddress, OctetString? community, int interval, CancellationToken token)
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
                // throw new NotSupportedException("SNMP v3 is not supported");
                var discovery = new Discovery(Messenger.NextMessageId, _requestId, Messenger.MaxMessageSize);
                bytes = discovery.ToBytes();
            }
            else
            {
                var message = new GetRequestMessage(_requestId, version, community!, _defaultVariables);
                bytes = message.ToBytes();
            }

            using (var udp = new UdpClient(addressFamily))
            {
                udp.EnableBroadcast = true;

                // TODO: is it safe?
                udp.SendAsync(bytes, broadcastAddress, token).GetAwaiter().GetResult();
                var activeBefore = Interlocked.CompareExchange(ref _active, Active, Inactive);
                if (activeBefore == Active)
                {
                    // If already started, we've nothing to do.
                    return;
                }

                _bufferSize = udp.Client.ReceiveBufferSize = Messenger.MaxMessageSize;

#if ASYNC
                Task.Factory.StartNew(() => AsyncBeginReceive(udp.Client, token));
#else
                Task.Factory.StartNew(() => AsyncReceive(udp.Client));
#endif

                Thread.Sleep(interval);
                Interlocked.CompareExchange(ref _active, Inactive, Active);
            }
        }

#if ASYNC
        private void AsyncBeginReceive(Socket socket)
        {
            while (true)
            {
                // If no more active, then stop.
                if (Interlocked.Exchange(ref _active, _active) == Inactive)
                {
                    return;
                }

                byte[] buffer = new byte[_bufferSize];
                EndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                IAsyncResult iar = null;
                try
                {
                    iar = socket.BeginReceiveFrom(buffer, 0, _bufferSize, SocketFlags.None, ref remote, AsyncEndReceive, new Tuple<Socket, byte[]>(socket, buffer));
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

                if (iar != null)
                {
                    iar.AsyncWaitHandle.WaitOne();
                }
            }
        }

        private void AsyncEndReceive(IAsyncResult iar)
        {
            // If no more active, then stop. This discards the received packet, if any (indeed, we may be there either
            // because we've received a packet, or because the socket has been closed).
            if (Interlocked.Exchange(ref _active, _active) == Inactive)
            {
                return;
            }

            //// We start another receive operation.
            //AsyncBeginReceive();

            Tuple<Socket, byte[]> data = (Tuple<Socket, byte[]>)iar.AsyncState;
            byte[] buffer = data.Item2;

            try
            {
                EndPoint remote = data.Item1.AddressFamily == AddressFamily.InterNetwork ? new IPEndPoint(IPAddress.Any, 0) : new IPEndPoint(IPAddress.IPv6Any, 0);
                int count = data.Item1.EndReceiveFrom(iar, ref remote);
                HandleMessage(buffer, count, (IPEndPoint)remote);
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
#endif

        /// <summary>
        /// Discovers agents of the specified version in a specific time interval.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="broadcastAddress">The broadcast address.</param>
        /// <param name="community">The community.</param>
        /// <param name="interval">The discovering time interval, in milliseconds.</param>
        /// <remarks><paramref name="broadcastAddress"/> must be an IPv4 address. IPv6 is not yet supported here.</remarks>
        public async Task DiscoverAsync(VersionCode version, IPEndPoint broadcastAddress, OctetString community, int interval, CancellationToken token)
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
                // throw new NotSupportedException("SNMP v3 is not supported");
                var discovery = new Discovery(Messenger.NextMessageId, _requestId, Messenger.MaxMessageSize);
                bytes = discovery.ToBytes();
            }
            else
            {
                var message = new GetRequestMessage(_requestId, version, community, _defaultVariables);
                bytes = message.ToBytes();
            }

            using (var udp = new Socket(addressFamily, SocketType.Dgram, ProtocolType.Udp))
            {
                udp.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
                var buffer = new ArraySegment<byte>(bytes);
                await udp.SendToAsync(buffer, SocketFlags.None, broadcastAddress, token);

                var activeBefore = Interlocked.CompareExchange(ref _active, Active, Inactive);
                if (activeBefore == Active)
                {
                    // If already started, we've nothing to do.
                    return;
                }

                _bufferSize = udp.ReceiveBufferSize;
                await Task.WhenAny(
                    ReceiveAsync(udp, token),
                    Task.Delay(interval));

                Interlocked.CompareExchange(ref _active, Inactive, Active);
                try
                {
                    udp.Shutdown(SocketShutdown.Both);
                }
                catch (SocketException ex)
                {
                    // This exception is thrown in .NET Core <=2.1.4 on non-Windows systems.
                    // However, the shutdown call is necessary to release the socket binding.
                    if (!SnmpMessageExtension.IsRunningOnWindows && ex.SocketErrorCode == SocketError.NotConnected)
                    {
                    }
                }
            }
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

                try
                {
                    EndPoint remote = new IPEndPoint(IPAddress.Any, 0);

                    var buffer = new byte[_bufferSize];
                    var result = await socket.ReceiveMessageFromAsync(new ArraySegment<byte>(buffer), SocketFlags.None, remote, token);
                    await Task.Factory.StartNew(() => HandleMessage(buffer, result.ReceivedBytes, (IPEndPoint) result.RemoteEndPoint))
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
    #endif
}
