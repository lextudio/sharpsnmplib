// INFORM request message type.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

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
using System.Globalization;
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
        private readonly Header _header;
        private readonly SecurityParameters _parameters;
        private readonly Scope _scope;
        private readonly IPrivacyProvider _privacy;
        private readonly uint _time;
        private readonly ObjectIdentifier _enterprise;

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
            if (variables == null)
            {
                throw new ArgumentNullException("variables");
            }
            
            if (enterprise == null)
            {
                throw new ArgumentNullException("enterprise");
            }
            
            if (community == null)
            {
                throw new ArgumentNullException("community");
            }
            
            if (version == VersionCode.V3)
            {
                throw new ArgumentException("only v1 and v2c are supported", "version");
            }
            
            _version = version;
            _enterprise = enterprise;
            _time = time;
            _header = Header.Empty;
            _parameters = new SecurityParameters(null, null, null, community, null, null);
            InformRequestPdu pdu = new InformRequestPdu(
                requestId,
                enterprise,
                time,
                variables);
            _scope = new Scope(pdu);
            _privacy = DefaultPrivacyProvider.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InformRequestMessage"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="messageId">The message id.</param>
        /// <param name="requestId">The request id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="enterprise">The enterprise.</param>
        /// <param name="time">The time.</param>
        /// <param name="variables">The variables.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <param name="report">The report.</param>
        [CLSCompliant(false)]
        public InformRequestMessage(VersionCode version, int messageId, int requestId, OctetString userName, ObjectIdentifier enterprise, uint time, IList<Variable> variables, IPrivacyProvider privacy, ISnmpMessage report)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }
            
            if (variables == null)
            {
                throw new ArgumentNullException("variables");
            }
            
            if (version != VersionCode.V3)
            {
                throw new ArgumentException("only v3 is supported", "version");
            }

            if (enterprise == null)
            {
                throw new ArgumentNullException("enterprise");
            }

            if (report == null)
            {
                throw new ArgumentNullException("report");
            }
            
            if (privacy == null)
            {
                throw new ArgumentNullException("privacy");
            }

            _version = version;
            _privacy = privacy;
            _enterprise = enterprise;
            _time = time;
            Levels recordToSecurityLevel = PrivacyProviderExtension.ToSecurityLevel(privacy);
            recordToSecurityLevel |= Levels.Reportable;
            byte b = (byte)recordToSecurityLevel;
            
            // TODO: define more constants.
            _header = new Header(new Integer32(messageId), new Integer32(0xFFE3), new OctetString(new[] { b }), new Integer32(3));
            _parameters = new SecurityParameters(
                report.Parameters.EngineId,
                report.Parameters.EngineBoots,
                report.Parameters.EngineTime,
                userName,
                _privacy.AuthenticationProvider.CleanDigest,
                _privacy.Salt);
            var pdu = new InformRequestPdu(
                requestId,
                enterprise,
                time,
                variables);
            _scope = new Scope(report.Scope.ContextEngineId, report.Scope.ContextName, pdu);
        }

        internal InformRequestMessage(VersionCode version, Header header, SecurityParameters parameters, Scope scope, IPrivacyProvider privacy)
        {
            if (scope == null)
            {
                throw new ArgumentNullException("scope");
            }
            
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            
            if (header == null)
            {
                throw new ArgumentNullException("header");
            }
            
            if (privacy == null)
            {
                throw new ArgumentNullException("privacy");
            }

            _version = version;
            _header = header;
            _parameters = parameters;
            _scope = scope;
            _privacy = privacy;
            InformRequestPdu pdu = (InformRequestPdu)scope.Pdu;
            _enterprise = pdu.Enterprise;
            _time = pdu.TimeStamp;
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
        /// Gets the community.
        /// </summary>
        /// <value>The community.</value>
        public OctetString Community
        {
            get { return _parameters.UserName; }
        }

        /// <summary>
        /// Gets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        [CLSCompliant(false)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TimeStamp")]
        public uint TimeStamp
        {
            get { return _time; }
        }

        /// <summary>
        /// Enterprise.
        /// </summary>
        public ObjectIdentifier Enterprise
        {
            get { return _enterprise; }
        }

        /// <summary>
        /// Sends this <see cref="InformRequestMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="timeout">The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</param>
        /// <param name="receiver">Agent.</param>
        /// <returns></returns>
        public ISnmpMessage GetResponse(int timeout, IPEndPoint receiver)
        {
            if (receiver == null)
            {
                throw new ArgumentNullException("receiver");
            }

            using (Socket socket = Helper.GetSocket(receiver))
            {
                return GetResponse(timeout, receiver, socket);
            }
        }

        /// <summary>
        /// Sends this <see cref="InformRequestMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="timeout">The time-out value, in milliseconds. The default value is 0, which indicates an infinite time-out period. Specifying -1 also indicates an infinite time-out period.</param>
        /// <param name="receiver">Agent.</param>
        /// <param name="udpSocket">The UDP <see cref="Socket"/> to use to send/receive.</param>
        /// <returns></returns>
        public ISnmpMessage GetResponse(int timeout, IPEndPoint receiver, Socket udpSocket)
        {
            if (udpSocket == null)
            {
                throw new ArgumentNullException("udpSocket");
            }

            if (receiver == null)
            {
                throw new ArgumentNullException("receiver");
            }

            UserRegistry registry = UserRegistry.Default;
            if (Version == VersionCode.V3)
            {
                Helper.Authenticate(this, _privacy);
                registry.Add(_parameters.UserName, _privacy);
            }

            return MessageFactory.GetResponse(receiver, ToBytes(), MessageId, timeout, registry, udpSocket);
        }
        
        /// <summary>
        /// Gets the request ID.
        /// </summary>
        /// <value>The request ID.</value>
        public int RequestId
        {
            get
            {
                return _scope.Pdu.RequestId.ToInt32();
            }
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
                return (_header == Header.Empty) ? RequestId : _header.MessageId;
            }
        }
        
        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return Helper.PackMessage(_version, _privacy, _header, _parameters, _scope).ToBytes();
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
        /// Returns a <see cref="string"/> that represents this <see cref="InformRequestMessage"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString(null);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="objects">The objects.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        [CLSCompliant(false)]
        public string ToString(IObjectRegistry objects)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "INFORM request message: time stamp: {0}; community: {1}; enterprise: {2}; varbind count: {3}",
                TimeStamp.ToString(CultureInfo.InvariantCulture),
                Community,
                Enterprise.ToString(objects),
                Variables.Count.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Generates the response message.
        /// </summary>
        /// <returns></returns>
        [Obsolete("Please use SnmpEngine instead.")]
        public GetResponseMessage GenerateResponse()
        {
            // TODO: make more efficient here.
            if (_version == VersionCode.V2)
            {
                InformRequestPdu pdu = (InformRequestPdu)_scope.Pdu;
                return new GetResponseMessage(_scope.Pdu.RequestId.ToInt32(), _version, _parameters.UserName, ErrorCode.NoError, 0, pdu.AllVariables);
            }

            // TODO: implement this later.
            return null;
        }
    }
}
