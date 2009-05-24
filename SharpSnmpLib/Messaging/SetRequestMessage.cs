using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// SET request message.
    /// </summary>
    public class SetRequestMessage : ISnmpMessage
    {
        private readonly VersionCode _version;
        private Header _header;
        private SecurityParameters _parameters;
        private Scope _scope;
        private ProviderPair _pair;       

        /// <summary>
        /// Creates a <see cref="SetRequestMessage"/> with all contents.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="version">Protocol version</param>
        /// <param name="community">Community name</param>
        /// <param name="variables">Variables</param>
        public SetRequestMessage(int requestId, VersionCode version, OctetString community, IList<Variable> variables)
        {
            if (version == VersionCode.V3)
            {
                throw new ArgumentException("only v1 and v2c are supported", "version");
            }
            
            _version = version;
            _header = Header.Empty;
            _parameters = new SecurityParameters(null, null, null, community, null, null);
            SetRequestPdu pdu = new SetRequestPdu(
                requestId,
                ErrorCode.NoError,
                0,
                variables);
            _scope = new Scope(null, null, pdu);
            _pair = ProviderPair.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetRequestMessage"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="messageId">The message id.</param>
        /// <param name="requestId">The request id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="variables">The variables.</param>
        /// <param name="pair">The pair.</param>
        public SetRequestMessage(VersionCode version, int messageId, int requestId, Lextm.SharpSnmpLib.OctetString userName, List<Variable> variables, ProviderPair pair, ReportMessage report)
        {
            if (version != VersionCode.V3)
            {
                throw new ArgumentException("only v3 is supported", "version");
            }

            _version = version;
            if (pair == null)
            {
                throw new ArgumentException("pair");
            }

            _pair = pair;
            SecurityLevel recordToSecurityLevel = pair.ToSecurityLevel();
            recordToSecurityLevel |= SecurityLevel.Reportable;
            byte b = (byte)recordToSecurityLevel;
            // TODO: define more constants.
            _header = new Header(new Integer32(messageId), new Integer32(0xFFE3), new OctetString(new byte[] { b }), new Integer32(3));
            _parameters = new SecurityParameters(
                report.Parameters.EngineId, 
                report.Parameters.EngineBoots, 
                report.Parameters.EngineTime,
                userName, 
                _pair.Authentication.CleanDigest,
                _pair.Privacy.Salt);
            SetRequestPdu pdu = new SetRequestPdu(
                requestId, 
                ErrorCode.NoError,
                0,
                variables);
            _scope = new Scope(report.Scope.ContextEngineId, report.Scope.ContextName, pdu);
        }

        internal SetRequestMessage(VersionCode version, Header header, SecurityParameters parameters, Scope scope, ProviderPair record)
        {
            if (record == null)
            {
                throw new ArgumentException("record");
            }

            _version = version;
            _header = header;
            _parameters = parameters;
            _scope = scope;
            _pair = record;
        }

        /// <summary>
        /// Sends this <see cref="SetRequestMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="receiver">Agent.</param>
        /// <returns></returns>
        public ISnmpMessage GetResponse(int timeout, IPEndPoint receiver)
        {
            return GetResponse(timeout, receiver, Messenger.GetSocket(receiver));
        }

        /// <summary>
        /// Sends this <see cref="SetRequestMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="receiver">Agent.</param>
        /// <param name="udpSocket">The UDP <see cref="Socket"/> to use to send/receive.</param>
        /// <returns></returns>
        public ISnmpMessage GetResponse(int timeout, IPEndPoint receiver, Socket udpSocket)
        {
            UserRegistry registry = new UserRegistry();
            if (Version == VersionCode.V3)
            {
                MessageFactory.Authenticate(this, _pair);
                registry.Add(_parameters.UserName, _pair);
            }

            return MessageFactory.GetResponse(receiver, ToBytes(), RequestId, timeout, registry, udpSocket);
        }
        
        /// <summary>
        /// Returns a <see cref="string"/> that represents this <see cref="SetRequestMessage"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "SET request message: version: " + _version + "; " + _parameters.UserName + "; " + _scope.Pdu;
        }
        
        internal int RequestId
        {
            get { return _scope.Pdu.RequestId.ToInt32(); }
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
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return MessageFactory.PackMessage(_version, _pair.Privacy, _header, _parameters, _scope).ToBytes();
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
            get { return _parameters; }
        }

        /// <summary>
        /// Gets the scope.
        /// </summary>
        /// <value>The scope.</value>
        public Scope Scope
        {
            get { return _scope; }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public VersionCode Version
        {
            get { return _version; }
        }
    }
}
