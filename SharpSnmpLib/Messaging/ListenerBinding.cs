// Listener binding class.
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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Lextm.SharpSnmpLib.Messaging
{
    using Lextm.SharpSnmpLib.Security;

    /// <summary>
    /// Binding class for <see cref="Listener"/>.
    /// </summary>
    public sealed class ListenerBinding : IDisposable, IListenerBinding
    {
        private readonly UserRegistry _users;
        private Socket _socket;
        private int _bufferSize;
        private int _active; // = Inactive
        private bool _disposed;
        private const int Active = 1;
        private const int Inactive = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListenerBinding"/> class.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <param name="endpoint">The endpoint.</param>
        public ListenerBinding(UserRegistry users, IPEndPoint endpoint)
        {
            _users = users;
            Endpoint = endpoint;
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

            _disposed = true;
            if (disposing)
            {
                _active = Inactive;
                if (_socket != null)
                {
                    if (SnmpMessageExtension.IsRunningOnWindows)
                    {
                        _socket.Shutdown (SocketShutdown.Both);    // Note that closing the socket releases the _socket.ReceiveFrom call.
                    }

                    _socket.Dispose();
                    _socket = null;
                }
            }
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
        /// <remarks>The exception can be both <see cref="SocketException"/> and <see cref="SnmpException"/>.</remarks>
        public event EventHandler<ExceptionRaisedEventArgs> ExceptionRaised;

        /// <summary>
        /// Occurs when a message is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
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
                throw new ArgumentNullException(nameof(response));
            }

            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            if (_socket == null)
            {
                return;
            }

            var buffer = response.ToBytes();
            try
            {
                _socket.SendTo(buffer, 0, buffer.Length, 0, receiver);
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode != SocketError.Interrupted)
                {
                    // IMPORTANT: interrupted means the socket is closed.
                    throw;
                }
            }
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
                throw new ObjectDisposedException(GetType().FullName);
            }

            var addressFamily = Endpoint.AddressFamily;
            if (addressFamily == AddressFamily.InterNetwork && !Socket.OSSupportsIPv4)
            {
                throw new InvalidOperationException(Listener.ErrorIPv4NotSupported);
            }

            if (addressFamily == AddressFamily.InterNetworkV6 && !Socket.OSSupportsIPv6)
            {
                throw new InvalidOperationException(Listener.ErrorIPv6NotSupported);
            }

            var activeBefore = Interlocked.CompareExchange(ref _active, Active, Inactive);
            if (activeBefore == Active)
            {
                // If already started, we've nothing to do.
                return;
            }

            _socket = new Socket(addressFamily, SocketType.Dgram, ProtocolType.Udp);
            if (SnmpMessageExtension.IsRunningOnWindows)
            { 
                _socket.ExclusiveAddressUse = true;
            }

            _socket.SetSocketOption(
                addressFamily == AddressFamily.InterNetwork
                    ? SocketOptionLevel.IP
                    : SocketOptionLevel.IPv6,
                SocketOptionName.PacketInformation, true);

            try
            {
                _socket.Bind(Endpoint);
            }
            catch (SocketException ex)
            {
                Interlocked.Exchange(ref _active, Inactive);
                if (ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
                {
                    throw new PortInUseException("Endpoint is already in use", ex) { Endpoint = Endpoint };
                }

                throw;
            }

            _bufferSize = _socket.ReceiveBufferSize = Messenger.MaxMessageSize;

#if ASYNC
            Task.Factory.StartNew(() => AsyncBeginReceive());
#else
            Task.Factory.StartNew(() => AsyncReceive());
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
                throw new ObjectDisposedException(GetType().FullName);
            }

            var activeBefore = Interlocked.CompareExchange(ref _active, Inactive, Active);
            if (activeBefore != Active)
            {
                return;
            }

            if (SnmpMessageExtension.IsRunningOnWindows)
            {
                _socket.Shutdown(SocketShutdown.Both);    // Note that closing the socket releases the _socket.ReceiveFrom call.
            }

            _socket.Dispose();
            _socket = null;
        }

#if ASYNC
        private void AsyncBeginReceive()
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
                    iar = _socket.BeginReceiveFrom(buffer, 0, _bufferSize, SocketFlags.None, ref remote, AsyncEndReceive, buffer);
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

            byte[] buffer = (byte[])iar.AsyncState;

            try
            {
                EndPoint remote = _socket.AddressFamily == AddressFamily.InterNetwork ? new IPEndPoint(IPAddress.Any, 0) : new IPEndPoint(IPAddress.IPv6Any, 0);
                int count = _socket.EndReceiveFrom(iar, ref remote);
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
        private void AsyncReceive()
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
                    EndPoint remote = _socket.AddressFamily == AddressFamily.InterNetwork ? new IPEndPoint(IPAddress.Any, 0) : new IPEndPoint(IPAddress.IPv6Any, 0);

                    var count = _socket.ReceiveFrom(buffer, ref remote);
                    Task.Factory.StartNew(() => HandleMessage(buffer, count, (IPEndPoint)remote));
                }
                catch (SocketException ex)
                {
                    // ignore WSAECONNRESET, http://bytes.com/topic/c-sharp/answers/237558-strange-udp-socket-problem
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
            var handler = ExceptionRaised;
            if (handler == null)
            {
                return;
            }

            handler(this, new ExceptionRaisedEventArgs(exception));
        }


        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void HandleMessage(byte[] buffer, int count, IPEndPoint remote)
        {
            IList<ISnmpMessage> messages = null;
            try
            {
                messages = MessageFactory.ParseMessages(buffer, 0, count, _users);
            }
            catch (Exception ex)
            {
                var exception = new MessageFactoryException("Invalid message bytes found. Use tracing to analyze the bytes.", ex);
                exception.SetBytes(buffer);
                HandleException(exception);
            }

            if (messages == null)
            {
                return;
            }

            foreach (var message in messages)
            {
                var handler = MessageReceived;
                if (handler != null)
                {
                    handler(this, new MessageReceivedEventArgs(remote, message, this));
                }
            }
        }

        /// <summary>
        /// Sends a response message.
        /// </summary>
        /// <param name="response">
        /// A <see cref="ISnmpMessage"/>.
        /// </param>
        /// <param name="receiver">Receiver.</param>
        public async Task SendResponseAsync(ISnmpMessage response, EndPoint receiver)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (receiver == null)
            {
                throw new ArgumentNullException(nameof(receiver));
            }

            if (_disposed)
            {
                throw new ObjectDisposedException("Listener");
            }

            if (_socket == null)
            {
                return;
            }

            var buffer = response.ToBytes();
            var info = SocketExtension.EventArgsFactory.Create();

            try
            {
                info.RemoteEndPoint = receiver;
                info.SetBuffer(buffer, 0, buffer.Length);
                using (var awaitable1 = new SocketAwaitable(info))
                {
                    await _socket.SendToAsync(awaitable1);
                }
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode != SocketError.Interrupted)
                {
                    // IMPORTANT: interrupted means the socket is closed.
                    throw;
                }
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <exception cref="PortInUseException"/>
        public async Task StartAsync()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            var addressFamily = Endpoint.AddressFamily;
            if (addressFamily == AddressFamily.InterNetwork && !Socket.OSSupportsIPv4)
            {
                throw new InvalidOperationException(Listener.ErrorIPv4NotSupported);
            }

            if (addressFamily == AddressFamily.InterNetworkV6 && !Socket.OSSupportsIPv6)
            {
                throw new InvalidOperationException(Listener.ErrorIPv6NotSupported);
            }

            var activeBefore = Interlocked.CompareExchange(ref _active, Active, Inactive);
            if (activeBefore == Active)
            {
                // If already started, we've nothing to do.
                return;
            }

            _socket = new Socket(addressFamily, SocketType.Dgram, ProtocolType.Udp);

            try
            {
                _socket.Bind(Endpoint);
            }
            catch (SocketException ex)
            {
                Interlocked.Exchange(ref _active, Inactive);
                if (ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
                {
                    throw new PortInUseException("Endpoint is already in use", ex) { Endpoint = Endpoint };
                }

                throw;
            }

            _bufferSize = _socket.ReceiveBufferSize;
            await ReceiveAsync();
        }

        private async Task ReceiveAsync()
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
                    EndPoint remote = _socket.AddressFamily == AddressFamily.InterNetwork ? new IPEndPoint(IPAddress.Any, 0) : new IPEndPoint(IPAddress.IPv6Any, 0);
                    args.RemoteEndPoint = remote;
                    args.SetBuffer(reply, 0, _bufferSize);
                    using (var awaitable = new SocketAwaitable(args))
                    {
                        count = await _socket.ReceiveAsync(awaitable);
                    }

                    await Task.Factory.StartNew(() => HandleMessage(reply, count, (IPEndPoint)args.RemoteEndPoint));
                }
                catch (SocketException ex)
                {
                    // ignore WSAECONNRESET, http://bytes.com/topic/c-sharp/answers/237558-strange-udp-socket-problem
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
