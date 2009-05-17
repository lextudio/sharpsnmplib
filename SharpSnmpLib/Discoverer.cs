using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Lextm.SharpSnmpLib.Security;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Class to discover SNMP agents in the same network.
    /// </summary>
    public partial class Discoverer : Component
    {
        private long _active;
        private int _bufferSize;
        private int requestId;

        /// <summary>
        /// Initializes a new instance of the <see cref="Discoverer"/> class.
        /// </summary>
        public Discoverer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Discoverer"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public Discoverer(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            
            container.Add(this);
            InitializeComponent();
        }

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
        /// <param name="timeout">The timeout.</param>
        public void Discover(VersionCode version, IPEndPoint broadcastAddress, OctetString community, int timeout)
        {
            if (version == VersionCode.V3)
            {
                throw new NotSupportedException("SNMP v3 is not supported");
            }

            Variable v = new Variable(new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 1, 1, 0 }));
            List<Variable> variables = new List<Variable>();
            variables.Add(v);

            requestId = RequestCounter.NextCount;
            GetRequestMessage message = new GetRequestMessage(requestId, version, community, variables);
            byte[] bytes = message.ToBytes();
            using (UdpClient udp = new UdpClient(broadcastAddress.AddressFamily))
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
            Receive((Socket) dummy);
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

        private void HandleException(Exception exception)
        {
            EventHandler<ExceptionRaisedEventArgs> handler = ExceptionRaised;
            if (handler == null)
            {
                return;
            }

            SocketException ex = exception as SocketException;
            if (ex != null && ex.ErrorCode == 10048)
            {
                exception = new SharpSnmpException("Port is already used", exception);
            }

            handler(this, new ExceptionRaisedEventArgs(exception));
        }

        private void HandleMessage(object o)
        {
            HandleMessage((MessageParams)o);
        }

        private void HandleMessage(MessageParams param)
        {
            ByteTool.Capture(param.GetBytes(), param.Number);

            foreach (ISnmpMessage message in MessageFactory.ParseMessages(param.GetBytes(), 0, param.Number, new UserRegistry()))
            {
                if (message.Pdu.TypeCode != SnmpType.GetResponsePdu)
                {
                    continue;
                }

                GetResponseMessage response = (GetResponseMessage) message;
                if (response.RequestId != requestId)
                {
                    continue;
                }

                if (response.ErrorStatus != ErrorCode.NoError)
                {
                    continue;
                }

                EventHandler<AgentFoundEventArgs> handler = AgentFound;
                if (handler != null)
                {
                    handler(this, new AgentFoundEventArgs(param.Sender, response.Variables[0]));
                }
            }
        }
    }
}