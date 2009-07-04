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
        /// <param name="number">The <see cref="GetResponseMessage.RequestId"/> of the SNMP message.</param>
        /// <param name="timeout">The timeout above which, if the response is not received, a <see cref="SharpTimeoutException"/> is thrown.</param>
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

            ByteTool.Capture(bytes); // log request
            #if CF
            int bufSize = 8192;
            #else
            int bufSize = socket.ReceiveBufferSize;
            #endif
            byte[] reply = new byte[bufSize];

            // Whatever you change, try to keep the Send and the BeginReceive close to each other.
            socket.SendTo(bytes, receiver);
//            IAsyncResult result = socket.BeginReceive(reply, 0, bufSize, SocketFlags.None, null, null);
//            // ReSharper disable PossibleNullReferenceException
//            result.AsyncWaitHandle.WaitOne(timeout, false);
//            // ReSharper restore PossibleNullReferenceException
//            if (!result.IsCompleted)
//            {
//                throw SharpTimeoutException.Create(receiver.Address, timeout);
//            }
//
//            int count = socket.EndReceive(result);
            socket.ReceiveTimeout = timeout;
            
            int count;
            try 
            {
                count = socket.Receive(reply, 0, bufSize, SocketFlags.None);
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.TimedOut)
                {
                    throw SharpTimeoutException.Create(receiver.Address, timeout);
                }

                throw ex;
            }

            ISnmpMessage message;

            // Passing 'count' is not necessary because ParseMessages should ignore it, but it offer extra safety (and would avoid a bug if parsing >1 response).
            using (MemoryStream m = new MemoryStream(reply, 0, count, false))
            {
                message = ParseMessages(m, registry)[0];
            }

            if (message.Pdu.TypeCode == SnmpType.GetResponsePdu || message.Pdu.TypeCode == SnmpType.ReportPdu)
            {
                if (message.Pdu.RequestId.ToInt32() != number)
                {
                    throw SharpOperationException.Create("wrong response sequence", receiver.Address);
                }

                ByteTool.Capture(reply); // log response
                return message;
            }

            throw SharpOperationException.Create("wrong response type", receiver.Address);
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

            VersionCode version = (VersionCode)((Integer32)body[0]).ToInt32();
            Header header = body.Count == 3 ? Header.Empty : new Header(body[1]);
            SecurityParameters parameters = body.Count == 3
                ? new SecurityParameters(null, null, null, (OctetString)body[1], null, null)
                : new SecurityParameters((OctetString)body[2]);
            ProviderPair record = body.Count == 3 ? ProviderPair.Default :
                registry.Find(parameters.UserName);
            
            Scope scope;
            if (body.Count == 3)
            {
                scope = new Scope(null, null, (ISnmpPdu)body[2]);
            }
            else if (body[3].TypeCode == SnmpType.Sequence)
            {
                scope = new Scope((Sequence)body[3]);
            }
            else if (body[3].TypeCode == SnmpType.OctetString)
            {
                scope = new Scope((Sequence)record.Privacy.Decrypt(body[3], parameters));
            }
            else
            {
                throw new SharpSnmpException("invalid v3 packets scoped data: " + body[3].TypeCode);
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
                    throw new SharpSnmpException("unsupported pdu: " + pdu.TypeCode);
            }
        }
        
        // TODO: add this method to all message exchanges.
        internal static void Capture(ISnmpMessage message)
        {
            byte[] buffer = message.ToBytes();
            ByteTool.Capture(buffer);
        }
    }
}
