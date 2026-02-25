using DotNetSnmp.Asn1.Serialization;
using System.Formats.Asn1;

namespace DotNetSnmp.Asn1.SyntaxObjects
{
    /// <summary>
    /// Represents the Opaque type.
    /// </summary>
    public readonly record struct Opaque(byte[] OctetString) : IAsnSerializable
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
