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
        private static bool? captureNeeded;

        private static bool CaptureNeeded
        {
            get
            {
                if (captureNeeded == null)
                {
                    object setting = ConfigurationManager.AppSettings["CaptureEnabled"];
                    captureNeeded = setting != null && System.Convert.ToBoolean(setting.ToString(), CultureInfo.InvariantCulture);
                }

                return captureNeeded.Value;
            }
        }  

        internal static void Capture(byte[] buffer)
        {
            Capture(buffer, buffer.Length);
        }

        /// <summary>
        /// Captures a byte array in the log.
        /// </summary>
        /// <param name="buffer">Byte buffer.</param>
        /// <param name="length">Length to log.</param>
        public static void Capture(byte[] buffer, int length)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            
            if (length > buffer.Length)
            {
                throw new ArgumentException("length is too long.");
            }
            
            if (!CaptureNeeded)
            {
                return;
            }

            TraceSource source = new TraceSource("Library");
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                builder.AppendFormat("{0:X2} ", buffer[i]);
            }

            source.TraceInformation("SNMP packet captured at {0}, length {1}", DateTime.Now, length);
            source.TraceInformation(builder.ToString());
            source.Flush();
            source.Close();
        }

        /// <summary>
        /// Converts the byte string.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public static byte[] ConvertByteString(string description)
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
        /// <param name="buffer">The buffer.</param>
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