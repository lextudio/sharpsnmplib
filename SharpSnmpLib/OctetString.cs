using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Text;

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

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// OctetString type.
    /// </summary>
	public struct OctetString // This namespace has its own concept of string
		: ISnmpData, IEquatable<OctetString>
	{
		byte[] _raw;
        byte[] _bytes;

        /// <summary>
        /// Creates an <see cref="OctetString"/> from raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
		public OctetString(byte[] raw)
		{
			_raw = raw;
            _bytes = null;
		}

        //public OctetString(byte[] buffer,int index,int count)
        //    : this(new MemoryStream(buffer, index, count, false).ToArray()) { }
        /// <summary>
        /// Creates an <see cref="OctetString"/> with a specific <see cref="String"/>.
        /// </summary>
        /// <param name="str"></param>
		public OctetString(string str)
            : this(Encoding.ASCII.GetBytes(str))
		{
			if (str == null) 
			{
				throw new ArgumentNullException("str");
			}
            //TODO: current implementation is ASCII only.
		}

//		static public implicit operator string(OctetString x)
//		{
//			if (x == null)
//			{	
//				return null;
//			}
////			int index = x._raw.Length;
////			if (index == 0)
////				return "";
////			StringBuilder sb = new StringBuilder(index, index);
////			for (int j = 0; j < index; j++)
////				sb.Append((char)x._raw[j]);
////			return sb.ToString();
//			return Encoding.ASCII.GetString(x._raw);
//		}
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="OctetString"/>.
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			if (_raw.Length==8||_raw.Length==11) // may be index date
			{
				uint yr = _raw[0];
				yr = yr*256 + _raw[1];
				uint mo = _raw[2];
				uint dy = _raw[3];
				if (yr<2005&&yr>1990 && mo<13 && dy<32)
					return ""+dy+"/"+mo+"/"+yr;
			}
			return Encoding.ASCII.GetString(_raw);//(string)this;
		}
//		static public OctetString operator+ (OctetString other,OctetString str)
//		{
//			MemoryStream stream = new MemoryStream();
//			stream.Write(other._raw, 0, other._raw.Length);
//			stream.Write(str._raw, 0, other._raw.Length);
////			int j;
////			byte[] r = new byte[other._raw.Length+str._raw.Length];
////			for (j=0;j<other._raw.Length;j++)
////				r[j] = other._raw[j];
////			for (j=0;j<str._raw.Length;j++)
////				r[j+other._raw.Length] = str._raw[j];
//			byte[] r = stream.ToArray();
//			return new OctetString(r);
//		}
		/// <summary>
		/// Type code.
		/// </summary>
		public SnmpType TypeCode {
			get {
				return SnmpType.OctetString;
			}
		}
		/// <summary>
		/// Converts to byte format.
		/// </summary>
		/// <returns></returns>
		
		public byte[] ToBytes()
		{
			if (_bytes == null) {
				MemoryStream m = new MemoryStream();
				m.WriteByte((byte)TypeCode);
				ByteTool.WriteMultiByteLength(m, _raw.Length);
				m.Write(_raw, 0, _raw.Length);
				_bytes = m.ToArray();
			}
			return _bytes;
		}
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><value>true</value> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <value>false</value>.
        ///</returns>
		public bool Equals(OctetString other)
		{
			return ByteTool.CompareRaw(_raw, other._raw);
		}
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="OctetString"/>. 
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="OctetString"/>. </param>
        /// <returns><value>true</value> if the specified <see cref="Object"/> is equal to the current <see cref="OctetString"/>; otherwise, <value>false</value>.
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
			return Equals((OctetString)obj);
		}
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="OctetString"/>.</returns>
		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}
        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <param name="left">Left <see cref="OctetString"/> object</param>
        /// <param name="right">Right <see cref="OctetString"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are equal, <c>false</c> otherwise.</returns>
        public static bool operator ==(OctetString left, OctetString right)
        {
            return left.Equals(right);
        }
        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <param name="left">Left <see cref="OctetString"/> object</param>
        /// <param name="right">Right <see cref="OctetString"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        public static bool operator !=(OctetString left, OctetString right)
        {
            return !(left == right);
        }
    }
	// all references here are to ITU-X.690-12/97
}
