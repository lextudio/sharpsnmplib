/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/11
 * Time: 12:33
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
    /// GETNEXT request message.
    /// </summary>
    public class GetNextRequestMessage : ISnmpMessage
    {
        private readonly VersionCode _version;
        private readonly IList<Variable> _variables;
        private readonly byte[] _bytes;
        
        // TODO: [Obsolete]
        private readonly IPAddress _agent;
        private readonly OctetString _community;
        private readonly ISnmpPdu _pdu;
        private readonly int _sequenceNumber;
        
        /// <summary>
        /// Creates a <see cref="GetNextRequestMessage"/> with all contents.
        /// </summary>
        /// <param name="version">Protocol version</param>
        /// <param name="agent">Agent address</param>
        /// <param name="community">Community name</param>
        /// <param name="variables">Variables</param>
        [Obsolete("Please use overload version instead.")]
        public GetNextRequestMessage(VersionCode version, IPAddress agent, OctetString community, IList<Variable> variables)
        {
            _version = version;
            _agent = agent;
            _community = community;
            _variables = variables;
            GetNextRequestPdu pdu = new GetNextRequestPdu(
                ErrorCode.NoError,
                0,
                _variables);
            _sequenceNumber = pdu.SequenceNumber;
            _bytes = pdu.ToMessageBody(_version, _community).ToBytes();
        }

        /// <summary>
        /// Creates a <see cref="GetNextRequestMessage"/> with all contents.
        /// </summary>
        /// <param name="version">Protocol version</param>
        /// <param name="community">Community name</param>
        /// <param name="variables">Variables</param>
        public GetNextRequestMessage(VersionCode version, OctetString community, IList<Variable> variables)
        {
            _version = version;
            _community = community;
            _variables = variables;
            GetNextRequestPdu pdu = new GetNextRequestPdu(
                ErrorCode.NoError,
                0,
                _variables);
            _sequenceNumber = pdu.SequenceNumber;
            _bytes = pdu.ToMessageBody(_version, _community).ToBytes();
        }   

        /// <summary>
        /// Creates a <see cref="GetNextRequestMessage"/> with a specific <see cref="Sequence"/>.
        /// </summary>
        /// <param name="body">Message body</param>
        public GetNextRequestMessage(Sequence body)
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
            if (_pdu.TypeCode != SnmpType.GetNextRequestPdu)
            {
                throw new ArgumentException("wrong message type");
            }
            
            _sequenceNumber = ((GetNextRequestPdu)_pdu).SequenceNumber;
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
        /// Sends this <see cref="GetNextRequestMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="port">Port number.</param>
        /// <returns></returns>
        [Obsolete("Please use GetResponse instead. Otherwise, make sure you called the obsolete constructor for this object.")]
        public IList<Variable> Send(int timeout, int port)
        {
            byte[] bytes = _bytes;
            IPEndPoint agent = new IPEndPoint(_agent, port);
            using (UdpClient udp = new UdpClient())
            {
                udp.Send(bytes, bytes.Length, agent);
                IPEndPoint from = new IPEndPoint(IPAddress.Any, 0);
                IAsyncResult result = udp.BeginReceive(null, this);
                result.AsyncWaitHandle.WaitOne(timeout, false);
                if (!result.IsCompleted)
                {
                    throw SharpTimeoutException.Create(_agent, timeout);
                }
                
                bytes = udp.EndReceive(result, ref from);
                udp.Close();
            }
            
            MemoryStream m = new MemoryStream(bytes, false);
            ISnmpMessage message = MessageFactory.ParseMessages(m)[0];
            if (message.Pdu.TypeCode != SnmpType.GetResponsePdu)
            {
                throw SharpOperationException.Create("wrong response type", _agent);
            }
            
            GetResponseMessage response = (GetResponseMessage)message;
            if (response.SequenceNumber != SequenceNumber)
            {
                throw SharpOperationException.Create("wrong response sequence", _agent);
            }
            
            if (response.ErrorStatus != ErrorCode.NoError)
            {
                throw SharpErrorException.Create(
                    "error in response",
                    _agent,
                    response.ErrorStatus,
                    response.ErrorIndex,
                    response.Variables[response.ErrorIndex - 1].Id);
            }
            
            return response.Variables;
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
        /// Returns a <see cref="string"/> that represents this <see cref="GetNextRequestMessage"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "GET NEXT request message: version: " + _version + "; " + _community + "; " + _pdu;
        }

        /// <summary>
        /// Sends this <see cref="GetNextRequestMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="receiver">Port number.</param>
        /// <returns></returns>
        public GetResponseMessage GetResponse(int timeout, IPEndPoint receiver)
        {
            return ByteTool.GetResponse(this, receiver, _bytes, SequenceNumber, timeout); 
        }
    }
}
