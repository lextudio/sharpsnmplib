using SharpSnmpLib;
using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Text;

// ASN.1 BER encoding library by Malcolm Crowe at the University of the West of Scotland
// See http://cis.paisley.ac.uk/crow-ci0
// This is version 0 of the library, please advise me about any bugs
// mailto:malcolm.crowe@paisley.ac.uk

// Restrictions: It is assumed that no encoding has a length greater than 2^31-1.
// UNIVERSAL TYPES
// Some of the more unusual Universal encodings are supported but not fully implemented
// Should you require these types, as an alternative to changing this code
// you can catch the exception that is thrown and examine the contents yourself.
// APPLICATION TYPES
// If you want to handle Application types systematically, you can derive a class from
// Universal, and provide the Creator and Creators methods for your class
// You will see an example of how to do this in the Snmplib
// CONTEXT AND PRIVATE TYPES
// Ad hoc coding can be used for these, as an alterative to derive a class as above.

namespace X690
{
	public class OctetString // This namespace has its own concept of string
		: ISnmpData
	{
		public byte[] octVal = null;
		public OctetString(byte[] b)
		{
			octVal = b;
		}
		public OctetString(byte[] b,uint a,uint n)
		{
			octVal = new byte[n];
			for (int j=0;j<n;j++)
				octVal[j] = b[a+j];
		}
		public OctetString(string s)
		{
	            octVal = new byte[s.Length];
	            for (int j = 0; j < s.Length; j++)
	                octVal[j] = (byte)s[j];
		}
	        static public implicit operator string(OctetString x)
		{
	            if (x == null)
	                return null;
	            int n = x.octVal.Length;
	            if (n == 0)
	                return "";
	            StringBuilder sb = new StringBuilder(n, n);
	            for (int j = 0; j < n; j++)
	                sb.Append((char)x.octVal[j]);
	            return sb.ToString();
		}
	        public override string ToString()
		{
			if (octVal.Length==8||octVal.Length==11) // may be a date
			{
				uint yr = octVal[0];
				yr = yr*256 + octVal[1];
				uint mo = octVal[2];
				uint dy = octVal[3];
				if (yr<2005&&yr>1990 && mo<13 && dy<32) 
					return ""+dy+"/"+mo+"/"+yr;
			}
			return (string)this;
		}
		static public OctetString operator+ (OctetString a,OctetString b)
		{
			int j;
			byte[] r = new byte[a.octVal.Length+b.octVal.Length];
			for (j=0;j<a.octVal.Length;j++)
				r[j] = a.octVal[j];
			for (j=0;j<b.octVal.Length;j++)
				r[j+a.octVal.Length] = b.octVal[j];
			return new OctetString(r);
		}
		
		public virtual Snmp.SnmpType DataType {
			get {
				return Snmp.SnmpType.OctetString;
			}
		}
	}
	// all references here are to ITU-X.690-12/97
}
