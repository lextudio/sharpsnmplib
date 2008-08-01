using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Globalization;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// TRAP v2 message.
    /// </summary>
    public class TrapV2Message : ISnmpMessage, IDisposable
    {
        private UdpClient udp = new UdpClient();
        private byte[] _bytes;
        private ISnmpPdu _pdu;
        private VersionCode _version;
        private string _community;
        private IList<Variable> _variables;
        private ObjectIdentifier _enterprise;
        private uint _time;
        
        /// <summary>
        /// Creates a <see cref="TrapV2Message"/> instance with all content.
        /// </summary>
        /// <param name="version">Version code</param>
        /// <param name="community">Community</param>
        /// <param name="enterprise">Enterprise</param>
        /// <param name="time">Time stamp</param>
        /// <param name="variables">Variables</param>
        [CLSCompliant(false)]
        public TrapV2Message(VersionCode version, string community, ObjectIdentifier enterprise, uint time, IList<Variable> variables)
        {
            _version = version;
            _community = community;
            _enterprise = enterprise;
            _time = time;
            _variables = variables;
            _pdu = new TrapV2Pdu(new Integer32((int)_version), _enterprise, new TimeTicks(_time), _variables);
            _bytes = _pdu.ToMessageBody(_version, _community).ToBytes();
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
            
            _community = body.Items[1].ToString();
            _version = (VersionCode)((Integer32)body.Items[0]).ToInt32();
            _pdu = (ISnmpPdu)body.Items[2];
            if (_pdu.TypeCode != TypeCode)
            {
                throw new ArgumentException("wrong message type");
            }
            
            _variables = _pdu.Variables;
            TrapV2Pdu pdu = (TrapV2Pdu)_pdu;
            _time = pdu.TimeStamp;
            _enterprise = pdu.Enterprise;
            _bytes = body.ToBytes();
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
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get { return SnmpType.TrapV2Pdu; }
        }
        
        /// <summary>
        /// To byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return _bytes;
        }

        #endregion

        #region IDisposable Members

        private bool _disposed;
        
        /// <summary>
        /// Finalizer of <see cref="TrapV2Message"/>.
        /// </summary>
        ~TrapV2Message()
        {
            Dispose(false);
        }
        
        /// <summary>
        /// Releases all resources used by the <see cref="TrapV2Message"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
      
        /// <summary>
        /// Disposes of the resources (other than memory) used by the <see cref="TrapV2Message"/>.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) 
            {
                return;
            }
            
            if (disposing)
            {
                (udp as IDisposable).Dispose();
            }
            
            _disposed = true;
        }

        #endregion

        /// <summary>
        /// Sends this <see cref="TrapV2Message"/>.
        /// </summary>
        /// <param name="manager">Manager address</param>
        /// <param name="port">Port number</param>
        public void Send(IPAddress manager, int port)
        {
            byte[] bytes = _bytes;
            IPEndPoint agent = new IPEndPoint(manager, port);
            udp.Send(bytes, bytes.Length, agent);
        }
        
        /// <summary>
        /// Community name.
        /// </summary>
        public string Community
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
        /// Returns a <see cref="String"/> that represents the current <see cref="TrapV2Message"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "SNMPv2 trap: time stamp: {0}; community: {1}; enterprise: {2}; varbind count: {3}",
                TimeStamp, 
                Community, 
                Enterprise, 
                Variables.Count);
        }
    }
}
