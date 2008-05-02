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

namespace SharpSnmpLib
{
	/// <summary>
	/// Description of GetResponsePdu.
	/// </summary>
	public class GetResponsePdu: ISnmpPdu
	{
        private Int _errorStatus;
        
		public int ErrorStatus {
			get { return _errorStatus.ToInt32(); }
		}
        private Int _errorIndex;
        
		public int ErrorIndex {
			get { return _errorIndex.ToInt32(); }
		}
        private IList<Variable> _variables;
        private SnmpArray _varbindSection;
		
		public GetResponsePdu(byte[] raw)
		{
			MemoryStream m = new MemoryStream(raw);
			SnmpDataFactory.CreateSnmpData(m);
			_errorStatus = (Int)SnmpDataFactory.CreateSnmpData(m);
			_errorIndex = (Int)SnmpDataFactory.CreateSnmpData(m);
			_varbindSection = (SnmpArray)SnmpDataFactory.CreateSnmpData(m);
			_variables = Variable.ConvertFrom(_varbindSection);
		}
		
		public IList<Variable> Variables
        {
            get
            {
                return _variables;
            }
        }
		
		public SnmpType TypeCode {
			get {
				return SnmpType.GetResponsePDU;
			}
		}
		
		public ISnmpData ToMessageBody(VersionCode version, string community)
		{
			throw new NotImplementedException();
		}
		
		public byte[] ToBytes()
		{
			throw new NotImplementedException();
		}
	}
}
