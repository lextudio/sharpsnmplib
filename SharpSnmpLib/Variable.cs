/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/25
 * Time: 20:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using Snmp;
using X690;

namespace SharpSnmpLib
{
	public class Variable
	{
		ObjectIdentifier _oid;
		object _data;

        public Variable(ObjectIdentifier oid)
        {
            _oid = oid;
        }
        
        public Variable(ObjectIdentifier oid, OctetString data)
        {
        	_oid = oid;
        	_data = data;
        	_dataUniversal = new Universal(data);
        }
		
		public Variable(Universal varbind)
		{
	        Universal[] content = (Universal[])varbind.Value;
			_oid = (ObjectIdentifier)content[0].Value;
			_dataUniversal = content[1];
			_data = content[1].Value;
		}
		
		public ObjectIdentifier Id
		{
			get
			{
				return _oid;
			}
		}
		
		public SnmpType DataType
		{
			get
			{
				return ((ISnmpData)_data).DataType;
			}
		}
		
		Universal _dataUniversal;
		
		public Universal DataUniversal
		{
			get 
			{
				return _dataUniversal;
			}
		}
		
		public object Data
		{
			get
			{
				return _data;
			}
            set
            {
                Universal data = value as Universal;
                if (data == null)
                {
                    throw new ArgumentException("wrong data");
                }
                _dataUniversal = data;
                _data = data.Value;
            }
		}
	}
}
