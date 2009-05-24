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

using Lextm.SharpSnmpLib.Security;
using System.Net.Sockets;

namespace Lextm.SharpSnmpLib.Messaging
{
	/// <summary>
    /// The signature of the callback method to use when calling BeginGetResponse,
    /// ie the method used when we receive a response.
    /// </summary>
    /// <param name="from">The <see cref="IPAddress"/> of the equipment from which comes the response.</param>
    /// <param name="responseMsg">The response itself.</param>
    public delegate void GetResponseCallback(IPAddress from, GetResponseMessage responseMsg);
    
    /// <summary>
    /// The signature of the callback method to use when calling BeginGetResponseRaw (ie before parsing bytes into a <see cref="ISnmpMessage"/>,
    /// ie the method used when we receive a response.
    /// </summary>
    /// <param name="from">The <see cref="IPAddress"/> of the equipment from which comes the response.</param>
    /// <param name="rawResponseBuffer">The buffer where the response is stored, non-deserialized and with extra bytes
    /// (the buffer may contain more bytes than the response itself).</param>
    /// <param name="byteCount">The number of useful byte to read from <paramref name="rawResponseBuffer"/>.</param>
    public delegate void GetResponseRawCallback(IPAddress from, byte[] rawResponseBuffer, int byteCount);

    /// <summary>
    /// Factory that creates <see cref="ISnmpMessage"/> instances from byte format.
    /// </summary>
    public static class MessageFactory
    {
        /// <summary>
        /// Creates <see cref="ISnmpMessage"/> instances from a string.
        /// </summary>
        /// <param name="bytes">Byte string.</param>
        /// <param name="registry">The registry.</param>
        /// <returns></returns>
        public static IList<ISnmpMessage> ParseMessages(string bytes, UserRegistry registry)
        {
            return ParseMessages(ByteTool.ConvertByteString(bytes), registry);
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

            VersionCode version = (VersionCode)(((Integer32)body[0]).ToInt32());
            Header header = body.Count == 3 ? Header.Empty : new Header(body[1]);
            SecurityParameters parameters = body.Count == 3
                ? new SecurityParameters(null, null, null, (OctetString)body[1], null, null)
                : new SecurityParameters((OctetString)body[2]);
            ProviderPair record = body.Count == 3 ? ProviderPair.Default :
                registry.Find(parameters.UserName);
            Scope scope = body.Count == 3
                ? new Scope(null, null, (ISnmpPdu)body[2])
                : new Scope((Sequence)record.Privacy.Decrypt(body[3], parameters));
            ISnmpPdu pdu = scope.Pdu;

            switch (pdu.TypeCode)
            {
                case SnmpType.TrapV1Pdu:
                    return new TrapV1Message(body);//(version, header, parameters, scope, record);
                case SnmpType.TrapV2Pdu:
                    return new TrapV2Message(body);//(version, header, parameters, scope, record);
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
                    return new InformRequestMessage(body);//(version, header, parameters, scope, record);
                default:
                    throw new SharpSnmpException("unsupported pdu: " + pdu.TypeCode);
            }
        }		
		
        internal static Sequence PackMessage(VersionCode version, params ISegment[] segments)
        {
            List<ISnmpData> collection = new List<ISnmpData>(segments.Length + 1);
            collection.Add(new Integer32((int)version + 1));
            foreach (ISegment segment in segments)
            {
                collection.Add(segment.GetData(version));
            }
            
            return new Sequence(collection);
        }

        internal static Sequence PackMessage(VersionCode version, IPrivacyProvider privacy, Header header, SecurityParameters parameters, Scope scope)
        {
            List<ISnmpData> collection = new List<ISnmpData>(4);
            collection.Add(new Integer32((int)version));
            collection.Add(header.GetData(version));
            collection.Add(parameters.GetData(version));
            collection.Add(privacy.Encrypt(scope.GetData(version), parameters));
            return new Sequence(collection);
        }
		
		/// <summary>
        /// Packs the message.
        /// </summary>
        /// <param name="version">The protocol version.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static Sequence PackMessage(VersionCode version, params ISnmpData[] data)
        {
            List<ISnmpData> collection = new List<ISnmpData>(1 + data.Length);
            collection.Add(new Integer32((int)version));
            collection.AddRange(data);
            return new Sequence(collection);
        }
		
		// TODO: add this method to all message exchanges.
        internal static void Capture(ISnmpMessage message)
        {
            byte[] buffer = message.ToBytes();
            ByteTool.Capture(buffer);
        }
		
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
            IAsyncResult result = socket.BeginReceive(reply, 0, bufSize, SocketFlags.None, null, null);
// ReSharper disable PossibleNullReferenceException
            result.AsyncWaitHandle.WaitOne(timeout, false);
// ReSharper restore PossibleNullReferenceException
            if (!result.IsCompleted)
            {
                throw SharpTimeoutException.Create(receiver.Address, timeout);
            }

            int count = socket.EndReceive(result);

            ISnmpMessage message;
            
            // Passing 'count' is not necessary because ParseMessages should ignore it, but it offer extra safety (and would avoid a bug if parsing >1 response).
            using (MemoryStream m = new MemoryStream(reply, 0, count, false))
            {
                message = MessageFactory.ParseMessages(m, registry)[0];
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
        /// Authenticates this message.
        /// </summary>
        public static void Authenticate(ISnmpMessage message, ProviderPair pair)
        {
            message.Parameters.AuthenticationParameters = pair.Authentication.ComputeHash(message);
        }
    }
}
