// Interface for SNMP transport listener.
// Copyright (C) 2026 LeXtudio Inc.
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

using System.Net;
using System.Threading.Channels;

namespace Lextm.SharpSnmpLib.Transport;

/// <summary>
/// Defines the contract for an SNMP transport listener that produces received
/// messages from any transport (UDP, TCP, etc.).
/// </summary>
/// <remarks>
/// <para>
/// Implementations of this interface handle the transport-specific details
/// (socket I/O, framing, connection management) and expose a unified
/// <see cref="ChannelReader{T}"/> of <see cref="SnmpDatagram"/> instances.
/// The SNMP processing pipeline reads from <see cref="DatagramReader"/>
/// without needing to know the underlying transport.
/// </para>
/// <para>
/// For UDP, the implementation runs multiple concurrent receive loops on a
/// single bound socket. For TCP, it accepts connections and parses
/// length-prefixed frames (RFC 3430).
/// </para>
/// </remarks>
public interface ITransportListener : IAsyncDisposable
{
    /// <summary>
    /// Gets the channel reader that produces received datagrams.
    /// </summary>
    /// <remarks>
    /// Consumers read from this to process SNMP messages regardless
    /// of the underlying transport. Each <see cref="SnmpDatagram"/> contains
    /// a pooled buffer that <b>must</b> be returned via
    /// <see cref="SnmpDatagram.ReturnBuffer"/> after processing.
    /// </remarks>
    ChannelReader<SnmpDatagram> DatagramReader { get; }

    /// <summary>
    /// Sends a response message back to the specified address.
    /// </summary>
    /// <param name="response">The encoded SNMP response bytes.</param>
    /// <param name="receiver">The destination socket address.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="ValueTask"/> that completes when the send finishes.</returns>
    ValueTask SendResponseAsync(
        ReadOnlyMemory<byte> response,
        SocketAddress receiver,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the local endpoint this listener is bound to.
    /// </summary>
    EndPoint LocalEndPoint { get; }

    /// <summary>
    /// Starts the listener. After this call returns, the listener is actively
    /// receiving messages and writing them to <see cref="DatagramReader"/>.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task StartAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Stops the listener gracefully, completing the datagram channel and
    /// draining in-flight operations.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task StopAsync(CancellationToken cancellationToken = default);
}
