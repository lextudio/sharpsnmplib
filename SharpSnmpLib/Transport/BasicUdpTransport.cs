using System.Net;
using System.Net.Sockets;

namespace DotNetSnmp.Transport
{
    /// <summary>
    /// Represents the BasicUdpTransport type.
    /// </summary>
    public class BasicUdpTransport : ISnmpTransport
    {
        private readonly UdpClient _udpClient;

        /// <summary>
        /// Stores udp Client.
        /// </summary>
        public UdpClient UdpClient => _udpClient;

        /// <summary>
        /// Gets listen Endpoint.
        /// </summary>
        public IPEndPoint ListenEndpoint { get; init; }

        private const int MaxUdpSize = 65536;

        /// <summary>
        /// Gets target End Point.
        /// </summary>
        public IPEndPoint TargetEndPoint { get; private set; }

        /// <summary>
        /// Initializes a new instance of BasicUdpTransport.
        /// </summary>
        public BasicUdpTransport(
            IPEndPoint localEndPoint,
            IPEndPoint targetEndPoint
            )
        {
            ListenEndpoint = localEndPoint;
            TargetEndPoint = targetEndPoint;
            _udpClient = new UdpClient(localEndPoint);
        }

        /// <summary>
        /// Initializes a new instance of BasicUdpTransport.
        /// </summary>
        public BasicUdpTransport(IPEndPoint targetEndPoint)
            : this(new IPEndPoint(IPAddress.Any, 0), targetEndPoint)
        {

        }

        /// <summary>
        /// Sends async.
        /// </summary>
        public ValueTask<int> SendAsync(
            ReadOnlyMemory<byte> message,
            IPEndPoint targetEndPoint,
            CancellationToken cancellationToken = default)
        {
            return _udpClient.SendAsync(
                message,
                endPoint: targetEndPoint,
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Receives async.
        /// </summary>
        public async ValueTask<ReadOnlyMemory<byte>> ReceiveAsync(
            IPEndPoint targetEndPoint,
            CancellationToken cancellationToken = default)
        {
            byte[] buffer = GC.AllocateArray<byte>(MaxUdpSize, pinned: true);

            var received = await _udpClient.Client.ReceiveFromAsync(
                buffer,
                SocketFlags.None,
                targetEndPoint);

            var receivedBytes = received.ReceivedBytes;

            return buffer.AsMemory(0, receivedBytes);
        }

        /// <summary>
        /// Closes the underlying UDP socket.
        /// </summary>
        public void Close()
        {
            _udpClient.Close();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the underlying UDP client.
        /// </summary>
        /// <param name="disposing">Whether to dispose managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _udpClient?.Dispose();
            }
        }

        ~BasicUdpTransport()
        {
            Dispose(false);
        }
    }
}
