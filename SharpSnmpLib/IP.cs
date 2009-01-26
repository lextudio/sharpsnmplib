using System;
using System.IO;
using System.Net;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// IPAddress type.
    /// </summary>
    public sealed class IP : ISnmpData, IEquatable<IP>
    {
        private readonly IPAddress _ip;
        
        /// <summary>
        /// Creates an <see cref="IP"/> with a specific <see cref="IPAddress"/>.
        /// </summary>
        /// <param name="ip">IP address</param>
        public IP(IPAddress ip)
        {
            if (ip == null)
            {
                throw new ArgumentNullException("ip");
            }
            
            _ip = ip;
        }
        
        /// <summary>
        /// Creates an <see cref="IP"/> from raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        private IP(byte[] raw): this(raw.Length, new MemoryStream(raw))
        {
           
        }
        
        /// <summary>
        /// Creates an <see cref="IP"/> from a specific <see cref="String"/>.
        /// </summary>
        /// <param name="ip">IP string</param>
        public IP(string ip) : this(IPAddress.Parse(ip))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IP"/> class.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="stream">The stream.</param>
        public IP(int length, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (length != 4)
            {
                throw new ArgumentException("bytes must contain 4 elements");
            }

            byte[] raw = new byte[4];
            stream.Read(raw, 0, 4);
            _ip = new IPAddress(raw);
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="IP"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _ip.ToString();
        }
        
        /// <summary>
        /// Returns an <see cref="IPAddress"/> that represents this <see cref="IP"/>.
        /// </summary>
        /// <returns></returns>
        public IPAddress ToIPAddress()
        {
            return _ip;
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
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        internal byte[] ToBytes()
        {
            MemoryStream result = new MemoryStream();
            AppendBytesTo(result);
            return result.ToArray();
        }

        /// <summary>
        /// Appends the bytes to <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void AppendBytesTo(Stream stream)
        {
            ByteTool.AppendBytes(stream, TypeCode, _ip.GetAddressBytes());
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
        public static bool Equals(IP left, IP right)
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
            
            return left.ToIPAddress() == right.ToIPAddress();
        }
    }
}
