using DotNetSnmp.Asn1.Serialization;
using System.Formats.Asn1;
using System.Text;

namespace DotNetSnmp.Asn1.SyntaxObjects
{
    public readonly record struct OctetString : IAsnSerializable, IEquatable<OctetString>
    {
        public static OctetString Empty { get; } = new(string.Empty);

        public byte[] Octets { get; }

        public OctetString(string str)
        {
            Octets = Encoding.UTF8.GetBytes(str);
        }

        public OctetString(byte[] octets)
        {
            Octets = octets;
        }

        public override string ToString()
        {
            var hasNonPrintable = Octets.Any(
                c => char.IsControl((char)c) && !char.IsWhiteSpace((char)c));

            if (hasNonPrintable == false)
            {
                return "String: " + Encoding.UTF8.GetString(Octets);
            }

            var hex = Convert.ToHexString(Octets)
                          .Chunk(2)
                          .Select(c => string.Concat(c))
                          .Aggregate((a, b) => a + " " + b);

            return "HexString: " + hex;
        }

        public void WriteTo(AsnWriter writer)
        {
            writer.WriteOctetString(Octets);
        }

        public static OctetString ReadFrom(AsnReader reader)
        {
            var octets = reader.ReadOctetString();
            return new(octets);
        }

        public static implicit operator string(OctetString o) => o.ToString();

        // Add these methods to your OctetString struct
        public bool Equals(OctetString other)
        {
            if (Octets == null && other.Octets == null)
                return true;
            if (Octets == null || other.Octets == null)
                return false;

            return Octets.SequenceEqual(other.Octets);
        }

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
