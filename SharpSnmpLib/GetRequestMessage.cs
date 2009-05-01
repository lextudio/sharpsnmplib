using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// GET request message.
    /// </summary>
    public class GetRequestMessage : ISnmpMessage
    {
        private int _messageId = 0;
        private readonly VersionCode _version;
        private IList<Variable> _variables;
        private readonly byte[] _bytes;
        private readonly OctetString _community;
        private ISnmpPdu _pdu;
        private int _requestId;
        private SecurityLevel _level;
        private IPrivacyProvider _privacy = DefaultPrivacyProvider.Instance;
        private IAuthenticationProvider _authentication = DefaultAuthenticationProvider.Instance;

        /// <summary>
        /// Creates a <see cref="GetRequestMessage"/> with all contents.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="version">Protocol version</param>
        /// <param name="community">Community name</param>
        /// <param name="variables">Variables</param>
        public GetRequestMessage(int requestId, VersionCode version, OctetString community, IList<Variable> variables)
        {
            _version = version;
            _community = community;
            _variables = variables;
            GetRequestPdu pdu = new GetRequestPdu(
                requestId,
                ErrorCode.NoError,
                0,
                _variables);
            _requestId = pdu.RequestId;
            _bytes = pdu.ToMessageBody(_version, _community).ToBytes();
        }

        /// <summary>
        /// Creates a <see cref="GetRequestMessage"/> with a specific <see cref="Sequence"/>.
        /// </summary>
        /// <param name="body">Message body</param>
        public GetRequestMessage(Sequence body)
        {
            if (body == null)
            {
                throw new ArgumentNullException("body");
            }
            
            if (body.Items.Count != 3 && body.Items.Count != 4)
            {
                throw new ArgumentException("wrong message body");
            }
               
            _version = (VersionCode)((Integer32)body.Items[0]).ToInt32();
            _bytes = body.ToBytes();
            _community = (OctetString)body[body.Items.Count - 2]; // re-used for securityParameters
 
            if (body.Items.Count == 3)
            {
                ProcessPdu(body.Items[2]);
                _messageId = _requestId;
                _level = SecurityLevel.None;
                return;
            }
            
            Sequence headerData = (Sequence)body.Items[1];
            _messageId = ((Integer32)headerData.Items[0]).ToInt32();
            Integer32 maxMessageSize = (Integer32)headerData.Items[1]; // 0xFF E3
            byte messageFlags = ((OctetString)headerData.Items[2]).GetRaw()[0];
            int authFlag = messageFlags & 0x1;
            int PrivFlag = messageFlags & 0x10;
            int reportableFlag = messageFlags & 0x100;
            _level = (SecurityLevel)messageFlags;

            SecurityModel model = (SecurityModel)((Integer32)headerData.Items[3]).ToInt32();

            Sequence scopedPdu = Privacy.Decrypt(body[3]);            
            OctetString contextEngineID = (OctetString)scopedPdu[0];
            OctetString contextName = (OctetString)scopedPdu[1];

            ProcessPdu(scopedPdu[2]);
        }

        private void ProcessPdu(ISnmpData pdu)
        {
            _pdu = (ISnmpPdu)pdu;
            if (_pdu.TypeCode != SnmpType.GetRequestPdu)
            {
                throw new ArgumentException("wrong message type");
            }

            _requestId = ((GetRequestPdu)_pdu).RequestId;
            _variables = _pdu.Variables;
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
        /// Gets or sets security level.
        /// </summary>
        /// <value>The level.</value>
        public SecurityLevel Level
        {
            get { return _level; }
            set { _level = value; }
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
                return _messageId;
            }
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
        /// Sends this <see cref="GetRequestMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="receiver">Agent.</param>
        /// <returns></returns>
        public GetResponseMessage GetResponse(int timeout, IPEndPoint receiver)
        {
            return ByteTool.GetResponse(receiver, _bytes, RequestId, timeout);
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
            return ByteTool.GetResponse(receiver, _bytes, RequestId, timeout, udpSocket);
        }

        /// <summary>
        /// Sends this <see cref="GetRequestMessage"/> and handles the response from agent asynchronously.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="receiver">Agent.</param>
        /// <param name="callback">The callback called once the response has been received.</param>
        public void BeginGetResponse(int timeout, IPEndPoint receiver, GetResponseCallback callback)
        {
            ByteTool.BeginGetResponse(receiver, _bytes, RequestId, timeout, callback);
        }

        /// <summary>
        /// Sends this <see cref="GetRequestMessage"/> and handles the response from agent asynchronously.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="receiver">Agent.</param>
        /// <param name="callback">The callback called once the response has been received.</param>
        /// <param name="udpSocket">The UDP <see cref="Socket"/> to use to send/receive.</param>
        public void BeginGetResponse(int timeout, IPEndPoint receiver, GetResponseCallback callback, Socket udpSocket)
        {
            ByteTool.BeginGetResponse(receiver, _bytes, RequestId, timeout, callback, udpSocket);
        }

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
            get { return _requestId; }
        }

        /// <summary>
        /// Community name.
        /// </summary>
        public OctetString Community
        {
            get { return _community; }
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
        /// Returns a <see cref="string"/> that represents this <see cref="GetRequestMessage"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "GET request message: version: " + _version + "; " + _community + "; " + _pdu;
        }
    }
}
