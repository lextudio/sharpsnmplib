using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// GET request message.
    /// </summary>
    public class GetRequestMessage : ISnmpMessage
    {
        private Header _header;
        private SecurityParameters _parameters;
        private Scope _scope;
        private readonly VersionCode _version;
        private byte[] _bytes;
        private ProviderPair _record;

        /// <summary>
        /// Creates a <see cref="GetRequestMessage"/> with all contents.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="version">Protocol version</param>
        /// <param name="community">Community name</param>
        /// <param name="variables">Variables</param>
        public GetRequestMessage(int requestId, VersionCode version, OctetString community, IList<Variable> variables)
        {
            if (version == VersionCode.V3)
            {
                throw new ArgumentException("Please use overload constructor for v3", "version");
            }
            
            _version = version;
            _header = Header.Empty;
            _parameters = new SecurityParameters(null, null, null, community, null, null);
            GetRequestPdu pdu = new GetRequestPdu(
                new Integer32(requestId),
                ErrorCode.NoError,
                new Integer32(0),
                variables);
            _scope = new Scope(null, null, pdu);
            _record = ProviderPair.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRequestMessage"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="messageId">The message id.</param>
        /// <param name="requestId">The request id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="variables">The variables.</param>
        /// <param name="record">The record.</param>
        public GetRequestMessage(VersionCode version, int messageId, int requestId, OctetString userName, IList<Variable> variables, ProviderPair record)
        {
            _version = version;
            if (record == null)
            {
                throw new ArgumentException("record");
            }

            _record = record;
            SecurityLevel recordToSecurityLevel = record.ToSecurityLevel();
            recordToSecurityLevel |= SecurityLevel.Reportable;
            byte b = (byte)recordToSecurityLevel;
            _header = new Header(new Integer32(messageId), new Integer32(0xFFE3), new OctetString(new byte[] { b }), new Integer32(3));
            // TODO: add salt later.
            _parameters = new SecurityParameters(
                OctetString.Empty, 
                new Integer32(0), 
                new Integer32(0), 
                userName, 
                _record.Authentication.CleanDigest,
                new OctetString(_record.Privacy.Salt));
            GetRequestPdu pdu = new GetRequestPdu(
                new Integer32(requestId), 
                ErrorCode.NoError,
                new Integer32(0),
                variables);
            _scope = new Scope(OctetString.Empty, OctetString.Empty, pdu);
        }
        
        internal GetRequestMessage(VersionCode version, Header header, SecurityParameters parameters, Scope scope, ProviderPair record)
        {
            if (record == null)
            {
                throw new ArgumentException("record");
            }

            _version = version;
            _header = header;
            _parameters = parameters;
            _scope = scope;
            _record = record;
        }

        internal GetRequestMessage(GetRequestMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            if (message.Version != VersionCode.V3)
            {
                throw new ArgumentException("only v3 message can be cloned", "message");
            }

            _version = message._version;
            _header = message._header;//.Clone();
            _parameters = message._parameters.Clone();
            _scope = message._scope;//.Clone;
            _record = ProviderPair.Default;
        }

        /// <summary>
        /// Discovers the specified timeout.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <param name="receiver">The receiver.</param>
        /// <param name="requestId">The request id.</param>
        /// <param name="messageId">The message id.</param>
        /// <returns></returns>
        public ReportMessage Discover(int timeout, IPEndPoint receiver, int requestId, int messageId)
        {
            GetRequestMessage discovery = new GetRequestMessage(
                VersionCode.V3,
                new Header(
                    new Integer32(messageId),
                    new Integer32(0xFFE3),
                    new OctetString(new byte[] { (byte)SecurityLevel.Reportable }),
                    new Integer32(3)),
                new SecurityParameters(
                    OctetString.Empty,
                    new Integer32(0),
                    new Integer32(0),
                    OctetString.Empty,
                    OctetString.Empty,
                    OctetString.Empty),
                new Scope(
                    OctetString.Empty,
                    OctetString.Empty,
                    new GetRequestPdu(new Integer32(requestId), ErrorCode.NoError, new Integer32(0), new List<Variable>())),
                    ProviderPair.Default
               );
            ReportMessage report = (ReportMessage)ByteTool.GetReply(receiver, discovery.ToBytes(), 0x2C6B, timeout, new UserRegistry(), Messenger.GetSocket(receiver));
            report.Update(this); // {.1.3.6.1.6.3.15.1.1.4.0} Counter (number of counts)
            return report;
        }

        /// <summary>
        /// Authenticates this instance.
        /// </summary>
        public void Authenticate()
        {
            _parameters.AuthenticationParameters = _record.Authentication.ComputeHash(this);
        }
        
        /// <summary>
        /// Gets the message ID.
        /// </summary>
        /// <value>The message ID.</value>
        /// <remarks>For v3, message ID is different from request ID. For v1 and v2c, they are the same.</remarks>
        public int MessageId
        {
            get
            {
                return (_header == null) ? RequestId : _header.MessageId;
            }
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
        /// Sends this <see cref="GetRequestMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="receiver">Agent.</param>
        /// <returns></returns>
        public GetResponseMessage GetResponse(int timeout, IPEndPoint receiver)
        {
            return GetResponse(timeout, receiver, Messenger.GetSocket(receiver));
        }

        /// <summary>
        /// Sends this <see cref="GetRequestMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="receiver">Agent.</param>
        /// <param name="udpSocket">The UDP <see cref="Socket"/> to use to send/receive.</param>
        /// <returns></returns>
        public GetResponseMessage GetResponse(int timeout, IPEndPoint receiver, Socket udpSocket)
        {
            UserRegistry registry = new UserRegistry();
            if (Version == VersionCode.V3)
            {
                Authenticate();
                registry.Add(_parameters.UserName, _record);
            }

            return ByteTool.GetResponse(receiver, ToBytes(), RequestId, timeout, registry, udpSocket);
        }

        ///// <summary>
        ///// Sends this <see cref="GetRequestMessage"/> and handles the response from agent asynchronously.
        ///// </summary>
        ///// <param name="timeout">Timeout.</param>
        ///// <param name="receiver">Agent.</param>
        ///// <param name="callback">The callback called once the response has been received.</param>
        //public void BeginGetResponse(int timeout, IPEndPoint receiver, GetResponseCallback callback)
        //{
        //    ByteTool.BeginGetResponse(receiver, _bytes, RequestId, timeout, callback);
        //}

        ///// <summary>
        ///// Sends this <see cref="GetRequestMessage"/> and handles the response from agent asynchronously.
        ///// </summary>
        ///// <param name="timeout">Timeout.</param>
        ///// <param name="receiver">Agent.</param>
        ///// <param name="callback">The callback called once the response has been received.</param>
        ///// <param name="udpSocket">The UDP <see cref="Socket"/> to use to send/receive.</param>
        //public void BeginGetResponse(int timeout, IPEndPoint receiver, GetResponseCallback callback, Socket udpSocket)
        //{
        //    ByteTool.BeginGetResponse(receiver, _bytes, RequestId, timeout, callback, udpSocket);
        //}

        /// <summary>
        /// Sends this <see cref="GetRequestMessage"/> and handles the response from agent asynchronously.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="receiver">Agent.</param>
        /// <param name="callback">The callback called once the response has been received.</param>
        /// <param name="udpSocket">The UDP <see cref="Socket"/> to use to send/receive.</param>
        public void BeginGetResponseRaw(int timeout, IPEndPoint receiver, GetResponseRawCallback callback, Socket udpSocket)
        {
            ByteTool.BeginGetResponseRaw(receiver, _bytes, timeout, callback, udpSocket);
        }

        /// <summary>
        /// Version.
        /// </summary>
        public VersionCode Version
        {
            get { return _version; }
        }
        
        /// <summary>
        /// Request ID.
        /// </summary>
        public int RequestId
        {
            get { return _scope.Pdu.RequestId.ToInt32(); }
        }

        /// <summary>
        /// Community name.
        /// </summary>
        public OctetString Community
        {
            get { return _parameters.UserName; }
        }

        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <value>The level.</value>
        public SecurityLevel Level
        {
            get
            {
                return _record.ToSecurityLevel();
            }
        }
        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            //if (_bytes == null)
            //{
                _bytes = ByteTool.PackMessage(_version, _record.Privacy, _header, _parameters, _scope).ToBytes();
            //}
            
            return _bytes;
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
        /// Returns a <see cref="string"/> that represents this <see cref="GetRequestMessage"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "GET request message: version: " + _version + "; " + Community + "; " + Pdu;
        }
    }
}
