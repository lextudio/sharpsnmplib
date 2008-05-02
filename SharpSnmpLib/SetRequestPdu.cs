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
using System.Text;
using System.IO;

namespace SharpSnmpLib
{
    public class SetRequestPdu: ISnmpPdu
    {
    	public SetRequestPdu(int errorStatus, int errorIndex, IList<Variable> variables) 
    		: this(new Int(errorStatus), new Int(errorIndex), variables) {}
    	
        public SetRequestPdu(Int errorStatus, Int errorIndex, IList<Variable> variables)
        {
            _seq = PduCounter.GetNextCount();                      
            _errorStatus = errorStatus; 
            _errorIndex = errorIndex;
            _variables = variables;
            _varbindSection = Variable.ConvertTo(_variables);
            _raw = ByteTool.ParseItems(_seq, _errorStatus, _errorIndex, _varbindSection);
        }
        
        public SetRequestPdu(byte[] raw)
        {
        	_raw = raw;
			MemoryStream m = new MemoryStream(raw);
			_seq = (Int)SnmpDataFactory.CreateSnmpData(m);
			_errorStatus = (Int)SnmpDataFactory.CreateSnmpData(m);
			_errorIndex = (Int)SnmpDataFactory.CreateSnmpData(m);
			_varbindSection = (SnmpArray)SnmpDataFactory.CreateSnmpData(m);
			_variables = Variable.ConvertFrom(_varbindSection);
        }

        private Int _errorStatus;
        private Int _errorIndex;
        private IList<Variable> _variables;
        private Int _seq;
        private byte[] _raw;
        private SnmpArray _varbindSection;

        public IList<Variable> Variables
        {
            get
            {
                return _variables;
            }
        }

        #region ISnmpPdu Members

        public ISnmpData ToMessageBody(VersionCode version, string community)
        {
            Int ver = new Int((int)version);
            OctetString comm = new OctetString(community);
            SetRequestPdu pdu = this;
            SnmpArray array = new SnmpArray(ver, comm, pdu);
            return array;
        }

        #endregion

        #region ISnmpData Members

        public SnmpType TypeCode
        {
            get { return SnmpType.SetRequestPDU; }
        }

        byte[] _bytes;

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
    }
}