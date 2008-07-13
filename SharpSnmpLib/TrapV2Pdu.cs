using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// TRAP v2 PDU.
    /// </summary>
    public class TrapV2Pdu : ISnmpPdu
    {
        private Integer32 _errorStatus;
        private Integer32 _errorIndex;
        private IList<Variable> _variables;
        private Integer32 _sequenceNumber;
        private byte[] _raw;
        private Sequence _varbindSection;
        private int _time;
        private ObjectIdentifier _enterprise;

        /// <summary>
        /// Creates a <see cref="TrapV2Pdu"/> instance from raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        public TrapV2Pdu(byte[] raw)
        {
        	_raw = raw;
			MemoryStream m = new MemoryStream(raw);
			_sequenceNumber = (Integer32)SnmpDataFactory.CreateSnmpData(m); // version number v2c
			_errorStatus = (Integer32)SnmpDataFactory.CreateSnmpData(m); // 0
			_errorIndex = (Integer32)SnmpDataFactory.CreateSnmpData(m); // 0
			_varbindSection = (Sequence)SnmpDataFactory.CreateSnmpData(m);
			_variables = Variable.ConvertFrom(_varbindSection); // v[0] is timestamp. v[1] oid, v[2] value.
            _time = ((TimeTicks)_variables[0].Data).ToInt32();
            _variables.RemoveAt(0);
            _enterprise = (ObjectIdentifier)_variables[0].Data;
            _variables.RemoveAt(0);
        }

        #region ISnmpPdu Members
        /// <summary>
        /// Converts to message body.
        /// </summary>
        /// <param name="version">Prtocol version</param>
        /// <param name="community">Community name</param>
        /// <returns></returns>
        public ISnmpData ToMessageBody(VersionCode version, string community)
        {
            return ByteTool.PackMessage(version, community, this);
        }
        /// <summary>
        /// Variables.
        /// </summary>
        public IList<Variable> Variables
        {
            get { return _variables; }
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

        byte[] _bytes;
        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            if (_bytes == null)
            {
                MemoryStream result = new MemoryStream();
                result.WriteByte((byte)TypeCode);
                ByteTool.WriteMultiByteLength(result, _raw.Length); //it seems that trap does not use this function
                result.Write(_raw, 0, _raw.Length);
                _bytes = result.ToArray();
            }
            return _bytes;
        }

        #endregion
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
        /// Returns a <see cref="String"/> that represents this <see cref="SetRequestPdu"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "TRAP v2 PDU: enterprise: {0}; time stamp: {1}; variable count: {2}",
                _enterprise, _time, _variables.Count);
        }
    }
}
