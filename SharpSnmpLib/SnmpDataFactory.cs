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

namespace Lextm.SharpSnmpLib
{
	/// <summary>
	/// Factory that creates <see cref="ISnmpData"/> instances.
	/// </summary>
	public static class SnmpDataFactory
	{
		/// <summary>
		/// Creates an <see cref="ISnmpData"/> instance from buffer.
		/// </summary>
		/// <param name="buffer">Buffer</param>
		/// <returns></returns>
		public static ISnmpData CreateSnmpData(byte[] buffer)
		{
			return CreateSnmpData(buffer, 0, buffer.Length);
		}
		/// <summary>
		/// Creates an <see cref="ISnmpData"/> instance from buffer.
		/// </summary>
		/// <param name="buffer">Buffer</param>
		/// <param name="index">Index</param>
		/// <param name="count">Count</param>
		/// <returns></returns>
		public static ISnmpData CreateSnmpData(byte[] buffer, int index, int count)
		{
			MemoryStream m = new MemoryStream(buffer, index, count, false);
			return CreateSnmpData(m);
		}
		/// <summary>
		/// Creates an <see cref="ISnmpData"/> instance from stream.
		/// </summary>
		/// <param name="stream">Stream</param>
		/// <returns></returns>
		public static ISnmpData CreateSnmpData(MemoryStream stream)
		{
			int type = stream.ReadByte();
			int length = ByteTool.ReadMultiByteLength(stream);
			byte[] bytes = new byte[length];
			stream.Read(bytes, 0, length);
			switch ((SnmpType)type) 
			{
                case SnmpType.Counter32:
                    return new Counter32(bytes);
                case SnmpType.Gauge32:
                    return new Gauge32(bytes);
				case SnmpType.ObjectIdentifier:
					return new ObjectIdentifier(bytes);
				case SnmpType.Null:
					return new Null();
				case SnmpType.NoSuchInstance:
					return new NoSuchInstance();
				case SnmpType.NoSuchObject:
					return new NoSuchObject();
				case SnmpType.EndOfMibView:
					return new EndOfMibView();
				case SnmpType.Integer32:
					return new Integer32(bytes);
				case SnmpType.OctetString:
					return new OctetString(bytes);
				case SnmpType.IPAddress:
					return new IP(bytes);
				case SnmpType.TimeTicks:
					return new TimeTicks(bytes);
				case SnmpType.Sequence:
					return new Sequence(bytes);
				case SnmpType.TrapV1Pdu:
					return new TrapV1Pdu(bytes);
				case SnmpType.GetRequestPdu:
					return new GetRequestPdu(bytes);
				case SnmpType.GetResponsePdu:
					return new GetResponsePdu(bytes);
				default:
					throw new SharpSnmpException("unsupported data type: " + (SnmpType)type);
			}			
		}
	}
}
