using System;
using System.Globalization;
#if (!CF)
using System.Net.NetworkInformation;
#endif
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
    public sealed class OctetString // This namespace has its own concept of string
        : ISnmpData, IEquatable<OctetString>
    {
        private byte[] _raw;
        private Encoding _encoding;
        
        /// <summary>
        /// Creates an <see cref="OctetString"/> from raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        public OctetString(byte[] raw)
        {
            _raw = raw;
            _encoding = DefaultEncoding;
        }
        
        /// <summary>
        /// Creates an <see cref="OctetString"/> with a specific <see cref="String"/>. This string is treated in specific <see cref="Encoding"/>.
        /// </summary>
        /// <param name="content">String.</param>
        /// <param name="encoding">Encoding.</param>
        public OctetString(string content, Encoding encoding)
        {
            _encoding = encoding;
            _raw = _encoding.GetBytes(content);
        }
        
        /// <summary>
        /// Creates an <see cref="OctetString"/> with a specific <see cref="String"/>. This string is treated as UTF-16.
        /// </summary>
        /// <param name="content">String.</param>
        public OctetString(string content)
            : this(content, DefaultEncoding)
        {
        }
        
        /// <summary>
        /// Encoding of this <see cref="OctetString"/>
        /// </summary>
        public Encoding Encoding 
        {
            get { return _encoding; }
        }
        
        /// <summary>
        /// Returns a <see cref="String"/> in a hex form that represents this <see cref="OctetString"/>.
        /// </summary>
        /// <returns></returns>
        public string ToHexString()
        {
            StringBuilder result = new StringBuilder();
            foreach (byte b in _raw)
            {
                result.Append(b.ToString("X2", CultureInfo.InvariantCulture));
            }
            
            return result.ToString();
        }
        
        /// <summary>
        /// Returns a <see cref="String"/> in a specific <see cref="Encoding"/> that represents this <see cref="OctetString"/>.
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string ToString(Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            return encoding.GetString(_raw);
        }

        /// <summary>
        /// Returns a <see cref="String"/> in UTF-16 that represents this <see cref="OctetString"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString(_encoding);
        }
        
        /// <summary>
        /// Converts octets to data string.
        /// </summary>
        /// <returns></returns>
        public string ToDateString()
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
                    return dy.ToString(CultureInfo.InvariantCulture) + "/" + mo.ToString(CultureInfo.InvariantCulture) + "/" + yr.ToString(CultureInfo.InvariantCulture);
                }
            }
            
            return null;
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
            return Equals(this, other);
        }
        
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="OctetString"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="OctetString"/>. </param>
        /// <returns><value>true</value> if the specified <see cref="Object"/> is equal to the current <see cref="OctetString"/>; otherwise, <value>false</value>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(this, obj as OctetString);
        }
        
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="OctetString"/>.</returns>
        public override int GetHashCode()
        {
            return ToString(Encoding.Unicode).GetHashCode();
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
            return Equals(left, right);
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

        #if (!CF)
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
        #endif 
        private static Encoding defaultEncoding = Encoding.ASCII;
        
        /// <summary>
        /// Default encoding of <see cref="OctetString"/> type.
        /// </summary>
        public static Encoding DefaultEncoding
        {
            get { return defaultEncoding; }
            set { defaultEncoding = value; }
        }
        
        /// <summary>
        /// The comparison.
        /// </summary>
        /// <param name="left">Left <see cref="OctetString"/> object</param>
        /// <param name="right">Right <see cref="OctetString"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        public static bool Equals(OctetString left, OctetString right)
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
            
            return ByteTool.CompareArray(left._raw, right._raw);
        }
    }
    
    // all references here are to ITU-X.690-12/97
}
