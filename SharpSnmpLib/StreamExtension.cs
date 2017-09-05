// Stream extension class.
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

using System;
using System.Collections.Generic;
using System.IO;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Stream extension class.
    /// </summary>
    public static class StreamExtension
    {
        internal static Tuple<int, byte[]> ReadPayloadLength(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var list = new List<byte>();
            var first = stream.ReadByte();
            var firstByte = (byte)first;
            if ((firstByte & 0x80) == 0)
            {
                return new Tuple<int, byte[]>(first, new[] { firstByte });
            }

            list.Add(firstByte);
            
            var result = 0;
            var octets = firstByte & 0x7f;
            for (var j = 0; j < octets; j++)
            {
                var n = stream.ReadByte();
                if (n == -1)
                {
                    throw new SnmpException("BER end of file");
                }

                var nextByte = (byte)n;
                result = (result << 8) + nextByte;
                list.Add(nextByte);
            }
            
            return new Tuple<int, byte[]>(result, list.ToArray());
        }

        internal static void IgnoreBytes(this Stream stream, int length)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var bytes = new byte[length];
            stream.Read(bytes, 0, length);
        }

        internal static void AppendBytes(this Stream stream, SnmpType typeCode, byte[] length, byte[] raw)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (raw == null)
            {
                throw new ArgumentNullException(nameof(raw));
            }

            stream.WriteByte((byte)typeCode);
            if (length == null)
            {
                length = raw.Length.WritePayloadLength();
            }

            stream.Write(length, 0, length.Length);
            stream.Write(raw, 0, raw.Length);
        }
    }
}