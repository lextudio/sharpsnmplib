// Data factory type.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/30
 * Time: 19:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Globalization;
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
        /// <returns>An <see cref="ISnmpData"/> instance created from the specified buffer.</returns>
        public static ISnmpData CreateSnmpData(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            return CreateSnmpData(buffer, 0, buffer.Length);
        }
        
        /// <summary>
        /// Creates an <see cref="ISnmpData"/> instance from stream.
        /// </summary>
        /// <param name="stream">Stream.</param>
        /// <param name="type">Type code.</param>
        /// <returns>An <see cref="ISnmpData"/> instance created from the specified stream and type code.</returns>
        public static ISnmpData CreateSnmpData(int type, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            
            var length = stream.ReadPayloadLength();
            try
            {
                return (SnmpType)type switch
                {
                    SnmpType.Counter32 => new Counter32(length, stream),
                    SnmpType.Counter64 => new Counter64(length, stream),
                    SnmpType.Gauge32 => new Gauge32(length, stream),
                    SnmpType.ObjectIdentifier => new ObjectIdentifier(length, stream),
                    SnmpType.Null => new Null(length, stream),
                    SnmpType.NoSuchInstance => new NoSuchInstance(length, stream),
                    SnmpType.NoSuchObject => new NoSuchObject(length, stream),
                    SnmpType.EndOfMibView => new EndOfMibView(length, stream),
                    SnmpType.Integer32 => new Integer32(length, stream),
                    SnmpType.OctetString => new OctetString(length, stream),
                    SnmpType.IPAddress => new IP(length, stream),
                    SnmpType.TimeTicks => new TimeTicks(length, stream),
                    SnmpType.Sequence => new Sequence(length, stream),
                    SnmpType.TrapV1Pdu => new TrapV1Pdu(length, stream),
                    SnmpType.TrapV2Pdu => new TrapV2Pdu(length, stream),
                    SnmpType.GetRequestPdu => new GetRequestPdu(length, stream),
                    SnmpType.ResponsePdu => new ResponsePdu(length, stream),
                    SnmpType.GetBulkRequestPdu => new GetBulkRequestPdu(length, stream),
                    SnmpType.GetNextRequestPdu => new GetNextRequestPdu(length, stream),
                    SnmpType.SetRequestPdu => new SetRequestPdu(length, stream),
                    SnmpType.InformRequestPdu => new InformRequestPdu(length, stream),
                    SnmpType.ReportPdu => new ReportPdu(length, stream),
                    SnmpType.Opaque => new Opaque(length, stream),
                    SnmpType.EndMarker => throw new SnmpException("unexpected end marker"),
                    SnmpType.Unsigned32 => new Gauge32(length, stream),// IMPORTANT: return Gauge32 for Unsigned32 case as workaround of RFC 1442 time entities.
                    _ => throw new SnmpException(string.Format(CultureInfo.InvariantCulture, "unsupported data type: {0}", (SnmpType)type)),
                };
            }
            catch (Exception ex)
            {
                if (ex is SnmpException)
                {
                    throw;
                }

                throw new SnmpException("data construction exception", ex);
            }
        }

        /// <summary>
        /// Creates an <see cref="ISnmpData"/> instance from buffer.
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="index">Index</param>
        /// <param name="count">Count</param>
        /// <returns>An <see cref="ISnmpData"/> instance created from the specified buffer, index, and count.</returns>
        public static ISnmpData CreateSnmpData(byte[] buffer, int index, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            using var m = new MemoryStream(buffer, index, count, false);
            return CreateSnmpData(m);
        }
        
        /// <summary>
        /// Creates an <see cref="ISnmpData"/> instance from stream.
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>An <see cref="ISnmpData"/> instance created from the specified stream.</returns>
        public static ISnmpData CreateSnmpData(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return CreateSnmpData(stream.ReadByte(), stream);
        }
    }
}
