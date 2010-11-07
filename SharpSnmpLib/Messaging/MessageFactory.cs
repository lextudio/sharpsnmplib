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
        // http://msdn.microsoft.com/en-us/library/ms740668(VS.85).aspx
        private const int WSAETIMEDOUT = 10060;

        /// <summary>
        /// Sends an SNMP message and wait for its responses.
        /// </summary>
        /// <param name="receiver">The IP address and port of the target to talk to.</param>
        /// <param name="bytes">The byte array representing the SNMP message.</param>
        /// <param name="number">The <see cref="ResponseMessage.MessageId"/> of the SNMP message.</param>
        /// <param name="timeout">The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</param>
        /// <param name="registry">The registry.</param>
        /// <param name="socket">The UDP <see cref="Socket"/> to use to send/receive.</param>
        /// <returns>
        /// The response message (<see cref="ISnmpMessage"/>).
        /// </returns>
        /// <exception cref="TimeoutException">Timeout happens.</exception>
        internal static ISnmpMessage GetResponse(IPEndPoint receiver, byte[] bytes, int number, int timeout, UserRegistry registry, Socket socket)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            if (receiver == null)
            {
                throw new ArgumentNullException("receiver");
            }

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
                // FIXME: Mono/openSUSE uses 10035 for timeout.
                if (SnmpMessageExtension.IsRunningOnMono && ex.ErrorCode == 10035)
                {
                    throw TimeoutException.Create(receiver.Address, timeout);
                }

                if (ex.ErrorCode == WSAETIMEDOUT)
                {
                    throw TimeoutException.Create(receiver.Address, timeout);
                }

                throw;
            }

            // Passing 'count' is not necessary because ParseMessages should ignore it, but it offer extra safety (and would avoid a bug if parsing >1 response).
            ISnmpMessage message = ParseMessages(reply, 0, count, registry)[0];
            if (message.Pdu.TypeCode == SnmpType.ResponsePdu || message.Pdu.TypeCode == SnmpType.ReportPdu)
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
        public static IList<ISnmpMessage> ParseMessages(IEnumerable<char> bytes, UserRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }
            
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            
            return ParseMessages(ByteTool.Convert(bytes), registry);
        }

        /// <summary>
        /// Creates <see cref="ISnmpMessage"/> instances from buffer.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="registry">The registry.</param>
        /// <returns></returns>
        public static IList<ISnmpMessage> ParseMessages(byte[] buffer, UserRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            return ParseMessages(buffer, 0, buffer.Length, registry);
        }

        /// <summary>
        /// Creates <see cref="ISnmpMessage"/> instances from buffer.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="index">The index.</param>
        /// <param name="length">The length.</param>
        /// <param name="registry">The registry.</param>
        /// <returns></returns>
        public static IList<ISnmpMessage> ParseMessages(byte[] buffer, int index, int length, UserRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            
            IList<ISnmpMessage> result = new List<ISnmpMessage>();
            using (Stream stream = new MemoryStream(buffer, index, length, false))
            {                
                int first;
                while ((first = stream.ReadByte()) != -1)
                {
                    ISnmpMessage message = ParseMessage(first, stream, registry);
                    if (message == null)
                    {
                        continue;
                    }
    
                    result.Add(message);
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
                throw new SnmpException("not an SNMP message");
            }

            Sequence body = (Sequence)array;
            if (body.Count != 3 && body.Count != 4)
            {
                throw new SnmpException("not an SNMP message");
            }

            VersionCode version = (VersionCode)((Integer32)body[0]).ToInt32();
            Header header;
            SecurityParameters parameters;
            IPrivacyProvider privacy;
            Scope scope;
            if (body.Count == 3)
            {
                header = Header.Empty;
                parameters = new SecurityParameters(null, null, null, (OctetString)body[1], null, null);
                privacy = DefaultPrivacyProvider.DefaultPair;                
                scope = new Scope((ISnmpPdu)body[2]);
            }
            else
            {
                header = new Header(body[1]);
                parameters = new SecurityParameters((OctetString)body[2]);
                privacy = registry.Find(parameters.UserName);
                if (privacy == null)
                {
                    // handle decryption exception.
                    return new MalformedMessage(header.MessageId, parameters.UserName);
                }

                if (body[3].TypeCode == SnmpType.Sequence)
                {
                    // v3 not encrypted
                    scope = new Scope((Sequence)body[3]);
                }
                else if (body[3].TypeCode == SnmpType.OctetString)
                {
                    // v3 encrypted
                    try
                    {
                        scope = new Scope((Sequence)privacy.Decrypt(body[3], parameters));
                    }
                    catch (DecryptionException)
                    {
                        // handle decryption exception.
                        return new MalformedMessage(header.MessageId, parameters.UserName);
                    }
                }
                else
                {
                    throw new SnmpException(string.Format(CultureInfo.InvariantCulture, "invalid v3 packets scoped data: {0}", body[3].TypeCode));
                }

                if (!privacy.AuthenticationProvider.VerifyHash(version, header, parameters, body[3], privacy))
                {
                    throw new SnmpException("invalid v3 packet data");
                }
            }

            ISnmpPdu pdu = scope.Pdu;
            switch (pdu.TypeCode)
            {
                case SnmpType.TrapV1Pdu:
                    return new TrapV1Message(body);
                case SnmpType.TrapV2Pdu:
                    return new TrapV2Message(version, header, parameters, scope, privacy);
                case SnmpType.GetRequestPdu:
                    return new GetRequestMessage(version, header, parameters, scope, privacy);
                case SnmpType.ResponsePdu:
                    return new ResponseMessage(version, header, parameters, scope, privacy, false);
                case SnmpType.SetRequestPdu:
                    return new SetRequestMessage(version, header, parameters, scope, privacy);
                case SnmpType.GetNextRequestPdu:
                    return new GetNextRequestMessage(version, header, parameters, scope, privacy);
                case SnmpType.GetBulkRequestPdu:
                    return new GetBulkRequestMessage(version, header, parameters, scope, privacy);
                case SnmpType.ReportPdu:
                    return new ReportMessage(version, header, parameters, scope, privacy);
                case SnmpType.InformRequestPdu:
                    return new InformRequestMessage(version, header, parameters, scope, privacy);
                default:
                    throw new SnmpException(string.Format(CultureInfo.InvariantCulture, "unsupported pdu: {0}", pdu.TypeCode));
            }
        }
    }
}
