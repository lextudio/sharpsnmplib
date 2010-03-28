// Message factory type.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

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
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;

using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Factory that creates <see cref="ISnmpMessage"/> instances from byte format.
    /// </summary>
    public static class MessageFactory
    {
        /// <summary>
        /// Sends an SNMP message and wait for its responses.
        /// </summary>
        /// <param name="receiver">The IP address and port of the target to talk to.</param>
        /// <param name="bytes">The byte array representing the SNMP message.</param>
        /// <param name="number">The <see cref="GetResponseMessage.MessageId"/> of the SNMP message.</param>
        /// <param name="timeout">The timeout above which, if the response is not received, a <see cref="TimeoutException"/> is thrown.</param>
        /// <param name="registry">The registry.</param>
        /// <param name="socket">The UDP <see cref="Socket"/> to use to send/receive.</param>
        /// <returns>
        /// The response message (<see cref="ISnmpMessage"/>).
        /// </returns>
        internal static ISnmpMessage GetResponse(IPEndPoint receiver, byte[] bytes, int number, int timeout, UserRegistry registry, Socket socket)
        {
            if (socket == null)
            {
                throw new ArgumentNullException("socket");
            }

            #if CF
            int bufSize = 8192;
            #else
            int bufSize = socket.ReceiveBufferSize;
            #endif
            byte[] reply = new byte[bufSize];

            // Whatever you change, try to keep the Send and the Receive close to each other.
            socket.SendTo(bytes, receiver);
            #if !(CF)
            socket.ReceiveTimeout = timeout;
            #endif
            int count;
            try 
            {
                count = socket.Receive(reply, 0, bufSize, SocketFlags.None);
            }
            catch (SocketException ex)
            {
                if (ex.ErrorCode == 10060)
                {
                    throw TimeoutException.Create(receiver.Address, timeout);
                }

                throw;
            }

            ISnmpMessage message;

            // Passing 'count' is not necessary because ParseMessages should ignore it, but it offer extra safety (and would avoid a bug if parsing >1 response).
            using (MemoryStream m = new MemoryStream(reply, 0, count, false))
            {
                message = ParseMessages(m, registry)[0];
            }

            if (message.Pdu.TypeCode == SnmpType.GetResponsePdu || message.Pdu.TypeCode == SnmpType.ReportPdu)
            {
                if (message.MessageId != number)
                {
                    throw OperationException.Create(string.Format(CultureInfo.InvariantCulture, "wrong response sequence: expected {0}, received {1}", number, message.MessageId), receiver.Address);
                }

                return message;
            }

            throw OperationException.Create(string.Format(CultureInfo.InvariantCulture, "wrong response type: {0}", message.Pdu.TypeCode), receiver.Address);
        }
        
        /// <summary>
        /// Creates <see cref="ISnmpMessage"/> instances from a string.
        /// </summary>
        /// <param name="bytes">Byte string.</param>
        /// <param name="registry">The registry.</param>
        /// <returns></returns>
        public static IList<ISnmpMessage> ParseMessages(string bytes, UserRegistry registry)
        {
            return ParseMessages(ByteTool.Convert(bytes), registry);
        }

        /// <summary>
        /// Creates <see cref="ISnmpMessage"/> instances from buffer.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Byte count.</param>
        /// <param name="registry">The registry.</param>
        /// <returns></returns>
        public static IList<ISnmpMessage> ParseMessages(byte[] buffer, int index, int count, UserRegistry registry)
        {
            return ParseMessages(new MemoryStream(buffer, index, count, false), registry);
        }

        /// <summary>
        /// Creates <see cref="ISnmpMessage"/> instances from buffer.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="registry">The registry.</param>
        /// <returns></returns>
        public static IList<ISnmpMessage> ParseMessages(byte[] buffer, UserRegistry registry)
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
        /// <param name="registry">The registry.</param>
        /// <returns></returns>
        public static IList<ISnmpMessage> ParseMessages(Stream stream, UserRegistry registry)
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

        private static ISnmpMessage ParseMessage(int first, Stream stream, UserRegistry registry)
        {
            ISnmpData array = DataFactory.CreateSnmpData(first, stream);
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
                throw new SnmpException("not an SNMP message");
            }

            VersionCode version = (VersionCode)((Integer32)body[0]).ToInt32();
            Header header = body.Count == 3 ? Header.Empty : new Header(body[1]);
            SecurityParameters parameters = body.Count == 3
                ? new SecurityParameters(null, null, null, (OctetString)body[1], null, null)
                : new SecurityParameters((OctetString)body[2]);
            ProviderPair record = body.Count == 3 ? ProviderPair.Default :
                registry.Find(parameters.UserName);
            if (record == null)
            {
                // handle decryption exception.
                return new MalformedMessage(header.MessageId, parameters.UserName);
            }

            Scope scope;
            if (body.Count == 3)
            {
                // v1 and v2
                scope = new Scope(null, null, (ISnmpPdu)body[2]);
            }
            else if (body[3].TypeCode == SnmpType.Sequence)
            {
                // v3 not encrypted
                scope = new Scope((Sequence)body[3]);
            }
            else if (body[3].TypeCode == SnmpType.OctetString)
            {
                // v3 encrypted
                try
                {
                    scope = new Scope((Sequence)record.Privacy.Decrypt(body[3], parameters));
                }
                catch (DecryptionException)
                {
                    // handle decryption exception.
                    return new MalformedMessage(header.MessageId, parameters.UserName);
                }
            }
            else
            {
                throw new SnmpException("invalid v3 packets scoped data: " + body[3].TypeCode);
            }

            ISnmpPdu pdu = scope.Pdu;
            switch (pdu.TypeCode)
            {
                case SnmpType.TrapV1Pdu:
                    return new TrapV1Message(body);
                case SnmpType.TrapV2Pdu:
                    return new TrapV2Message(version, header, parameters, scope, record);
                case SnmpType.GetRequestPdu:
                    return new GetRequestMessage(version, header, parameters, scope, record);
                case SnmpType.GetResponsePdu:
                    return new GetResponseMessage(version, header, parameters, scope, record);
                case SnmpType.SetRequestPdu:
                    return new SetRequestMessage(version, header, parameters, scope, record);
                case SnmpType.GetNextRequestPdu:
                    return new GetNextRequestMessage(version, header, parameters, scope, record);
                case SnmpType.GetBulkRequestPdu:
                    return new GetBulkRequestMessage(version, header, parameters, scope, record);
                case SnmpType.ReportPdu:
                    return new ReportMessage(version, header, parameters, scope, record);
                case SnmpType.InformRequestPdu:
                    return new InformRequestMessage(version, header, parameters, scope, record);
                default:
                    throw new SnmpException("unsupported pdu: " + pdu.TypeCode);
            }
        }
    }
}
