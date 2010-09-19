// OCTET STRING SNMP data type.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Globalization;
using System.IO;
#if (!CF) && (!SILVERLIGHT)
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
        private readonly byte[] _raw;
        private readonly Encoding _encoding;
        
        // IMPORTANT: use GetEncoding because of CF.
        private static Encoding _defaultEncoding = Encoding.GetEncoding("ASCII");

        /// <summary>
        /// Initializes a new instance of the <see cref="OctetString"/> class.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="stream">The stream.</param>
        public OctetString(int length, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            _raw = new byte[length];
            stream.Read(_raw, 0, length);
            _encoding = DefaultEncoding;
        }
        
        /// <summary>
        /// Creates an <see cref="OctetString"/> from raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        public OctetString(byte[] raw)
        {
            if (raw == null)
            {
                throw new ArgumentNullException("raw");
            }
            
            _raw = new byte[raw.Length];
            Array.Copy(raw, _raw, raw.Length);
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
        /// Gets raw bytes.
        /// </summary>
        /// <returns></returns>
        public byte[] GetRaw()
        {
            return _raw;
        }
        
        private static readonly OctetString EmptyString = new OctetString(string.Empty, Encoding.GetEncoding("ASCII"));

        /// <summary>
        /// Gets the empty string.
        /// </summary>
        /// <value>The empty.</value>
        public static OctetString Empty
        {
            get { return EmptyString; }
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

            return encoding.GetString(_raw, 0, _raw.Length); // use this call for SL3.
        }

        /// <summary>
        /// Returns a <see cref="String"/> in UTF-16 that represents this <see cref="OctetString"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString(_encoding);
        }
        
        internal string ToDateAndTimeString()
        {
            byte[] bs = _raw;
            byte[] bs2 = new byte[8];
            bs2[0] = bs[1];   // Little/Big Endian
            bs2[1] = bs[0];   // Little/Big Endian
            bs2[2] = bs[2];
            bs2[3] = bs[3];
            bs2[4] = bs[4];
            bs2[5] = bs[5];
            bs2[6] = bs[6];
            bs2[7] = bs[7];
            return " Year:" + BitConverter.ToInt16(bs2, 0) + ", Month:" +bs2[2] + ", Day:" + bs2[3] + ", Hour:" + bs2[4] + ", Minute:" + bs2[5] + ", Seconds:" + bs2[6] + ", Thenths:" + bs2[7];
        }
        
        /// <summary>
        /// Converts octets to data string.
        /// </summary>
        /// <returns></returns>
        /// <remarks>For date section of DateAndTime type in HOST-RESOURCES-MIB.</remarks>
        [ObsoleteAttribute("This method will be removed")]
        public string ToDateString()
        {
            // TODO: make it internal and prepare for future usage.
            // may be DateAndTime
            if (_raw.Length == 8 || _raw.Length == 11)
            {
                uint yr = _raw[0];
                yr = (yr * 256) + _raw[1];
                uint mo = _raw[2];
                uint dy = _raw[3];

                if (yr > 1990 && mo < 13 && dy < 32)
                {
                    return new DateTime((int)yr, (int)mo, (int)dy).ToString();
                }
            }
            
            return string.Empty;
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
        [Obsolete("Use AppendBytesTo instead.")]
        public byte[] ToBytes()
        {
            using (MemoryStream result = new MemoryStream())
            {
                AppendBytesTo(result);
                return result.ToArray();
            }
        }

        /// <summary>
        /// Appends the bytes to <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void AppendBytesTo(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            ByteTool.AppendBytes(stream, TypeCode, _raw);
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

        #if (!CF) && (!SILVERLIGHT)
        /// <summary>
        /// Converts octets to physical address.
        /// </summary>
        /// <returns></returns>
        public PhysicalAddress ToPhysicalAddress()
        {
            return _raw.Length != 6 ? null : new PhysicalAddress(_raw);
        }
        #endif
        
        /// <summary>
        /// Default encoding of <see cref="OctetString"/> type.
        /// </summary>
        public static Encoding DefaultEncoding
        {
            get { return _defaultEncoding; }
            set { _defaultEncoding = value; }
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
    
    // all references here are to ITU-X.690-12/97
}
