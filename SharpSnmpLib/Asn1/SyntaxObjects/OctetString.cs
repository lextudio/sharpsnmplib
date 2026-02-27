using DotNetSnmp.Asn1.Serialization;
using System;
using System.Formats.Asn1;
using System.Text;

namespace DotNetSnmp.Asn1.SyntaxObjects
{
    /// <summary>
    /// Represents the OctetString type.
    /// </summary>
    public readonly record struct OctetString : IAsnSerializable, IEquatable<OctetString>
    {
        /// <summary>
        /// Default text encoding used by string-based OctetString conversions.
        /// </summary>
        public static Encoding DefaultEncoding { get; set; } = Encoding.ASCII;

        /// <summary>
        /// An empty octet string value.
        /// </summary>
        public static OctetString Empty { get; } = new(string.Empty, Encoding.ASCII);

        /// <summary>
        /// Gets octets.
        /// </summary>
        public byte[] Octets { get; }

        /// <summary>
        /// Gets the text encoding associated with this value.
        /// </summary>
        public Encoding Encoding { get; }

        /// <summary>
        /// Initializes a new instance of OctetString.
        /// </summary>
        public OctetString(string str)
            : this(str, DefaultEncoding)
        {
        }

        /// <summary>
        /// Initializes a new instance of OctetString.
        /// </summary>
        public OctetString(string str, Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull(str);
            ArgumentNullException.ThrowIfNull(encoding);

            Octets = encoding.GetBytes(str);
            Encoding = encoding;
        }

        /// <summary>
        /// Initializes a new instance of OctetString.
        /// </summary>
        public OctetString(byte[] octets)
        {
            ArgumentNullException.ThrowIfNull(octets);
            Octets = octets;
            Encoding = DefaultEncoding;
        }

        /// <summary>
        /// Returns a <see cref="String"/> using a specified encoding.
        /// </summary>
        public string ToString(Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull(encoding);
            return Octets is null ? string.Empty : encoding.GetString(Octets);
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="OctetString"/>.
        /// </summary>
        public override string ToString()
        {
            return ToString(Encoding);
        }

        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            writer.WriteOctetString(Octets);
        }

        /// <summary>
        /// Reads a value from an ASN.1 reader.
        /// </summary>
        public static OctetString ReadFrom(AsnReader reader)
        {
            var octets = reader.ReadOctetString();
            return new(octets);
        }

        /// <summary>
        /// Performs a conversion to string.
        /// </summary>
        public static implicit operator string(OctetString o) => o.ToString();

        // Add these methods to your OctetString struct
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        public bool Equals(OctetString other)
        {
            if (Octets == null && other.Octets == null)
                return true;
            if (Octets == null || other.Octets == null)
                return false;

            return Octets.SequenceEqual(other.Octets);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        public override int GetHashCode()
        {
            if (Octets == null)
                return 0;

            // Create a hash code based on the contents of the Octets array
            HashCode hash = new HashCode();
            foreach (byte b in Octets)
            {
                hash.Add(b);
            }
            return hash.ToHashCode();
        }
    }
}
