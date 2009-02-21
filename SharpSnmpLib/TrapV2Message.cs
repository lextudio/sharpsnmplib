using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// TRAP v2 message.
    /// </summary>
    public class TrapV2Message : ISnmpMessage
    {
        private byte[] _bytes;
        private readonly ISnmpPdu _pdu;
        private readonly VersionCode _version;
        private readonly OctetString _community;
        private readonly IList<Variable> _variables;
        private readonly ObjectIdentifier _enterprise;
        private readonly uint _time;
        
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
            _version = version;
            _community = community;
            _enterprise = enterprise;
            _time = time;
            _variables = variables;
            _pdu = new TrapV2Pdu(new Integer32(requestId), _enterprise, new TimeTicks(_time), _variables);
			//_bytes = _pdu.ToMessageBody(_version, _community).ToBytes();
        }
        
        /// <summary>
        /// Creates a <see cref="TrapV2Message"/> with a specific <see cref="Sequence"/>.
        /// </summary>
        /// <param name="body">Message body</param>
        public TrapV2Message(Sequence body)
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
            if (_pdu.TypeCode != SnmpType.TrapV2Pdu)
            {
                throw new ArgumentException("wrong message type");
            }
            
            _variables = _pdu.Variables;
            TrapV2Pdu pdu = (TrapV2Pdu)_pdu;
            _time = pdu.TimeStamp;
            _enterprise = pdu.Enterprise;
			//_bytes = body.ToBytes();
        }

        #region ISnmpMessage Members
        /// <summary>
        /// PDU.
        /// </summary>
        public ISnmpPdu Pdu
        {
            get { return _pdu; }
        }

        #endregion

        #region ISnmpData Members

        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
			if(_bytes == null)
			{
				_bytes = _pdu.ToMessageBody(_version, _community).ToBytes();
			}

            return _bytes;
        }

        #endregion

        /// <summary>
        /// Sends this <see cref="TrapV2Message"/>.
        /// </summary>
        /// <param name="manager">Manager address</param>
        /// <param name="port">Port number</param>
        [Obsolete("Please use overload version instead.")]
        public void Send(IPAddress manager, int port)
        {
			byte[] bytes = ToBytes();
            ByteTool.Capture(bytes); // log response
            IPEndPoint endpoint = new IPEndPoint(manager, port);
            using (UdpClient udp = new UdpClient()) 
            {
                udp.Send(bytes, bytes.Length, endpoint);
                udp.Close();
            }
        }

        /// <summary>
        /// Sends this <see cref="TrapV2Message"/>.
        /// </summary>
        /// <param name="manager">Manager.</param>
        public void Send(IPEndPoint manager)
        {
			byte[] bytes = ToBytes();
            ByteTool.Capture(bytes); // log response
            using (UdpClient udp = new UdpClient())
            {
                udp.Send(bytes, bytes.Length, manager);
                udp.Close();
            }
        }

        /// <summary>
        /// Community name.
        /// </summary>
        public OctetString Community
        {
            get { return _community; }
        }
        
        /// <summary>
        /// Enterprise.
        /// </summary>
        public ObjectIdentifier Enterprise
        {
            get { return _enterprise; }
        }
        
        /// <summary>
        /// Time stamp.
        /// </summary>
        [CLSCompliant(false)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TimeStamp")]
        public uint TimeStamp
        {
            get { return _time; }
        }
        
        /// <summary>
        /// Variables.
        /// </summary>
        public IList<Variable> Variables
        {
            get { return _variables; }
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="TrapV2Message"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "SNMPv2 trap: time stamp: {0}; community: {1}; enterprise: {2}; varbind count: {3}",
                TimeStamp.ToString(CultureInfo.InvariantCulture),
                Community, 
                Enterprise, 
                Variables.Count.ToString(CultureInfo.InvariantCulture));
        }
    }
}
