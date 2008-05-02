/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/30
 * Time: 19:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;

namespace SharpSnmpLib
{
	/// <summary>
	/// Description of SnmpDataFactory.
	/// </summary>
	public sealed class SnmpDataFactory
	{
		SnmpDataFactory()
		{
		}
				
		public static ISnmpData CreateSnmpData(byte[] raw)
		{
			return CreateSnmpData(raw, raw.Length);
		}
		public static ISnmpData CreateSnmpData(byte[] buffer, int length)
		{
			MemoryStream m = new MemoryStream(buffer, 0, length, false);
			return CreateSnmpData(m);
		}
		
		public static ISnmpData CreateSnmpData(MemoryStream stream)
		{
			int type = stream.ReadByte();
			int length = ByteTool.ReadMultiByteLength(stream);
			byte[] bytes = new byte[length];
			stream.Read(bytes, 0, length);
			switch ((SnmpType)type) 
			{
				case SnmpType.ObjectIdentifier:
					return new ObjectIdentifier(bytes);
				case SnmpType.Null:
					return new Null();
				case SnmpType.Integer:
					return new Int(bytes);
				case SnmpType.OctetString:
					return new OctetString(bytes);
				case SnmpType.IpAddress:
					return new IP(bytes);
				case SnmpType.Timeticks:
					return new Timeticks(bytes);
				case SnmpType.Array:
					return new SnmpArray(bytes);
				case SnmpType.TrapPDUv1:
					return new TrapV1Pdu(bytes);
				case SnmpType.GetRequestPDU:
					return new GetRequestPdu(bytes);
				case SnmpType.GetResponsePDU:
					return new GetResponsePdu(bytes);
				default:
					throw new SharpSnmpException("unsupported data type: " + (SnmpType)type);
			}			
		}
	}
}
