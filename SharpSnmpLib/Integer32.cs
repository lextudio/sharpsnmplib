// SNMP INTEGER type.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

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
        private readonly byte[] _length;
        
        /// <summary>
        /// Zero.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly Integer32 Zero = new Integer32(0);

        private byte[] _raw;

        /// <summary>
        /// Creates an <see cref="Integer32"/> instance.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        internal Integer32(byte[] raw) : this(new Tuple<int, byte[]>(raw.Length, raw.Length.WritePayloadLength()), new MemoryStream(raw))
        {
            // IMPORTANT: for test project only.
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
        public Integer32(Tuple<int, byte[]> length, Stream stream)
        {
            if (length == null)
            {
                throw new ArgumentNullException(nameof(length));
            }

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (length.Item1 <= 0)
            {
                throw new ArgumentException("Byte length cannot be 0.", nameof(length));
            }

            if (length.Item1 > 4)
            {
                throw new ArgumentException("Truncation error for 32-bit integer coding.", nameof(length));
            }

            _raw = new byte[length.Item1];
            stream.Read(_raw, 0, length.Item1);
            _int = ((_raw[0] & 0x80) == 0x80) ? -1 : 0; // sign extended! Guy McIlroy
            for (var j = 0; j < length.Item1; j++)
            {
                _int = (_int << 8) | _raw[j];
            }

            _length = length.Item2;
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
        /// Converts to <see cref="ErrorCode"/>.
        /// </summary>
        /// <returns></returns>
        public ErrorCode ToErrorCode()
        {
            if (_int > 19 || _int < 0)
            {
                throw new InvalidCastException();
            }

            return (ErrorCode)_int;
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="Integer32"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _int.ToString(CultureInfo.InvariantCulture);
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
        /// Appends the bytes to <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void AppendBytesTo(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            
            stream.AppendBytes(TypeCode, _length, _raw ?? (_raw = ByteTool.GetRawBytes(BitConverter.GetBytes(_int), _int < 0)));
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
        private static bool Equals(Integer32 left, Integer32 right)
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
