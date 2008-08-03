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
        internal static bool CompareRaw(byte[] left, byte[] right)
        {
            if (left.Length != right.Length)
            {
                return false;
            }
            
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i]) 
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
        
        internal static int ReadPayloadLength(MemoryStream m)
        {
            int first = m.ReadByte();
            return ReadLength(m, (byte)first);
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
                throw (new SharpSnmpException("BER end of file"));
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

        internal static Sequence PackMessage(VersionCode version, string community, ISnmpPdu pdu)
        {
            return new Sequence(new Integer32((int)version), new OctetString(community), pdu);
        }
    }
}
