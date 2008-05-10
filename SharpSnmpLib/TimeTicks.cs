using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib
{
	/// <summary>
	/// TimeTicks type.
	/// </summary>
	/// <remarks>Represents SNMP TimeTicks type.</remarks>
	public struct TimeTicks: ISnmpData, IEquatable<TimeTicks>
    {		
        byte[] _bytes;
        Integer32 _count;
		/// <summary>
		/// Creates a <see cref="TimeTicks"/> instance with a specific count.
		/// </summary>
		/// <param name="count">Count</param>
		public TimeTicks(int count) 
        {
            _count = new Integer32(count);
            _bytes = null;
        }
		/// <summary>
		/// Creates a <see cref="TimeTicks"/> instance with raw bytes.
		/// </summary>
		/// <param name="raw">Raw bytes</param>
        public TimeTicks(byte[] raw)
        {
            _count = new Integer32(raw);
            _bytes = null;
        }
		/// <summary>
		/// Returns an <see cref="Int32"/> that represents the current <see cref="TimeTicks"/>
		/// </summary>
		/// <returns></returns>
        public int ToInt32()
        {
            byte[] raw = _count.GetRaw();
            if (raw.Length > 4)
            {
                throw (new SharpSnmpException("truncation error for 32-bit integer coding"));
            }
            int result = ((raw[0] & 0x80) == 0x80) ? -1 : 0; // sign extended! Guy McIlroy
            for (int j = 0; j < raw.Length; j++)
            {
                result = (result << 8) | (int)raw[j];
            }
            return result;
        }
#region disable cast operators
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
#endregion		
		/// <summary>
		/// Type code.
		/// </summary>
		public SnmpType TypeCode {
			get {
				return SnmpType.TimeTicks;
			}
		}
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="TimeTicks"/>. 
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="TimeTicks"/>. </param>
        /// <returns><value>true</value> if the specified <see cref="Object"/> is equal to the current <see cref="TimeTicks"/>; otherwise, <value>false</value>.
		/// </returns>
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
			return Equals((TimeTicks)obj);
		}
		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>A hash code for the current <see cref="TimeTicks"/>.</returns>
		public override int GetHashCode()
		{
			return _count.GetHashCode();
		}

        #region ISnmpData Members

		/// <summary>
		/// To byte format.
		/// </summary>
		/// <returns></returns>
        public byte[] ToBytes()
        {
            if (_bytes == null)
            {
                _bytes = ByteTool.ToBytes(TypeCode, _count.GetRaw());
            }
            return _bytes;
        }

        #endregion
		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns><value>true</value> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <value>false</value>.
		///</returns>
		public bool Equals(TimeTicks other)
		{
			return _count == other._count;
		}
		/// <summary>
		/// The equality operator.
		/// </summary>
		/// <param name="left">Left <see cref="TimeTicks"/> object</param>
		/// <param name="right">Right <see cref="TimeTicks"/> object</param>
		/// <returns>
		/// Returns <c>true</c> if the values of its operands are equal, <c>false</c> otherwise.</returns>
		public static bool operator == (TimeTicks left, TimeTicks right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// The inequality operator.
		/// </summary>
		/// <param name="left">Left <see cref="TimeTicks"/> object</param>
		/// <param name="right">Right <see cref="TimeTicks"/> object</param>
		/// <returns>
		/// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
		public static bool operator != (TimeTicks left, TimeTicks right)
		{
			return !(left == right);
		}
    }
}
