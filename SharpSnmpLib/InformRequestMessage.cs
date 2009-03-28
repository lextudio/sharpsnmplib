/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/8/3
 * Time: 15:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// INFORM request message.
    /// </summary>
    public class InformRequestMessage : ISnmpMessage
    {
        private readonly VersionCode _version;
        private readonly IList<Variable> _variables;
        private readonly byte[] _bytes;
        private readonly OctetString _community;
        private readonly ISnmpPdu _pdu;
        private readonly int _sequenceNumber;

        /// <summary>
        /// Creates a <see cref="InformRequestMessage"/> with all contents.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="version">Protocol version.</param>
        /// <param name="community">Community name.</param>
        /// <param name="enterprise">Enterprise.</param>
        /// <param name="time">Time ticks.</param>
        /// <param name="variables">Variables.</param>
        [CLSCompliant(false)]
        public InformRequestMessage(int requestId, VersionCode version, OctetString community, ObjectIdentifier enterprise, uint time, IList<Variable> variables)
        {
            _version = version;
            _community = community;
            _variables = variables;
            InformRequestPdu pdu = new InformRequestPdu(new Integer32(requestId), enterprise, new TimeTicks(time), _variables);
            _sequenceNumber = requestId;
            _bytes = pdu.ToMessageBody(_version, _community).ToBytes();
        }
        
        /// <summary>
        /// Creates a <see cref="InformRequestMessage"/> with a specific <see cref="Sequence"/>.
        /// </summary>
        /// <param name="body">Message body</param>
        public InformRequestMessage(Sequence body)
        {
            if (body == null)
            {
                throw new ArgumentNullException("body");
            }
            
            if (body.Items.Count != 3)
            {
                throw new ArgumentException("wrong message body");
            }
            
            _community = (OctetString)body.Items[1];
            _version = (VersionCode)((Integer32)body.Items[0]).ToInt32();
            _pdu = (ISnmpPdu)body.Items[2];
            if (_pdu.TypeCode != SnmpType.InformRequestPdu)
            {
                throw new ArgumentException("wrong message type");
            }
            
            InformRequestPdu pdu = (InformRequestPdu)_pdu;
            _sequenceNumber = pdu.RequestId;
            _variables = _pdu.Variables;
            _bytes = body.ToBytes();
        }
        
        /// <summary>
        /// Variables.
        /// </summary>
        public IList<Variable> Variables
        {
            get
            {
                return _variables;
            }
        }

        /// <summary>
        /// Sends the response.
        /// </summary>
        /// <param name="receiver">The receiver.</param>
        public void SendResponse(IPEndPoint receiver)
        {
            if (receiver == null)
            {
                throw new ArgumentNullException("receiver");
            }
            
            // TODO: make more efficient here.
            InformRequestPdu pdu = (InformRequestPdu)_pdu;
            new GetResponseMessage(_sequenceNumber, _version, receiver.Address, _community, pdu.AllVariables).Send(receiver.Port);
        }

        /// <summary>
        /// Sends this <see cref="InformRequestMessage"/> and handles the response from receiver (managers or agents).
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="receiver">Receiver.</param>
        /// <returns></returns>
        public GetResponseMessage GetResponse(int timeout, IPEndPoint receiver)
        {
            return ByteTool.GetResponse(receiver, _bytes, SequenceNumber, timeout);
        }

        /// <summary>
        /// Sends this <see cref="InformRequestMessage"/> and handles the response from receiver (managers or agents).
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="receiver">Receiver.</param>
        /// <param name="socket">The socket.</param>
        /// <returns></returns>
        public GetResponseMessage GetResponse(int timeout, IPEndPoint receiver, Socket socket)
        {
            return ByteTool.GetResponse(receiver, _bytes, SequenceNumber, timeout, socket);
        }
        
        /// <summary>
        /// Sends this <see cref="InformRequestMessage"/> and handles the response from receiver (managers or agents).
        /// </summary>
        /// <param name="receiver">Receiver address.</param>
        /// <param name="timeout">Timeout.</param>
        /// <param name="port">Port number.</param>
        [Obsolete("Please use GetResponse instead. Otherwise, make sure you called the obsolete constructor for this object.")]
        public void Send(IPAddress receiver, int timeout, int port)
        {
            byte[] bytes = _bytes;
            ByteTool.Capture(bytes); // log request
            IPEndPoint agent = new IPEndPoint(receiver, port);
            using (UdpClient udp = new UdpClient())
            {
                udp.Send(bytes, bytes.Length, agent);
                IPEndPoint from = new IPEndPoint(IPAddress.Any, 0);
                IAsyncResult result = udp.BeginReceive(null, this);
                result.AsyncWaitHandle.WaitOne(timeout, false);
                if (!result.IsCompleted)
                {
                    throw SharpTimeoutException.Create(receiver, timeout);
                }
                
                bytes = udp.EndReceive(result, ref from);
                udp.Close();
            }
            
            using (MemoryStream m = new MemoryStream(bytes, false))
            {
                ISnmpMessage message = MessageFactory.ParseMessages(m)[0];
                
                if (message.Pdu.TypeCode != SnmpType.GetResponsePdu)
                {
                    throw SharpOperationException.Create("wrong response type", receiver);
                }
                
                GetResponseMessage response = (GetResponseMessage)message;
                if (response.SequenceNumber != SequenceNumber)
                {
                    throw SharpOperationException.Create("wrong response sequence", receiver);
                }
                
                if (response.ErrorStatus != ErrorCode.NoError)
                {
                    throw SharpErrorException.Create(
                        "error in response",
                        receiver,
                        response);
                }
                
                ByteTool.Capture(bytes); // log response
            }
        }
        
        internal int SequenceNumber
        {
            get
            {
                return _sequenceNumber;
            }
        }
        
        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return _bytes;
        }

        /// <summary>
        /// PDU.
        /// </summary>
        public ISnmpPdu Pdu
        {
            get
            {
                return _pdu;
            }
        }
        
        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="InformRequestMessage"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "INFORM request message: version: " + _version + "; " + _community + "; " + _pdu;
        }
    }
}
