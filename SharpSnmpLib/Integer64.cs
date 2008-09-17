/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/6
 * Time: 20:23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Integer64 type.
    /// </summary>
    internal sealed class Integer64 : IEquatable<Integer64>
    {
        private byte[] _raw;
        
        public Integer64(byte[] raw)
        {
            _raw = raw;
        }
        
        public Integer64(long value)
        {
            if (value >= -127 && value <= 127)
            {
                _raw = new byte[1];
                _raw[0] = (byte)value;
            }
            else
            {
                IList<byte> v = new List<byte>();
                long n = value;
                while (n != 0 && n != -1)
                {
                    if (n < 256 && n >= 128)
                    {
                        v.Add((byte)n);
                        v.Add((byte)0);
                        break;
                    }
                    
                    v.Add((byte)(n & 0xff));
                    n >>= 8;
                }
                
                _raw = new byte[v.Count];
                int len = 0;
                for (int j = v.Count - 1; j >= 0; j--)
                {
                    _raw[len++] = v[j];
                }
            }
        }
        
        /// <summary>
        /// Returns an <see cref="Int64"/> that represents this <see cref="Integer64"/>.
        /// </summary>
        /// <returns></returns>
        public long ToInt64()
        {
            if (_raw.Length > 8)
            {
                throw new SharpSnmpException("truncation error for 64-bit integer coding");
            }
            
            long result = ((_raw[0] & 0x80) == 0x80) ? -1 : 0; // sign extended! Guy McIlroy
            for (int j = 0; j < _raw.Length; j++)
            {
                result = (result << 8) | (long)_raw[j];
            }
            
            return result;
        }
        
        #region Equals and GetHashCode implementation
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="Integer64"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="Integer64"/>. </param>
        /// <returns><value>true</value> if the specified <see cref="Object"/> is equal to the current <see cref="Integer64"/>; otherwise, <value>false</value>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(this, obj as Integer64);
        }
        
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><value>true</value> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <value>false</value>.
        /// </returns>
        public bool Equals(Integer64 other)
        {
            return Equals(this, other);
        }
        
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="Integer64"/>.</returns>
        public override int GetHashCode()
        {
            // combine the hash codes of all members here (e.g. with XOR operator ^)
            return ToInt64().GetHashCode();
        }
        
        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <param name="lhs">Left <see cref="Integer64"/> object</param>
        /// <param name="rhs">Right <see cref="Integer64"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are equal, <c>false</c> otherwise.</returns>
        public static bool operator ==(Integer64 lhs, Integer64 rhs)
        {
            return Equals(lhs, rhs);
        }
        
        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <param name="lhs">Left <see cref="Integer64"/> object</param>
        /// <param name="rhs">Right <see cref="Integer64"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        public static bool operator !=(Integer64 lhs, Integer64 rhs)
        {
            return !(lhs == rhs); // use operator == and negate result
        }
        #endregion
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="Integer64"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToInt64().ToString(CultureInfo.InvariantCulture);
        }
        
        public static bool Equals (Integer64 left, Integer64 right)
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
            
            return left.ToInt64() == right.ToInt64();
        }
    }
}
