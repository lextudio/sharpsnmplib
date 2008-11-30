/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 20:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// SET request PDU.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pdu")]
    public class SetRequestPdu : ISnmpPdu
    {
        private Integer32 _errorStatus;
        private Integer32 _errorIndex;
        private IList<Variable> _variables;
        private Integer32 _sequenceNumber;
        private byte[] _raw;
        private Sequence _varbindSection;
        
        /// <summary>
        /// Creates a <see cref="SetRequestPdu"/> instance with all contents.
        /// </summary>
        /// <param name="errorStatus">Error status</param>
        /// <param name="errorIndex">Error index</param>
        /// <param name="variables">Variables</param>
        public SetRequestPdu(ErrorCode errorStatus, int errorIndex, IList<Variable> variables)
            : this(new Integer32((int)errorStatus), new Integer32(errorIndex), variables)
        {
        }
        
        /// <summary>
        /// Creates a <see cref="SetRequestPdu"/> instance with all contents.
        /// </summary>
        /// <param name="errorStatus">Error status</param>
        /// <param name="errorIndex">Error index</param>
        /// <param name="variables">Variables</param>
        private SetRequestPdu(Integer32 errorStatus, Integer32 errorIndex, IList<Variable> variables)
        {
            _sequenceNumber = PduCounter.NextCount;
            _errorStatus = errorStatus;
            _errorIndex = errorIndex;
            _variables = variables;
            _varbindSection = Variable.Transform(_variables);
            _raw = ByteTool.ParseItems(_sequenceNumber, _errorStatus, _errorIndex, _varbindSection);
        }
        
        /// <summary>
        /// Creates a <see cref="SetRequestPdu"/> instance from raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        public SetRequestPdu(byte[] raw)
        {
            _raw = raw;
            MemoryStream m = new MemoryStream(raw);
            _sequenceNumber = (Integer32)SnmpDataFactory.CreateSnmpData(m);
            _errorStatus = (Integer32)SnmpDataFactory.CreateSnmpData(m);
            _errorIndex = (Integer32)SnmpDataFactory.CreateSnmpData(m);
            _varbindSection = (Sequence)SnmpDataFactory.CreateSnmpData(m);
            _variables = Variable.Transform(_varbindSection);
        }
        
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="SetRequestPdu"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "SET request PDU: seq: {0}; status: {1}; index: {2}; variable count: {3}",
                _sequenceNumber,
                _errorStatus,
                _errorIndex,
                _variables.Count.ToString(CultureInfo.InvariantCulture));
        }

        internal int SequenceNumber
        {
            get
            {
                return _sequenceNumber.ToInt32();
            }
        }
        
        /// <summary>
        /// Variables.
        /// </summary>
        public IList<Variable> Variables
        {
            get
            {
                return _variables;
            }
        }

        #region ISnmpPdu Members
        /// <summary>
        /// Converts to message body.
        /// </summary>
        /// <param name="version">Prtocol version</param>
        /// <param name="community">Community name</param>
        /// <returns></returns>
        public ISnmpData ToMessageBody(VersionCode version, OctetString community)
        {
            return ByteTool.PackMessage(version, community, this);
        }

        #endregion

        #region ISnmpData Members
        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get { return SnmpType.SetRequestPdu; }
        }

        private byte[] _bytes;
        
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
                ByteTool.WritePayloadLength(result, _raw.Length); // it seems that trap does not use this function
                result.Write(_raw, 0, _raw.Length);
                _bytes = result.ToArray();
            }
            
            return _bytes;
        }

        #endregion
    }
}