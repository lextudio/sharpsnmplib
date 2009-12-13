/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/23
 * Time: 19:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Listener component.
    /// </summary>
    /// <remarks>
    /// Drag this component into your form in designer, or create an instance in code.
    /// </remarks>
    public sealed class Listener : Component
    {
        private const int Defaultport = 162;

        /// <summary>
        /// Error message for non IP v6 OS.
        /// </summary>
        public static readonly string ErrorIPv6NotSupported = "cannot use IP v6 as the OS does not support it";
        private readonly IPEndPoint _defaultEndPoint = new IPEndPoint(IPAddress.Any, Defaultport);
        private Socket _socket;
        private int _bufferSize;
        private readonly IList<IListenerAdapter> _adapters = new List<IListenerAdapter>();
        
        /// <summary>
        /// 1 = true, 0 = false
        /// </summary>
        private long _active; // = 0
        private bool _disposed;
        private int _port;
        private readonly UserRegistry _users = new UserRegistry();

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Listener"/> class.
        /// </summary>
        public Listener()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Listener"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public Listener(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            
            if (disposing)
            {
                _active = 0;
                if (_socket != null)
                {
                    _socket.Close();    // Note that closing the socket releases the _socket.ReceiveFrom call.
                }
            }
            
            base.Dispose(disposing);
            _disposed = true;
        }

        #endregion Constructor

        #region Events

        /// <summary>
        /// Occurs when an exception is raised.
        /// </summary>
        public event EventHandler<ExceptionRaisedEventArgs> ExceptionRaised;

        /// <summary>
        /// Occurs when a message is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs<ISnmpMessage>> MessageReceived;
        #endregion Events

        /// <summary>
        /// Port number.
        /// </summary>
        public int Port
        {
            get
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("Listener");
                }

                return _port;
            }
        }
        
        /// <summary>
        /// Sends a response message.
        /// </summary>
        /// <param name="response">
        /// A <see cref="ISnmpMessage"/>.
        /// </param>
        /// <param name="receiver">Receiver.</param>
        public void SendResponse(ISnmpMessage response, EndPoint receiver)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }
            
            if (_socket != null)
            {
                byte[] buffer = response.ToBytes();
                _socket.BeginSendTo(buffer, 0, buffer.Length, 0, receiver, null, null);
            }
        }

        /// <summary>
        /// Starts.
        /// </summary>
        /// <remarks><para>This function will only monitor IP v4 incoming packets. IP v6 packets will be ignored. </para><para>If you actually need to monitor IP v6 packets, please use the overloading version that requires an IPEndpoint object.</para></remarks>
        public void Start()
        {
            Start(_defaultEndPoint);
        }

        /// <summary>
        /// Starts on a specific port number.
        /// </summary>
        /// <param name="port">Port number.</param>
        /// <remarks><para>This function will only monitor IP v4 incoming packets. IP v6 packets will be ignored. </para><para>If you actually need to monitor IP v6 packets, please use the overloading version that requires an IPEndpoint object.</para></remarks>
        public void Start(int port)
        {
            Start(new IPEndPoint(IPAddress.Any, port));
        }

        /// <summary>
        /// Starts on a specific end point.
        /// </summary>
        /// <param name="endpoint">End point.</param>
        /// <remarks>This function supports both IP v4 and v6 packets.</remarks>
        public void Start(IPEndPoint endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }

            if (_disposed)
            {
                throw new ObjectDisposedException("Listener");
            }
            
            if (endpoint.AddressFamily == AddressFamily.InterNetworkV6 && !Socket.OSSupportsIPv6)
            {
                throw new InvalidOperationException(ErrorIPv6NotSupported);
            }

            long activeBefore = Interlocked.CompareExchange(ref _active, 1, 0);
            if (activeBefore == 1)
            {
                // If already started, we've nothing to do.
                return;
            }
            
            _port = endpoint.Port;
            _socket = new Socket(endpoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            try
            {
                _socket.Bind(endpoint);
            }
            catch (SocketException ex)
            {
                Interlocked.Exchange(ref _active, 0);
                HandleException(ex);
                return;
            }

            #if CF
            _bufferSize = 8192;
            #else
            _bufferSize = _socket.ReceiveBufferSize;
            #endif

            #if ASYNC
            ThreadPool.QueueUserWorkItem(AsyncBeginReceive);
            #else
            ThreadPool.QueueUserWorkItem(AsyncReceive);
            #endif
        }

        /// <summary>
        /// Stops.
        /// </summary>
        public void Stop()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("Listener");
            }

            long activeBefore = Interlocked.CompareExchange(ref _active, 0, 1);

            if (activeBefore == 1)
            {
                _socket.Close();    // Note that closing the socket releases the _socket.ReceiveFrom call.
                _socket = null;
            }
        }

        /// <summary>
        /// Closes this <see cref="Listener"/> instance and releases all resources.
        /// </summary>
        public void Close()
        {
            Stop();
            Dispose();
        }

        #if ASYNC
        private void AsyncBeginReceive(object dummy)
        {
            AsyncBeginReceive();
        }

        private void AsyncBeginReceive()
        {
            while (true)
            {
                // If no more active, then stop.
                if (Interlocked.Read(ref _active) == 0)
                    return;

                //Console.WriteLine("Waiting");

                byte[] buffer = new byte[_bufferSize];
                EndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                IAsyncResult iar = null;
                try
                {
                    iar = _socket.BeginReceiveFrom(buffer, 0, _bufferSize, SocketFlags.None, ref remote, AsyncEndReceive, buffer);
                }
                catch (SocketException ex)
                {
                    Interlocked.Exchange(ref _active, 0);    // If there is an exception here, mark the TrapListener as inactive.
                    HandleException(ex);
                }

                iar.AsyncWaitHandle.WaitOne();
            }
        }

        private void AsyncEndReceive(IAsyncResult iar)
        {
            // If no more active, then stop. This discards the received packet, if any (indeed, we may be there either
            // because we've received a packet, or because the socket has been closed).
            if (Interlocked.Read(ref _active) == 0)
                return;

            //// We start another receive operation.
            //AsyncBeginReceive();

            byte[] buffer = (byte[])iar.AsyncState;

            try
            {
                EndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                int count = _socket.EndReceiveFrom(iar, ref remote);
                HandleMessage(new MessageParams(buffer, count, remote));
            }
            catch (SocketException ex)
            {
                HandleException(ex);
            }
        }
        #else
        private void AsyncReceive(object dummy)
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
                    EndPoint remote;
                    if (_socket.AddressFamily == AddressFamily.InterNetwork)
                    {
                        remote = new IPEndPoint(IPAddress.Any, 0);
                    }
                    else
                    {
                        remote = new IPEndPoint(IPAddress.IPv6Any, 0);
                    }

                    int count = _socket.ReceiveFrom(buffer, ref remote);
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
        #endif

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
            IList<ISnmpMessage> messages = null;
            try
            {
                messages = MessageFactory.ParseMessages(param.GetBytes(), 0, param.Number, _users);
            }
            catch (Exception ex)
            {
                SharpMessageFactoryException exception = new SharpMessageFactoryException("Invalid message bytes found. Use tracing to analyze the bytes.", ex);
                exception.Bytes = param.GetBytes();
                HandleException(exception);
            }

            if (messages == null)
            {
                return;
            }

            foreach (ISnmpMessage message in messages)
            {
                EventHandler<MessageReceivedEventArgs<ISnmpMessage>> handler = MessageReceived;
                if (handler != null)
                {
                    handler(this, new MessageReceivedEventArgs<ISnmpMessage>(param.Sender, message));
                }
                
                // TODO: will remove listener adapters in the future.
                foreach (IListenerAdapter adapter in _adapters)
                {
                    adapter.Process(message, param.Sender);
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents a <see cref="Listener"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Listener";
        }

        /// <summary>
        /// Returns a value if the listener is still working.
        /// </summary>
        public bool Active
        {
            get
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("Listener");
                }

                return Interlocked.Read(ref _active) == 1;
            }
        }
        
        /// <summary>
        /// Adapters.
        /// </summary>      
        public IList<IListenerAdapter> Adapters
        {
            get { return _adapters; }
        }
    }
}
