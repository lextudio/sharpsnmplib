using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Gauge32 type.
    /// </summary>
    public struct Gauge32 : ISnmpData, IEquatable<Gauge32>
    {
        private Counter32 _count;
        
        /// <summary>
        /// Creates a <see cref="Gauge32"/> instance from raw bytes.
        /// </summary>
        /// <param name="raw"></param>
        public Gauge32(byte[] raw)
        {
            _count = new Counter32(raw);
        }
        
        /// <summary>
        /// Creates a <see cref="Gauge32"/> with a specific <see cref="UInt32"/>.
        /// </summary>
        /// <param name="value">Value</param>
        [CLSCompliant(false)]
        public Gauge32(uint value)
        {
            _count = new Counter32(value);
        }

        #region ISnmpData Members
        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get { return SnmpType.Gauge32; }
        }
        
        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return ByteTool.ToBytes(TypeCode, _count.GetRaw());
        }

        #endregion
        /// <summary>
        /// Returns a <see cref="UInt32"/> that represents a <see cref="Gauge32"/>.
        /// </summary>
        /// <returns></returns>
        [CLSCompliant(false)]
        public uint ToUInt32()
        {
            return _count.ToUInt32();
        }
        
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="Gauge32"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToUInt32().ToString(CultureInfo.InvariantCulture);
        }
                
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><value>true</value> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <value>false</value>.
        /// </returns>
        public bool Equals(Gauge32 other)
        {
            return ToUInt32() == other.ToUInt32();
        }
        
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="Gauge32"/>. 
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="Gauge32"/>. </param>
        /// <returns><value>true</value> if the specified <see cref="Object"/> is equal to the current <see cref="Gauge32"/>; otherwise, <value>false</value>.
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
        
            return Equals((Gauge32)obj);
        }
        
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="Gauge32"/>.</returns>
        public override int GetHashCode()
        {
            return ToUInt32().GetHashCode();
        }        
        
        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <param name="left">Left <see cref="Gauge32"/> object</param>
        /// <param name="right">Right <see cref="Gauge32"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are equal, <c>false</c> otherwise.</returns>
        public static bool operator ==(Gauge32 left, Gauge32 right)
        {
            return left.Equals(right);
        }
        
        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <param name="left">Left <see cref="Gauge32"/> object</param>
        /// <param name="right">Right <see cref="Gauge32"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        public static bool operator !=(Gauge32 left, Gauge32 right)
        {
            return !(left == right);
        }
    }
}