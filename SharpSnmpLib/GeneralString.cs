/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/2
 * Time: 13:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using System.Text;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// GeneralString type.
    /// </summary>
    public sealed class GeneralString : ISnmpData, IEquatable<GeneralString>
    {
        private readonly byte[] _raw;
        
        /// <summary>
        /// Creates a <see cref="GeneralString"/> from raw bytes (by cloning them).
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        public GeneralString(byte[] raw) : this(raw, true)
        {
        }

		/// <summary>
		/// Creates a <see cref="GeneralString"/> from raw bytes (by cloning them).
		/// </summary>
		/// <param name="raw">Raw bytes</param>
		/// <param name="doCloning">true to clone the raw bytes, false to use them directly.</param>
		//[Obsolete("raw constructor obsolete", true)]
		public GeneralString(byte[] raw, bool doCloning)
		{
			_raw = doCloning ? (byte[])raw.Clone() : raw;
		}

        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get
            {
                return SnmpType.GeneralString;
            }
        }
        
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="GeneralString"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Encoding.ASCII.GetString(_raw);
        }
        
        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use AppendBytesTo instead.")]
        public byte[] ToBytes()
        {
            MemoryStream result = new MemoryStream();
            AppendBytesTo(result);
            return result.ToArray();
        }

        /// <summary>
        /// Appends the bytes to <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void AppendBytesTo(Stream stream)
        {
            ByteTool.AppendBytes(stream, TypeCode, _raw);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="GeneralString"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="GeneralString"/>. </param>
        /// <returns><value>true</value> if the specified <see cref="Object"/> is equal to the current <see cref="GeneralString"/>; otherwise, <value>false</value>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(this, obj as GeneralString);
        }
        
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="GeneralString"/>.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
        
        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <param name="left">Left <see cref="GeneralString"/> object</param>
        /// <param name="right">Right <see cref="GeneralString"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are equal, <c>false</c> otherwise.</returns>
        public static bool operator ==(GeneralString left, GeneralString right)
        {
            return Equals(left, right);
        }
        
        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <param name="left">Left <see cref="GeneralString"/> object</param>
        /// <param name="right">Right <see cref="GeneralString"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        public static bool operator !=(GeneralString left, GeneralString right)
        {
            return !(left == right);
        }

        #region IEquatable<GeneralString> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><value>true</value> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <value>false</value>.
        /// </returns>
        public bool Equals(GeneralString other)
        {
            return Equals(this, other);
        }

        #endregion
        
        /// <summary>
        /// The comparison.
        /// </summary>
        /// <param name="left">Left <see cref="GeneralString"/> object</param>
        /// <param name="right">Right <see cref="GeneralString"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        public static bool Equals(GeneralString left, GeneralString right)
        {
            object lo = left;
            object ro = right;
            if (lo == ro)
            {
                return true;
            }

            if (lo == null || ro == null)
            {
                return false;
            }
            
            return ByteTool.CompareArray(left._raw, right._raw);
        }
    }
}
