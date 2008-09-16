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

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// GeneralString type.
    /// </summary>
    public class GeneralString : ISnmpData, IEquatable<GeneralString>
    {
        private byte[] _raw;
        
        /// <summary>
        /// Creates a <see cref="GeneralString"/> from raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        public GeneralString(byte[] raw)
        {
            _raw = raw;
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
            return ASCIIEncoding.ASCII.GetString(_raw);
        }
        
        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return ByteTool.ToBytes(TypeCode, _raw);
        }
        
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="GeneralString"/>. 
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="GeneralString"/>. </param>
        /// <returns><value>true</value> if the specified <see cref="Object"/> is equal to the current <see cref="GeneralString"/>; otherwise, <value>false</value>.
        /// </returns>
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
            
            return Equals((GeneralString)obj);
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
            if ((object)left == null)
            {
                return (object)right == null;    
            }
            
            return left.Equals(right);
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
            if (other == null) 
            {
                return false;    
            }
            
            return ByteTool.CompareRaw(_raw, other._raw);
        }

        #endregion
    }
}
