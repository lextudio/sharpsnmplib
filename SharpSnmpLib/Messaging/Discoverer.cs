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
    /// <summary>
    /// Discoverer class to discover SNMP agents in the same network.
    /// </summary>
    public sealed class Discoverer
    {
        private int _active;
        private int _bufferSize;
        private int _requestId;
        private static readonly UserRegistry Empty = new UserRegistry();
        private readonly IList<Variable> _defaultVariables = new List<Variable> { new Variable(new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 1, 1, 0 })) };
        private const int Active = 1;
        private const int Inactive = 0;

        /// <summary>
        /// Occurs when an SNMP agent is found.
        /// </summary>
        public event EventHandler<AgentFoundEventArgs> AgentFound;

        /// <summary>
        /// Occurs when an exception is raised.
        /// </summary>
        /// <remarks>The exception is typical <see cref="SocketException"/> here.</remarks>
        public event EventHandler<ExceptionRaisedEventArgs> ExceptionRaised;

        /// <summary>
        /// Discovers agents of the specified version in a specific time interval.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="broadcastAddress">The broadcast address.</param>
        /// <param name="community">The community.</param>
        /// <param name="interval">The discovering time interval, in milliseconds.</param>
        /// <remarks><paramref name="broadcastAddress"/> must be an IPv4 address. IPv6 is not yet supported here.</remarks>
        public void Discover(VersionCode version, IPEndPoint broadcastAddress, OctetString community, int interval)
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

            using (var udp = new UdpClient(addressFamily))
            {
#if (!CF)
                udp.EnableBroadcast = true;
#endif
#if NET452
                udp.Send(bytes, bytes.Length, broadcastAddress);
#else
                AsyncHelper.RunSync(() => udp.SendAsync(bytes, bytes.Length, broadcastAddress));
#endif
                var activeBefore = Interlocked.CompareExchange(ref _active, Active, Inactive);
                if (activeBefore == Active)
                {
                    // If already started, we've nothing to do.
                    return;
                }

                _bufferSize = udp.Client.ReceiveBufferSize = Messenger.MaxMessageSize;

#if ASYNC
                Task.Factory.StartNew(() => AsyncBeginReceive(udp.Client));
#else
                Task.Factory.StartNew(() => AsyncReceive(udp.Client));
#endif

                Thread.Sleep(interval);
                Interlocked.CompareExchange(ref _active, Inactive, Active);
#if NET452
                udp.Close();
#endif
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
#else

        private void AsyncReceive(Socket socket)
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
                    var buffer = new byte[_bufferSize];
                    EndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                    var count = socket.ReceiveFrom(buffer, ref remote);
                    Task.Factory.StartNew(()=> HandleMessage(buffer, count, (IPEndPoint)remote));
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode != SocketError.ConnectionReset)
                    {
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
        private void HandleException(Exception exception)
        {
            ExceptionRaised?.Invoke(this, new ExceptionRaisedEventArgs(exception));
        }

        private void HandleMessage(byte[] buffer, int count, IPEndPoint remote)
        {
            foreach (var message in MessageFactory.ParseMessages(buffer, 0, count, Empty))
            {
                var code = message.TypeCode();
                if (code == SnmpType.ReportPdu)
                {
                    var report = (ReportMessage)message;
                    if (report.RequestId() != _requestId)
                    {
                        continue;
                    }

                    if (report.Pdu().ErrorStatus.ToErrorCode() != ErrorCode.NoError)
                    {
                        continue;
                    }

                    AgentFound?.Invoke(this, new AgentFoundEventArgs(remote, null));
                }

                if (code != SnmpType.ResponsePdu)
                {
                    continue;
                }

                var response = (ResponseMessage)message;
                if (response.RequestId() != _requestId)
                {
                    continue;
                }

                if (response.ErrorStatus != ErrorCode.NoError)
                {
                    continue;
                }

                AgentFound?.Invoke(this, new AgentFoundEventArgs(remote, response.Variables()[0]));
            }
        }

        /// <summary>
        /// Discovers agents of the specified version in a specific time interval.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="broadcastAddress">The broadcast address.</param>
        /// <param name="community">The community.</param>
        /// <param name="interval">The discovering time interval, in milliseconds.</param>
        /// <remarks><paramref name="broadcastAddress"/> must be an IPv4 address. IPv6 is not yet supported here.</remarks>
        public async Task DiscoverAsync(VersionCode version, IPEndPoint broadcastAddress, OctetString community, int interval)
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
                var info = SocketExtension.EventArgsFactory.Create();
                info.RemoteEndPoint = broadcastAddress;
                info.SetBuffer(bytes, 0, bytes.Length);

                using (var awaitable1 = new SocketAwaitable(info))
                {
                    await udp.SendToAsync(awaitable1);
                }

                var activeBefore = Interlocked.CompareExchange(ref _active, Active, Inactive);
                if (activeBefore == Active)
                {
                    // If already started, we've nothing to do.
                    return;
                }

                _bufferSize = udp.ReceiveBufferSize;
                await Task.WhenAny(
                    ReceiveAsync(udp),
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

        private async Task ReceiveAsync(Socket socket)
        {
            while (true)
            {
                // If no more active, then stop.
                if (Interlocked.Exchange(ref _active, _active) == Inactive)
                {
                    return;
                }

                int count;
                var reply = new byte[_bufferSize];
                var args = SocketExtension.EventArgsFactory.Create();
                try
                {
                    EndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                    args.RemoteEndPoint = remote;
                    args.SetBuffer(reply, 0, _bufferSize);
                    using (var awaitable = new SocketAwaitable(args))
                    {
                        count = await socket.ReceiveMessageFromAsync(awaitable);
                    }

                    await Task.Factory.StartNew(() => HandleMessage(reply, count, (IPEndPoint) args.RemoteEndPoint))
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
                catch (NullReferenceException)
                {
                    args.UserToken = SocketAsyncEventArgsFactory.DisposedMessage;
                }
            }
        }
    }
}
