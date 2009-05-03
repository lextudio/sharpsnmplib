/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/8/3
 * Time: 15:36
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using Lextm.SharpSnmpLib.Security;
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
        private IList<Variable> _variables;
        private readonly byte[] _bytes;
        private readonly OctetString _community;
        private ISnmpPdu _pdu;
        private int _requestId;
        private SecurityParameters _parameters;
        private ISegment _scope;
        
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
            _bytes = ByteTool.PackMessage(_version, _community, pdu).ToBytes();
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
            
            if (body.Count != 4)
            {
                throw new ArgumentException("wrong message body");
            }            
            
            int v = ((Integer32)body[0]).ToInt32() - 1;
            _version = (VersionCode)v;
            if (_version != VersionCode.V3)
            {
                throw new ArgumentException("REPORT message is for v3 only");
            }

            _bytes = body.ToBytes();
            
            // TODO: ignore header as it is default.
            // Sequence headerData = (Sequence)body[1];
            // _messageId = ((Integer32)headerData[0]).ToInt32();
            // Integer32 maxMessageSize = (Integer32)headerData[1]; // 0xFF E3
            
//            byte messageFlags = ((OctetString)headerData[2]).GetRaw()[0];
//            _level = (SecurityLevel)messageFlags;
//            SecurityModel model = (SecurityModel)((Integer32)headerData[3]).ToInt32();
            
            SecurityParameters securityParameters = DefaultAuthenticationProvider.Instance.Decrypt(body[2]);
            _community = securityParameters.User;

            Scope scope = DefaultPrivacyProvider.Instance.Decrypt(body[3]);

            ProcessPdu(scope.Pdu);
        }

        private void ProcessPdu(ISnmpData pdu)
        {
            _pdu = (ISnmpPdu)pdu;
            if (_pdu.TypeCode != SnmpType.ReportPdu)
            {
                throw new ArgumentException("wrong message type");
            }

            _requestId = ((ReportPdu)_pdu).RequestId;
            _variables = _pdu.Variables;
        }
        
        /// <summary>
        /// Security parameters.
        /// </summary>
        public SecurityParameters Parameters 
        {
            get { return _parameters; }
        }
        
        /// <summary>
        /// Variables.
        /// </summary>
        public IList<Variable> Variables
        {
            get { return _variables; }
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
        
        internal int RequestId
        {
            get { return _requestId; }
        }

        /// <summary>
        /// Gets the scope.
        /// </summary>
        /// <value>The scope.</value>
        public ISegment Scope
        {
            get
            {
                return _scope;
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
            get { return _pdu; }
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
