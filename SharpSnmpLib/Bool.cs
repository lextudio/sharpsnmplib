/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/2
 * Time: 13:28
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace SharpSnmpLib
{
	/// <summary>
	/// Description of Bool.
	/// </summary>
	public struct Bool : ISnmpData, IEquatable<Bool>
	{
		byte[] _raw;
		
		public Bool(byte[] raw)
		{
			if (null == raw) 
			{
				throw new ArgumentNullException("raw");
			}
			if (raw.Length != 1) 
			{
				throw new ArgumentException("raw must be one item");
			}
			_raw = raw;
		}
		
		public bool ToBoolean()
		{
			return (_raw[0] > 0);
		}
		
		public SnmpType TypeCode {
			get {
				return SnmpType.Boolean;
			}
		}
		
		public byte[] ToBytes()
		{
			return ByteTool.ToBytes(TypeCode, _raw);
		}

        public override int GetHashCode()
        {
            return ToBoolean().GetHashCode();
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
            return Equals((Bool)obj);
        }

        public static bool operator ==(Bool left, Bool right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Bool left, Bool right)
        {
            return !(left == right);
        }

        #region IEquatable<Bool> Members

        public bool Equals(Bool other)
        {
            return ToBoolean() == other.ToBoolean();
        }

        #endregion
    }
}
