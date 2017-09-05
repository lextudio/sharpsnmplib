// SNMP IP address type.
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
using System.Linq;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// IPAddress type.
    /// </summary>
    public sealed class IP : ISnmpData, IEquatable<IP>
    {
        private readonly byte[] _ip;
        private readonly byte[] _length;

        private const int IPv4Length = 4;

        /// <summary>
        /// Initializes a new instance of the <see cref="IP"/> class.
        /// </summary>
        /// <param name="ip">The IP bytes.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="ip" /> is <c>null</c>.</exception>
        public IP(byte[] ip)
        {
            if (ip == null)
            {
                throw new ArgumentNullException(nameof(ip));
            }

            _ip = ip;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="IP"/> class from a specific <see cref="String"/>.
        /// </summary>
        /// <param name="ip">IP string</param>
        public IP(string ip)
        {
            // IMPORTANT: copied from Mono's IPAddress.cs
            int pos = ip.IndexOf(' ');
            if (pos != -1)
            {
                string[] nets = ip.Substring(pos + 1).Split(new char[] { '.' });
                if (nets.Length > 0)
                {
                    string lastNet = nets[nets.Length - 1];
                    if (lastNet.Length == 0)
                        throw new FormatException("An invalid IP address was specified.");
#if NET_2_1 //workaround for smcs, as it generate code that can't access string.GetEnumerator ()
					foreach (char c in lastNet.ToCharArray ())
#else
                    foreach (char c in lastNet)
#endif
                        if (!IsHexDigit(c))
                            throw new FormatException("An invalid IP address was specified.");
                }
                ip = ip.Substring(0, pos);
            }

            if (ip.Length == 0 || ip[ip.Length - 1] == '.')
                throw new FormatException("An invalid IP address was specified.");

            string[] ips = ip.Split(new char[] { '.' });
            if (ips.Length > IPv4Length)
                throw new FormatException("An invalid IP address was specified.");

            // Make the number in network order
            try
            {
                _ip = new byte[IPv4Length];
                long val = 0;
                for (int i = 0; i < ips.Length; i++)
                {
                    string subnet = ips[i];
                    if ((3 <= subnet.Length && subnet.Length <= 4) &&
                        (subnet[0] == '0') && (subnet[1] == 'x' || subnet[1] == 'X'))
                    {
                        if (subnet.Length == 3)
                            val = (byte)FromHex(subnet[2]);
                        else
                            val = (byte)((FromHex(subnet[2]) << 4) | FromHex(subnet[3]));
                    }
                    else if (subnet.Length == 0)
                        throw new FormatException("An invalid IP address was specified.");
                    else if (subnet[0] == '0')
                    {
                        // octal
                        val = 0;
                        for (int j = 1; j < subnet.Length; j++)
                        {
                            if ('0' <= subnet[j] && subnet[j] <= '7')
                                val = (val << 3) + subnet[j] - '0';
                            else
                                throw new FormatException("An invalid IP address was specified.");
                        }
                    }
                    else
                    {
                        if (!long.TryParse(subnet, NumberStyles.None, null, out val))
                            throw new FormatException("An invalid IP address was specified.");
                    }

                    if (i == (ips.Length - 1))
                    {
                        if (i != 0 && val >= (256 << ((3 - i) * 8)))
                            throw new FormatException("An invalid IP address was specified.");
                        else if (val > 0x3fffffffe) // this is the last number that parses correctly with MS
                            throw new FormatException("An invalid IP address was specified.");
                        i = 3;
                    }
                    else if (val >= 0x100)
                        throw new FormatException("An invalid IP address was specified.");
                    _ip[i] = (byte)val;
                }
            }
            catch (Exception)
            {
                throw new FormatException("An invalid IP address was specified.");
            }
        }

        private static int FromHex(char digit)
        {
            if ('0' <= digit && digit <= '9')
            {
                return (int)(digit - '0');
            }

            if ('a' <= digit && digit <= 'f')
                return (int)(digit - 'a' + 10);

            if ('A' <= digit && digit <= 'F')
                return (int)(digit - 'A' + 10);

            throw new ArgumentException("Invalid digit.", nameof(digit));
        }

        private static bool IsHexDigit(char character)
        {
            return (('0' <= character && character <= '9') ||
                    ('a' <= character && character <= 'f') ||
                    ('A' <= character && character <= 'F'));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IP"/> class.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="stream">The stream.</param>
        public IP(Tuple<int, byte[]> length, Stream stream)
        {
            if (length == null)
            {
                throw new ArgumentNullException(nameof(length));
            }

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (length.Item1 != IPv4Length)
            {
                throw new ArgumentException("Bytes must contain 4 or 16 elements.", nameof(length));
            }

            var raw = new byte[length.Item1];
            stream.Read(raw, 0, length.Item1);
            _ip = raw;
            _length = length.Item2;
        }

        /// <summary>
        /// Gets the raw bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] GetRaw()
        {
            return _ip;
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="IP"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}.{3}", _ip[0], _ip[1], _ip[2], _ip[3]);
        }
        
        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get
            {
                return SnmpType.IPAddress;
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
            
            stream.AppendBytes(TypeCode, _length, _ip);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="IP"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="IP"/>. </param>
        /// <returns><value>true</value> if the specified <see cref="Object"/> is equal to the current <see cref="IP"/>; otherwise, <value>false</value>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(this, obj as IP);
        }
        
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><value>true</value> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <value>false</value>.
        /// </returns>
        public bool Equals(IP other)
        {
            return Equals(this, other);
        }
        
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="IP"/>.</returns>
        public override int GetHashCode()
        {
            return _ip.GetHashCode();
        }
        
        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <param name="left">Left <see cref="IP"/> object</param>
        /// <param name="right">Right <see cref="IP"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are equal, <c>false</c> otherwise.</returns>
        public static bool operator ==(IP left, IP right)
        {
            return Equals(left, right);
        }
        
        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <param name="left">Left <see cref="IP"/> object</param>
        /// <param name="right">Right <see cref="IP"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        public static bool operator !=(IP left, IP right)
        {
            return !(left == right);
        }
        
        /// <summary>
        /// The comparison.
        /// </summary>
        /// <param name="left">Left <see cref="IP"/> object</param>
        /// <param name="right">Right <see cref="IP"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        private static bool Equals(IP left, IP right)
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

            return left._ip.SequenceEqual(right._ip);           
        }
    }
}
