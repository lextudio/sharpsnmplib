/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/7/9
 * Time: 20:20
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// NoSuchObject exception.
    /// </summary>
    public sealed class NoSuchObject : ISnmpData, IEquatable<NoSuchObject>
    {
        #region Equals and GetHashCode implementation
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="NoSuchObject"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="NoSuchObject"/>. </param>
        /// <returns><value>true</value> if the specified <see cref="Object"/> is equal to the current <see cref="NoSuchObject"/>; otherwise, <value>false</value>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(this, obj as NoSuchObject);
        }
        
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="NoSuchObject"/>.</returns>
        public override int GetHashCode()
        {
            return 0;
        }
        
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><value>true</value> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <value>false</value>.
        /// </returns>
        public bool Equals(NoSuchObject other)
        {
            return Equals(this, other);
        }
        
        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <param name="lhs">Left <see cref="NoSuchObject"/> object</param>
        /// <param name="rhs">Right <see cref="NoSuchObject"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are equal, <c>false</c> otherwise.</returns>
        public static bool operator ==(NoSuchObject lhs, NoSuchObject rhs)
        {
            return Equals(lhs, rhs);
        }
        
        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <param name="lhs">Left <see cref="NoSuchObject"/> object</param>
        /// <param name="rhs">Right <see cref="NoSuchObject"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        public static bool operator !=(NoSuchObject lhs, NoSuchObject rhs)
        {
            return !(lhs == rhs); // use operator == and negate result
        }
        #endregion
        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get
            {
                return SnmpType.NoSuchObject;
            }
        }
        
        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return constantNoSuchObject;
        }
        
        private readonly static byte[] constantNoSuchObject = new byte[] { 0x80, 0x00 };
        
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="NoSuchObject"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "NoSuchObject";
        }
        
        /// <summary>
        /// The comparison.
        /// </summary>
        /// <param name="left">Left <see cref="NoSuchObject"/> object</param>
        /// <param name="right">Right <see cref="NoSuchObject"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        public static bool Equals(NoSuchObject left, NoSuchObject right)
        {
            object lo = left as object;
            object ro = right as object;
            if (lo == ro)
            {
                return true;
            }

            if (lo == null || ro == null)
            {
                return false;
            }
            
            return true;
        }
    }
}
