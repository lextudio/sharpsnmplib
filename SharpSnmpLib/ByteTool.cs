// Byte related function helper.
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
 * Date: 2008/5/1
 * Time: 12:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Helper utility that performs data conversions from/to bytes.
    /// </summary>
    public static class ByteTool
    {
        /// <summary>
        /// Converts decimal string to bytes.
        /// </summary>
        /// <param name="description">The decimal string.</param>
        /// <returns>The converted bytes.</returns>
        /// <remarks><c>" 16 18 "</c> is converted to <c>new byte[] { 0x10, 0x12 }</c>.</remarks>
        public static byte[] ConvertDecimal(string description)
        {
            if (description == null)
            {
                throw new ArgumentNullException(nameof(description));
            }
            
            var result = new List<byte>();
            var content = description.Trim().Split(new[] { ' ' });
            foreach (var part in content)
            {
                byte temp;
                if (byte.TryParse(part, out temp))
                {
                    result.Add(temp);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Converts the byte string to bytes.
        /// </summary>
        /// <param name="description">The HEX string.</param>
        /// <returns>The converted bytes.</returns>
        /// <remarks><c>"80 00"</c> is converted to <c>new byte[] { 0x80, 0x00 }</c>.</remarks>
        public static byte[] Convert(IEnumerable<char> description)
        {
            if (description == null)
            {
                throw new ArgumentNullException(nameof(description));
            }

            var result = new List<byte>();
            var buffer = new StringBuilder(2);
            foreach (var c in description.Where(c => !char.IsWhiteSpace(c)))
            {
                if (!char.IsLetterOrDigit(c))
                {
                    throw new ArgumentException("Illegal character found.", nameof(description));
                }
                
                buffer.Append(c);
                if (buffer.Length != 2)
                {
                    continue;
                }
                byte temp;
                if (byte.TryParse(buffer.ToString(), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out temp))
                {
                    result.Add(temp);
                }

                buffer.Length = 0;
            }
            
            if (buffer.Length != 0)
            {
                throw new ArgumentException("Not a complete byte string.", nameof(description));
            }
            
            return result.ToArray();
        }

        /// <summary>
        /// Converts bytes to a byte string.
        /// </summary>
        /// <param name="buffer">The bytes.</param>
        /// <returns>The formatted string.</returns>
        /// <remarks><c>new byte[] { 0x80, 0x00 }</c> is converted to <c>"80 00"</c>.</remarks>
        public static string Convert(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            return BitConverter.ToString(buffer).Replace('-', ' ');
        }

        internal static byte[] ParseItems(params ISnmpData[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            using (var result = new MemoryStream())
            {
                foreach (var item in items)
                {
                    if (item == null)
                    {
                        throw new ArgumentException("Item in the collection cannot be null.", nameof(items));
                    }

                    item.AppendBytesTo(result);
                }
                
                return result.ToArray();
            }
        }

        internal static byte[] ParseItems(IEnumerable<ISnmpData> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            using (var result = new MemoryStream())
            {
                foreach (var item in items)
                {
                    item.AppendBytesTo(result);
                }
                
                return result.ToArray();
            }
        }

        internal static byte[] GetRawBytes(IEnumerable<byte> orig, bool negative)
        {
            if (orig == null)
            {
                throw new ArgumentNullException(nameof(orig));
            }

            byte flag;
            byte sign;
            if (negative)
            {
                flag = 0xff;
                sign = 0x80;
            }
            else
            {
                flag = 0x0;
                sign = 0x0;
            }

            var list = new List<byte>(orig);
            while (list.Count > 1)
            {
                if (list[list.Count - 1] == flag)
                {
                    list.RemoveAt(list.Count - 1);
                }
                else
                {
                    break;
                }
            }

            // if sign bit is not correct, add an extra byte
            if ((list[list.Count - 1] & 0x80) != sign)
            {
                list.Add(flag);
            }

            list.Reverse();
            return list.ToArray();
        }

        /// <summary>
        /// Packs parts into a single message body.
        /// </summary>
        /// <param name="length">Message length.</param>
        /// <param name="version">Message version.</param>
        /// <param name="header">Header.</param>
        /// <param name="parameters">Security parameters.</param>
        /// <param name="data">Scope data.</param>
        /// <returns>The <see cref="Sequence" /> object that represents the message body.</returns>
        public static Sequence PackMessage(byte[] length, VersionCode version, ISegment header, ISegment parameters, ISnmpData data)
        {
            if (header == null)
            {
                throw new ArgumentNullException(nameof(header));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var items = new[] 
            {
                new Integer32((int)version),
                header.GetData(version),
                parameters.GetData(version),
                data
            };
            return new Sequence(length, items);
        }

        internal static byte[] WritePayloadLength(this int length) // excluding initial octet
        {
            if (length < 0)
            {
                throw new ArgumentException("length cannot be negative.", nameof(length));
            }

            var stream = new MemoryStream();

            if (length < 127)
            {
                stream.WriteByte((byte)length);
                return stream.ToArray();
            }
            
            var c = new byte[16];
            var j = 0;
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

            return stream.ToArray();
        }
    }
}
