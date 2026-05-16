using Lextm.SharpSnmpLib;
using System;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Represents the Opaque type.
    /// </summary>
    public readonly record struct Opaque(byte[] OctetString) : ISnmpData
    {
        /// <summary>Gets the SNMP type code.</summary>
        public SnmpType TypeCode => SnmpType.Opaque;

        /// <summary>Returns the raw octets.</summary>
        public byte[] GetRaw() => OctetString ?? Array.Empty<byte>();

        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            writer.WriteOctetString(OctetString, AsnTypes.Opaque);
        }

        /// <summary>
        /// Performs a conversion to byte[].
        /// </summary>
        public static implicit operator byte[](Opaque o) => o.OctetString;
    }
}
