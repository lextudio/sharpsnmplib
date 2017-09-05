namespace Lextm.SharpSnmpLib.Messaging
{
    using System;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class SocketAwaitable : INotifyCompletion, IDisposable
    {
        private readonly static Action SENTINEL = () => { };

        internal bool m_wasCompleted;
        internal Action m_continuation;
        internal SocketAsyncEventArgs m_eventArgs;
        private bool _disposed;

        public SocketAwaitable(SocketAsyncEventArgs eventArgs)
        {
            if (eventArgs == null)
            {
                throw new ArgumentNullException(nameof(eventArgs));
            }

            m_eventArgs = eventArgs;
            m_eventArgs.Completed += Completed;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SocketAwaitable()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            if (disposing)
            {
                m_eventArgs.Completed -= Completed;
                SocketExtension.EventArgsFactory.Reuse(m_eventArgs);
                m_eventArgs = null;
            }
        }

        private void Completed(object sender, SocketAsyncEventArgs e)
        {
            (m_continuation ?? Interlocked.CompareExchange(
                    ref m_continuation, SENTINEL, null))?.Invoke();
        }

        internal void Reset()
        {
            m_wasCompleted = false;
            m_continuation = null;
        }

        public SocketAwaitable GetAwaiter() { return this; }

        public bool IsCompleted { get { return m_wasCompleted; } }

        public void OnCompleted(Action continuation)
        {
            if (m_continuation == SENTINEL ||
                Interlocked.CompareExchange(
                    ref m_continuation, continuation, null) == SENTINEL)
            {
                Task.Run(continuation);
            }
        }

        public int GetResult()
        {
            if (m_eventArgs.SocketError != SocketError.Success)
                throw new SocketException((int)m_eventArgs.SocketError);

            return m_eventArgs.BytesTransferred;
        }
    }
}
