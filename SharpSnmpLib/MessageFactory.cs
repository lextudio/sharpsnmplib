/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 11:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Factory that creates <see cref="ISnmpMessage"/> instances from byte format.
    /// </summary>
    public static class MessageFactory
    {
        /// <summary>
        /// Creates <see cref="ISnmpMessage"/> instances from buffer.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Byte count.</param>
        /// <returns></returns>
        public static IList<ISnmpMessage> ParseMessages(byte[] buffer, int index, int count)
        {
            return ParseMessages(new MemoryStream(buffer, index, count, false));
        }
        
        /// <summary>
        /// Creates <see cref="ISnmpMessage"/> instances from buffer.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <returns></returns>
        public static IList<ISnmpMessage> ParseMessages(byte[] buffer)
        {
            return ParseMessages(buffer, 0, buffer.Length);
        }
        
        /// <summary>
        /// Creates <see cref="ISnmpMessage"/> instances from stream.
        /// </summary>
        /// <param name="stream">Stream.</param>
        /// <returns></returns>
        public static IList<ISnmpMessage> ParseMessages(MemoryStream stream)
        {
            IList<ISnmpMessage> result = new List<ISnmpMessage>();
            int first;
            while ((first = stream.ReadByte()) != -1)
            {
                result.Add(ParseMessage((byte)first, stream));
            }
            
            return result;
        }
        
        private static ISnmpMessage ParseMessage(byte first, MemoryStream stream)
        {
            ISnmpData array = SnmpDataFactory.CreateSnmpData(first, stream);
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
            switch (pdu.TypeCode)
            {
                case SnmpType.TrapV1Pdu:
                    return new TrapV1Message(body);
                case SnmpType.TrapV2Pdu:
                    return new TrapV2Message(body);
                case SnmpType.GetRequestPdu:
                    return new GetRequestMessage(body);
                case SnmpType.GetResponsePdu:
                    return new GetResponseMessage(body);
                case SnmpType.SetRequestPdu:
                    return new SetRequestMessage(body);
                case SnmpType.GetNextRequestPdu:
                    return new GetNextRequestMessage(body);
                case SnmpType.GetBulkRequestPdu:
                    return new GetBulkRequestMessage(body);
                case SnmpType.ReportPdu:
                    return new ReportMessage(body);
                case SnmpType.InformRequestPdu:
                    return new InformRequestMessage(body);
                default:
                    throw new SharpSnmpException("unsupported pdu: " + pdu.TypeCode);
            }
        }
    }
}
