using System.Net;

namespace DotNetSnmp.Transport
{
    /// <summary>
    /// Defines the contract for ISnmpTransport.
    /// </summary>
    public interface ISnmpTransport : IDisposable
    {
        /// <summary>
        /// Sends async.
        /// </summary>
        ValueTask<int> SendAsync(
            ReadOnlyMemory<byte> message,
            IPEndPoint targetEndPoint,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Receives async.
        /// </summary>
        ValueTask<ReadOnlyMemory<byte>> ReceiveAsync(
            IPEndPoint targetEndPoint,
            CancellationToken cancellationToken);
    }
}
