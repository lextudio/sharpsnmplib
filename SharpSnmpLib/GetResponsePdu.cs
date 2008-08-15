/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 18:10
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// GET response PDU.
    /// </summary>
    public class GetResponsePdu : ISnmpPdu
    {
        private Integer32 _errorStatus;
        private Integer32 _sequenceNumber;
        private Integer32 _errorIndex;
        private IList<Variable> _variables;
        private Sequence _varbindSection;
        private byte[] _bytes;
        private byte[] _raw;
                
        /// <summary>
        /// Creates a <see cref="GetResponsePdu"/> with all contents.
        /// </summary>
        /// <param name="errorStatus">Error status.</param>
        /// <param name="errorIndex">Error index.</param>
        /// <param name="sequenceNumber">Sequence number.</param>
        /// <param name="variables">Variables.</param>
        public GetResponsePdu(Integer32 sequenceNumber, ErrorCode errorStatus, Integer32 errorIndex, IList<Variable> variables)
        {
            _sequenceNumber = sequenceNumber;
            _errorStatus = new Integer32((int)errorStatus);
            _errorIndex = errorIndex;
            _variables = variables;
            _varbindSection = Variable.Transform(variables);
            _raw = ByteTool.ParseItems(_sequenceNumber, _errorStatus, _errorIndex, _varbindSection);
        }
        
        /// <summary>
        /// Creates a <see cref="GetResponsePdu"/> from raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        public GetResponsePdu(byte[] raw)
        {
            MemoryStream m = new MemoryStream(raw);
            _sequenceNumber = (Integer32)SnmpDataFactory.CreateSnmpData(m);
            _errorStatus = (Integer32)SnmpDataFactory.CreateSnmpData(m);
            _errorIndex = (Integer32)SnmpDataFactory.CreateSnmpData(m);
            _varbindSection = (Sequence)SnmpDataFactory.CreateSnmpData(m);
            _variables = Variable.Transform(_varbindSection);
            _raw = raw;
        }
        
        internal int SequenceNumber
        {
            get
            {
                return _sequenceNumber.ToInt32();
            }
        }
        
        /// <summary>
        /// Error status.
        /// </summary>
        public ErrorCode ErrorStatus
        {
            get { return (ErrorCode)_errorStatus.ToInt32(); }
        }
        
        /// <summary>
        /// Error index.
        /// </summary>
        public int ErrorIndex
        {
            get { return _errorIndex.ToInt32(); }
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
        
        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get
            {
                return SnmpType.GetResponsePdu;
            }
        }
        
        /// <summary>
        /// Converts to message body.
        /// </summary>
        /// <param name="version">Protocol version</param>
        /// <param name="community">Community name</param>
        /// <returns></returns>
        public ISnmpData ToMessageBody(VersionCode version, string community)
        {
            return ByteTool.PackMessage(version, community, this);
        }
        
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
        
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="GetResponsePdu"/>/
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "GET response PDU: seq: {0}; status: {1}; index: {2}; variable count: {3}",
                _sequenceNumber, 
                _errorStatus, 
                _errorIndex, 
                _variables.Count);
        }
    }
}
