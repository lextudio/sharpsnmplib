using SharpSnmpLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Globalization;

// ASN.1 BER encoding library by Malcolm Crowe at the University of the West of Scotland
// See http://cis.paisley.ac.uk/crow-ci0
// This is version 0 of the library, please advise me about any bugs
// mailto:malcolm.crowe@paisley.ac.uk

// Restrictions: It is assumed that no encoding has index length greater than 2^31-1.
// UNIVERSAL TYPES
// Some of the more unusual Universal encodings are supported but not fully implemented
// Should you require these types, as an alternative to changing this code
// you can catch the exception that is thrown and examine the contents yourself.
// APPLICATION TYPES
// If you want to handle Application types systematically, you can derive index class from
// Universal, and provide the Creator and Creators methods for your class
// You will see an example of how to do this in the Snmplib
// CONTEXT AND PRIVATE TYPES
// Ad hoc coding can be used for these, as an alterative to derive index class as above.

namespace SharpSnmpLib
{
	public struct Int // This namespace has its own concept of Integer
		: ISnmpData, IEquatable<Int>
	{
		byte[] _raw;

		public Int(byte[] raw)
		{
			_raw = raw;
            _bytes = null;
		}

		public Int(int iVal)
		{
            _bytes = null;
			if (iVal>=-127 && iVal<=127)
			{
				_raw = new byte[1];
				_raw[0] = (byte)iVal;
			}
			else
			{
				IList<byte> v = new List<byte>();
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
				_raw = new byte[v.Count];
				int len = 0;
				for (int j=v.Count-1;j>=0;j--)
				{	_raw[len++] = v[j]; }
			}
		}

		public Int(long i64Val)
		{
            _bytes = null;
			if (i64Val>=-127 && i64Val<=127)
			{
				_raw = new byte[1];
				_raw[0] = (byte)i64Val;
			}
			else
			{
				IList<byte> v = new List<byte>();
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
				_raw = new byte[v.Count];
				int len = 0;
				for (int j=v.Count-1;j>=0;j--)
					_raw[len++] = v[j];
			}
		}

        public int ToInt32()
        {
            if (_raw.Length > 4)
            {
                throw (new SharpSnmpException("truncation error for 32-bit integer coding"));
            }
            int iVal = ((_raw[0] & 0x80) == 0x80) ? -1 : 0; // sign extended! Guy McIlroy
            for (int j = 0; j < _raw.Length; j++)
            {
                iVal = (iVal << 8) | (int)_raw[j];
            }
            return iVal;
        }

        public long ToInt64()
        {
            if (_raw.Length > 8)
            {
                throw (new SharpSnmpException("truncation error for 64-bit integer coding"));
            }
            long i64Val = ((_raw[0] & 0x80) == 0x80) ? -1 : 0; // sign extended! Guy McIlroy
            for (int j = 0; j < _raw.Length; j++)
            {    
                i64Val = (i64Val << 8) | (long)_raw[j];
            }
            return i64Val;
        }

        //static public implicit operator int(Int x)
        //{
        //    if (x._raw.Length > 4)
        //        throw (new Exception("truncation error for 32-bit integer coding"));
        //    int iVal = ((x._raw[0] & 0x80) == 0x80) ? -1 : 0; // sign extended! Guy McIlroy
        //    for (int j = 0; j < x._raw.Length; j++)
        //        iVal = (iVal << 8) | (int)x._raw[j];
        //    return iVal;
        //}

        //static public implicit operator long(Int x)
        //{
        //    if (x._raw.Length > 8)
        //        throw (new Exception("truncation error for 64-bit integer coding"));
        //    long i64Val = ((x._raw[0] & 0x80) == 0x80) ? -1 : 0; // sign extended! Guy McIlroy
        //    for (int j = 0; j < x._raw.Length; j++)
        //        i64Val = (i64Val << 8) | (long)x._raw[j];
        //    return i64Val;
        //}

		public override string ToString()
		{
			return ToInt64().ToString(CultureInfo.CurrentCulture);
		}
		
		public SnmpType TypeCode {
			get {
				return SnmpType.Integer;
			}
		}
		
		byte[] _bytes;
		
		public byte[] ToBytes()
		{
			if (_bytes == null) {
                _bytes = ByteTool.ToBytes(TypeCode, _raw);
			}
			return _bytes;
		}

        internal byte[] GetRaw()
        {
            return _raw;
        }
        
		public override int GetHashCode()
		{
			return ToInt64().GetHashCode();
		}
        
		public override bool Equals(object obj)
		{
			if (obj == null) {
				return false;
			}
			if (object.ReferenceEquals(this, obj)) {
				return true;
			}
			if (GetType() != obj.GetType()) {
				return false;	
			}
			return Equals((Int)obj);
		}
		
		public bool Equals(Int other)
		{
			return ByteTool.CompareRaw(_raw, other._raw);
		}
		
		public static bool operator == (Int left, Int right)
		{
			return left.Equals(right);
		}
		
		public static bool operator != (Int left, Int right)
		{
			return !(left == right);
		}
    }
	// all references here are to ITU-X.690-12/97
}
