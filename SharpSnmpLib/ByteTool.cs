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
using System.IO;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Description of ByteTool.
    /// </summary>
    internal static class ByteTool
    {
        internal static bool CompareArray<T>(IList<T> left, IList<T> right) where T: IEquatable<T>
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
            MemoryStream result = new MemoryStream();
            foreach (ISnmpData item in items)
            {
                byte[] data = item.ToBytes();
                result.Write(data, 0, data.Length);
            }
            
            return result.ToArray();
        }
        
        internal static byte[] ParseItems(IEnumerable items)
        {
            if (!(items is IEnumerable<ISnmpData>))
            {
                throw new ArgumentException("items must be IEnumerable<ISnmpData>");
            }
            
            MemoryStream result = new MemoryStream();
            foreach (ISnmpData item in items)
            {
                byte[] data = item.ToBytes();
                result.Write(data, 0, data.Length);
            }
            
            return result.ToArray();
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
        
        internal static byte[] GetBytes(Stream stream)
        {
            int length = ByteTool.ReadPayloadLength(stream);
            byte[] bytes = new byte[length];
            stream.Read(bytes, 0, length);
            return bytes;
        }        
        
        // copied from universal
        private static int ReadLength(Stream stream, byte first) // x is initial octet
        {
            if ((first & 0x80) == 0)
            {
                return (int)first;
            }
            
            int result = 0;
            int octets = (int)(first & 0x7f);
            for (int j = 0; j < octets; j++)
            {
                result = (result << 8) + (int)ReadByte(stream);
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
        
        internal static byte[] ToBytes(SnmpType typeCode, byte[] raw)
        {
            MemoryStream result = new MemoryStream();
            result.WriteByte((byte)typeCode);
            ByteTool.WritePayloadLength(result, raw.Length);
            result.Write(raw, 0, raw.Length);
            return result.ToArray();
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

        internal static Sequence PackMessage(VersionCode version, OctetString community, ISnmpPdu pdu)
        {
            return new Sequence(new Integer32((int)version), community, pdu);
        }
    }
}
