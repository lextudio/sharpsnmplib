using System;
using System.Globalization;
using System.IO;

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
    /// Integer32 type in SMIv2 (or INTEGER in SMIv1).
    /// </summary>
    public sealed class Integer32 // This namespace has its own concept of Integer
        : ISnmpData, IEquatable<Integer32>
    {
        private readonly int _int;
        
        /// <summary>
        /// Creates an <see cref="Integer32"/> instance.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        internal Integer32(byte[] raw): this(raw.Length, new MemoryStream(raw))
        {
            
        }
        
        /// <summary>
        /// Creates an <see cref="Integer32"/> instance with a specific <see cref="Int32"/>.
        /// </summary>
        /// <param name="value">Value</param>
        public Integer32(int value)
        {
            _int = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Integer32"/> class.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="stream">The stream.</param>
        public Integer32(int length, Stream stream)
        {
            if (length == 0)
            {
                throw new ArgumentException("length cannot be 0.", "length");
            }

            if (length > 4)
            {
                throw new ArgumentException("truncation error for 32-bit integer coding", "length");
            }

            byte[] raw = new byte[length];
            stream.Read(raw, 0, length);
            _int = ((raw[0] & 0x80) == 0x80) ? -1 : 0; // sign extended! Guy McIlroy
            for (int j = 0; j < length; j++)
            {
                _int = (_int << 8) | raw[j];
            }
        }

        /// <summary>
        /// Returns an <see cref="Int32"/> that represents this <see cref="Integer32"/>.
        /// </summary>
        /// <returns></returns>
        public int ToInt32()
        {
            return _int;
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="Integer32"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToInt32().ToString(CultureInfo.CurrentCulture);
        }
        
        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get
            {
                return SnmpType.Integer32;
            }
        }
        
        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            return ByteTool.ToBytes(SnmpType.Integer32, ByteTool.GetRawBytes(BitConverter.GetBytes(_int), _int < 0));
        }
        
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="Integer32"/>.</returns>
        public override int GetHashCode()
        {
            return ToInt32().GetHashCode();
        }
        
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="Integer32"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="Integer32"/>. </param>
        /// <returns><value>true</value> if the specified <see cref="Object"/> is equal to the current <see cref="Integer32"/>; otherwise, <value>false</value>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(this, obj as Integer32);
        }
        
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><value>true</value> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <value>false</value>.
        /// </returns>
        public bool Equals(Integer32 other)
        {
            return Equals(this, other);
        }
        
        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <param name="left">Left <see cref="Integer32"/> object</param>
        /// <param name="right">Right <see cref="Integer32"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are equal, <c>false</c> otherwise.</returns>
        public static bool operator ==(Integer32 left, Integer32 right)
        {
            return Equals(left, right);
        }
        
        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <param name="left">Left <see cref="Integer32"/> object</param>
        /// <param name="right">Right <see cref="Integer32"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        public static bool operator !=(Integer32 left, Integer32 right)
        {
            return !(left == right);
        }
        
        /// <summary>
        /// The comparison.
        /// </summary>
        /// <param name="left">Left <see cref="Integer32"/> object</param>
        /// <param name="right">Right <see cref="Integer32"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        public static bool Equals(Integer32 left, Integer32 right)
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
            
            return left.ToInt32() == right.ToInt32();
        }
    }
    
    // all references here are to ITU-X.690-12/97
}
