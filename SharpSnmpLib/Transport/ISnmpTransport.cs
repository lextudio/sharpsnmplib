using System.Net;

namespace DotNetSnmp.Transport
{
    public interface ISnmpTransport : IDisposable
    {
        ValueTask<int> SendAsync(
            ReadOnlyMemory<byte> message,
            IPEndPoint targetEndPoint,
            CancellationToken cancellationToken = default);

        ValueTask<ReadOnlyMemory<byte>>
            ReceiveAsync(
            IPEndPoint targetEndPoint,
            CancellationToken cancellationToken);
    }
}
