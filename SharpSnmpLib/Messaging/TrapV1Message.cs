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
        private readonly uint _time;
        private readonly OctetString _community;
        private readonly ObjectIdentifier _enterprise;
        private readonly IPAddress _agent;
        private readonly GenericCode _generic;
        private readonly int _specific;
        private readonly IList<Variable> _variables;
        private readonly VersionCode _version;
        private readonly ISnmpPdu _pdu;
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
            _version = version;
            _agent = agent;
            _community = community;
            _variables = variables;
            _enterprise = enterprise;
            _generic = generic;
            _specific = specific;
            _time = time;
            TrapV1Pdu pdu = new TrapV1Pdu(
                _enterprise,
                new IP(_agent),
                new Integer32((int)_generic),
                new Integer32(_specific),
                new TimeTicks(_time),
                _variables);
            _bytes = Helper.PackMessage(_version, _community, pdu).ToBytes();
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
            
            _community = (OctetString)body[1];
            _version = (VersionCode)((Integer32)body[0]).ToInt32();
            _pdu = (ISnmpPdu)body[2];
            if (_pdu.TypeCode != SnmpType.TrapV1Pdu)
            {
                throw new ArgumentException("wrong message type");
            }
            
            TrapV1Pdu trapPdu = (TrapV1Pdu)_pdu;
            _enterprise = trapPdu.Enterprise;
            _agent = trapPdu.AgentAddress.ToIPAddress();
            _generic = trapPdu.Generic;
            _specific = trapPdu.Specific;
            _time = trapPdu.TimeStamp.ToUInt32();
            _variables = _pdu.Variables;
            ////_bytes = body.ToBytes();
        }

        /// <summary>
        /// Sends this <see cref="TrapV1Message"/>.
        /// </summary>
        /// <param name="manager">Manager</param>
        public void Send(EndPoint manager)
        {
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
        [CLSCompliant(false)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TimeStamp")]
        public uint TimeStamp
        {
            get
            {
                return _time;
            }
        }
        
        /// <summary>
        /// Community name.
        /// </summary>
        public OctetString Community
        {
            get
            {
                return _community;
            }
        }
        
        /// <summary>
        /// Enterprise.
        /// </summary>
        public ObjectIdentifier Enterprise
        {
            get
            {
                return _enterprise;
            }
        }
        
        /// <summary>
        /// Agent address.
        /// </summary>
        public IPAddress AgentAddress
        {
            get
            {
                return _agent;
            }
        }
        
        /// <summary>
        /// Generic type.
        /// </summary>
        public GenericCode Generic
        {
            get
            {
                return _generic;
            }
        }
        
        /// <summary>
        /// Specific type.
        /// </summary>
        public int Specific
        {
            get
            {
                return _specific;
            }
        }
        
        /// <summary>
        /// Variable binds.
        /// </summary>
        public IList<Variable> Variables
        {
            get
            {
                return _variables;
            }
        }
        
        /// <summary>
        /// Protocol version.
        /// </summary>
        public VersionCode Version
        {
            get
            {
                return _version;
            }
        }

        /// <summary>
        /// Gets the request ID.
        /// </summary>
        /// <value>The request ID.</value>
        public int RequestId
        {
            get { throw new InvalidOperationException(); }
        }

        /// <summary>
        /// To byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return _bytes ?? (_bytes = Helper.PackMessage(_version, _community, _pdu).ToBytes());
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
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        /// <remarks><see cref="TrapV1Message"/> returns null here.</remarks>
        public SecurityParameters Parameters
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the scope.
        /// </summary>
        /// <value>The scope.</value>
        /// <remarks><see cref="TrapV1Message"/> returns null here.</remarks>
        public Scope Scope
        {
            get { return null; }
        }
        
        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="TrapV1Message"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "SNMPv1 trap: {0}",
                _pdu);
        }
    }
}
