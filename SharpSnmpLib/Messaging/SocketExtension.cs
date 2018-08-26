namespace Lextm.SharpSnmpLib.Messaging
{
    using System.Net.Sockets;

    internal static class SocketExtension
    {
        public static SocketAwaitable ReceiveMessageFromAsync(this Socket socket,
            SocketAwaitable awaitable)
        {
            awaitable.Reset();
            if (!socket.ReceiveMessageFromAsync(awaitable.m_eventArgs))
                awaitable.m_wasCompleted = true;
            return awaitable;
        }

        public static SocketAwaitable SendToAsync(this Socket socket,
            SocketAwaitable awaitable)
        {
            awaitable.Reset();
            if (!socket.SendToAsync(awaitable.m_eventArgs))
                awaitable.m_wasCompleted = true;
            return awaitable;
        }

        internal static readonly SocketAsyncEventArgsFactory EventArgsFactory = new SocketAsyncEventArgsFactory();
    }
}
