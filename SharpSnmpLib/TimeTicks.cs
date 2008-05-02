using System;
using System.Collections.Generic;
using System.Text;

namespace SharpSnmpLib
{
	public struct Timeticks: ISnmpData, IEquatable<Timeticks>
    {
        Int _count;

		public Timeticks(int count) 
        {
            _count = new Int(count);
            _bytes = null;
        }
		
        public Timeticks(byte[] raw)
        {
            _count = new Int(raw);
            _bytes = null;
        }

        public int ToInt32()
        {
            byte[] raw = _count.GetRaw();
            if (raw.Length > 4)
            {
                throw (new SharpSnmpException("truncation error for 32-bit integer coding"));
            }
            int iVal = ((raw[0] & 0x80) == 0x80) ? -1 : 0; // sign extended! Guy McIlroy
            for (int j = 0; j < raw.Length; j++)
            {
                iVal = (iVal << 8) | (int)raw[j];
            }
            return iVal;
        }

        //static public implicit operator int(Timeticks x)
        //{
        //    byte[] raw = x._count.GetRaw();
        //    if (raw.Length > 4)
        //    { 
        //        throw (new Exception("truncation error for 32-bit integer coding"));
        //    }
        //    int iVal = ((raw[0] & 0x80) == 0x80) ? -1 : 0; // sign extended! Guy McIlroy
        //    for (int j = 0; j < raw.Length; j++)
        //    {
        //        iVal = (iVal << 8) | (int)raw[j];
        //    }
        //    return iVal;
        //}

        //static public implicit operator long(Timeticks x)
        //{
        //    byte[] raw = x._count.GetRaw();
        //    if (raw.Length > 8)
        //    {
        //        throw (new Exception("truncation error for 64-bit integer coding"));
        //    }
        //    long i64Val = ((raw[0] & 0x80) == 0x80) ? -1 : 0; // sign extended! Guy McIlroy
        //    for (int j = 0; j < raw.Length; j++)
        //    {
        //        i64Val = (i64Val << 8) | (long)raw[j];
        //    }
        //    return i64Val;
        //}
		
		public SnmpType TypeCode {
			get {
				return SnmpType.Timeticks;
			}
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
			return Equals((Timeticks)obj);
		}
		
		public override int GetHashCode()
		{
			return _count.GetHashCode();
		}

        #region ISnmpData Members

        byte[] _bytes;

        public byte[] ToBytes()
        {
            if (_bytes == null)
            {
                _bytes = ByteTool.ToBytes(TypeCode, _count.GetRaw());
            }
            return _bytes;
        }

        #endregion
		
		public bool Equals(Timeticks other)
		{
			return _count == other._count;
		}
		
		public static bool operator == (Timeticks left, Timeticks right)
		{
			return left.Equals(right);
		}
		
		public static bool operator != (Timeticks left, Timeticks right)
		{
			return !(left == right);
		}
    }
}
