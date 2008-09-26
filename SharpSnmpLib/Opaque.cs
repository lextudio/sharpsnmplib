using System;
using System.Collections;
using System.Globalization;
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
    /// Opaque type.
    /// </summary>
    public sealed class Opaque : ISnmpData, IEquatable<Opaque>
    {
        private byte[] _raw;

        /// <summary>
        /// Creates an <see cref="Opaque"/> from raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        public Opaque(byte[] raw)
        {
            if (raw == null)
            {
                throw new ArgumentNullException("raw");
            }
            
            _raw = raw;
        }
        
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="Opaque"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder("Opaque type: ");
            foreach (byte b in _raw)
            {
                result.Append(b.ToString("X2", CultureInfo.InvariantCulture));
            }
            
            return result.ToString();
        }

        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get
            {
                return SnmpType.Opaque;
            }
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
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><value>true</value> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <value>false</value>.
        /// </returns>
        public bool Equals(Opaque other)
        {
            return Equals(this, other);
        }
        
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="Opaque"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="Opaque"/>. </param>
        /// <returns><value>true</value> if the specified <see cref="Object"/> is equal to the current <see cref="Opaque"/>; otherwise, <value>false</value>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(this, obj as Opaque);
        }
        
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="Opaque"/>.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
        
        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <param name="left">Left <see cref="Opaque"/> object</param>
        /// <param name="right">Right <see cref="Opaque"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are equal, <c>false</c> otherwise.</returns>
        public static bool operator ==(Opaque left, Opaque right)
        {
            return Equals(left, right);
        }
        
        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <param name="left">Left <see cref="Opaque"/> object</param>
        /// <param name="right">Right <see cref="Opaque"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        public static bool operator !=(Opaque left, Opaque right)
        {
            return !(left == right);
        }
        
        public static bool Equals(Opaque left, Opaque right)
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
            
            return ByteTool.CompareRaw(left._raw, right._raw);
        }
    }
    
    // all references here are to ITU-X.690-12/97
}
