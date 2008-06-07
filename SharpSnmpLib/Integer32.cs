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

namespace Lextm.SharpSnmpLib
{
	/// <summary>
	/// Integer32 type in SMIv2 (or INTEGER in SMIv1).
	/// </summary>
	public struct Integer32 // This namespace has its own concept of Integer
		: ISnmpData, IEquatable<Integer32>
	{
		byte[] _raw;
		/// <summary>
		/// Creates an <see cref="Integer32"/> instance.
		/// </summary>
		/// <param name="raw">Raw bytes</param>
		public Integer32(byte[] raw)
		{
			_raw = raw;
			_bytes = ByteTool.ToBytes(SnmpType.Integer32, _raw);
		}
		/// <summary>
		/// Creates an <see cref="Integer32"/> instance with a specific <see cref="Int32"/>.
		/// </summary>
		/// <param name="value">Value</param>
		public Integer32(int value)
		{
			if (value>=-127 && value<=127)
			{
				_raw = new byte[1];
				_raw[0] = (byte)value;
			}
			else
			{
				IList<byte> v = new List<byte>();
				int n = value;
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
			_bytes = ByteTool.ToBytes(SnmpType.Integer32, _raw);
		}
//		/// <summary>
//		/// Creates an <see cref="Integer32"/> instance with a specific <see cref="Int64"/>.
//		/// </summary>
//		/// <param name="value">Value</param>
//		public Integer32(long value)
//		{
//			_bytes = null;
//			if (value>=-127 && value<=127)
//			{
//				_raw = new byte[1];
//				_raw[0] = (byte)value;
//			}
//			else
//			{
//				IList<byte> v = new List<byte>();
//				System.Int64 n = value;
//				while (n!=0 && n!=-1)
//				{
//					if (n < 256 && n >= 128)
//					{
//						v.Add((byte)n);
//						v.Add((byte)0);
//						break;
//					}
//					v.Add((byte)(n & 0xff));
//					n >>= 8;
//				}
//				_raw = new byte[v.Count];
//				int len = 0;
//				for (int j=v.Count-1;j>=0;j--)
//				{
//					_raw[len++] = v[j];
//				}
//			}
//		}
		/// <summary>
		/// Returns an <see cref="Int32"/> that represents this <see cref="Integer32"/>.
		/// </summary>
		/// <returns></returns>
		public int ToInt32()
		{
			if (_raw.Length > 4)
			{
				throw (new SharpSnmpException("truncation error for 32-bit integer coding"));
			}
			int result = ((_raw[0] & 0x80) == 0x80) ? -1 : 0; // sign extended! Guy McIlroy
			for (int j = 0; j < _raw.Length; j++)
			{
				result = (result << 8) | (int)_raw[j];
			}
			return result;
		}
//		/// <summary>
//		/// Returns an <see cref="Int64"/> that represents this <see cref="Integer32"/>.
//		/// </summary>
//		/// <returns></returns>
//		public long ToInt64()
//		{
//			if (_raw.Length > 8)
//			{
//				throw (new SharpSnmpException("truncation error for 64-bit integer coding"));
//			}
//			long result = ((_raw[0] & 0x80) == 0x80) ? -1 : 0; // sign extended! Guy McIlroy
//			for (int j = 0; j < _raw.Length; j++)
//			{
//				result = (result << 8) | (long)_raw[j];
//			}
//			return result;
//		}

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
		/// <summary>
		/// Returns a <see cref="String"/> that represents this <see cref="Integer32"/>.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return ToInt32().ToString(CultureInfo.CurrentCulture);
		}
		/// <summary>
		/// Type code.
		/// </summary>
		public SnmpType TypeCode {
			get {
				return SnmpType.Integer32;
			}
		}
		
		byte[] _bytes;
		/// <summary>
		/// Converts to byte format.
		/// </summary>
		/// <returns></returns>
		public byte[] ToBytes()
		{
			return _bytes;
		}
		/// <summary>
		/// Gets that raw bytes.
		/// </summary>
		/// <returns></returns>
		internal byte[] GetRaw()
		{
			return _raw;
		}
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="Integer32"/>.</returns>
		public override int GetHashCode()
		{
			return ToInt32().GetHashCode();
		}
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="Integer32"/>. 
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="Integer32"/>. </param>
        /// <returns><value>true</value> if the specified <see cref="Object"/> is equal to the current <see cref="Integer32"/>; otherwise, <value>false</value>.
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
			return Equals((Integer32)obj);
		}
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><value>true</value> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <value>false</value>.
        ///</returns>
		public bool Equals(Integer32 other)
		{
			return ByteTool.CompareRaw(_raw, other._raw);
		}
        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <param name="left">Left <see cref="Integer32"/> object</param>
        /// <param name="right">Right <see cref="Integer32"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are equal, <c>false</c> otherwise.</returns>
		public static bool operator == (Integer32 left, Integer32 right)
		{
			return left.Equals(right);
		}
        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <param name="left">Left <see cref="Integer32"/> object</param>
        /// <param name="right">Right <see cref="Integer32"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
		public static bool operator != (Integer32 left, Integer32 right)
		{
			return !(left == right);
		}
	}
	// all references here are to ITU-X.690-12/97
}
