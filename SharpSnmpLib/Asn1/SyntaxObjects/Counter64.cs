using DotNetSnmp.Asn1.Serialization;
using System.Formats.Asn1;

namespace DotNetSnmp.Asn1.SyntaxObjects
{
    public readonly record struct Counter64(ulong Value) : IAsnSerializable
    {
        public void WriteTo(AsnWriter writer)
    {
        writer.WriteInteger(
            Value,
            tag: AsnTypes.Counter64);
    }

    public static Counter64 ReadFrom(AsnReader reader)
    {
        reader.TryReadUInt64(
            out var value,
            expectedTag: AsnTypes.Counter64);
        return new(value);
    }

    public void Deconstruct(out ulong value)
    {
        value = Value;
    }

    public static implicit operator ulong(Counter64 x) => x.Value;
}
}
