using System;
using System.Collections.Generic;
using System.Text;
using X690;
using System.Net;

namespace SharpSnmpLib
{
    public class IpAddress: OctetString, ISnmpData
    {
        public IpAddress(byte[] bytes)
            : base(bytes)
        {
            if (bytes.Length != 4)
            {
                throw new ArgumentException("bytes must contain 4 elements");
            }
        }

        IPAddress _ip;

        IPAddress ip
        {
            get
            {
                if (_ip == null)
                {
                    _ip = new IPAddress(octVal);
                }
                return _ip;
            }
        }

        public override string ToString()
        {
            return ip.ToString();
        }

        public IPAddress ToIPAddress()
        {
            return ip;
        }
    	
		public override Snmp.SnmpType DataType {
			get {
				return Snmp.SnmpType.IpAddress;
			}
		}
    }
}
