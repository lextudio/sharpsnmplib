using DotNetSnmp.Asn1.Serialization;
using System.Formats.Asn1;

namespace DotNetSnmp.Asn1.SyntaxObjects
{
    public readonly record struct Opaque(byte[] OctetString) : IAsnSerializable
    {
        public void WriteTo(AsnWriter writer)
    {
        writer.WriteOctetString(OctetString, AsnTypes.Opaque);
    }

    public static implicit operator byte[](Opaque o) => o.OctetString;
}
}
