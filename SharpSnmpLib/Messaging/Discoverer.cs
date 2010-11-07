// Discoverer classes.
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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Discoverer class to discover SNMP agents in the same network.
    /// </summary>
    public class Discoverer
    {
        private long _active;
        private int _bufferSize;
        private int _requestId;
        private static readonly UserRegistry Empty = new UserRegistry();
        private const int WSAECONNRESET = 10054; // http://msdn.microsoft.com/en-us/library/ms740668(VS.85).aspx
        private const int WSAEADDRINUSE = 10048; // http://msdn.microsoft.com/en-us/library/ms740668(VS.85).aspx

        /// <summary>
        /// Occurs when an SNMP agent is found.
        /// </summary>
        public event EventHandler<AgentFoundEventArgs> AgentFound;

        /// <summary>
        /// Occurs when an exception is raised.
        /// </summary>
        public event EventHandler<ExceptionRaisedEventArgs> ExceptionRaised;

        /// <summary>
        /// Discovers the specified version.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="broadcastAddress">The broadcast address.</param>
        /// <param name="community">The community.</param>
        /// <param name="timeout">The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</param>
        /// <remarks><paramref name="broadcastAddress"/> must be an IPv4 address. IPv6 is not yet supported here.</remarks>
        public void Discover(VersionCode version, IPEndPoint broadcastAddress, OctetString community, int timeout)
        {
            if (broadcastAddress == null)
            {
                throw new ArgumentNullException("broadcastAddress");
            }
            
            if (version != VersionCode.V3 && community == null)
            {
                throw new ArgumentNullException("community");
            }

            var addressFamily = broadcastAddress.AddressFamily;
            if (addressFamily == AddressFamily.InterNetworkV6)
            {
                throw new ArgumentException("IP v6 is not yet supported", "broadcastAddress");
            }

            byte[] bytes;
            _requestId = Messenger.NextRequestId;
            if (version == VersionCode.V3)
            {
                // throw new NotSupportedException("SNMP v3 is not supported");
                Discovery discovery = new Discovery(Messenger.NextMessageId, _requestId, Messenger.MaxMessageSize);
                bytes = discovery.ToBytes();
            }
            else
            {
                Variable v = new Variable(new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 1, 1, 0 }));
                List<Variable> variables = new List<Variable> { v };
                GetRequestMessage message = new GetRequestMessage(_requestId, version, community, variables);
                bytes = message.ToBytes();
            }

            using (UdpClient udp = new UdpClient(addressFamily))
            {
                #if (!CF)
                udp.EnableBroadcast = true;
                #endif
                udp.Send(bytes, bytes.Length, broadcastAddress);

                long activeBefore = Interlocked.CompareExchange(ref _active, 1, 0);
                if (activeBefore == 1)
                {
                    // If already started, we've nothing to do.
                    return;
                }

                #if CF
                _bufferSize = 8192;
                #else
                _bufferSize = udp.Client.ReceiveBufferSize;
                #endif

                #if ASYNC
                ThreadPool.QueueUserWorkItem(AsyncBeginReceive);
                #else
                ThreadPool.QueueUserWorkItem(AsyncReceive, udp.Client);
                #endif

                Thread.Sleep(timeout);                
                Interlocked.CompareExchange(ref _active, 0, 1);
                udp.Close();
            }
        }

        private void AsyncReceive(object dummy)
        {
            Receive((Socket)dummy);
        }
        
        private void Receive(Socket socket)
        {            
            while (true)
            {
                // If no more active, then stop.
                if (Interlocked.Read(ref _active) == 0)
                {
                    return;
                }

                try
                {
                    byte[] buffer = new byte[_bufferSize];
                    EndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                    int count = socket.ReceiveFrom(buffer, ref remote);
                    ThreadPool.QueueUserWorkItem(HandleMessage, new MessageParams(buffer, count, remote));
                }
                catch (SocketException ex)
                {
                    if (ex.ErrorCode != WSAECONNRESET)
                    {
                        // If the SnmpTrapListener was active, marks it as stopped and call HandleException.
                        // If it was inactive, the exception is likely to result from this, and we raise nothing.
                        long activeBefore = Interlocked.CompareExchange(ref _active, 0, 1);
                        if (activeBefore == 1)
                        {
                            HandleException(ex);
                        }
                    }
                }
            }
        }

        private void HandleException(Exception exception)
        {
            EventHandler<ExceptionRaisedEventArgs> handler = ExceptionRaised;
            if (handler == null)
            {
                return;
            }

            SocketException ex = exception as SocketException;
            if (ex != null && ex.ErrorCode == WSAEADDRINUSE)
            {
                exception = new SnmpException("Port is already used", exception);
            }

            handler(this, new ExceptionRaisedEventArgs(exception));
        }

        private void HandleMessage(object o)
        {
            HandleMessage((MessageParams)o);
        }

        private void HandleMessage(MessageParams param)
        {
            foreach (ISnmpMessage message in MessageFactory.ParseMessages(param.GetBytes(), 0, param.Number, Empty))
            {
                EventHandler<AgentFoundEventArgs> handler;
                var code = message.Pdu.TypeCode;
                if (code == SnmpType.ReportPdu)
                {
                    ReportMessage report = (ReportMessage)message;
                    if (report.RequestId != _requestId)
                    {
                        continue;
                    }

                    if (report.Pdu.ErrorStatus.ToErrorCode() != ErrorCode.NoError)
                    {
                        continue;
                    }

                    handler = AgentFound;
                    if (handler != null)
                    {
                        handler(this, new AgentFoundEventArgs(param.Sender, null));
                    }
                }

                if (code != SnmpType.ResponsePdu)
                {
                    continue;
                }

                ResponseMessage response = (ResponseMessage)message;
                if (response.RequestId != _requestId)
                {
                    continue;
                }

                if (response.ErrorStatus != ErrorCode.NoError)
                {
                    continue;
                }

                handler = AgentFound;
                if (handler != null)
                {
                    handler(this, new AgentFoundEventArgs(param.Sender, response.Variables[0]));
                }
            }
        }
    }
}