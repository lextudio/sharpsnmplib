// <summary>#SNMP Library. An open source SNMP implementation for .NET.</summary>
// <copyright company="Lex Y. Li" file="BitString.cs">Copyright (C) 2008  Lex Y. Li</copyright>
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
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// BitString type.
    /// </summary>
    [Obsolete("The BIT STRING type has been temporarily defined in RFC 1442 and obsoleted by RFC 2578. Use OctetString (i.e. BITS syntax) instead.")]
    public sealed class BitString : ISnmpData, IEquatable<BitString> // BitArray seems to be bad news, so here goes
    {
        private readonly int _nbits;
        private readonly int _size;
        private readonly int[] _bits;
        private byte[] _raw;

        /// <summary>
        /// Creates a <see cref="BitString"/> from raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes.</param>
        public BitString(byte[] raw)
        {
            if (raw == null)
            {
                throw new ArgumentNullException("raw");
            }
            
            if (raw.Length == 0)
            {
                throw new ArgumentException("raw cannot be empty.", "raw");
            }
            
            _nbits = (raw.Length * 8) - raw[0]; // 8.6.2
            _size = (_nbits + 31) / 32;
            _bits = new int[_size];
            for (int j = 0; j < _size; j++)
            {
                _bits[j] = (raw[4 * j] << 24) | (raw[(4 * j) + 1] << 16) | (raw[(4 * j) + 2] << 8) | raw[(4 * j) + 3];
            }
            
            ////_raw = raw;
        }
        
        /// <summary>
        /// Creates a <see cref="BitString"/> with a bit length and a bit array.
        /// </summary>
        /// <param name="number">Bit length.</param>
        /// <param name="bits">Bit array.</param>
        public BitString(int number, int[] bits)
        {
            if (bits == null)
            {
                throw new ArgumentNullException("bits");
            }

            if (bits.Length == 0)
            {
                throw new ArgumentException("bits cannot be empty.", "bits");
            }

            if (bits.Length != (number + 31) / 32)
            {
                throw new ArgumentException("wrong bits length");
            }
            
            _nbits = number;
            _size = bits.Length;
            _bits = bits;
            ////_raw = ParseItem(_nbits, _bits);
        }
        
        /// <summary>
        /// Creates a <see cref="BitString"/> from a <see cref="BitString"/>.
        /// </summary>
        /// <param name="other">Another <see cref="BitString"/> instance.</param>
        public BitString(BitString other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            ////_raw = (byte[])other._raw.Clone();
            _nbits = other._nbits;
            _size = other._size;
            _bits = (int[])other._bits.Clone();
        }
        
        /// <summary>
        /// Returns a bit at specific index.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <returns>Bit inside string at the specific index.</returns>
        public bool this[int index]
        {
            get { return (_bits[index >> 5] & (1 << (31 - (index & 31)))) != 0; }
        }
        
        /// <summary>
        /// And operator.
        /// </summary>
        /// <param name="other">Another <see cref="BitString"/> instance.</param>
        /// <returns>A <see cref="BitString"/> that is the result of the AND operation.</returns>
        public BitString And(BitString other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            Debug.Assert(_nbits == other._nbits, "AND operation must be done on BitString of the same size");
            int[] bits = new int[_size];
            for (uint j = 0; j < _size; j++)
            {
                bits[j] = _bits[j] & other._bits[j];
            }
            
            return new BitString(_nbits, bits);
        }
        
        /// <summary>
        /// Or operator.
        /// </summary>
        /// <param name="other">Another <see cref="BitString"/> instance.</param>
        /// <returns>A <see cref="BitString"/> that is the result of the OR operation.</returns>
        public BitString Or(BitString other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            Debug.Assert(_nbits == other._nbits, "OR operation must be done on BitString of the same size");
            int[] bits = new int[_size];
            for (uint j = 0; j < _size; j++)
            {
                bits[j] = _bits[j] | other._bits[j];
            }
            
            return new BitString(_nbits, bits);
        }
        
        /// <summary>
        /// Gets a value that indicates how many bits are set.
        /// </summary>
        public int Card
        {
            get
            {
                int r = 0;
                for (int i = 0; i < _nbits; i++)
                {
                    if (this[i])
                    {
                        r++;
                    }
                }
                
                return r;
            }
        }
        
        /// <summary>
        /// Cat operator.
        /// </summary>
        /// <param name="other">Another <see cref="BitString"/> instance.</param>
        /// <returns>A <see cref="BitString"/> that is the result of the CAT operation.</returns>
        public BitString Cat(BitString other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            
            int nbits = _nbits + other._nbits;
            int size = _size + other._size;
            int[] bits = new int[size];
            uint i = 0, j;
            for (j = 0; j < _nbits; j++)
            {
                bits[i++] = _bits[j];
            }
            
            for (j = 0; j < other._nbits; j++)
            {
                bits[i++] = other._bits[j];
            }
            
            return new BitString(nbits, bits);
        }
        
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="BitString"/>.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int n = 0;
                for (uint j = 0; j < _size; j++)
                {                    
                    n += _bits[j];
                }
                
                return n;
            }  
        }
        
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="BitString"/>.
        /// </summary>
        public override string ToString()
        {
            StringBuilder r = new StringBuilder();
            for (int i = 0; i < _nbits; i++)
            {
                r.Append(((_bits[i >> 5] & (1 << (i & 31))) != 0) ? '1' : '0');
            }
            
            return r.ToString();
        }
        
        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get
            {
                return SnmpType.BitString;
            }
        }
        
        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
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
            if (_raw == null)
            {
                _raw = ParseItem(_nbits, _bits);
            }

            ByteTool.AppendBytes(stream, TypeCode, _raw);
        }

        private static byte[] ParseItem(int nbits, int[] bits)
        {
            // encoding 8.6.2
            int n = (nbits + 7) / 8;
            byte r = (byte)(8 - (nbits % 8));
            byte[] result = new byte[n + 1];
            int ln = 0;
            result[ln++] = r;
            int k = 24;
            int i = 0;
            for (int j = 0; j < n; j++)
            {
                result[ln++] = (byte)(bits[i] >> k);
                k -= 8;
                if (k < 0)
                {
                    i++;
                    k = 24;
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="BitString"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="BitString"/>. </param>
        /// <returns><value>true</value> if the specified <see cref="Object"/> is equal to the current <see cref="BitString"/>; otherwise, <value>false</value>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(this, obj as BitString);
        }
        
        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <param name="left">Left <see cref="BitString"/> object</param>
        /// <param name="right">Right <see cref="BitString"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are equal, <c>false</c> otherwise.</returns>
        public static bool operator ==(BitString left, BitString right)
        {
            return Equals(left, right);
        }
        
        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <param name="left">Left <see cref="BitString"/> object</param>
        /// <param name="right">Right <see cref="BitString"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        public static bool operator !=(BitString left, BitString right)
        {
            return !(left == right);
        }

        #region IEquatable<BitString> Members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><value>true</value> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <value>false</value>.
        /// </returns>
        public bool Equals(BitString other)
        {
            return Equals(this, other);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Equals(BitString left, BitString right)
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

            return left._nbits == right._nbits && ByteTool.CompareArray(left._bits, right._bits);
        }
    }
}
