using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Represents the Opaque type.
    /// </summary>
    public readonly record struct Opaque(byte[] OctetString) : ISnmpData
    {
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
