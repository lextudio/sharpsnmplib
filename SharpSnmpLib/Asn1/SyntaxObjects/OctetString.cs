using Lextm.SharpSnmpLib;
using System;
using System.Formats.Asn1;
using System.Net.NetworkInformation;
using System.Text;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Represents the OctetString type.
    /// </summary>
    public readonly record struct OctetString : ISnmpData, IEquatable<OctetString>
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
        /// Initializes a new instance of OctetString with a security level.
        /// </summary>
        public OctetString(Levels level)
            : this(new[] { (byte)level })
        {
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

        /// <summary>Gets the SNMP type code.</summary>
        public SnmpType TypeCode => SnmpType.OctetString;

        /// <summary>Returns the raw octets.</summary>
        public byte[] GetRaw() => Octets ?? Array.Empty<byte>();

        /// <inheritdoc/>
        ReadOnlySpan<byte> ISnmpData.GetRaw() => Octets;

        /// <summary>Returns a hex string representation.</summary>
        public string ToHexString()
        {
            var raw = GetRaw();
            return BitConverter.ToString(raw).Replace("-", "").ToLowerInvariant();
        }

        /// <summary>Converts to PhysicalAddress (MAC address).</summary>
        public PhysicalAddress ToPhysicalAddress() => new(GetRaw());

        /// <summary>Converts to Levels (security level byte).</summary>
        public Levels ToLevels()
        {
            var raw = GetRaw();
            if (raw.Length == 0) return default;
            return (Levels)(raw[0] & 7);
        }

        /// <summary>Returns true if value is null or empty.</summary>
        public static bool IsNullOrEmpty(OctetString value) => value.Octets is null || value.Octets.Length == 0;

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
