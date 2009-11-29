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
        private readonly VersionCode _version;
        private readonly Header _header;
        private readonly SecurityParameters _parameters;
        private readonly Scope _scope;
        private readonly ProviderPair _pair;
        
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
            if (version == VersionCode.V3)
            {
                throw new ArgumentException("only v1 and v2c are supported", "version");
            }
            
            _version = version;
            _header = Header.Empty;
            _parameters = new SecurityParameters(null, null, null, community, null, null);
            TrapV2Pdu pdu = new TrapV2Pdu(
                requestId,
                enterprise,
                time,
                variables);
            _scope = new Scope(null, null, pdu);
            _pair = ProviderPair.Default;
            _enterprise = enterprise;
            _time = time;
        }
        
        internal TrapV2Message(VersionCode version, Header header, SecurityParameters parameters, Scope scope, ProviderPair record)
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
            TrapV2Pdu pdu = (TrapV2Pdu)_scope.Pdu;
            _enterprise = pdu.Enterprise;
            _time = pdu.TimeStamp;
        }
        
        #region ISnmpMessage Members
        /// <summary>
        /// PDU.
        /// </summary>
        public ISnmpPdu Pdu
        {
            get { return _scope.Pdu; }
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

        #endregion

        #region ISnmpData Members

        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return Helper.PackMessage(_version, _pair.Privacy, _header, _parameters, _scope).ToBytes();
        }

        #endregion

        /// <summary>
        /// Sends this <see cref="TrapV2Message"/>.
        /// </summary>
        /// <param name="manager">Manager.</param>
        public void Send(EndPoint manager)
        {
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
            get { return _parameters.UserName; }
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
            get { return _scope.Pdu.Variables; }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public VersionCode Version
        {
            get { return _version; }
        }

        public int RequestId
        {
            get { return _scope.Pdu.RequestId.ToInt32(); }
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
