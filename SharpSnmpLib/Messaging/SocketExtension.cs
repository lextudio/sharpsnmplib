// Socket extension class.
// Copyright (C) 2021 Bianco Veigel, and other contributors.
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

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Extension methods for <see cref="Socket"/>.
    /// </summary>
    public static class SocketExtension
    {
        public static async Task<int> SendToAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags, EndPoint remoteEP, CancellationToken cancellationToken)
        {
            var cancellation = Task.Delay(Timeout.Infinite, cancellationToken);
            var send = socket.SendToAsync(buffer, socketFlags, remoteEP);
            var result = await Task.WhenAny(send, cancellation).ConfigureAwait(false);
            //if this is the cancellation Task, it will throw so the next await will not be executed and thus not block
            await result.ConfigureAwait(false);
            //if not cancelled, await the original Task to get the result
            return await send.ConfigureAwait(false);
        }

        public static async Task<SocketReceiveMessageFromResult> ReceiveMessageFromAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags socketFlags, EndPoint remoteEndPoint, CancellationToken cancellationToken)
        {
            var cancellation = Task.Delay(Timeout.Infinite, cancellationToken);
            var receive = socket.ReceiveMessageFromAsync(buffer, socketFlags, remoteEndPoint);
            var result = await Task.WhenAny(receive, cancellation).ConfigureAwait(false);
            //if this is the cancellation Task, it will throw so the next await will not be executed and thus not block
            await result.ConfigureAwait(false);
            //if not cancelled, await the original Task to get the result
            return await receive.ConfigureAwait(false);
        }
    }
}
