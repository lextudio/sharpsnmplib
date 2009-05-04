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
        private Header _header;
        private SecurityParameters _parameters;
        private Scope _scope;
        private readonly VersionCode _version;
        private byte[] _bytes;
        private SecurityRecord _record;

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
                requestId,
                ErrorCode.NoError,
                0,
                variables);
            _scope = new Scope(null, null, pdu);
            _record = SecurityRecord.Default;
        }

        public GetRequestMessage(int requestId, VersionCode version, int messageId, OctetString userName, IList<Variable> variables, SecurityRecord record)
        {
            _version = version;
            if (record == null)
            {
                throw new ArgumentException("record");
            }

            _record = record;
            _header = new Header(new Integer32(messageId), new Integer32(0xFFE3), new OctetString(new byte[] { (byte)record.ToSecurityLevel() }), new Integer32(3));
            _parameters = new SecurityParameters(OctetString.Empty, new Integer32(0), new Integer32(0), userName, OctetString.Empty, OctetString.Empty);
            GetRequestPdu pdu = new GetRequestPdu(
                requestId,
                ErrorCode.NoError,
                0,
                variables);
            _scope = new Scope(OctetString.Empty, OctetString.Empty, pdu);
        }
        
        internal GetRequestMessage(VersionCode version, Header header, SecurityParameters parameters, Scope scope, SecurityRecord record)
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
            
            _version = (VersionCode)((Integer32)body[0]).ToInt32();
            
            if (body.Count == 3)
            {
                _header = Header.Empty;
                _parameters = new SecurityParameters(null, null, null, (OctetString)body[1], null, null);
                _scope = new Scope(null, null, (ISnmpPdu)body[2]);
                return;
            }

            if (body.Count == 4)
            {
                _header = new Header(body[1]);
                _parameters = _record.Authentication.Decrypt(body[2]);
                _scope = _record.Privacy.Decrypt(body[3]);
                return;
            }

            throw new ArgumentException("wrong message body");
        }

        public ISegment[] Discover(int timeout, IPEndPoint receiver, int requestId, int messageId, Socket socket)
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
                    new GetRequestPdu(requestId, ErrorCode.NoError, 0, new List<Variable>())),
                    SecurityRecord.Default
               );
            ReportMessage report = (ReportMessage)ByteTool.GetReply(receiver, discovery.ToBytes(), 0x2C6B, timeout, socket);
            report.Update(this);

            ObjectIdentifier oid = report.Pdu.Variables[0].Id;
            Counter32 value = (Counter32)report.Pdu.Variables[0].Data;
            return new ISegment[] { report.Parameters, report.Scope };
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
        
        public GetResponseMessage GetResponseV3(int timeout, IPEndPoint receiver, Socket udpSocket)
        {
            //Discover(timeout, receiver, udpSocket);
            return ByteTool.GetResponse(receiver, ToBytes(), RequestId, timeout, udpSocket);
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
            get { return _scope.Pdu.RequestId; }
        }

        /// <summary>
        /// Community name.
        /// </summary>
        public OctetString Community
        {
            get { return _parameters.UserName; }
        }
        
        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            if (_bytes == null)
            {                
                _bytes = ByteTool.PackMessage(_version, _header, _parameters, _scope).ToBytes();
            }
            
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

        public SecurityParameters Parameters
        {
            get { return _parameters; }
        }

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
