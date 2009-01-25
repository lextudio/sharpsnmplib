/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/2
 * Time: 13:28
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Boolean type.
    /// </summary>
    public sealed class Bool : ISnmpData, IEquatable<Bool>
    {
        private readonly bool _boolean;
        
        /// <summary>
        /// Creates a <see cref="Bool"/> instance from raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        public Bool(byte[] raw)
        {
            if (raw == null)
            {
                throw new ArgumentNullException("raw");
            }
            
            if (raw.Length != 1)
            {
                throw new ArgumentException("raw must be one item");
            }
            
            _boolean = (raw[0] > 0);
        }
        
        /// <summary>
        /// Returns a <see cref="Boolean"/> that represents this <see cref="Bool"/>.
        /// </summary>
        /// <returns></returns>
        public bool ToBoolean()
        {
            return _boolean;
        }
        
        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get
            {
                return SnmpType.Boolean;
            }
        }
        
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="Bool"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToBoolean().ToString();
        }
        
        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return ByteTool.ToBytes(TypeCode, new byte[1] { 1 });
        }
        
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="Bool"/>.</returns>
        public override int GetHashCode()
        {
            return ToBoolean().GetHashCode();
        }
        
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="Bool"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="Bool"/>. </param>
        /// <returns><value>true</value> if the specified <see cref="Object"/> is equal to the current <see cref="Bool"/>; otherwise, <value>false</value>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(this, obj as Bool);
        }
        
        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <param name="left">Left <see cref="Bool"/> object</param>
        /// <param name="right">Right <see cref="Bool"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are equal, <c>false</c> otherwise.</returns>
        public static bool operator ==(Bool left, Bool right)
        {
            return Equals(left, right);
        }
        
        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <param name="left">Left <see cref="Bool"/> object</param>
        /// <param name="right">Right <see cref="Bool"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        public static bool operator !=(Bool left, Bool right)
        {
            return !(left == right);
        }

        #region IEquatable<Bool> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><value>true</value> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <value>false</value>.
        /// </returns>
        public bool Equals(Bool other)
        {
            return Equals(this, other);
        }

        #endregion
        
        /// <summary>
        /// The comparison.
        /// </summary>
        /// <param name="left">Left <see cref="Bool"/> object</param>
        /// <param name="right">Right <see cref="Bool"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        public static bool Equals(Bool left, Bool right)
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
            
            return left.ToBoolean() == right.ToBoolean();
        }
    }
}
