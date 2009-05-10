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
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Factory that creates <see cref="ISnmpMessage"/> instances from byte format.
    /// </summary>
    public static class MessageFactory
    {
        /// <summary>
        /// Creates <see cref="ISnmpMessage"/> instances from a string.
        /// </summary>
        /// <param name="bytes">Byte string.</param>
        /// <returns></returns>
        public static IList<ISnmpMessage> ParseMessages(string bytes, SecurityRegistry registry)
        {
            return ParseMessages(ByteTool.ConvertByteString(bytes), registry);
        }
        
        /// <summary>
        /// Creates <see cref="ISnmpMessage"/> instances from buffer.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Byte count.</param>
        /// <returns></returns>
        public static IList<ISnmpMessage> ParseMessages(byte[] buffer, int index, int count, SecurityRegistry registry)
        {
            return ParseMessages(new MemoryStream(buffer, index, count, false), registry);
        }
        
        /// <summary>
        /// Creates <see cref="ISnmpMessage"/> instances from buffer.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <returns></returns>
        public static IList<ISnmpMessage> ParseMessages(byte[] buffer, SecurityRegistry registry)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            return ParseMessages(buffer, 0, buffer.Length, registry);
        }
        
        /// <summary>
        /// Creates <see cref="ISnmpMessage"/> instances from stream.
        /// </summary>
        /// <param name="stream">Stream.</param>
        /// <returns></returns>
        public static IList<ISnmpMessage> ParseMessages(Stream stream, SecurityRegistry registry)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            IList<ISnmpMessage> result = new List<ISnmpMessage>();
            int first;
            while ((first = stream.ReadByte()) != -1)
            {
                ISnmpMessage message = ParseMessage(first, stream, registry);
                if (message != null)
                {
                    result.Add(message);
                    break;
                }
            }
            
            return result;
        }

        private static ISnmpMessage ParseMessage(int first, Stream stream, SecurityRegistry registry)
        {
            ISnmpData array;
            try
            {
                array = DataFactory.CreateSnmpData(first, stream);
            }
            catch (Exception)
            {
                throw new SharpSnmpException("Invalid message bytes found. Use tracing to analyze the bytes.");
            }

            if (array == null)
            {
                return null;
            }

            if (array.TypeCode != SnmpType.Sequence)
            {
                throw new ArgumentException("not an SNMP message");
            }

            Sequence body = (Sequence)array;
            if (body.Count != 3 && body.Count != 4)
            {
                throw new SharpSnmpException("not an SNMP message");
            }

            VersionCode version = (VersionCode)(((Integer32)body[0]).ToInt32() - 1);
            Header header = body.Count == 3 ? Header.Empty : new Header(body[1]);
            SecurityParameters parameters = body.Count == 3
                ? new SecurityParameters(null, null, null, (OctetString)body[1], null, null)
                : new SecurityParameters((OctetString)body[2]);
            SecurityRecord record = body.Count == 3 ? SecurityRecord.Default :
                registry.Find(parameters.UserName);
            Scope scope = body.Count == 3
                ? new Scope(null, null, (ISnmpPdu)body[2])
                : record.Privacy.Decrypt(body[3], parameters);
            ISnmpPdu pdu = scope.Pdu;

            switch (pdu.TypeCode)
            {
                case SnmpType.TrapV1Pdu:
                    return new TrapV1Message(body);
                case SnmpType.TrapV2Pdu:
                    return new TrapV2Message(body);
                case SnmpType.GetRequestPdu:
                    return new GetRequestMessage(version, header, parameters, scope, record);
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
