// GET request message type.
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

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// GET request message.
    /// </summary>
    public class GetRequestMessage : ISnmpMessage
    {
        private readonly VersionCode _version;
        private readonly Header _header;
        private readonly SecurityParameters _parameters;
        private readonly Scope _scope;
        private readonly ProviderPair _pair;

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
                throw new ArgumentException("only v1 and v2c are supported", "version");
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
            _pair = ProviderPair.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRequestMessage"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="messageId">The message id.</param>
        /// <param name="requestId">The request id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="variables">The variables.</param>
        /// <param name="pair">The pair.</param>
        /// <param name="report">The report.</param>
        public GetRequestMessage(VersionCode version, int messageId, int requestId, OctetString userName, IList<Variable> variables, ProviderPair pair, ISnmpMessage report)
        {
            if (version != VersionCode.V3)
            {
                throw new ArgumentException("only v3 is supported", "version");
            }

            if (report == null)
            {
                throw new ArgumentNullException("report");
            }
            
            if (pair == null)
            {
                throw new ArgumentNullException("pair");
            }

            _version = version;
            _pair = pair;
            Levels recordToSecurityLevel = pair.ToSecurityLevel();
            recordToSecurityLevel |= Levels.Reportable;
            byte b = (byte)recordToSecurityLevel;
            
            // TODO: define more constants.
            _header = new Header(new Integer32(messageId), new Integer32(0xFFE3), new OctetString(new[] { b }), new Integer32(3));
            _parameters = new SecurityParameters(
                report.Parameters.EngineId,
                report.Parameters.EngineBoots,
                report.Parameters.EngineTime,
                userName,
                _pair.Authentication.CleanDigest,
                _pair.Privacy.Salt);
            GetRequestPdu pdu = new GetRequestPdu(
                requestId,
                ErrorCode.NoError,
                0,
                variables);
            _scope = new Scope(report.Scope.ContextEngineId, report.Scope.ContextName, pdu);
        }
        
        internal GetRequestMessage(VersionCode version, Header header, SecurityParameters parameters, Scope scope, ProviderPair record)
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
        public ISnmpMessage GetResponse(int timeout, IPEndPoint receiver)
        {
            using (Socket socket = Helper.GetSocket(receiver))
            {
                return GetResponse(timeout, receiver, socket);
            }
        }

        /// <summary>
        /// Sends this <see cref="GetRequestMessage"/> and handles the response from agent.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        /// <param name="receiver">Agent.</param>
        /// <param name="udpSocket">The UDP <see cref="Socket"/> to use to send/receive.</param>
        /// <returns></returns>
        public ISnmpMessage GetResponse(int timeout, IPEndPoint receiver, Socket udpSocket)
        {
            UserRegistry registry = null;
            if (Version == VersionCode.V3)
            {
                registry = UserRegistry.Default;
                Helper.Authenticate(this, _pair);
                registry.Add(_parameters.UserName, _pair);
            }

            return MessageFactory.GetResponse(receiver, ToBytes(), MessageId, timeout, registry, udpSocket);
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
        public Levels Level
        {
            get
            {
                return _pair.ToSecurityLevel();
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
