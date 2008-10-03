using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// ObjectIdentifier type.
    /// </summary>
    public sealed class ObjectIdentifier : ISnmpData, IEquatable<ObjectIdentifier>
    {
        private uint[] _oid;
        
        /// <summary>
        /// Creates an <see cref="ObjectIdentifier"/> instance from textual ID.
        /// </summary>
        /// <param name="text">String in one of the formats, "[module]:[name]" or "*.*.*.*".</param>
        public ObjectIdentifier(string text) : this(ParseString(text))
        {
        }

        private static uint[] ParseString(string text)
        {
            if (text.Contains("::"))
            {
                return Mib.ObjectRegistry.Instance.Translate(text);
            }
            else
            {
                return ObjectIdentifier.Convert(text);
            }
        }
        
        /// <summary>
        /// Creates an <see cref="ObjectIdentifier"/> instance from numerical ID.
        /// </summary>
        /// <param name="oid">OID <see cref="uint"/> array</param>
        [CLSCompliant(false)]
        public ObjectIdentifier(uint[] oid)
        {
            if (oid.Length < 2)
            {
                throw new ArgumentException("The length of the shortest identifier is two", "oid");
            }

            if (oid[0] > 2)
            {
                throw new ArgumentException("The first sub-identifier must be 0, 1, or 2.", "oid");
            }

            if (oid[1] > 39)
            {
                throw new ArgumentException("The second sub-identifier must be less than 40", "oid");
            }

            _oid = oid;
        }
        
        /// <summary>
        /// Creates an <see cref="ObjectIdentifier"/> instance from raw bytes.
        /// </summary>
        /// <param name="raw">Raw bytes</param>
        public ObjectIdentifier(byte[] raw)
        {
            // _oid = ParsePduFormat(raw, (uint)raw.Length);
            _oid = ParseRaw(raw);
        }

        private static uint[] ParseRaw(byte[] raw)
        {
            List<uint> result = new List<uint>();
            result.Add((uint)(raw[0] / 40));
            result.Add((uint)(raw[0] % 40));
            uint buffer = 0;
            for (int i = 1; i < raw.Length; i++)
            {
                if ((raw[i] & 0x80) == 0)
                {
                    result.Add(raw[i] + (buffer << 7));
                    buffer = 0;
                }
                else
                {
                    buffer <<= 7;
                    buffer += (uint)(raw[i] & 0x7F);
                }
            }
            
            return result.ToArray();
        }

        /// <summary>
        /// Convers to numerical ID.
        /// </summary>
        /// <returns></returns>
        [CLSCompliant(false)]
        public uint[] ToNumerical()
        {
            return _oid;
        }
        
        /// <summary>
        /// Textual ID.
        /// </summary>
        public string Textual
        {
            get
            {
                return Mib.ObjectRegistry.Instance.Translate(_oid);
            }
        }
        
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="ObjectIdentifier"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Convert(_oid);
        }

        /// <summary>
        /// Converts uint array to dotted <see cref="String"/>.
        /// </summary>
        /// <param name="numerical"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static string Convert(uint[] numerical)
        {
            StringBuilder result = new StringBuilder();
            for (int k = 0; k < numerical.Length; k++)
            {
                result.Append("." + numerical[k].ToString(CultureInfo.InvariantCulture));
            }

            return result.ToString();
        }

        /// <summary>
        /// Converts dotted <see cref="String"/> to uint array.
        /// </summary>
        /// <param name="dotted">Dotted string.</param>
        /// <returns>uint array.</returns>
        [CLSCompliant(false)]
        public static uint[] Convert(string dotted)
        {
            string[] parts = dotted.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            uint[] result = new uint[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                result[i] = uint.Parse(parts[i], CultureInfo.InvariantCulture);
            }
            
            return result;
        }
        
        /// <summary>
        /// Converts to byte format.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            List<byte> temp = new List<byte>();
            byte first = (byte)((40 * _oid[0]) + _oid[1]);
            temp.Add(first);
            for (int i = 2; i < _oid.Length; i++)
            {
                temp.AddRange(ConvertToBytes(_oid[i]));
            }
            
            return ByteTool.ToBytes(SnmpType.ObjectIdentifier, temp.ToArray());
        }

        private static IEnumerable<byte> ConvertToBytes(uint subIdentifier)
        {
            List<byte> result = new List<byte>();
            result.Add((byte)(subIdentifier & 0x7F));
            while ((subIdentifier = subIdentifier >> 7) > 0)
            {
                result.Add((byte)((subIdentifier & 0x7F) | 0x80));
            }
            
            result.Reverse();
            return result;
        }
        
        /// <summary>
        /// Type code.
        /// </summary>
        public SnmpType TypeCode
        {
            get
            {
                return SnmpType.ObjectIdentifier;
            }
        }
        
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="ObjectIdentifier"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="ObjectIdentifier"/>. </param>
        /// <returns><value>true</value> if the specified <see cref="Object"/> is equal to the current <see cref="ObjectIdentifier"/>; otherwise, <value>false</value>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(this, obj as ObjectIdentifier);
        }
        
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><value>true</value> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <value>false</value>.
        /// </returns>
        public bool Equals(ObjectIdentifier other)
        {
            return Equals(this, other);
        }
        
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="ObjectIdentifier"/>.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
        
        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <param name="left">Left <see cref="ObjectIdentifier"/> object</param>
        /// <param name="right">Right <see cref="ObjectIdentifier"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are equal, <c>false</c> otherwise.</returns>
        public static bool operator ==(ObjectIdentifier left, ObjectIdentifier right)
        {
            return Equals(left, right);
        }
        
        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <param name="left">Left <see cref="ObjectIdentifier"/> object</param>
        /// <param name="right">Right <see cref="ObjectIdentifier"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        public static bool operator !=(ObjectIdentifier left, ObjectIdentifier right)
        {
            return !(left == right);
        }
        
        /// <summary>
        /// The comparison.
        /// </summary>
        /// <param name="left">Left <see cref="ObjectIdentifier"/> object</param>
        /// <param name="right">Right <see cref="ObjectIdentifier"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        public static bool Equals(ObjectIdentifier left, ObjectIdentifier right)
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
            
            return ByteTool.CompareArray(left._oid, right._oid);
        }
    }
}