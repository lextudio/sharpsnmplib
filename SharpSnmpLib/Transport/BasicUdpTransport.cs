using System.Net;
using System.Net.Sockets;

namespace DotNetSnmp.Transport
{
    public class BasicUdpTransport : ISnmpTransport
    {
        private readonly UdpClient _udpClient;

        public UdpClient UdpClient => _udpClient;

        public IPEndPoint ListenEndpoint { get; init; }

        private const int MaxUdpSize = 65536;

        public IPEndPoint TargetEndPoint { get; private set; }

        public BasicUdpTransport(
            IPEndPoint localEndPoint,
            IPEndPoint targetEndPoint
            )
        {
            ListenEndpoint = localEndPoint;
            TargetEndPoint = targetEndPoint;
            _udpClient = new UdpClient(localEndPoint);
        }

        public BasicUdpTransport(IPEndPoint targetEndPoint)
            : this(new IPEndPoint(IPAddress.Any, 0), targetEndPoint)
        {

        }

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

        public void Close()
        {
            _udpClient.Close();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _udpClient.Close();
            _udpClient.Dispose();
        }
    }
}
