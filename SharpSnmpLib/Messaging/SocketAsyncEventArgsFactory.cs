// SNMP application factory class.
// Copyright (C) 2009-2010 Lex Li
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

using System.Collections.Generic;
using System.Net.Sockets;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// SNMP application factory, who holds all pipeline instances.
    /// </summary>
    public sealed class SocketAsyncEventArgsFactory
    {
        internal const string DisposedMessage = "disposed";
        private readonly object _root = new object();
        private readonly Queue<SocketAsyncEventArgs> _queue = new Queue<SocketAsyncEventArgs>();

        /// <summary>
        /// Finds an available event args.
        /// </summary>
        /// <returns></returns>
        public SocketAsyncEventArgs Create()
        {
            lock (_root)
            {
                SocketAsyncEventArgs result = null;

                while (_queue.Count > 0)
                {
                    var item = _queue.Dequeue();
                    if (item?.UserToken?.ToString() == DisposedMessage)
                    {
                        item.Dispose();
                        continue;
                    }

                    result = item;
                    break;
                }

                return result ?? new SocketAsyncEventArgs();
            }
        }

        /// <summary>
        /// Reuses the specified event args.
        /// </summary>
        /// <param name="args">The resource.</param>
        internal void Reuse(SocketAsyncEventArgs args)
        {
            lock (_root)
            {
                _queue.Enqueue(args);
            }
        }
    }
}
