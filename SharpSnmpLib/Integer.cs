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
	public class Integer // This namespace has its own concept of Integer
		: ISnmpData
	{
		public byte[] octVal = null;
		public Integer(byte[] b)
		{
			octVal = b;
		}
		public Integer(int iVal)
		{
			if (iVal>=-127 && iVal<=127)
			{
				octVal = new byte[1];
				octVal[0] = (byte)iVal;
			} 
			else 
			{
				ArrayList v = new ArrayList();
				int n = iVal;
				while (n!=0 && n!=-1)
				{
	                    if (n<256 && n>=128)
	                    {
	                        v.Add((byte)n);
	                        v.Add((byte)0);
	                        break;
	                    }
					v.Add((byte)(n&0xff));
					n >>= 8;
				}
				octVal = new byte[v.Count];
				int len = 0;
				for (int j=v.Count-1;j>=0;j--)
					octVal[len++] = (byte)v[j];
			}
		}
		public Integer(long i64Val)
		{
			if (i64Val>=-127 && i64Val<=127)
			{
				octVal = new byte[1];
				octVal[0] = (byte)i64Val;
			} 
			else 
			{
				ArrayList v = new ArrayList();
				System.Int64 n = i64Val;
				while (n!=0 && n!=-1)
				{
	                    if (n < 256 && n >= 128)
	                    {
	                        v.Add((byte)n);
	                        v.Add((byte)0);
	                        break;
	                    }
	                    v.Add((byte)(n & 0xff));
					n >>= 8;
				}
				octVal = new byte[v.Count];
				int len = 0;
				for (int j=v.Count-1;j>=0;j--)
					octVal[len++] = (byte)v[j];
			}
		}
		static public implicit operator int(Integer x)
		{
			if (x.octVal.Length>4)
					throw(new Exception("truncation error for 32-bit integer coding"));
	                int iVal = ((x.octVal[0] & 0x80) == 0x80) ? -1 : 0; // sign extended! Guy McIlroy
			for (int j=0;j<x.octVal.Length;j++)
					iVal = (iVal<<8)|(int)x.octVal[j];
			return iVal;
		}
		static public implicit operator long(Integer x)
		{
			if (x.octVal.Length>8)
				throw(new Exception("truncation error for 64-bit integer coding"));
	            long i64Val = ((x.octVal[0] & 0x80) == 0x80) ? -1 : 0; // sign extended! Guy McIlroy
			for (int j=0;j<x.octVal.Length;j++)
				i64Val = (i64Val<<8)|(long)x.octVal[j]; 
			return i64Val;
		}
		public override string ToString()
		{
			return ((long)this).ToString();
		}
		
		public virtual Snmp.SnmpType DataType {
			get {
				return Snmp.SnmpType.Integer;
			}
		}
	}
	// all references here are to ITU-X.690-12/97
}
