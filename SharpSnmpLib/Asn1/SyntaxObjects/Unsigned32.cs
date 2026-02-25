using DotNetSnmp.Asn1.Serialization;
using System.Formats.Asn1;

namespace DotNetSnmp.Asn1.SyntaxObjects
{
    public readonly record struct Unsigned32(uint Value) : IAsnSerializable
    {
        public void WriteTo(AsnWriter writer)
    {
        writer.WriteInteger(
            Value,
            tag: AsnTypes.Unsigned32);
    }

    public void Deconstruct(out uint value)
    {
        value = Value;
    }

    public static implicit operator uint(Unsigned32 x) => x.Value;
}
}
