
using System.IO;
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
	public class SnmpBER : Universal
	{
		// These 3 declarations are needed to make the Universal machinery for SEQUENCEs work correctly
		protected override Universal Creator(Stream s) { return new SnmpBER(s); }
		protected override Universal[] Creators(int n) { return new SnmpBER[n]; }
		protected override bool ValueOf(uint n)
		{					  
			switch ((SnmpType)(type.ToByte()))
			{
				case SnmpType.UInt32: val = GetUInt(); break;
				case SnmpType.Counter32:
					val = (uint)GetUInt(); break;
				case SnmpType.Counter64:
					val = GetULong(); break;
				case SnmpType.Gauge:
					val = GetUInt(); break;
				case SnmpType.Timeticks:
					val = GetUInt(); break;
				case SnmpType.IpAddress:
					val = b; break;
				default:
					return base.ValueOf(n); // handle Universal types
			}
			return true;
		}
		public override string ToString()
		{
			if (val==null)
				ValueOf(len);
			if (val is byte[]) 
			{
				string r = "";
				for (int j=0;j<len;j++) 
				{
					r += b[j];
					if (j<len-1)
						r += ".";
				}
				return r;
			} else
				return base.ToString();
		}
		public SnmpBER(Stream s) : base(s) {}
		public SnmpBER(SnmpType t)
		{
			b = new byte[0];
			e = true;
			type = new SnmpTag(t);
		}
		public SnmpBER(ulong c,SnmpType t)
		{
			type = new SnmpTag(t);
			e = true;
			b = new byte[ULongLength(c)];
			int n=0;
			PutULong(ref n,c);
		}
		public SnmpBER(uint c) : this((ulong)c,SnmpType.Counter32) {}
		public SnmpBER(byte[] a) // IPAddress
		{
			type = new SnmpTag(SnmpType.IpAddress);
			e = true;
			b = a;
		}
		public SnmpBER(SnmpType t,params Universal[] obs) : base(new SnmpTag(t),obs) {}
		void PutULong(ref int ln,ulong a) 
		{
			if (a==0)
			{
				b[ln++] = 0;
				return;
			}
			byte[] c = new byte[16];
			int j = 0;
			while (a>0) 
			{
				c[j++] = (byte)(a&0xff);
				a = a>>8;
			}
			while (j>0) 
				b[ln++] = c[--j];
		}
		uint ULongLength(ulong a)
		{
	            if (a == 0)
	                return 1;
			uint j=0;
			while (a>0)
			{
				j++;
				a = a>>8;
			}
			return j;
		}
		ulong GetULong()
		{
			ulong r = 0;
			int p = 0;
			byte x;
			do 
			{
				x = b[p++];
				r = (r<<8) + (ulong)x;	
			} while (p<len);
			return r;
		}	
		ulong GetUInt()
		{
			uint r = 0;
			int p = 0;
			byte x;
			do 
			{
				x = b[p++];
				r = (r<<8) + (uint)x;	
			} while (p<len);
			return r;
		}	
	}
}
