// SnmpDatagram struct for high-performance UDP datagram handling.
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

using System.Buffers;
using System.Net;
using System.Net.Sockets;

namespace Lextm.SharpSnmpLib.Transport;

/// <summary>
/// Represents a received UDP datagram with a pooled buffer and the sender's address.
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="Buffer"/> is rented from <see cref="ArrayPool{T}.Shared"/> and
/// <b>must</b> be returned after processing. Consumers should call
/// <see cref="ReturnBuffer"/> (or return the buffer manually) in a <c>finally</c>
/// block to prevent pool exhaustion.
/// </para>
/// <para>
/// <see cref="SenderAddress"/> is a <see cref="SocketAddress"/> snapshot copied from
/// the receive loop's reusable instance. Use <see cref="GetSenderEndPoint"/> when an
/// <see cref="IPEndPoint"/> is needed (note: this allocates).
/// </para>
/// </remarks>
public readonly struct SnmpDatagram
{
    /// <summary>
    /// Initializes a new instance of <see cref="SnmpDatagram"/>.
    /// </summary>
    /// <param name="buffer">
    /// A buffer rented from <see cref="ArrayPool{T}.Shared"/>. The caller transfers
    /// ownership to the consumer.
    /// </param>
    /// <param name="length">Number of valid bytes in <paramref name="buffer"/>.</param>
    /// <param name="senderAddress">
    /// A copy of the sender's <see cref="SocketAddress"/>.
    /// </param>
    public SnmpDatagram(byte[] buffer, int length, SocketAddress senderAddress)
    {
        Buffer = buffer;
        Length = length;
        SenderAddress = senderAddress;
    }

    /// <summary>
    /// Gets the rented buffer containing the datagram payload.
    /// Only the first <see cref="Length"/> bytes are valid.
    /// </summary>
    public byte[] Buffer { get; }

    /// <summary>
    /// Gets the number of valid bytes in <see cref="Buffer"/>.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Gets the sender's socket address (a snapshot, safe to use after the receive loop
    /// overwrites its working copy).
    /// </summary>
    public SocketAddress SenderAddress { get; }

    /// <summary>
    /// Creates an <see cref="IPEndPoint"/> from <see cref="SenderAddress"/>.
    /// This allocates; prefer using <see cref="SenderAddress"/> directly on the hot path.
    /// </summary>
    public IPEndPoint GetSenderEndPoint()
    {
        // IPEndPoint.Create accepts a SocketAddress and returns a new EndPoint.
        return (IPEndPoint)new IPEndPoint(
            SenderAddress.Family == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Any : IPAddress.Any,
            0).Create(SenderAddress);
    }

    /// <summary>
    /// Returns <see cref="Buffer"/> to <see cref="ArrayPool{T}.Shared"/>.
    /// Call this exactly once after the datagram has been fully processed.
    /// </summary>
    public void ReturnBuffer()
    {
        if (Buffer != null)
        {
            ArrayPool<byte>.Shared.Return(Buffer);
        }
    }
}
