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

namespace SharpSnmpLib
{
	/// <summary>
	/// Description of MessageFactory.
	/// </summary>
	public sealed class MessageFactory
	{
		MessageFactory()
		{
		}
		
		public static ISnmpMessage ParseMessage(byte[] buffer, int length)
		{			
			return ParseMessage(new MemoryStream(buffer, 0, length, false));
		}
		
		public static ISnmpMessage ParseMessage(byte[] value)
		{
			return ParseMessage(value, value.Length);
		}
		
		public static ISnmpMessage ParseMessage(MemoryStream stream)
		{
			ISnmpData array = SnmpDataFactory.CreateSnmpData(stream);
			if (array.TypeCode != SnmpType.Array) 
			{
				throw new ArgumentException("not an SNMP message");
			}
			SnmpArray body = (SnmpArray)array;
			if (body.Items.Count != 3) 
			{
				throw new ArgumentException("not an SNMP message");
			}
			ISnmpData pdu = body.Items[2];
			switch (pdu.TypeCode) {
				case SnmpType.TrapPDUv1:
					return new TrapMessage(body);
                case SnmpType.GetRequestPDU:
                    return new GetRequestMessage(body);
                case SnmpType.GetResponsePDU:
                    return new GetResponseMessage(body);
				default:
					throw new SharpSnmpException("unsupported pdu: " + pdu.TypeCode);
			}
		}
	}
}
