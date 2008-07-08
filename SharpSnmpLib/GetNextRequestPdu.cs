/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/11
 * Time: 12:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// GETNEXT request PDU.
    /// </summary>
    public class GetNextRequestPdu: ISnmpPdu
    {
        private Integer32 _errorStatus;
        private Integer32 _errorIndex;
        private IList<Variable> _variables;
        private Integer32 _seq;
        private byte[] _raw;
        private Sequence _varbindSection;
        private byte[] _bytes;        
        /// <summary>
        /// Creates a <see cref="GetNextRequestPdu"/> with all contents.
        /// </summary>
        /// <param name="errorStatus">Error status</param>
        /// <param name="errorIndex">Error index</param>
        /// <param name="variables">Variables</param>
    	public GetNextRequestPdu(ErrorCode errorStatus, int errorIndex, IList<Variable> variables) 
    		: this(new Integer32((int)errorStatus), new Integer32(errorIndex), variables) {}
    	
        GetNextRequestPdu(Integer32 errorStatus, Integer32 errorIndex, IList<Variable> variables)
        {
            _seq = PduCounter.NextCount;                      
            _errorStatus = errorStatus; 
            _errorIndex = errorIndex;
            _variables = variables;
            _varbindSection = Variable.ConvertTo(variables);
            _raw = ByteTool.ParseItems(_seq, _errorStatus, _errorIndex, _varbindSection);
        }
        /// <summary>
        /// Creates a <see cref="GetNextRequestPdu"/> with raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        public GetNextRequestPdu(byte[] raw)
        {
        	_raw = raw;
			MemoryStream m = new MemoryStream(raw);
			_seq = (Integer32)SnmpDataFactory.CreateSnmpData(m);
			_errorStatus = (Integer32)SnmpDataFactory.CreateSnmpData(m);
			_errorIndex = (Integer32)SnmpDataFactory.CreateSnmpData(m);
			_varbindSection = (Sequence)SnmpDataFactory.CreateSnmpData(m);
			_variables = Variable.ConvertFrom(_varbindSection);
        }
        
        internal int SequenceNumber
        {
        	get
        	{
        		return _seq.ToInt32();
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
        /// <param name="version">Protocol version</param>
        /// <param name="community">Community name</param>
        /// <returns></returns>
        public ISnmpData ToMessageBody(VersionCode version, string community)
        {
            Integer32 ver = new Integer32((int)version);
            OctetString comm = new OctetString(community);
            GetNextRequestPdu pdu = this;
            Sequence array = new Sequence(ver, comm, pdu);
            return array;
        }

        #endregion

        #region ISnmpData Members
        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get { return SnmpType.GetNextRequestPdu; }
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
                ByteTool.WriteMultiByteLength(result, _raw.Length); //it seems that trap does not use this function
                result.Write(_raw, 0, _raw.Length);
                _bytes = result.ToArray();
            }
            return _bytes;
        }

        #endregion
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="GetNextRequestPdu"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "GET NEXT request PDU: seq: {0}; status: {1}; index: {2}; variable count: {3}",
                _seq, _errorStatus, _errorIndex, _variables.Count);
        }
    }
}

