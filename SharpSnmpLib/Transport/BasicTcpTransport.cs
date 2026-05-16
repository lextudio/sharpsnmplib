// Basic TCP transport for SNMP over TCP (RFC 3430).
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

using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// SNMP-over-TCP client transport implementing RFC 3430 length-prefix framing.
/// </summary>
/// <remarks>
/// <para>
/// Each SNMP message is preceded by a 4-byte big-endian length prefix per RFC 3430 §2.
/// The connection is established lazily on the first <see cref="SendAsync"/> call and
/// reused for subsequent operations.
/// </para>
/// <para>
/// This is an <b>experimental</b> implementation. It is suitable for simple
/// request/response workflows (GET, SET, WALK) against TCP-capable SNMP agents.
/// </para>
/// </remarks>
public class BasicTcpTransport : ISnmpTransport
{
    private readonly IPEndPoint _target;
    private Socket? _socket;
    private NetworkStream? _stream;
    private bool _disposed;

    /// <summary>
    /// Maximum SNMP message size (same as UDP maximum for consistency).
    /// </summary>
    private const int MaxMessageSize = 65535;

    /// <summary>
    /// Initializes a new instance of <see cref="BasicTcpTransport"/>.
    /// </summary>
    /// <param name="targetEndPoint">The remote SNMP agent endpoint to connect to.</param>
    public BasicTcpTransport(IPEndPoint targetEndPoint)
    {
        _target = targetEndPoint ?? throw new ArgumentNullException(nameof(targetEndPoint));
    }

    /// <summary>
    /// Gets the target endpoint.
    /// </summary>
    public IPEndPoint TargetEndPoint => _target;

    /// <summary>
    /// Sends an SNMP message with RFC 3430 length-prefix framing.
    /// Establishes the TCP connection on first call.
    /// </summary>
    public async ValueTask<int> SendAsync(
        ReadOnlyMemory<byte> message,
        IPEndPoint targetEndPoint,
        CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        await EnsureConnectedAsync(cancellationToken).ConfigureAwait(false);

        // RFC 3430: 4-byte big-endian length prefix
        var lengthPrefix = new byte[4];
        BinaryPrimitives.WriteInt32BigEndian(lengthPrefix, message.Length);

        await _stream!.WriteAsync(lengthPrefix, cancellationToken).ConfigureAwait(false);
        await _stream.WriteAsync(message, cancellationToken).ConfigureAwait(false);
        await _stream.FlushAsync(cancellationToken).ConfigureAwait(false);

        return message.Length;
    }

    /// <summary>
    /// Receives an SNMP response with RFC 3430 length-prefix framing.
    /// </summary>
    public async ValueTask<ReadOnlyMemory<byte>> ReceiveAsync(
        IPEndPoint targetEndPoint,
        CancellationToken cancellationToken)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_stream == null)
        {
            throw new InvalidOperationException("Not connected. Call SendAsync first.");
        }

        // Read 4-byte length prefix
        var lengthBuf = new byte[4];
        await _stream.ReadExactlyAsync(lengthBuf, cancellationToken).ConfigureAwait(false);
        int messageLength = BinaryPrimitives.ReadInt32BigEndian(lengthBuf);

        if (messageLength <= 0 || messageLength > MaxMessageSize)
        {
            throw new InvalidOperationException(
                $"Invalid SNMP-over-TCP frame length: {messageLength}. " +
                $"Expected 1..{MaxMessageSize}. The remote endpoint may not support RFC 3430.");
        }

        // Read the complete SNMP message
        var messageBuf = new byte[messageLength];
        await _stream.ReadExactlyAsync(messageBuf, cancellationToken).ConfigureAwait(false);

        return messageBuf.AsMemory();
    }

    /// <summary>
    /// Closes the TCP connection.
    /// </summary>
    public void Close()
    {
        _stream?.Dispose();
        _stream = null;
        _socket?.Close();
        _socket = null;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes managed resources.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            Close();
        }

        _disposed = true;
    }

    /// <summary>
    /// Finalizer.
    /// </summary>
    ~BasicTcpTransport()
    {
        Dispose(false);
    }

    private async ValueTask EnsureConnectedAsync(CancellationToken ct)
    {
        if (_socket != null && _socket.Connected)
        {
            return;
        }

        // Clean up any previous failed connection.
        Close();

        _socket = new Socket(_target.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _socket.NoDelay = true; // Disable Nagle for low-latency SNMP request/response

        await _socket.ConnectAsync(_target, ct).ConfigureAwait(false);
        _stream = new NetworkStream(_socket, ownsSocket: false);
    }
}
