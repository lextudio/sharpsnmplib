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
	public class GetResponsePdu: ISnmpPdu
	{
        private Integer32 _errorStatus;
        private Integer32 _sequenceNumber;
        private Integer32 _errorIndex;
        private IList<Variable> _variables;
        private Sequence _varbindSection;
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
			_variables = Variable.ConvertFrom(_varbindSection);
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
		public SnmpType TypeCode {
			get {
				return SnmpType.GetResponsePDU;
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
			throw new NotImplementedException();
		}
		/// <summary>
		/// Converts to byte format.
		/// </summary>
		/// <returns></returns>
		public byte[] ToBytes()
		{
			throw new NotImplementedException();
		}
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="GetResponsePdu"/>/
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "GET response PDU: seq: {0}; status: {1}; index: {2}; variable count: {3}",
                _sequenceNumber, _errorStatus, _errorIndex, _variables.Count);
        }
	}
}
