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
    public static class DataFactory
    {
        /// <summary>
        /// Creates an <see cref="ISnmpData"/> instance from buffer.
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <returns></returns>
        public static ISnmpData CreateSnmpData(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            return CreateSnmpData(buffer, 0, buffer.Length);
        }
        
        /// <summary>
        /// Creates an <see cref="ISnmpData"/> instance from stream.
        /// </summary>
        /// <param name="stream">Stream.</param>
        /// <param name="type">Type code.</param>
        /// <returns></returns>
        public static ISnmpData CreateSnmpData(int type, Stream stream)
        {
            switch ((SnmpType)type)
            {
                case SnmpType.Counter32:
                    return new Counter32(ByteTool.GetBytes(stream));
                case SnmpType.Counter64:
                    return new Counter64(ByteTool.GetBytes(stream));
                case SnmpType.Gauge32:
                    return new Gauge32(ByteTool.GetBytes(stream));
                case SnmpType.ObjectIdentifier:
                    return new ObjectIdentifier(ByteTool.GetBytes(stream));
                case SnmpType.Null:
                    ByteTool.GetBytes(stream);
                    return new Null();
                case SnmpType.NoSuchInstance:
                    ByteTool.GetBytes(stream);
                    return new NoSuchInstance();
                case SnmpType.NoSuchObject:
                    ByteTool.GetBytes(stream);
                    return new NoSuchObject();
                case SnmpType.EndOfMibView:
                    ByteTool.GetBytes(stream);
                    return new EndOfMibView();
                case SnmpType.Integer32:
                    return new Integer32(ByteTool.GetBytes(stream));
                case SnmpType.OctetString:
                    return new OctetString(ByteTool.GetBytes(stream));
                case SnmpType.IPAddress:
                    return new IP(ByteTool.GetBytes(stream));
                case SnmpType.TimeTicks:
                    return new TimeTicks(ByteTool.GetBytes(stream));
                case SnmpType.Sequence:
                    return new Sequence(ByteTool.GetBytes(stream));
                case SnmpType.TrapV1Pdu:
                    return new TrapV1Pdu(ByteTool.GetBytes(stream));
                case SnmpType.TrapV2Pdu:
                    return new TrapV2Pdu(ByteTool.GetBytes(stream));
                case SnmpType.GetRequestPdu:
                    return new GetRequestPdu(ByteTool.GetBytes(stream));
                case SnmpType.GetResponsePdu:
                    return new GetResponsePdu(ByteTool.GetBytes(stream));
                case SnmpType.GetBulkRequestPdu:
                    return new GetBulkRequestPdu(ByteTool.GetBytes(stream));
                case SnmpType.GetNextRequestPdu:
                    return new GetNextRequestPdu(ByteTool.GetBytes(stream));
                case SnmpType.SetRequestPdu:
                    return new SetRequestPdu(ByteTool.GetBytes(stream));
                case SnmpType.InformRequestPdu:
                    return new InformRequestPdu(ByteTool.GetBytes(stream));
                case SnmpType.ReportPdu:
                    return new ReportPdu(ByteTool.GetBytes(stream));
                case SnmpType.EndMarker:
                    return null;
                default:
                    throw new SharpSnmpException("unsupported data type: " + (SnmpType)type);
            }
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
        public static ISnmpData CreateSnmpData(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            return CreateSnmpData(stream.ReadByte(), stream);
        }
    }
}
