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

namespace Lextm.SharpSnmpLib
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
            _bytes = pdu.ToMessageBody(_version, _community).ToBytes();
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
            
            if (body.Items.Count != 3)
            {
                throw new ArgumentException("wrong message body");
            }
            
            _community = (OctetString)body.Items[1];
            _version = (VersionCode)((Integer32)body.Items[0]).ToInt32();
            _pdu = (ISnmpPdu)body.Items[2];
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
        public void Send(IPEndPoint manager)
        {
            byte[] bytes = ToBytes();
            ByteTool.Capture(bytes);
            using (UdpClient udp = new UdpClient(manager.AddressFamily))
            {
                udp.Send(bytes, bytes.Length, manager);
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

            byte[] bytes = ToBytes();
            ByteTool.Capture(bytes);
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
        /// To byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            if (_bytes == null)
            {
                _bytes = _pdu.ToMessageBody(_version, _community).ToBytes();
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
                return _pdu;
            }
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
