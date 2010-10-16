// TRAP message type (SNMP version 2 and above).
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
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// TRAP v2 message.
    /// </summary>
    public class TrapV2Message : ISnmpMessage
    {
        private readonly Header _header;
        private readonly IPrivacyProvider _privacy;

        /// <summary>
        /// Creates a <see cref="TrapV2Message"/> instance with all content.
        /// </summary>
        /// <param name="version">Version code.</param>
        /// <param name="community">Community.</param>
        /// <param name="enterprise">Enterprise.</param>
        /// <param name="time">Time stamp.</param>
        /// <param name="variables">Variables.</param>
        /// <param name="requestId">Request ID.</param>
        [CLSCompliant(false)]
        public TrapV2Message(int requestId, VersionCode version, OctetString community, ObjectIdentifier enterprise, uint time, IList<Variable> variables)
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
            
            Version = version;
            Enterprise = enterprise;
            TimeStamp = time;
            _header = Header.Empty;
            Parameters = new SecurityParameters(null, null, null, community, null, null);
            TrapV2Pdu pdu = new TrapV2Pdu(
                requestId,
                enterprise,
                time,
                variables);
            Scope = new Scope(pdu);
            _privacy = DefaultPrivacyProvider.DefaultPair;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrapV2Message"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="messageId">The message id.</param>
        /// <param name="requestId">The request id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="enterprise">The enterprise.</param>
        /// <param name="time">The time.</param>
        /// <param name="variables">The variables.</param>
        /// <param name="privacy">The privacy.</param>
        /// <param name="maxMessageSize">Size of the max message.</param>
        /// <param name="engineId">The engine ID.</param>
        /// <param name="engineBoots">The engine boots.</param>
        /// <param name="engineTime">The engine time.</param>
        [CLSCompliant(false)]
        public TrapV2Message(VersionCode version, int messageId, int requestId, OctetString userName, ObjectIdentifier enterprise, uint time, IList<Variable> variables, IPrivacyProvider privacy, int maxMessageSize, OctetString engineId, int engineBoots, int engineTime)
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

            if (engineId == null)
            {
                throw new ArgumentNullException("engineId");
            }
            
            if (privacy == null)
            {
                throw new ArgumentNullException("privacy");
            }

            Version = version;
            _privacy = privacy;
            Enterprise = enterprise;
            TimeStamp = time;
            Levels recordToSecurityLevel = PrivacyProviderExtension.ToSecurityLevel(privacy);
            byte b = (byte)recordToSecurityLevel;
            
            // TODO: define more constants.
            _header = new Header(new Integer32(messageId), new Integer32(maxMessageSize), new OctetString(new[] { b }), new Integer32(3));
            Parameters = new SecurityParameters(
                engineId,
                new Integer32(engineBoots), 
                new Integer32(engineTime), 
                userName,
                _privacy.AuthenticationProvider.CleanDigest,
                _privacy.Salt);
            var pdu = new TrapV2Pdu(
                requestId,
                enterprise,
                time,
                variables);
            Scope = new Scope(OctetString.Empty, OctetString.Empty, pdu);
        }

        internal TrapV2Message(VersionCode version, Header header, SecurityParameters parameters, Scope scope, IPrivacyProvider privacy)
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

            Version = version;
            _header = header;
            Parameters = parameters;
            Scope = scope;
            _privacy = privacy;
            TrapV2Pdu pdu = (TrapV2Pdu)Scope.Pdu;
            Enterprise = pdu.Enterprise;
            TimeStamp = pdu.TimeStamp;
        }
        
        #region ISnmpMessage Members
        /// <summary>
        /// PDU.
        /// </summary>
        public ISnmpPdu Pdu
        {
            get { return Scope.Pdu; }
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public SecurityParameters Parameters { get; private set; }

        /// <summary>
        /// Gets the scope.
        /// </summary>
        /// <value>The scope.</value>
        public Scope Scope { get; private set; }

        #endregion

        #region ISnmpData Members

        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return Helper.PackMessage(Version, _privacy, _header, Parameters, Scope).ToBytes();
        }

        #endregion

        /// <summary>
        /// Sends this <see cref="TrapV2Message"/>.
        /// </summary>
        /// <param name="manager">Manager.</param>
        public void Send(EndPoint manager)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            
            using (Socket socket = Helper.GetSocket(manager))
            {
                Send(manager, socket);
            }
        }

        /// <summary>
        /// Sends this <see cref="TrapV2Message"/>.
        /// </summary>
        /// <param name="manager">Manager.</param>
        /// <param name="socket">The socket.</param>
        public void Send(EndPoint manager, Socket socket)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            
            if (socket == null)
            {
                throw new ArgumentNullException("socket");
            }
            
            byte[] bytes = ToBytes();
            socket.SendTo(bytes, manager);
        }

        /// <summary>
        /// Community name.
        /// </summary>
        public OctetString Community
        {
            get { return Parameters.UserName; }
        }

        /// <summary>
        /// Enterprise.
        /// </summary>
        public ObjectIdentifier Enterprise { get; private set; }

        /// <summary>
        /// Time stamp.
        /// </summary>
        [CLSCompliant(false), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TimeStamp")]
        public uint TimeStamp { get; private set; }

        /// <summary>
        /// Variables.
        /// </summary>
        public IList<Variable> Variables
        {
            get { return Scope.Pdu.Variables; }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public VersionCode Version { get; private set; }

        /// <summary>
        /// Gets the request ID.
        /// </summary>
        /// <value>The request ID.</value>
        public int RequestId
        {
            get { return Scope.Pdu.RequestId.ToInt32(); }
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
                return _header == Header.Empty ? RequestId : _header.MessageId;
            }
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="TrapV2Message"/>.
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
                "SNMPv2 trap: time stamp: {0}; community: {1}; enterprise: {2}; varbind count: {3}",
                TimeStamp.ToString(CultureInfo.InvariantCulture),
                Community,
                Enterprise.ToString(objects),
                Variables.Count.ToString(CultureInfo.InvariantCulture));
        }
    }
}
