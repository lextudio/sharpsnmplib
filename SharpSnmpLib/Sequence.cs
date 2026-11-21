// SEQUENCE data type.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
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

/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/30
 * Time: 20:53
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Array type.
    /// </summary>
    /// <remarks>Represents SMIv1 SEQUENCE.</remarks>
    public sealed class Sequence : ISnmpData
    {
        private readonly List<ISnmpData> _list = new();
        private readonly byte[]? _length;
        private byte[]? _buffer;

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <summary>
        /// Creates an <see cref="Sequence"/> instance with varied <see cref="ISnmpData"/> instances.
        /// </summary>
        /// <param name="length">The length bytes.</param>
        /// <param name="items">The items.</param>
        public Sequence(byte[]? length, params ISnmpData?[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var data in items.Where(data => data != null))
            {
                _list.Add(data!);
            }

            _length = length;
        }

        /// <summary>
        /// Creates an <see cref="Sequence"/> instance with varied <see cref="ISnmpData"/> instances.
        /// </summary>
        /// <param name="items"></param>
        public Sequence(IEnumerable<ISnmpData> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var data in items.Where(data => data != null))
            {
                _list.Add(data);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sequence"/> class.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="stream">The stream.</param>
        public Sequence(Tuple<int, byte[]> length, Stream stream, int depth = DataFactory.MaxBerDepth)
        {
            if (length == null)
            {
                throw new ArgumentNullException(nameof(length));
            }

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (depth <= 0)
            {
                throw new SnmpException("BER nesting too deep");
            }

            _length = length.Item2;
            if (length.Item1 == 0)
            {
                return;
            }

            var original = stream.Position;
            var end = original + length.Item1;
            while (stream.Position < end)
            {
                var remaining = (int)(end - stream.Position);
                if (remaining < 2)
                {
                    throw new SnmpException("invalid BER length");
                }

                // Peek at the child header without consuming it, so an oversized
                // or malformed child is rejected before any recursion happens.
                if (!TryReadChildHeader(stream, out var headerLength, out var contentLength))
                {
                    throw new SnmpException("invalid BER length");
                }

                var totalChild = headerLength + (int)contentLength;
                if (contentLength < 0 || totalChild < headerLength || totalChild > remaining)
                {
                    throw new SnmpException("invalid BER length");
                }

                // Restore stream position to the start of the child, then parse.
                stream.Position -= headerLength;
                _list.Add(DataFactory.CreateSnmpData(stream, depth - 1));

                if (stream.Position > end)
                {
                    throw new SnmpException("invalid BER length");
                }
            }
        }

        private static bool TryReadChildHeader(Stream stream, out int headerLength, out long contentLength)
        {
            headerLength = 0;
            contentLength = 0;
            var start = stream.Position;
            try
            {
                if (stream.Position >= stream.Length)
                {
                    return false;
                }

                int first = stream.ReadByte();
                if (first < 0)
                {
                    stream.Position = start;
                    return false;
                }

                headerLength = 1;
                // High-tag-number form.
                if ((first & 0x1F) == 0x1F)
                {
                    while (true)
                    {
                        if (stream.Position >= stream.Length)
                        {
                            stream.Position = start;
                            return false;
                        }

                        int b = stream.ReadByte();
                        headerLength++;
                        if (b < 0)
                        {
                            stream.Position = start;
                            return false;
                        }

                        if ((b & 0x80) == 0)
                        {
                            break;
                        }
                    }
                }

                if (stream.Position >= stream.Length)
                {
                    stream.Position = start;
                    return false;
                }

                int firstLength = stream.ReadByte();
                headerLength++;
                if (firstLength < 0)
                {
                    stream.Position = start;
                    return false;
                }

                if ((firstLength & 0x80) == 0)
                {
                    contentLength = firstLength;
                }
                else
                {
                    int count = firstLength & 0x7F;
                    if (count == 0 || count > 8)
                    {
                        stream.Position = start;
                        return false;
                    }

                    contentLength = 0;
                    for (var i = 0; i < count; i++)
                    {
                        if (stream.Position >= stream.Length)
                        {
                            stream.Position = start;
                            return false;
                        }

                        int b = stream.ReadByte();
                        headerLength++;
                        if (b < 0)
                        {
                            stream.Position = start;
                            return false;
                        }

                        contentLength = (contentLength << 8) | b;
                    }
                }

                return true;
            }
            catch
            {
                try
                {
                    stream.Position = start;
                }
                catch
                {
                }

                return false;
            }
        }

        /// <summary>
        /// Item count in this <see cref="Sequence"/>.
        /// </summary>
        public int Length => _list.Count;

        /// <summary>
        /// Gets the <see cref="Lextm.SharpSnmpLib.ISnmpData"/> at the specified index.
        /// </summary>
        /// <value></value>
        public ISnmpData this[int index] => _list[index];

        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode => SnmpType.Sequence;

        /// <summary>
        /// Appends the bytes to <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void AppendBytesTo(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (_buffer == null)
            {
                _buffer = ByteTool.ParseItems(_list);
            }

            stream.AppendBytes(TypeCode, _length, _buffer);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="Sequence"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var result = new StringBuilder("SNMP SEQUENCE: ");
            foreach (var item in _list)
            {
                result.Append(item).Append("; ");
            }

            return result.ToString();
        }

        /// <summary>
        /// Gets the length bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[]? GetLengthBytes()
        {
            return _length;
        }
    }
}
