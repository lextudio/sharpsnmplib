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
        /// Creates an <see cref="ISnmpData"/> instance from stream.
        /// </summary>
        /// <param name="stream">Stream.</param>
        /// <param name="type">Type code.</param>
        /// <returns></returns>
        public static ISnmpData CreateSnmpData(int type, MemoryStream stream)
        {
            switch ((SnmpType)type)
            {
                case SnmpType.Counter32:
                    return new Counter32(GetBytes(stream));
                case SnmpType.Counter64:
                    return new Counter64(GetBytes(stream));
                case SnmpType.Gauge32:
                    return new Gauge32(GetBytes(stream));
                case SnmpType.ObjectIdentifier:
                    return new ObjectIdentifier(GetBytes(stream));
                case SnmpType.Null:
                    GetBytes(stream);
                    return new Null();
                case SnmpType.NoSuchInstance:
                    GetBytes(stream);
                    return new NoSuchInstance();
                case SnmpType.NoSuchObject:
                    GetBytes(stream);
                    return new NoSuchObject();
                case SnmpType.EndOfMibView:
                    GetBytes(stream);
                    return new EndOfMibView();
                case SnmpType.Integer32:
                    return new Integer32(GetBytes(stream));
                case SnmpType.OctetString:
                    return new OctetString(GetBytes(stream));
                case SnmpType.IPAddress:
                    return new IP(GetBytes(stream));
                case SnmpType.TimeTicks:
                    return new TimeTicks(GetBytes(stream));
                case SnmpType.Sequence:
                    return new Sequence(GetBytes(stream));
                case SnmpType.TrapV1Pdu:
                    return new TrapV1Pdu(GetBytes(stream));
                case SnmpType.TrapV2Pdu:
                    return new TrapV2Pdu(GetBytes(stream));
                case SnmpType.GetRequestPdu:
                    return new GetRequestPdu(GetBytes(stream));
                case SnmpType.GetResponsePdu:
                    return new GetResponsePdu(GetBytes(stream));
                case SnmpType.GetBulkRequestPdu:
                    return new GetBulkRequestPdu(GetBytes(stream));
                case SnmpType.GetNextRequestPdu:
                    return new GetNextRequestPdu(GetBytes(stream));
                case SnmpType.SetRequestPdu:
                    return new SetRequestPdu(GetBytes(stream));
                case SnmpType.InformRequestPdu:
                    return new InformRequestPdu(GetBytes(stream));
                case SnmpType.ReportPdu:
                    return new ReportPdu(GetBytes(stream));
                case SnmpType.EndMarker:
                    //GetBytes(stream);
                    //int i = 0;
                    //while (0 == stream.ReadByte())
                    //{
                    //    i++;
                    //}
                    //byte[] buffer = new byte[75];
                    //stream.Read(buffer, 0, 75);
                    return null;
                default:
                    throw new SharpSnmpException("unsupported data type: " + (SnmpType)type);
            }
        }

        private static byte[] GetBytes(MemoryStream stream)
        {
            int length = ByteTool.ReadPayloadLength(stream);
            byte[] bytes = new byte[length];
            stream.Read(bytes, 0, length);
            return bytes;
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
            return CreateSnmpData(stream.ReadByte(), stream);
        }
    }
}
