/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/2
 * Time: 13:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Text;

namespace SharpSnmpLib
{
	/// <summary>
	/// Description of GeneralString.
	/// </summary>
	public struct GeneralString : ISnmpData, IEquatable<GeneralString>
	{
		byte[] _raw;
		
		public GeneralString(byte[] raw)
		{
			_raw = raw;
		}
		
		public SnmpType TypeCode {
			get {
				return SnmpType.GeneralString;
			}
		}
		
		public override string ToString()
		{
			return ASCIIEncoding.ASCII.GetString(_raw);
		}
		
		public byte[] ToBytes()
		{
			return ByteTool.ToBytes(TypeCode, _raw);
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
            if(GetType() != obj.GetType())
            {
                return false;
            }
            return Equals((GeneralString)obj);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool operator ==(GeneralString left, GeneralString right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GeneralString left, GeneralString right)
        {
            return !(left == right);
        }

        #region IEquatable<GeneralString> Members

        public bool Equals(GeneralString other)
        {
            return ByteTool.CompareRaw(_raw, other._raw);
        }

        #endregion
    }
}
