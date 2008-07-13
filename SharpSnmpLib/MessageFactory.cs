/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 11:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;

namespace Lextm.SharpSnmpLib
{
	/// <summary>
	/// Factory that creates <see cref="ISnmpMessage"/> instances from byte format.
	/// </summary>
	public static class MessageFactory
	{
		/// <summary>
		/// Creates an <see cref="ISnmpMessage"/> instance from buffer.
		/// </summary>
		/// <param name="buffer">Buffer</param>
		/// <param name="index">Index</param>
		/// <param name="count">Byte count</param>
		/// <returns></returns>
		public static ISnmpMessage ParseMessage(byte[] buffer, int index, int count)
		{			
			return ParseMessage(new MemoryStream(buffer, index, count, false));
		}
		/// <summary>
		/// Creates an <see cref="ISnmpMessage"/> instance from buffer.
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public static ISnmpMessage ParseMessage(byte[] buffer)
		{
			return ParseMessage(buffer, 0, buffer.Length);
		}
		/// <summary>
		/// Creates an <see cref="ISnmpMessage"/> instance from stream.
		/// </summary>
		/// <param name="stream">Stream</param>
		/// <returns></returns>
		public static ISnmpMessage ParseMessage(MemoryStream stream)
		{
			ISnmpData array = SnmpDataFactory.CreateSnmpData(stream);
			if (array.TypeCode != SnmpType.Sequence) 
			{
				throw new ArgumentException("not an SNMP message");
			}
			Sequence body = (Sequence)array;
			if (body.Items.Count != 3) 
			{
				throw new ArgumentException("not an SNMP message");
			}
			ISnmpData pdu = body.Items[2];
			switch (pdu.TypeCode) {
				case SnmpType.TrapV1Pdu:
					return new TrapV1Message(body);
                case SnmpType.TrapV2Pdu:
                    return new TrapV2Message(body);
                case SnmpType.GetRequestPdu:
                    return new GetRequestMessage(body);
                case SnmpType.GetResponsePdu:
                    return new GetResponseMessage(body);
				default:
					throw new SharpSnmpException("unsupported pdu: " + pdu.TypeCode);
			}
		}
	}
}
