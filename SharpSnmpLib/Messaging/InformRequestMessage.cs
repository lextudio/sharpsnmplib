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
using Lextm.SharpSnmpLib.Mib;
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
        private readonly ProviderPair _pair;
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
            InformRequestPdu pdu = (InformRequestPdu) scope.Pdu;
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
        /// Sends the response.
        /// </summary>
        /// <param name="receiver">The receiver.</param>
        [Obsolete("Use Listener.SendResponse instead")]
        public void SendResponse(EndPoint receiver)
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
            using (Socket socket = Helper.GetSocket(receiver))
            {
                return MessageFactory.GetResponse(receiver, ToBytes(), MessageId, timeout, UserRegistry.Default, socket);
            }
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
            return MessageFactory.GetResponse(receiver, ToBytes(), MessageId, timeout, UserRegistry.Default, socket);
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
                return (_header == null) ? RequestId : _header.MessageId;
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
            return string.Format(CultureInfo.InvariantCulture,
                                 "INFORM request message: time stamp: {0}; community: {1}; enterprise: {2}; varbind count: {3}",
                                 TimeStamp.ToString(CultureInfo.InvariantCulture),
                                 Community,
                                 SearchResult.GetStringOf(Enterprise, objects),
                                 Variables.Count.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Generates the response message.
        /// </summary>
        /// <returns></returns>
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
