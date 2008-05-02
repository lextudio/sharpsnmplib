using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SharpSnmpLib
{
    public class GetRequestPdu: ISnmpPdu
    {
    	public GetRequestPdu(int errorStatus, int errorIndex, IList<Variable> variables) 
    		: this(new Int(errorStatus), new Int(errorIndex), variables) {}
    	
        public GetRequestPdu(Int errorStatus, Int errorIndex, IList<Variable> variables)
        {
            _seq = PduCounter.GetNextCount();                      
            _errorStatus = errorStatus; 
            _errorIndex = errorIndex;
            _variables = variables;
            _varbindSection = Variable.ConvertTo(variables);
            _raw = ByteTool.ParseItems(_seq, _errorStatus, _errorIndex, _varbindSection);
        }
        
        public GetRequestPdu(byte[] raw)
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
            //SnmpBER mess = new SnmpBER(SnmpType.Array,
            //                           new Universal(0), // version-1
            //                           new Universal(_community),
            //                           new SnmpBER(SnmpType.GetRequestPDU,
            //                                       new Universal(seq),
            //                                       new Universal(0), // errorStatus
            //                                       new Universal(0), // errorIndex
            //                                       new Universal(vbinds)));
            Int ver = new Int((int)version);
            OctetString comm = new OctetString(community);
            GetRequestPdu pdu = this;
            SnmpArray array = new SnmpArray(ver, comm, pdu);
            return array;
        }

        #endregion

        #region ISnmpData Members

        public SnmpType TypeCode
        {
            get { return SnmpType.GetRequestPDU; }
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
