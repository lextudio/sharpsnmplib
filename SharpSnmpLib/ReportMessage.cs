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
        private byte[] _bytes;
        private SecurityParameters _parameters;
        private Scope _scope;
        private Header _header;
        private IPrivacyProvider _privacy = DefaultPrivacyProvider.Instance;
        private IAuthenticationProvider _authentication = DefaultAuthenticationProvider.Instance;
        
//        /// <summary>
//        /// Creates a <see cref="ReportMessage"/> with all contents.
//        /// </summary>
//        /// <param name="requestId">The request id.</param>
//        /// <param name="version">Protocol version</param>
//        /// <param name="community">Community name</param>
//        /// <param name="variables">Variables</param>
//        public ReportMessage(int requestId, VersionCode version, OctetString community, IList<Variable> variables)
//        {
//            _version = version;
//            _community = community;
//            _variables = variables;
//            ReportPdu pdu = new ReportPdu(
//                requestId,
//                ErrorCode.NoError,
//                0,
//                _variables);
//            _requestId = pdu.RequestId;
//            _bytes = ByteTool.PackMessage(_version, _community, pdu).ToBytes();
//        }
        
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
            
            _version = (VersionCode)((Integer32)body[0]).ToInt32();
            _header = new Header((Sequence)body[1]);
            _parameters = Authentication.Decrypt(body[2]);
            _scope = Privacy.Decrypt(body[3]);
        }
        
        /// <summary>
        /// Gets or sets the privacy method.
        /// </summary>
        /// <value>The privacy method.</value>
        public IPrivacyProvider Privacy
        {
            get { return _privacy; }
            set { _privacy = value; }
        }

        /// <summary>
        /// Gets or sets the authentication method.
        /// </summary>
        /// <value>The authentication method.</value>
        public IAuthenticationProvider Authentication
        {
            get { return _authentication; }
            set { _authentication = value; }
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
            get { return _scope.Pdu.Variables; }
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
            get { return _scope.Pdu.RequestId.ToInt32(); }
        }

        /// <summary>
        /// Gets the scope.
        /// </summary>
        /// <value>The scope.</value>
        public Scope Scope
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
            get { return _scope.Pdu; }
        }
        
        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="ReportMessage"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "REPORT request message: version: " + _version + "; " + _parameters.UserName + "; " + _scope.Pdu;
        }

        internal void Update(ISnmpMessage request)
        {
            request.Parameters.EngineId = Parameters.EngineId;
            request.Parameters.EngineBoots = Parameters.EngineBoots;
            request.Parameters.EngineTime = Parameters.EngineTime;

            request.Scope.ContextEngineId = Scope.ContextEngineId;
            request.Scope.ContextName = Scope.ContextName;
        }
    }
}
