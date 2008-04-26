
using System;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using System.Text;
using System.Net;
using X690;

// SNMP library for .NET by Malcolm Crowe at University of the West of Scotland
// http://cis.paisley.ac.uk/crow-ci0/
// This is version 0 of the library. Email bugs to
// mailto:malcolm.crowe@paisley.ac.uk

// Getting Started
// The simplest way to get an SNMP value from a host is
// ManagerItem mi = new ManagerItem(
//								new ManagerSession(hostname,"public"),
//								"1.3.6.1.2.1.1.4.0");
// Then the actual OID is mi.Name and the value is in mi.Value.ToString().

// TODO: Tables, lists of bindings
//		 Friendly strings derived from MIBs

namespace Snmp
{
	public class SnmpTag : BERtag
	{
		public SnmpTag(SnmpType s) : base((byte)s) {}
		public SnmpTag() : base() {}
		public SnmpTag(byte t) : base(t) {}
		public override string ToString()
		{
			if (atp==BERtype.Universal)
				return base.ToString();
			return ((SnmpType)ToByte()).ToString().ToUpper();
		}
	}
}
