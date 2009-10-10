/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 12:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lextm.SharpSnmpLib
{    
    /// <summary>
    /// Description of ByteTool.
    /// </summary>
    public static class ByteTool
    {
        /// <summary>
        /// Converts decimal string to bytes.
        /// </summary>
        /// <param name="description">The decimal string.</param>
        /// <returns></returns>
        public static byte[] ConvertDecimal(string description)
        {
            List<byte> result = new List<byte>();
            string[] content = description.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in content)
            {
                result.Add(byte.Parse(part, NumberStyles.Integer, CultureInfo.InvariantCulture));
            }

            return result.ToArray();
        }
        
        /// <summary>
        /// Converts the byte string.
        /// </summary>
        /// <param name="description">The HEX string.</param>
        /// <returns></returns>
        public static byte[] Convert(string description)
        {
            List<byte> result = new List<byte>();
            StringBuilder buffer = new StringBuilder(2);
            foreach (char c in description)
            {
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                
                if (!char.IsLetterOrDigit(c))
                {
                    throw new ArgumentException("illegal character found", "description");
                }
                
                buffer.Append(c);
                if (buffer.Length == 2)
                {
                    result.Add(byte.Parse(buffer.ToString(), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture));
                    buffer.Length = 0;
                }
            }
            
            if (buffer.Length != 0)
            {
                throw new ArgumentException("not a complete byte string", "description");
            }
            
            return result.ToArray();
        }

        /// <summary>
        /// Converts the byte string.
        /// </summary>
        /// <param name="buffer">The bytes.</param>
        /// <returns></returns>
        public static string Convert(byte[] buffer)
        {
            StringBuilder result = new StringBuilder();
            foreach (byte b in buffer)
            {
                result.AppendFormat("{0:X2} ", b);
            }
            
            result.Length--;
            return result.ToString();
        }
        
        internal static bool CompareArray<T>(IList<T> left, IList<T> right) where T : IEquatable<T>
        {
            if (left.Count != right.Count)
            {
                return false;
            }
            
            for (int i = 0; i < left.Count; i++)
            {
                if (!left[i].Equals(right[i]))
                {
                    return false;
                }
            }
            
            return true;
        }

        internal static byte[] ParseItems(params ISnmpData[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            using (MemoryStream result = new MemoryStream())
            {
                foreach (ISnmpData item in items)
                {
                    item.AppendBytesTo(result);
                }
                
                return result.ToArray();
            }
        }
        
        internal static byte[] ParseItems(IEnumerable items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            if (!(items is IEnumerable<ISnmpData>))
            {
                throw new ArgumentException("items must be IEnumerable<ISnmpData>");
            }
            
            using (MemoryStream result = new MemoryStream())
            {
                foreach (ISnmpData item in items)
                {
                    item.AppendBytesTo(result);
                }
                
                return result.ToArray();
            }
        }
        
        internal static void WritePayloadLength(Stream stream, int length) // excluding initial octet
        {
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
        
        internal static int ReadPayloadLength(Stream stream)
        {
            int first = stream.ReadByte();
            return ReadLength(stream, (byte)first);
        }

        internal static void IgnoreBytes(Stream stream, int length)
        {
            byte[] bytes = new byte[length];
            stream.Read(bytes, 0, length);
            return;
        }
        
        // copied from universal
        private static int ReadLength(Stream stream, byte first) // x is initial octet
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
        
        // copied from universal
        private static byte ReadByte(Stream s)
        {
            int n = s.ReadByte();
            if (n == -1)
            {
                throw new SharpSnmpException("BER end of file");
            }
            
            return (byte)n;
        }
        
        internal static void AppendBytes(Stream stream, SnmpType typeCode, byte[] raw)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            
            stream.WriteByte((byte)typeCode);
            WritePayloadLength(stream, raw.Length);
            stream.Write(raw, 0, raw.Length);
        }
        
        internal static byte[] GetRawBytes(byte[] orig, bool negative)
        {
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

            List<byte> list = new List<byte>(orig);
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
                list.Add(sign);
            }

            list.Reverse();
            return list.ToArray();
        }
              
        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public static byte[] ToBytes(ISnmpData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
                
            using (MemoryStream result = new MemoryStream())
            {
                data.AppendBytesTo(result);
                return result.ToArray();
            }
        }
    }
}