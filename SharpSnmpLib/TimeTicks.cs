// TimeTicks data type.
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
using System.Diagnostics;
using System.IO;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// TimeTicks type.
    /// </summary>
    /// <remarks>Represents SNMP TimeTicks type.</remarks>
    [DebuggerDisplay("{count} ({ToTimeSpan()})")]
    public sealed class TimeTicks : ISnmpData, IEquatable<TimeTicks>
    {
        private readonly Counter32 _count;
        
        /// <summary>
        /// Creates a <see cref="TimeTicks"/> instance with a specific count.
        /// </summary>
        /// <param name="count">Count</param>
        [CLSCompliant(false)]
        public TimeTicks(uint count)
        {
            _count = new Counter32(count);
        }
        
        /// <summary>
        /// Creates a <see cref="TimeTicks"/> instance with <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="span">The time span.</param>        
        public TimeTicks(TimeSpan span) : this((uint)(span.TotalMilliseconds / 10))
        {            
        }
        
        /// <summary>
        /// Creates a <see cref="TimeTicks"/> instance with raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        internal TimeTicks(byte[] raw) : this(new Tuple<int, byte[]>(raw.Length, raw.Length.WritePayloadLength()), new MemoryStream(raw))
        {
            // IMPORTANT: for test project only.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeTicks"/> class.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="stream">The stream.</param>
        public TimeTicks(Tuple<int, byte[]> length, Stream stream)
        {
            if (length == null)
            {
                throw new ArgumentNullException(nameof(length));
            }

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            
            _count = new Counter32(length, stream);
        }

        /// <summary>
        /// Returns an <see cref="Int32"/> that represents the current <see cref="TimeTicks"/>
        /// </summary>
        /// <returns></returns>
        [CLSCompliant(false)]
        public uint ToUInt32()
        {
            return _count.ToUInt32();
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> representation.
        /// </summary>
        /// <returns></returns>
        public TimeSpan ToTimeSpan()
        {
            long root = ToUInt32();
            root *= 100000;
            return new TimeSpan(root);
        }
            
        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get
            {
                return SnmpType.TimeTicks;
            }
        }
        
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="TimeTicks"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="TimeTicks"/>. </param>
        /// <returns><value>true</value> if the specified <see cref="Object"/> is equal to the current <see cref="TimeTicks"/>; otherwise, <value>false</value>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(this, obj as TimeTicks);
        }
        
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="TimeTicks"/>.</returns>
        public override int GetHashCode()
        {
            return _count.GetHashCode();
        }

        #region ISnmpData Members

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
            
            stream.AppendBytes(TypeCode, _count.GetLengthBytes(), _count.GetRaw());
        }

        #endregion
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><value>true</value> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <value>false</value>.
        /// </returns>
        public bool Equals(TimeTicks other)
        {
            return Equals(this, other);
        }
        
        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <param name="left">Left <see cref="TimeTicks"/> object</param>
        /// <param name="right">Right <see cref="TimeTicks"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are equal, <c>false</c> otherwise.</returns>
        public static bool operator ==(TimeTicks left, TimeTicks right)
        {
            return Equals(left, right);
        }
        
        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <param name="left">Left <see cref="TimeTicks"/> object</param>
        /// <param name="right">Right <see cref="TimeTicks"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        public static bool operator !=(TimeTicks left, TimeTicks right)
        {
            return !(left == right);
        }
        
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="TimeTicks"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToTimeSpan().ToString();
        }
        
        /// <summary>
        /// The comparison.
        /// </summary> 
        /// <param name="left">Left <see cref="TimeTicks"/> object</param>
        /// <param name="right">Right <see cref="TimeTicks"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        private static bool Equals(TimeTicks left, TimeTicks right)
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
            
            return left._count == right._count;
        }
    }
}
