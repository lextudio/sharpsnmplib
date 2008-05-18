using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// ObjectIdentifier type.
    /// </summary>
	public struct ObjectIdentifier: ISnmpData, IEquatable<ObjectIdentifier>
	{
        uint[] _oid;
        byte[] _raw;
        byte[] _bytes;
		/// <summary>
		/// Creates an <see cref="ObjectIdentifier"/> instance from OID array.
		/// </summary>
		/// <param name="oid">OID <see cref="uint"/> array</param>
		[CLSCompliant(false)]
		public ObjectIdentifier(uint[] oid)
		{
			if (oid[0]!=1 || oid[1]!=3)
			{
				throw(new ArgumentException("OID must begin with .1.3"));
			}
			_oid = oid;
			_raw = new byte[GetPduFormatLength(_oid)];
			int ln = 0;
			PutOIDEl(ref _raw, ref ln,43);
			for (int j=2;j<_oid.Length;j++)
			{
				PutOIDEl(ref _raw, ref ln,_oid[j]);
			}
			_bytes = null;
		}
		/// <summary>
		/// Creates an <see cref="ObjectIdentifier"/> instance from raw bytes.
		/// </summary>
		/// <param name="raw">Raw bytes</param>
		public ObjectIdentifier(byte[] raw)
		{
			_raw = raw;
			_oid = ParsePduFormat(raw, (uint)raw.Length);
			_bytes = null;
		}
        /// <summary>
        /// Convers to OID array.
        /// </summary>
        /// <returns></returns>
		[CLSCompliant(false)]
		public uint[] ToOid()
		{
			return _oid;
		}
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="ObjectIdentifier"/>.
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			StringBuilder result = new StringBuilder();
			for (int k = 0; k < _oid.Length; k++)
			{
				result.Append("." + _oid[k]);
			}
			return result.ToString();
		}

        static uint GetOIDEl(byte[] bytes, ref int p)
		{
			uint r = 0;
			byte x;
			do
			{
				x = bytes[p++];
				r = (r << 7) + ((uint)x & 0x7f);	// 0.3.3a (Evan Watson)
			} while ((x & 0x80) != 0);
			return r;
		}

		static uint[] ParsePduFormat(byte[] bytes, uint n)
		{
			uint[] result;
			IList<uint> temp = new List<uint>();
			int p = 0;
			while (p < n)
			{ temp.Add(ObjectIdentifier.GetOIDEl(bytes, ref p)); }

			if (temp[0] == 43)
			{
				result = new uint[temp.Count + 1];
				result[0] = 1;
				result[1] = 3;
				for (int j = 1; j < temp.Count; j++)
				{ result[j + 1] = (uint)temp[j]; }
			}
			else // can happen that result is .0??
			{
				//				throw(new Exception("OID must begin with .1.3"));
				result = new uint[temp.Count];
				for (int j = 0; j < temp.Count; j++)
				{ result[j] = (uint)temp[j]; }
			}
			return result;
		}
		
		static int LengthOIDEl(uint a)
		{
			int j = 0;
			if (a==0)
			{
				return 1;
			}
			while (a>0)
			{
				j++;
				a = a>>7;
			}
			return j;
		}
		
		static void PutOIDEl(ref byte[] bytes, ref int ln, uint a)
		{
			if (a==0)
			{
				bytes[ln++] = 0;
				return;
			}
			byte[] c = new byte[16];
			int j = 0;
			while (a>0)
			{
				c[j++] = (byte)(a&0x7f);
				a = a>>7;
			}
			while (j>0)
			{
				int x = c[--j];
				if (j>0)
					x |= 0x80;
				bytes[ln++] = (byte)x;
			}
		}

		static int GetPduFormatLength(uint[] oid)
		{
			int result = 0;
			for (int j=1;j<oid.Length;j++)
			{
				result += LengthOIDEl(oid[j]);
			}
			return result;
		}

        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
		public byte[] ToBytes()
		{
			if (_bytes == null) {
				_bytes = ByteTool.ToBytes(TypeCode, _raw);
			}
			return _bytes;
		}
		/// <summary>
		/// Type code.
		/// </summary>
		public SnmpType TypeCode {
			get {
				return SnmpType.ObjectIdentifier;
			}
		}
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="ObjectIdentifier"/>. 
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="ObjectIdentifier"/>. </param>
        /// <returns><value>true</value> if the specified <see cref="Object"/> is equal to the current <see cref="ObjectIdentifier"/>; otherwise, <value>false</value>.
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
			return Equals((ObjectIdentifier)obj);
		}
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><value>true</value> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <value>false</value>.
        ///</returns>
		public bool Equals(ObjectIdentifier other)
		{
			return ByteTool.CompareRaw(_raw, other._raw);
		}
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="ObjectIdentifier"/>.</returns>
		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}
        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <param name="left">Left <see cref="ObjectIdentifier"/> object</param>
        /// <param name="right">Right <see cref="ObjectIdentifier"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are equal, <c>false</c> otherwise.</returns>
		public static bool operator == (ObjectIdentifier left, ObjectIdentifier right)
		{
			return left.Equals(right);
		}
        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <param name="left">Left <see cref="ObjectIdentifier"/> object</param>
        /// <param name="right">Right <see cref="ObjectIdentifier"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
		public static bool operator != (ObjectIdentifier left, ObjectIdentifier right)
		{
			return !(left == right);
		}
	}
}
