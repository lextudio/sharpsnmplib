using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// GET request message.
    /// </summary>
    public class GetRequestMessage : ISnmpMessage
    {
        private readonly VersionCode _version;
        private readonly IList<Variable> _variables;
        private readonly byte[] _bytes;
        private readonly OctetString _community;
        private readonly ISnmpPdu _pdu;
        private readonly int _requestId;        

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
            
            if (body.Items.Count != 3)
            {
                throw new ArgumentException("wrong message body");
            }
            
            _community = (OctetString)body.Items[1];
            _version = (VersionCode)((Integer32)body.Items[0]).ToInt32();
            _pdu = (ISnmpPdu)body.Items[2];
            if (_pdu.TypeCode != SnmpType.GetRequestPdu)
            {
                throw new ArgumentException("wrong message type");
            }
            
            _requestId = ((GetRequestPdu)_pdu).RequestId;
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
