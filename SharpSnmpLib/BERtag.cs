using Snmp;
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
	public class BERtag : BER
	{
		// Internal representation of Identifier octets 8.1.2.3
		public BERtype atp; 
		public bool comp;
		public ulong tag;
		public BERtag() { atp=BERtype.Universal; comp=false; tag=0; } // EndMarker
		public BERtag(BERtype a,bool c, ulong t) { atp=a; comp=c; tag=t; }
		public BERtag(SnmpType b) : this((byte)b) {}
		public BERtag(byte b) 
		{
			// decoding 8.1.2.4.3 for small tag values
			atp = (BERtype)(((uint)b&0xc0)>>6);
			comp = ((b&0x20)!=0);
			tag = (ulong)(b&0x1f);
			if (tag==0x1f)
				throw(new Exception("BER bad byte tag"));
		}
		public BERtag(Stream s) 
		{
			// decoding 8.1.2.4.3
			byte b = ReadByte(s);
			atp = (BERtype)((b&0xc0)>>6);
			comp = ((b&0x20)!=0);
			tag = (ulong)(b&0x1f);
			if ((b&0x1f)==0x1f) 
				tag = GetBigTag(s);
		}
		public void Send(Stream s)
		{
			// encoding 8.1.2.4.3
			byte b = (byte)(((uint)atp)<<6 | (uint)(comp?0x20:0));
			if (tag<31) 
			{
				b |= (byte)tag;
				s.WriteByte(b);
				return;
			}
			b |=31;
			s.WriteByte(b);
			PutBigTag(s,tag);
		}
		public byte ToByte()
		{
			byte b = (byte)(((uint)atp<<6) | (uint)(comp?0x20:0));
			if (tag>=31) 
				throw(new Exception("BERtag out of range for convert to byte"));
			b |= (byte)tag;
			return b;
		}
		public bool isEndMarker
		{
			get { return (atp==BERtype.Universal && !comp && tag==0); }
		}
		public ulong GetBigTag(Stream s)
		{
			ulong r = 0;
			byte x = ReadByte(s);
			do 
			{
				r = (r<<7)+(ulong)(x&0x7f);
			} while ((x&0x80)!=0);
			return r;
		}
		public void PutBigTag(Stream s,ulong n)
		{
			byte[] c = new byte[16];
			int j=0;
			while (n>0)
			{
				c[j++] = (byte)(n&0x7f);
				n = n>>7;
			}
			while (j>0) 
			{
				byte x = c[--j];
				if (j>0)
					x |= 0x80;
				WriteByte(s,x);
			}
		}
		public int ExtLength() // calculate length required for identifier octets
		{
			if (tag<31)
				return 1;
			ulong a = tag;
			int j = 1;
			while (a>0) 
			{
				j++;
				a = a>>7;
			}
			return j;
		}
		public override string ToString()
		{
			if (atp==BERtype.Universal && !comp)
				return ((SnmpType)tag).ToString().ToUpper();
			return "[" + atp.ToString().ToUpper()+" "+(comp?"SEQUENCE ":"")+tag+"]";
		}
	}
	// all references here are to ITU-X.690-12/97
}
