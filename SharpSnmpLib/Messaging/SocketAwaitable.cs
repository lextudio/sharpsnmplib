namespace Lextm.SharpSnmpLib.Messaging
{
    using System;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class SocketAwaitable : INotifyCompletion
    {
        private readonly static Action SENTINEL = () => { };

        internal bool m_wasCompleted;
        internal Action m_continuation;
        internal SocketAsyncEventArgs m_eventArgs;

        public SocketAwaitable(SocketAsyncEventArgs eventArgs)
        {
            if (eventArgs == null)
            {
                throw new ArgumentNullException(nameof(eventArgs));
            }

            this.m_eventArgs = eventArgs;
            eventArgs.Completed += delegate
            {
                var prev = m_continuation ?? Interlocked.CompareExchange(
                    ref m_continuation, SENTINEL, null);
                if (prev != null) prev();
            };
        }

        internal void Reset()
        {
            this.m_wasCompleted = false;
            this.m_continuation = null;
        }

        public SocketAwaitable GetAwaiter() { return this; }

        public bool IsCompleted { get { return this.m_wasCompleted; } }

        public void OnCompleted(Action continuation)
        {
            if (this.m_continuation == SENTINEL ||
                Interlocked.CompareExchange(
                    ref this.m_continuation, continuation, null) == SENTINEL)
            {
                Task.Run(continuation);
            }
        }

        public int GetResult()
        {
            if (this.m_eventArgs.SocketError != SocketError.Success)
                throw new SocketException((int)this.m_eventArgs.SocketError);

            return this.m_eventArgs.BytesTransferred;
        }
    }
}
