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
using System.Net;
using System.Net.Sockets;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// INFORM request message.
    /// </summary>
    public class InformRequestMessage : ISnmpMessage
    {
        private readonly VersionCode _version;
        private Header _header;
        private SecurityParameters _parameters;
        private Scope _scope;
        private ProviderPair _pair;

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
            if (version == VersionCode.V3)
            {
                throw new ArgumentException("only v1 and v2c are supported", "version");
            }
            
            _version = version;
            _header = Header.Empty;
            _parameters = new SecurityParameters(null, null, null, community, null, null);
            InformRequestPdu pdu = new InformRequestPdu(
                requestId,
                enterprise,
                time,
                variables);
            _scope = new Scope(null, null, pdu);
            _pair = ProviderPair.Default;
        }
        
        internal InformRequestMessage(VersionCode version, Header header, SecurityParameters parameters, Scope scope, ProviderPair record)
        {
            if (record == null)
            {
                throw new ArgumentNullException("record");
            }

            _version = version;
            _header = header;
            _parameters = parameters;
            _scope = scope;
            _pair = record;
        }
        
        /// <summary>
        /// Variables.
        /// </summary>
        public IList<Variable> Variables
        {
            get
            {
                return _scope.Pdu.Variables;
            }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public VersionCode Version
        {
            get { return _version; }
        }

        /// <summary>
        /// Sends the response.
        /// </summary>
        /// <param name="receiver">The receiver.</param>
  		[Obsolete("Use Listener.SendResponse instead")]      
		public void SendResponse(IPEndPoint receiver)
        {
            if (receiver == null)
            {
                throw new ArgumentNullException("receiver");
            }
            
            // TODO: make more efficient here.
            if (_version == VersionCode.V2)
            {
                InformRequestPdu pdu = (InformRequestPdu)_scope.Pdu;                
                new GetResponseMessage(_scope.Pdu.RequestId.ToInt32(), _version, _parameters.UserName, ErrorCode.NoError, 0, pdu.AllVariables).Send(receiver);
            }
            else
            {
                // TODO: do it for v3
            }
        }

        /// <summary>
        /// Sends this <see cref="InformRequestMessage"/> and handles the response from receiver (managers or agents).
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="receiver">Receiver.</param>
        /// <returns></returns>
        public ISnmpMessage GetResponse(int timeout, IPEndPoint receiver)
        {
            return MessageFactory.GetResponse(receiver, ToBytes(), RequestId, timeout, new UserRegistry(), Helper.GetSocket(receiver));
        }

        /// <summary>
        /// Sends this <see cref="InformRequestMessage"/> and handles the response from receiver (managers or agents).
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="receiver">Receiver.</param>
        /// <param name="socket">The socket.</param>
        /// <returns></returns>
        public ISnmpMessage GetResponse(int timeout, IPEndPoint receiver, Socket socket)
        {
            return MessageFactory.GetResponse(receiver, ToBytes(), RequestId, timeout, new UserRegistry(), socket);
        }
        
        internal int RequestId
        {
            get
            {
                return _scope.Pdu.RequestId.ToInt32();
            }
        }
        
        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return Helper.PackMessage(_version, _pair.Privacy, _header, _parameters, _scope).ToBytes();
        }

        /// <summary>
        /// PDU.
        /// </summary>
        public ISnmpPdu Pdu
        {
            get
            {
                return _scope.Pdu;
            }
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public SecurityParameters Parameters
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the scope.
        /// </summary>
        /// <value>The scope.</value>
        public Scope Scope
        {
            get { return null; }
        }
        
        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="InformRequestMessage"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "INFORM request message: version: " + _version + "; " + _parameters.UserName + "; " + _scope.Pdu;
        }
    }
}
