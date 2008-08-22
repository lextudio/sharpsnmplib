using System;
using System.Net.NetworkInformation;
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
    public class OctetString // This namespace has its own concept of string
        : ISnmpData, IEquatable<OctetString>
    {
        private byte[] _raw;

        /// <summary>
        /// Creates an <see cref="OctetString"/> from raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        public OctetString(byte[] raw)
        {
            _raw = raw;
        }
        
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
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="OctetString"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // may be index date
            if (_raw.Length == 8 || _raw.Length == 11) 
            {
                uint yr = _raw[0];
                yr = (yr * 256) + _raw[1];
                uint mo = _raw[2];
                uint dy = _raw[3];
                if (yr < 2005 && yr > 1990 && mo < 13 && dy < 32)
                {
                    return string.Empty + dy + "/" + mo + "/" + yr;
                }
            }
            
            return Encoding.ASCII.GetString(_raw); 
        }

        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode 
        {
            get
            {
                return SnmpType.OctetString;
            }
        }
        
        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>        
        public byte[] ToBytes()
        {
            return ByteTool.ToBytes(SnmpType.OctetString, _raw);
        }
        
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><value>true</value> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <value>false</value>.
        /// </returns>
        public bool Equals(OctetString other)
        {
            if (other == null)
            {
                return false;    
            }
            
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

        /// <summary>
        /// Converts octets to physical address.
        /// </summary>
        /// <returns></returns>
        public PhysicalAddress ToPhysicalAddress()
        {            
            if (_raw.Length != 6)
            {
                return null;
            }

            return new PhysicalAddress(_raw);
        }
    }
    
    // all references here are to ITU-X.690-12/97
}
