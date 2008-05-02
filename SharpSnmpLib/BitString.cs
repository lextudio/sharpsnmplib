using SharpSnmpLib;
using System;
using System.Collections;
using System.Diagnostics;

namespace SharpSnmpLib
{
    //TODO: remove all setters.
	public struct BitString: ISnmpData, IEquatable<BitString> // BitArray seems to be bad news, so here goes
	{
		int _nbits;
		int _size;
		int[] _bits;
		public int Length { get { return (int)_nbits; }}
		byte[] _raw;
		
		public BitString(byte[] raw)
		{
			_nbits = raw.Length * 8 - raw[0];//8.6.2
			_size = (_nbits + 31) / 32;
			_bits = new int[_size];
			for (int j = 0; j < _size; j++)
			{
				_bits[j] = (raw[4 * j] << 24) | (raw[4 * j + 1] << 16) | (raw[4 * j + 2] << 8) | raw[4 * j + 3];
			}
			_bytes = null;
			_raw = raw;
		}
		
        //public BitString(int index) {
        //    _nbits = index;
        //    _size = (index+31)/32;
        //    _bits = new int[_size];
        //    _bytes = null;
        //    _raw = parseItem(_nbits, _bits);
        //}

        public BitString(int nbits, int[] bits)
        {
            if (bits.Length != (nbits + 31) / 32)
            {
                throw new ArgumentException("wrong bits length");
            }
            _nbits = nbits;
            _size = bits.Length;
            _bits = bits;
            _bytes = null;
            _raw = parseItem(_nbits, _bits);
        }
		
		public BitString(BitString str)
		{
			_bytes = null;
			_raw = (byte[])str._raw.Clone();
			_nbits = str._nbits;
			_size = str._size;
			_bits = (int[])str._bits.Clone();
		}

		public bool this[int index]
		{
			get { return (_bits[index>>5]&(1<<(31-((int)index&31))))!=0; }
            //set { int bit = 1<<(31-((int)index&31));
            //    if (value)
            //        _bits[index>>5] |= bit;
            //    else
            //        _bits[index>>5] &= ~bit;
            //    OnBitsChanged();
            //}
		}

		public BitString And (BitString other)
		{
			Debug.Assert(_nbits==other._nbits);
            int[] bits = new int[_size];
            for (uint j = 0; j < _size; j++)
            {
                bits[j] = _bits[j] & other._bits[j];
            }
			return new BitString(_nbits, bits);
		}

		public BitString Or (BitString other)
		{
			Debug.Assert(_nbits==other._nbits);
            int[] bits = new int[_size];
            for (uint j = 0; j < _size; j++)
            {
                bits[j] = _bits[j] | other._bits[j];
            }
            return new BitString(_nbits, bits);
            //BitString r = new BitString(_nbits);
            //for (uint j=0;j<_size;j++)
            //    r._bits[j] = _bits[j]|other._bits[j];
            //r.OnBitsChanged();
			//return r;
		}

		public int Card
		{
			get {
				int r = 0;
                for (int i = 0; i < _nbits; i++)
                {
                    if (this[i])
                    {
                        r++;
                    }
                }
				return r;
			}
		}
		public BitString Cat (BitString other)
		{
            int nbits = _nbits + other._nbits;
            int size = _size + other._size;
            int[] bits = new int[size];
            //BitString r = new BitString(_nbits+other._nbits);
            uint i=0,j;
            for (j = 0; j < _nbits; j++)
            {
                bits[i++] = _bits[j];
            }
            for (j = 0; j < other._nbits; j++)
            {
                bits[i++] = other._bits[j];
            }
            return new BitString(nbits, bits);
            //r.OnBitsChanged();
            //return r;
		}
		
		public override int GetHashCode()
		{
			int n = 0;
			for (uint j=0;j<_size;j++)
				n += _bits[j];
			return n;
		}

		public override string ToString()
		{
			string r="";
			for (int i=0;i<_nbits;i++)
				if ((_bits[i>>5]&(1<<(i&31)))!=0)
				r+="1";
			else
				r+="0";
			return r;
		}
		
		public SnmpType TypeCode {
			get {
				return SnmpType.BitString;
			}
		}
		
		byte[] _bytes;
		
		public byte[] ToBytes()
		{
			if (_bytes == null)
			{
				_bytes = ByteTool.ToBytes(TypeCode, _raw);
			}
			return _bytes;
		}
		
		static byte[] parseItem(int nbits, int[] bits)
		{
			// encoding 8.6.2
			int n = (nbits+7)/8;
			byte r = (byte)(8-nbits%8);
			byte[] result = new byte[n+1];
			int ln = 0;
			result[ln++] = r;
			int k = 24;
			int i = 0;
			for (int j=0;j<n;j++) 
			{
				result[ln++] = (byte)(bits[i]>>k);
				k -= 8;
				if (k<0) 
				{
					i++;
					k = 24;
				}
			}
			return result;
		}

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            if (GetType() != obj.GetType())
            {
                return false;
            }
            return Equals((BitString)obj);
        }

        public static bool operator ==(BitString left, BitString right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BitString left, BitString right)
        {
            return !(left == right);
        }

        #region IEquatable<BitString> Members

        public bool Equals(BitString other)
        {
            return ByteTool.CompareRaw(_raw, other._raw);
        }

        #endregion
    }
}
