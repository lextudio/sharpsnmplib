// Stream extension class.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.IO;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Stream extension class.
    /// </summary>
    public static class StreamExtension
    {
        internal static void WritePayloadLength(this Stream stream, int length) // excluding initial octet
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (length < 0)
            {
                throw new ArgumentException("length cannot be negative", "length");
            }
            
            if (length < 127)
            {
                stream.WriteByte((byte)length);
                return;
            }
            
            byte[] c = new byte[16];
            int j = 0;
            while (length > 0)
            {
                c[j++] = (byte)(length & 0xff);
                length = length >> 8;
            }
            
            stream.WriteByte((byte)(0x80 | j));
            while (j > 0)
            {
                int x = c[--j];
                stream.WriteByte((byte)x);
            }
        }

        internal static int ReadPayloadLength(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            int first = stream.ReadByte();
            return stream.ReadLength((byte)first);
        }

        internal static void IgnoreBytes(this Stream stream, int length)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            byte[] bytes = new byte[length];
            stream.Read(bytes, 0, length);
        }

        internal static void IgnorePayloads(this Stream stream, int count)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            while (count > 0)
            {
                stream.ReadByte();
                stream.IgnoreBytes(stream.ReadPayloadLength());

                count--;
            }
        }

        internal static void IgnorePayloadStart(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            stream.ReadByte();
            stream.ReadPayloadLength();
        }

        private static int ReadLength(this Stream stream, byte first) // x is initial octet
        {
            if ((first & 0x80) == 0)
            {
                return first;
            }
            
            int result = 0;
            int octets = first & 0x7f;
            for (int j = 0; j < octets; j++)
            {
                result = (result << 8) + ReadByte(stream);
            }
            
            return result;
        }

        private static byte ReadByte(Stream s)
        {
            int n = s.ReadByte();
            if (n == -1)
            {
                throw new SnmpException("BER end of file");
            }
            
            return (byte)n;
        }

        internal static void AppendBytes(this Stream stream, SnmpType typeCode, byte[] raw)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (raw == null)
            {
                throw new ArgumentNullException("raw");
            }

            stream.WriteByte((byte)typeCode);
            stream.WritePayloadLength(raw.Length);
            stream.Write(raw, 0, raw.Length);
        }
        
 #region Copied from https://github.com/mono/mono/blob/master/mcs/class/corlib/System.IO/Stream.cs       
        internal static void CopyTo(this Stream source, Stream destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            
            source.CopyTo(destination, 16 * 1024);
        }
        
        private static void CopyTo(this Stream source, Stream destination, int bufferSize)
        {
            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }
            
            if (!source.CanRead)
            {
                throw new NotSupportedException("This stream does not support reading");
            }
            
            if (!destination.CanWrite)
            {
                throw new NotSupportedException("This destination stream does not support writing");
            }
            
            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException("bufferSize");
            }
            
            var buffer = new byte[bufferSize];
            int nread;
            while ((nread = source.Read(buffer, 0, bufferSize)) != 0)
            {
                destination.Write(buffer, 0, nread);
            }
        }
#endregion
    }
}
