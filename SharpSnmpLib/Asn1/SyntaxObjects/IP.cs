using Lextm.SharpSnmpLib;
using System.Formats.Asn1;
using System.Text;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// The IpAddress type represents a 32-bit internet address.  It is
    /// represented as an OCTET STRING of length 4, in network byte-order
    /// </summary>
    public readonly record struct IP : ISnmpData
    {
        /// <summary>
        /// Gets address Bytes.
        /// </summary>
        public readonly byte[] AddressBytes { get; }

        /// <summary>
        /// Initializes a new instance of IP.
        /// </summary>
        public IP(string address)
        {
            AddressBytes = Encoding.UTF8.GetBytes(address);
        }

        /// <summary>
        /// Initializes a new instance of IP.
        /// </summary>
        public IP(byte[] address)
        {
            if (address.Length != 4)
            {
                throw new ArgumentException("IpAddress must be a 4-Length OctetString");
            }

            AddressBytes = address;
        }

        /// <summary>
        /// Initializes a new instance of IP.
        /// </summary>
        public IP(System.Net.IPAddress address)
        {
            AddressBytes = address.GetAddressBytes();
        }

        /// <summary>Gets the SNMP type code.</summary>
        public SnmpType TypeCode => SnmpType.IPAddress;

        /// <summary>Returns the raw address bytes.</summary>
        public byte[] GetRaw() => AddressBytes;

        /// <inheritdoc/>
        ReadOnlySpan<byte> ISnmpData.GetRaw() => AddressBytes;

        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            writer.WriteOctetString(
                AddressBytes,
                tag: AsnTypes.IpAddress);
        }

        /// <summary>
        /// Reads a value from an ASN.1 reader.
        /// </summary>
        public static IP ReadFrom(AsnReader reader)
        {
            var octets = reader.ReadOctetString(
                expectedTag: AsnTypes.IpAddress);
            return new(octets);
        }

        /// <summary>
        /// Deconstructs the value into its components.
        /// </summary>
        public void Deconstruct(out System.Net.IPAddress address)
        {
            address = new System.Net.IPAddress(AddressBytes);
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="IP"/>.
        /// </summary>
        public override string ToString()
        {
            var ip = AddressBytes;
            return $"IpAddress: {ip[0]}.{ip[1]}.{ip[2]}.{ip[3]}";
        }
    }
}
