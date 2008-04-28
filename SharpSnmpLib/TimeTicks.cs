using System;
using System.Collections.Generic;
using System.Text;
using X690;

namespace SharpSnmpLib
{
	public class Timeticks: Integer, ISnmpData
    {
        public Timeticks(byte[] bytes)
            : base(bytes)
        { }
		
		public override Snmp.SnmpType DataType {
			get {
				return Snmp.SnmpType.Timeticks;
			}
		}
    }
}
