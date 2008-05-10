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
	public sealed class ByteTool
	{
		ByteTool(){	}
		
		internal static bool CompareRaw(byte[] left, byte[] right)
		{
			if (left.Length != right.Length)
			{
				return false;
			}
			for (int i = 0; i < left.Length; i++)
			{
				if (left[i] != right[i]) {
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
				
		internal static void WriteMultiByteLength(Stream s,int a) // excluding initial octet
		{
			if (a<=0)
			{
				s.WriteByte(0);
				return;
			}
			byte[] c = new byte[16];
			int j = 0;
			while (a>0) 
			{
				c[j++] = (byte)(a&0xff);
				a = a>>8;
			}
			s.WriteByte((byte)(0x80|j));			
			while (j>0) 
			{
				int x = c[--j];
				s.WriteByte((byte)x);
			}
		}
		
		internal static int ReadMultiByteLength(MemoryStream m)
        {
            int current = m.ReadByte();
            return ReadLength(m, (byte)current);
        }
		// copied from universal
        static int ReadLength(Stream s, byte x) // x is initial octet
        {
            if ((x & 0x80) == 0)
                return (int)x;
            int u = 0;
            int n = (int)(x & 0x7f);
            for (int j = 0; j < n; j++)
            {
                x = ReadByte(s);
                u = (u << 8) + (int)x;
            }
            return u;
        }
        //copied from universal
        static byte ReadByte(Stream s)
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
			ByteTool.WriteMultiByteLength(result, raw.Length);
			result.Write(raw,0,raw.Length);
			return result.ToArray();
        }
	}
}
