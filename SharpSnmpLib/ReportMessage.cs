/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/8/3
 * Time: 15:36
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// REPORT message.
    /// </summary>
    public class ReportMessage : ISnmpMessage
    {
        private readonly VersionCode _version;
        private readonly IList<Variable> _variables;
        private readonly byte[] _bytes;
        private readonly OctetString _community;
        private readonly ISnmpPdu _pdu;
        private readonly int _requestId;

        /// <summary>
        /// Creates a <see cref="ReportMessage"/> with all contents.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="version">Protocol version</param>
        /// <param name="community">Community name</param>
        /// <param name="variables">Variables</param>
        public ReportMessage(int requestId, VersionCode version, OctetString community, IList<Variable> variables)
        {
            _version = version;
            _community = community;
            _variables = variables;
            ReportPdu pdu = new ReportPdu(
                requestId,
                ErrorCode.NoError,
                0,
                _variables);
            _requestId = pdu.RequestId;
            _bytes = pdu.ToMessageBody(_version, _community).ToBytes();
        }
        
        /// <summary>
        /// Creates a <see cref="ReportMessage"/> with a specific <see cref="Sequence"/>.
        /// </summary>
        /// <param name="body">Message body</param>
        public ReportMessage(Sequence body)
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
            if (_pdu.TypeCode != SnmpType.ReportPdu)
            {
                throw new ArgumentException("wrong message type");
            }
            
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
        /// Sends this <see cref="ReportMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="receiver">Agent.</param>
        /// <returns></returns>
        public GetResponseMessage GetResponse(int timeout, IPEndPoint receiver)
        {
            return ByteTool.GetResponse(receiver, _bytes, RequestId, timeout);
        }

        /// <summary>
        /// Sends this <see cref="ReportMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="receiver">Agent.</param>
        /// <param name="socket">The socket.</param>
        /// <returns></returns>
        public GetResponseMessage GetResponse(int timeout, IPEndPoint receiver, Socket socket)
        {
            return ByteTool.GetResponse(receiver, _bytes, RequestId, timeout, socket);
        }
        
        [Obsolete("Use RequestId instead.")]
        internal int SequenceNumber
        {
            get
            {
                return _requestId;
            }
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
        /// Returns a <see cref="string"/> that represents this <see cref="ReportMessage"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "REPORT request message: version: " + _version + "; " + _community + "; " + _pdu;
        }
    }
}
