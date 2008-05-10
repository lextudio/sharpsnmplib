/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 20:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

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

using X690;

namespace SharpSnmpLib
{
	/// <summary>
	/// Description of GetResponsePdu.
	/// </summary>
	public class SetResponsePdu: ISnmpPdu
	{
        private Integer _errorStatus;
        
		public int ErrorStatus {
			get { return _errorStatus; }
		}
        private Integer _errorIndex;
        
		public int ErrorIndex {
			get { return _errorIndex; }
		}
        private IList<Variable> _variables;
        private Integer _seq;
        private byte[] _raw;
        private SnmpArray _varbindSection;
		
		public SetResponsePdu(byte[] raw)
		{
			_raw = raw;
			MemoryStream m = new MemoryStream(raw);
			_seq = (Integer)SnmpDataFactory.CreateSnmpData(m);
			_errorStatus = (Integer)SnmpDataFactory.CreateSnmpData(m);
			_errorIndex = (Integer)SnmpDataFactory.CreateSnmpData(m);
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
		
		public Snmp.SnmpType DataType {
			get {
				return Snmp.SnmpType.SetResponsePDU;
			}
		}
		
		public ISnmpData ToMessageBody(ProtocolVersion version, string community)
		{
			throw new NotImplementedException();
		}
		
		public byte[] ToBytes()
		{
			throw new NotImplementedException();
		}
	}
}
