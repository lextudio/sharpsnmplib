using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Binding class for <see cref="Listener"/>.
    /// </summary>
    public sealed class ListenerBinding : IDisposable
    {
        private Socket _socket;
        private int _bufferSize;
        private long _active; // = Inactive
        private bool _disposed;
        private const long Active = 1;
        private const long Inactive = 0;
        private readonly UserRegistry _users;
        private readonly IEnumerable<IListenerAdapter> _adapters;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListenerBinding"/> class.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <param name="adapters">The adapters.</param>
        /// <param name="endpoint">The endpoint.</param>
        public ListenerBinding(UserRegistry users, IEnumerable<IListenerAdapter> adapters, IPEndPoint endpoint)
        {
            _users = users;
            Endpoint = endpoint;
            _adapters = adapters;
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            
            if (disposing)
            {
                _active = Inactive;
                if (_socket != null)
                {
                    _socket.Close();    // Note that closing the socket releases the _socket.ReceiveFrom call.
                    (_socket as IDisposable).Dispose();
                    _socket = null;
                }
            }
            
            _disposed = true;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="Listener"/> is reclaimed by garbage collection.
        /// </summary>
        ~ListenerBinding()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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

            if (receiver == null)
            {
                throw new ArgumentNullException("receiver");
            }

            if (_disposed)
            {
                throw new ObjectDisposedException("Listener");
            }

            if (_socket == null)
            {
                return;
            }

            byte[] buffer = response.ToBytes();
            _socket.BeginSendTo(buffer, 0, buffer.Length, 0, receiver, null, null);
        }

        /// <summary>
        /// Gets or sets the endpoint.
        /// </summary>
        /// <value>The endpoint.</value>
        public IPEndPoint Endpoint { get; private set; }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <exception cref="PortInUseException"/>
        public void Start()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("Listener");
            }

            if (Endpoint.AddressFamily == AddressFamily.InterNetwork && !Socket.SupportsIPv4)
            {
                throw new InvalidOperationException(Listener.ErrorIPv4NotSupported);
            }
            
            if (Endpoint.AddressFamily == AddressFamily.InterNetworkV6 && !Socket.OSSupportsIPv6)
            {
                throw new InvalidOperationException(Listener.ErrorIPv6NotSupported);
            }

            long activeBefore = Interlocked.CompareExchange(ref _active, Active, Inactive);
            if (activeBefore == Active)
            {
                // If already started, we've nothing to do.
                return;
            }

            _socket = new Socket(Endpoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            try
            {
                _socket.Bind(Endpoint);
            }
            catch (SocketException ex)
            {
                Interlocked.Exchange(ref _active, Inactive);
                throw new PortInUseException("Endpoint is already in use", ex) {Endpoint = Endpoint};
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
        /// <exception cref="ObjectDisposedException"/>
        public void Stop()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("Listener");
            }

            long activeBefore = Interlocked.CompareExchange(ref _active, Inactive, Active);
            if (activeBefore != Active)
            {
                return;
            }

            _socket.Close();    // Note that closing the socket releases the _socket.ReceiveFrom call.
            _socket = null;
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
                if (Interlocked.Read(ref _active) == Inactive)
                {
                    return;
                }

                try
                {
                    byte[] buffer = new byte[_bufferSize];
                    EndPoint remote = _socket.AddressFamily == AddressFamily.InterNetwork ? new IPEndPoint(IPAddress.Any, 0) : new IPEndPoint(IPAddress.IPv6Any, 0);

                    int count = _socket.ReceiveFrom(buffer, ref remote);
                    ThreadPool.QueueUserWorkItem(HandleMessage, new MessageParams(buffer, count, remote));
                }
                catch (SocketException ex)
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
                exception = new PortInUseException("Port is already used", exception);
            }

            handler(this, new ExceptionRaisedEventArgs(exception));
        }

        private void HandleMessage(object o)
        {
            HandleMessage((MessageParams)o);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void HandleMessage(MessageParams param)
        {
            IList<ISnmpMessage> messages = null;
            try
            {
                messages = MessageFactory.ParseMessages(param.GetBytes(), 0, param.Number, _users);
            }
            catch (Exception ex)
            {
                MessageFactoryException exception = new MessageFactoryException("Invalid message bytes found. Use tracing to analyze the bytes.", ex);
                exception.SetBytes(param.GetBytes());
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
                    handler(this, new MessageReceivedEventArgs<ISnmpMessage>(param.Sender, message, this));
                }
                
                foreach (IListenerAdapter adapter in _adapters)
                {
                    adapter.Process(message, param.Sender, this);
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents a <see cref="Listener"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "ListenerBinding";
        }
    }
}