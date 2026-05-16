using Lextm.SharpSnmpLib;
using System.Formats.Asn1;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Represents the ObjectIdentifier type.
    /// </summary>
    public readonly record struct ObjectIdentifier(string Oid) : ISnmpData, IComparable<ObjectIdentifier>, IComparable
    {
        /// <summary>
        /// Initializes a new instance of ObjectIdentifier.
        /// </summary>
        [System.CLSCompliant(false)]
        public ObjectIdentifier(uint[] ids)
            : this(string.Join(".", ids))
        {
        }

        /// <summary>
        /// Initializes a new instance of ObjectIdentifier.
        /// </summary>
        public ObjectIdentifier(params byte[] octets)
            : this(Encoding.UTF8.GetString(octets))
        {
        }

        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            writer.WriteObjectIdentifier(Normalize(Oid));
        }

        /// <summary>
        /// Reads a value from an ASN.1 reader.
        /// </summary>
        public static ObjectIdentifier ReadFrom(AsnReader reader)
        {
            var oid = reader.ReadObjectIdentifier();
            return new(oid);
        }

        /// <summary>
        /// Returns a string representation of the current value.
        /// </summary>
        public override string ToString() => Normalize(Oid);

        /// <summary>
        /// Compares two object identifiers lexicographically by OID segments.
        /// </summary>
        public int CompareTo(ObjectIdentifier other)
        {
            var left = Oid.Split('.', StringSplitOptions.RemoveEmptyEntries);
            var right = other.Oid.Split('.', StringSplitOptions.RemoveEmptyEntries);
            var count = Math.Min(left.Length, right.Length);

            for (var i = 0; i < count; i++)
            {
                if (!ulong.TryParse(left[i], out var leftPart))
                {
                    leftPart = 0;
                }

                if (!ulong.TryParse(right[i], out var rightPart))
                {
                    rightPart = 0;
                }

                if (leftPart != rightPart)
                {
                    return leftPart.CompareTo(rightPart);
                }
            }

            return left.Length.CompareTo(right.Length);
        }

        /// <inheritdoc/>
        public int CompareTo(object? obj)
        {
            if (obj is ObjectIdentifier other)
            {
                return CompareTo(other);
            }

            return 1;
        }

        /// <summary>
        /// Performs a conversion to string.
        /// </summary>
        public static implicit operator string(ObjectIdentifier o) => Normalize(o.Oid);

        /// <summary>
        /// Performs a conversion to ObjectIdentifier.
        /// </summary>
        public static explicit operator ObjectIdentifier(string oid) => new(oid);

        /// <summary>
        /// Performs a conversion to ObjectIdentifier.
        /// </summary>
        public static explicit operator ObjectIdentifier(byte[] octets) => new(octets);

        /// <summary>
        /// Appends a single subidentifier to an existing OID array and returns the new array.
        /// This helper provides compatibility with the v12 `ObjectIdentifier.AppendTo` API.
        /// </summary>
        [System.CLSCompliant(false)]
        public static uint[] AppendTo(uint[]? original, uint extra)
        {
            if (original == null)
            {
                return new[] { extra };
            }

            var length = original.Length;
            var tmp = new uint[length + 1];
            Array.Copy(original, tmp, length);
            tmp[length] = extra;
            return tmp;
        }

        /// <summary>
        /// Converts unsigned integer array to dotted string representation.
        /// Compatible with v12 `ObjectIdentifier.Convert(uint[])`.
        /// </summary>
        [System.CLSCompliant(false)]
        public static string Convert(uint[] numerical)
        {
            if (numerical == null)
            {
                throw new ArgumentNullException(nameof(numerical));
            }

            var result = new StringBuilder(numerical[0].ToString(CultureInfo.InvariantCulture));
            for (var k = 1; k < numerical.Length; k++)
            {
                result.Append('.').Append(numerical[k].ToString(CultureInfo.InvariantCulture));
            }

            return result.ToString();
        }

        /// <summary>
        /// Converts dotted string representation to unsigned integer array.
        /// Compatible with v12 `ObjectIdentifier.Convert(string)`.
        /// </summary>
        [System.CLSCompliant(false)]
        public static uint[] Convert(string dotted)
        {
            if (dotted == null)
            {
                throw new ArgumentNullException(nameof(dotted));
            }

            var parts = dotted.Split('.');
            var result = new List<uint>();
            foreach (var s in parts)
            {
                if (string.IsNullOrEmpty(s))
                {
                    continue;
                }

                if (!uint.TryParse(s, out var temp))
                {
                    throw new ArgumentException($"Parameter {s} is out of 32 bit unsigned integer range.", nameof(dotted));
                }

                result.Add(temp);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Returns numerical representation of this OID as uint[] (v12-compatible).
        /// </summary>
        [System.CLSCompliant(false)]
        public uint[] ToNumerical()
        {
            return Convert(Normalize(Oid));
        }

        /// <summary>
        /// Compares whether left is greater than right.
        /// </summary>
        public static bool operator >(ObjectIdentifier left, ObjectIdentifier right) => left.CompareTo(right) > 0;

        /// <summary>
        /// Compares whether left is less than right.
        /// </summary>
        public static bool operator <(ObjectIdentifier left, ObjectIdentifier right) => left.CompareTo(right) < 0;

        /// <summary>
        /// Compares whether left is greater than or equal to right.
        /// </summary>
        public static bool operator >=(ObjectIdentifier left, ObjectIdentifier right) => left.CompareTo(right) >= 0;

        /// <summary>
        /// Compares whether left is less than or equal to right.
        /// </summary>
        public static bool operator <=(ObjectIdentifier left, ObjectIdentifier right) => left.CompareTo(right) <= 0;

        private static string Normalize(string oid)
        {
            if (string.IsNullOrWhiteSpace(oid))
            {
                return "0.0";
            }

            var normalized = string.Join(".", oid.Split('.', StringSplitOptions.RemoveEmptyEntries));
            return string.IsNullOrEmpty(normalized) ? "0.0" : normalized;
        }
    }
}
