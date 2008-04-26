using System;
using System.Collections;
using System.Diagnostics;

namespace X690
{
	public class BitSet // BitArray seems to be bad news, so here goes
	{
		uint nbits;
		public uint size;
		public int[] bits;
		public int Length { get { return (int)nbits; }}
		public BitSet(uint n) { 
			nbits = n; 
			size = (n+31)/32; 
			bits = new int[size];  
		}
		public BitSet(BitSet b) 
		{ 
			nbits = b.nbits;
			size = b.size;
			bits = (int[])b.bits.Clone();
		}
		public bool this[uint n] 
		{
			get { return (bits[n>>5]&(1<<(31-((int)n&31))))!=0; } 
			set { int bit = 1<<(31-((int)n&31));
				if (value) 
					bits[n>>5] |= bit;
				else
					bits[n>>5] &= ~bit;
			}
		}
		public BitSet And (BitSet a)
		{
			Debug.Assert(nbits==a.nbits);
			BitSet r = new BitSet(nbits);
			for (uint j=0;j<size;j++)
				r.bits[j] = bits[j]&a.bits[j];
			return r;
		}
		public BitSet Or (BitSet a)
		{
			Debug.Assert(nbits==a.nbits);
			BitSet r = new BitSet(nbits);
			for (uint j=0;j<size;j++)
				r.bits[j] = bits[j]|a.bits[j];
			return r;
		}
		public uint Card
		{ get {
			uint r = 0;
			for (uint i=0;i<nbits;i++)
				if (this[i])
					r++;
			  return r;
		  }	}
		public BitSet Cat (BitSet a)
		{
			BitSet r = new BitSet(nbits+a.nbits);
			uint i=0,j;
			for (j=0;j<nbits;j++)
				r[i++]=this[j];
			for (j=0;j<a.nbits;j++)
				r[i++]=a[j];
			return r;
		}
		public override bool Equals(object o)
		{
			BitSet a = (BitSet) o;
			Debug.Assert(nbits==a.nbits);
			for (int i=0;i<size;i++)
				if (bits[i]!=a.bits[i]) 
					return false;
			return true;
		}
		public override int GetHashCode()
		{
			int n = 0;
			for (uint j=0;j<size;j++)
					n += bits[j];
			return n;
		}

		public override string ToString()
		{
			string r="";
			for (int i=0;i<nbits;i++)
				if ((bits[i>>5]&(1<<(i&31)))!=0)
					r+="1";
				else
					r+="0";
			return r;
		}
	}
}
