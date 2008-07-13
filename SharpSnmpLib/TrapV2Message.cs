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
        UdpClient udp = new UdpClient();
        byte[] _bytes;
        ISnmpPdu _pdu;
        VersionCode _version;
        string _community;
        IList<Variable> _variables;
        ObjectIdentifier _enterprise;
        int _time;

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
			if (_disposed) {
				return;
			}
			if (disposing) {
				(udp as IDisposable).Dispose();
			}
			_disposed = true;
		}

        #endregion
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
        public int TimeStamp
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
            return string.Format(CultureInfo.InvariantCulture,
                "SNMPv2 trap: time stamp: {0}; community: {1}; enterprise: {2}; varbind count: {3}",
                TimeStamp, Community, Enterprise, Variables.Count);
        }
    }
}
