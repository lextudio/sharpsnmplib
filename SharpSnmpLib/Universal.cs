using System;
using System.Net.Sockets;
using System.IO;
using System.Collections;
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
	// The following class provides for the Universal encodings and the encodings needed for SNMP
	// For application or context-specific encodings, derive a class from this and override the
	// Evaluate method, which creates an object called val based on the contents in byte[] b.
	// Evaluate takes a single uint parameter which is the number of bytes to use in b.
	public class Universal : BER 
	{
		// override these two functions in any subclass of Universal so that SEQUENCES work correctly
		protected virtual Universal Creator(Stream s) { return new Universal(s); }
		protected virtual Universal[] Creators(int n) { return new Universal[n]; }
		protected Universal() {}
		public static Universal Null 
		{
			get 
			{
				return new Universal(new BERtag(UniversalType.Null),new Universal[0]); 
			}
		}
		public Universal(bool x) // 8.2
		{
			type = new BERtag(UniversalType.Boolean);
			val = x;
			e = true;
			b = new byte[1];
			b[0] = (byte)(x?0xff:0);
		}
		public Universal(int n) // 8.3
		{
			type = new BERtag(UniversalType.Integer);
			val = n;
			e = true;
			b = new Integer(n).octVal;
		}
		public Universal(double d)
		{
			type = new BERtag(UniversalType.Real);
			val = d;
			e = true;
			b = new Real(d).octVal;
		}
		public Universal(BitSet a)
		{
			// encoding 8.6.2
			type = new BERtag(UniversalType.BitString);
			val = a;
			e = true;
			int n = (a.Length+7)/8;
			byte r = (byte)(8-a.Length%8);
			b = new byte[n+1];
			int ln = 0;
			b[ln++] = r;
			int k = 24;
			int i = 0;
			for (int j=0;j<n;j++) 
			{
				b[ln++] = (byte)(a.bits[i]>>k);
				k -= 8;
				if (k<0) 
				{
					i++;
					k = 24;
				}
			}
		}
		public Universal(string s)
		{
			type = new BERtag(UniversalType.OctetString);
			val = s;
			e = true;
			b = (new ASCIIEncoding()).GetBytes(s);
		}
		/// <summary>
		/// Create a <see cref="Universal"></see> instance for object identifier.
		/// </summary>
		/// <param name="oid">OID</param>
		public Universal(uint[] oid)
		{
			type = new BERtag(UniversalType.ObjectIdentifier);
			val = oid;
			e = true;
			int ln = 0;
			int j;
			for (j=1;j<oid.Length;j++)
				ln += LengthOIDEl(oid[j]);
			b = new byte[ln];
			if (oid[0]!=1 || oid[1]!=3)
				throw(new Exception("OID must begin with .1.3"));
			ln = 0;
			PutOIDEl(ref ln,43);
			for (j=2;j<oid.Length;j++)
				PutOIDEl(ref ln,oid[j]);
		}
		public Universal(ArrayList a)
		{
			type = new BERtag(0,true,0);
			e = true;
			ArrayList al = new ArrayList(a.Count);
			val = al;
			for (int j=0;j<a.Count;j++)
			{
				object o = a[j];
				if (o is bool)
					al.Add(new Universal((bool)o));
				else if (o is int)
					al.Add(new Universal((int)o));
				else if (o is BitSet)
					al.Add(new Universal((BitSet)o));
				else if (o is string)
					al.Add(new Universal((string)o));
				else if (o is uint[])
					al.Add(new Universal((uint[])o));
			}
		}
		public Universal(BERtag t,params Universal[] obs)
		{
			type = t;
			val = obs;
			e = true;
			val = obs;
		}
		public Universal(params Universal[] obs) : this(new BERtag(0,true,16),obs) {}
		protected bool Children(Stream s) // handle SEQUENCE
		{
			if (type.isEndMarker)
				return true;
			byte x = ReadByte(s);
			if (x==0x80) // use end-of-contents marker
			{
				ArrayList al = new ArrayList();
				val = al;
				Universal bb;
				do
				{
					bb = Creator(s);
					al.Add(bb);
				} while (!bb.type.isEndMarker);
				Universal[] obs = Creators(al.Count-1);
				for (int i=0;i<al.Count;i++)
					obs[i] = (Universal)al[i];
				val = obs;
				return true;
			}
			int ln = ReadLength(s,x);
			b = new byte[ln];
			if (type.comp) 
			{
				ArrayList c = new ArrayList();
				int n = 0;
				Universal d;
				do
				{
					long pos = s.Position;
					d = Creator(s);
					n += (int)(s.Position-pos);
					c.Add(d);
				} while (n<ln);
				Universal[] obs = Creators(c.Count);
				for (int i=0;i<c.Count;i++)
					obs[i] = (Universal)c[i];
				val = obs;
				return true;
			}
			return false;
		}
		public Universal(Stream s)
		{
			e = false;
			type = new BERtag(s);
			if (Children(s)) 
			{
				if(type.tag!=0x10)
					CombineValues();
				return;
			}
			s.Read(b,0,(int)len);
			ValueOf(len);
		}
		protected int ReadLength(Stream s,byte x) // x is initial octet
		{
			if ((x&0x80)==0)
				return (int)x;
			int u = 0;
			int n = (int)(x&0x7f);
			for (int j=0;j<n;j++)
			{
				x = ReadByte(s);
				u = (u<<8) + (int)x;	
			}
			return u;
		}
		protected void WriteLength(Stream s,int a) // excluding initial octet
		{
			if (a<=0)
			{
				s.WriteByte(0);
				return;
			}
			byte[] c = new byte[16];
			int j = 0;
			while (a>0) 
			{
				c[j++] = (byte)(a&0xff);
				a = a>>8;
			}
			s.WriteByte((byte)(0x80|j));
			while (j>0) 
			{
				int x = c[--j];
				s.WriteByte((byte)x);
			}
		}
		protected int LengthLength(int a) // excluding initial octet
		{
			int j=0;
			while (a>0)
			{
				j++;
				a = a>>8;
			}
			return j;
		}
		void CombineValues() // handle some special cases
		{
			Universal[] a = (Universal[])val;
			int j;
			switch ((UniversalType)(type.ToByte()))
			{
				case UniversalType.BitString: // 8.6.3
					BitSet r = (BitSet)a[0].val;
					for (j=1;j<a.Length;j++)
						r = r.Cat((BitSet)a[j].val);
					val = r; break;
				case UniversalType.OctetString: // 8.7.3
					OctetString s = (OctetString)a[0].val;
					for (j=1;j<a.Length;j++)
						s += (OctetString)a[j].val;
					val = s; break;
			}
		}
		protected virtual bool ValueOf(uint n)
		{					  
			int j=0;
			switch ((UniversalType)(type.ToByte()))
			{
				case UniversalType.Boolean: val = (b[0]>0); break;
				case UniversalType.Integer: 
					val = new Integer(b); break;
				case UniversalType.BitString:
					byte r = b[0];
					BitSet bs = new BitSet(n*8-r); // 8.6.2
					for (j=0;j<bs.size;j++) 
						bs.bits[j] = (b[4*j]<<24)|(b[4*j+1]<<16)|(b[4*j+2]<<8)|b[4*j+3];
					val = bs; break;
				case UniversalType.OctetString:
					val = new OctetString(b,0,n); break;
				case UniversalType.Null:
					val = null;
					break;
				case UniversalType.ObjectIdentifier:
					ArrayList al = new ArrayList();
					int p = 0;
					while (p<n)
						al.Add(GetOIDEl(ref p));
					uint[] oid;
					if (((uint)al[0])==43) 
					{
						oid = new uint[al.Count+1];
						oid[0] = 1;
						oid[1] = 3;
						for (j=1;j<al.Count;j++)
							oid[j+1] = (uint)al[j];
					} 
					else // can happen that oid is .0??
					{
						//				throw(new Exception("OID must begin with .1.3"));
						oid = new uint[al.Count];
						for (j=0;j<al.Count;j++)
							oid[j] = (uint)al[j];
					}
					val = oid; break;
				case UniversalType.Real:
					val = new Real(b); break;
	                case UniversalType.GeneralString:
	                    val = ASCIIEncoding.ASCII.GetString(b); break;
	                case UniversalType.Enumerated:
	                    val = new Integer(b); break;
				default:
					val = type.ToString()+" unimplemented"; break;
			}
			return true;
		}
		protected int Length
		{
			get 
			{
				if (e && val is Universal[]) 
				{
					int r = 0;
					Universal[] a = (Universal[])val;
					for (int j=0;j<a.Length;j++) 
					{
						Universal u = a[j];
						r += u.Length+LengthLength(u.Length)+1+u.type.ExtLength();
					}
					return r;
				} 
				else
					return (int)len;
			}
		}
		public void Send(Stream s) // create the external representation
		{
			type.Send(s); 
			WriteLength(s,Length);
			if (e && val is Universal[])
			{
				Universal[] a = (Universal[])val;
				for (int j=0;j<a.Length;j++) 
					a[j].Send(s);
			} 
			else 
				s.Write(b,0,(int)Length);
		}
		protected void PutOIDEl(ref int ln, uint a) 
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
				c[j++] = (byte)(a&0x7f);
				a = a>>7;
			}
			while (j>0) 
			{
				int x = c[--j];
				if (j>0)
					x |= 0x80;
				b[ln++] = (byte)x;
			}
		}
		protected uint GetOIDEl(ref int p)
		{
			uint r = 0;
			byte x;
			do 
			{
				x = b[p++];
				r = (r<<7) + ((uint)x&0x7f);	// 0.3.3a (Evan Watson)
			} while ((x&0x80)!=0);
			return r;
		}
		int LengthOIDEl(uint a)
		{
			int j = 0;
			if (a==0)
				return 1;
			while (a>0)
			{
				j++;
				a = a>>7;
			}
			return j;
		}
		public object Value { get { return val; }}
		public string Type()
		{
			string r = type.ToString();
			if (type.comp) 
			{
				r += "{\n";
				Universal[] al = (Universal[])val;
				for (int j=0;j<al.Length;j++) 
				{
					r += al[j].Type();
					if (j<al.Length-1)
						r += ",\n";
				}
				r += "}";
			} 
			return r;
		}
		public override string ToString()
		{
			string r = "";
			if (type.comp) 
			{
				r += "{\n";
				Universal[] al = (Universal[])val;
				for (int j=0;j<al.Length;j++) 
				{
					r += al[j].ToString();
					if (j<al.Length-1)
						r += ",\n";
				}
				r += "}";
			} 
			else 
			{
				if (val==null)
					ValueOf(len);
				switch ((UniversalType)type.ToByte()) // Guy McIlroy
				{
					case UniversalType.BitString:
						r += "["+val.ToString()+"]";
						break;
					case UniversalType.ObjectIdentifier:
						uint[] oid = (uint[])val;
						for (int k=0;k<oid.Length;k++) 
							r += "."+oid[k];
						break;
					case UniversalType.OctetString:
						r += "\""+val.ToString()+"\"";
						break;
					case UniversalType.Null:
						r += "NULL"; break;
					default:
						r += val.ToString();
						break;
				}
			}
			return r;
		}
		public void Dump()
		{
			string[] r = ToString().Split('\n');
			for (int i=0;i<r.Length;i++)
				System.Console.WriteLine(r[i]);
		}
		public Universal this[int ix] 
		{
			get 
			{
				Universal[] ls = (Universal[])Value;
				return ls[ix];
			}
			set
			{
				Universal[] ls = (Universal[])Value;
				ls[ix] = value;
			}
		}
	}
	// all references here are to ITU-X.690-12/97
}
