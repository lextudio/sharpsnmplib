// TRAP message type (SNMP version 1 only).
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
 * Date: 2008/4/25
 * Time: 20:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Trap message.
    /// </summary>
    public class TrapV1Message : ISnmpMessage
    {
        private byte[] _bytes;

        /// <summary>
        /// Creates a <see cref="TrapV1Message"/> with all content.
        /// </summary>
        /// <param name="version">Protocol version</param>
        /// <param name="agent">Agent address</param>
        /// <param name="community">Community name</param>
        /// <param name="enterprise">Enterprise</param>
        /// <param name="generic">Generic</param>
        /// <param name="specific">Specific</param>
        /// <param name="time">Time</param>
        /// <param name="variables">Variables</param>
        [CLSCompliant(false)]
        public TrapV1Message(VersionCode version, IPAddress agent, OctetString community, ObjectIdentifier enterprise, GenericCode generic, int specific, uint time, IList<Variable> variables)
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
            
            if (agent == null)
            {
                throw new ArgumentNullException("agent");
            }
            
            Version = version;
            AgentAddress = agent;
            Community = community;
            Variables = variables;
            Enterprise = enterprise;
            Generic = generic;
            Specific = specific;
            TimeStamp = time;
            TrapV1Pdu pdu = new TrapV1Pdu(
                Enterprise,
                new IP(AgentAddress),
                new Integer32((int)Generic),
                new Integer32(Specific),
                new TimeTicks(TimeStamp),
                Variables);
            _bytes = Helper.PackMessage(Version, Community, pdu).ToBytes();
            Parameters = new SecurityParameters(null, null, null, Community, null, null);
        }
        
        /// <summary>
        /// Creates a <see cref="TrapV1Message"/> instance with a message body.
        /// </summary>
        /// <param name="body">Message body</param>
        public TrapV1Message(Sequence body)
        {
            if (body == null)
            {
                throw new ArgumentNullException("body");
            }
            
            if (body.Count != 3)
            {
                throw new ArgumentException("wrong message body");
            }
            
            Community = (OctetString)body[1];
            Version = (VersionCode)((Integer32)body[0]).ToInt32();
            Pdu = (ISnmpPdu)body[2];
            if (Pdu.TypeCode != SnmpType.TrapV1Pdu)
            {
                throw new ArgumentException("wrong message type");
            }

            TrapV1Pdu trapPdu = (TrapV1Pdu)Pdu;
            Enterprise = trapPdu.Enterprise;
            AgentAddress = trapPdu.AgentAddress.ToIPAddress();
            Generic = trapPdu.Generic;
            Specific = trapPdu.Specific;
            TimeStamp = trapPdu.TimeStamp.ToUInt32();
            Variables = Pdu.Variables;
            Parameters = new SecurityParameters(null, null, null, Community, null, null);
        }

        /// <summary>
        /// Sends this <see cref="TrapV1Message"/>.
        /// </summary>
        /// <param name="manager">Manager</param>
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
        /// Sends this <see cref="TrapV1Message"/>.
        /// </summary>
        /// <param name="manager">Manager</param>
        /// <param name="socket">The socket.</param>
        public void Send(EndPoint manager, Socket socket)
        {
            if (socket == null)
            {
                throw new ArgumentNullException("socket");
            }

            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            byte[] bytes = ToBytes();
            socket.SendTo(bytes, manager);
        }

        /// <summary>
        /// Time stamp.
        /// </summary>
        [CLSCompliant(false), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TimeStamp")]
        public uint TimeStamp { get; private set; }

        /// <summary>
        /// Community name.
        /// </summary>
        public OctetString Community { get; private set; }

        /// <summary>
        /// Enterprise.
        /// </summary>
        public ObjectIdentifier Enterprise { get; private set; }

        /// <summary>
        /// Agent address.
        /// </summary>
        public IPAddress AgentAddress { get; private set; }

        /// <summary>
        /// Generic type.
        /// </summary>
        public GenericCode Generic { get; private set; }

        /// <summary>
        /// Specific type.
        /// </summary>
        public int Specific { get; private set; }

        /// <summary>
        /// Variable binds.
        /// </summary>
        public IList<Variable> Variables { get; private set; }

        /// <summary>
        /// Protocol version.
        /// </summary>
        public VersionCode Version { get; private set; }

        /// <summary>
        /// Gets the request ID.
        /// </summary>
        /// <value>The request ID.</value>
        public int RequestId
        {
            get { throw new NotSupportedException(); }
        }
        
        /// <summary>
        /// Gets the message ID.
        /// </summary>
        /// <value>The message ID.</value>
        public int MessageId
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// To byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return _bytes ?? (_bytes = Helper.PackMessage(Version, Community, Pdu).ToBytes());
        }

        /// <summary>
        /// PDU.
        /// </summary>
        public ISnmpPdu Pdu { get; private set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        /// <remarks><see cref="TrapV1Message"/> returns null here.</remarks>
        public SecurityParameters Parameters { get; private set; }

        /// <summary>
        /// Gets the scope.
        /// </summary>
        /// <value>The scope.</value>
        /// <remarks><see cref="TrapV1Message"/> returns null here.</remarks>
        public Scope Scope
        {
            get { throw new NotSupportedException(); }
        }
        
        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="TrapV1Message"/>.
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
                "SNMPv1 trap: {0}",
                ((TrapV1Pdu)Pdu).ToString(objects));
        }
    }
}
