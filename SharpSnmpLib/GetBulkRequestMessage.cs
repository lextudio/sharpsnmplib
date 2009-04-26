/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/8/3
 * Time: 15:13
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
    /// GETBULK request message.
    /// </summary>
    public class GetBulkRequestMessage : ISnmpMessage
    {
        private readonly VersionCode _version;
        private readonly IList<Variable> _variables;
        private readonly byte[] _bytes;  
        private readonly OctetString _community;
        private readonly ISnmpPdu _pdu;
        private readonly int _requestId;        
        
        /// <summary>
        /// Creates a <see cref="GetBulkRequestMessage"/> with all contents.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="version">Protocol version.</param>
        /// <param name="community">Community name.</param>
        /// <param name="nonRepeaters">Non-repeaters.</param>
        /// <param name="maxRepetitions">Max repetitions.</param>
        /// <param name="variables">Variables.</param>
        public GetBulkRequestMessage(int requestId, VersionCode version, OctetString community, int nonRepeaters, int maxRepetitions, IList<Variable> variables)
        {
            _version = version;
            _community = community;
            _variables = variables;
            GetBulkRequestPdu pdu = new GetBulkRequestPdu(
                requestId,
                nonRepeaters,
                maxRepetitions,
                _variables);
            _requestId = requestId;
            _bytes = pdu.ToMessageBody(_version, _community).ToBytes();
        }

        /// <summary>
        /// Creates a <see cref="GetBulkRequestMessage"/> with a specific <see cref="Sequence"/>.
        /// </summary>
        /// <param name="body">Message body</param>
        public GetBulkRequestMessage(Sequence body)
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
            if (_pdu.TypeCode != SnmpType.GetBulkRequestPdu)
            {
                throw new ArgumentException("wrong message type");
            }
            
            _requestId = ((GetBulkRequestPdu)_pdu).RequestId;
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
        /// sends this <see cref="GetBulkRequestMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public GetResponseMessage GetResponse(int timeout, IPEndPoint receiver)
        {
            return ByteTool.GetResponse(receiver, _bytes, RequestId, timeout);
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <param name="receiver">The receiver.</param>
        /// <param name="socket">The socket.</param>
        /// <returns></returns>
        public GetResponseMessage GetResponse(int timeout, IPEndPoint receiver, Socket socket)
        {
            return ByteTool.GetResponse(receiver, _bytes, RequestId, timeout, socket);
        }    
        
        internal int RequestId
        {
            get { return _requestId; }
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
        /// Returns a <see cref="string"/> that represents this <see cref="GetBulkRequestMessage"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "GETBULK request message: version: " + _version + "; " + _community + "; " + _pdu;
        }
    }
}
