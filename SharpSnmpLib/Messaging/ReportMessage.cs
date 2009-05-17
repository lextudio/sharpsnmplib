using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/8/3
 * Time: 15:36
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// REPORT message.
    /// </summary>
    public class ReportMessage : ISnmpMessage
    {
        private readonly VersionCode _version;
        private SecurityParameters _parameters;
        private Scope _scope;
        private Header _header;
        private ProviderPair _pair = ProviderPair.Default;
      
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
            _parameters = new SecurityParameters((OctetString)body[2]);
            _scope = _pair.Privacy.Decrypt(body[3], _parameters);
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
        public ISnmpMessage GetResponse(int timeout, IPEndPoint receiver)
        {
            return MessageFactory.GetResponse(receiver, ToBytes(), RequestId, timeout, new UserRegistry(), Messenger.GetSocket(receiver));
        }

        /// <summary>
        /// Sends this <see cref="ReportMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="receiver">Agent.</param>
        /// <param name="socket">The socket.</param>
        /// <returns></returns>
        public ISnmpMessage GetResponse(int timeout, IPEndPoint receiver, Socket socket)
        {
            return MessageFactory.GetResponse(receiver, ToBytes(), RequestId, timeout, new UserRegistry(), socket);
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
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public VersionCode Version
        {
            get { return _version; }
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

        private void Update(ISnmpMessage request)
        {
            request.Parameters.EngineId = Parameters.EngineId;
            request.Parameters.EngineBoots = Parameters.EngineBoots;
            request.Parameters.EngineTime = Parameters.EngineTime;

            request.Scope.ContextEngineId = Scope.ContextEngineId;
            request.Scope.ContextName = Scope.ContextName;
        }

        /// <summary>
        /// Discovers the specified timeout.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="receiver">The receiver.</param>
        /// <param name="requestId">The request id.</param>
        /// <param name="messageId">The message id.</param>
        /// <returns></returns>
        public static ReportMessage Discover(ISnmpMessage message, int timeout, IPEndPoint receiver, int requestId, int messageId)
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
                    ProviderPair.Default
               );
            ReportMessage report = (ReportMessage)MessageFactory.GetReply(receiver, discovery.ToBytes(), 0x2C6B, timeout, new UserRegistry(), Messenger.GetSocket(receiver));
            report.Update(message); // {.1.3.6.1.6.3.15.1.1.4.0} Counter (number of counts)
            return report;
        }
    }
}
