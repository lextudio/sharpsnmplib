using DotNetSnmp.Asn1.Serialization;
using System.Formats.Asn1;

namespace DotNetSnmp.Asn1.SyntaxObjects
{
    public readonly record struct Integer32(int Value) : IAsnSerializable
    {
        public void WriteTo(AsnWriter writer)
    {
        writer.WriteInteger(
            Value,
            tag: AsnTypes.Integer32);
    }

    public void Deconstruct(out int value)
    {
        value = Value;
    }

    public static Integer32 ReadFrom(AsnReader reader)
    {
        reader.TryReadInt32(out var value);
        return new(value);
    }

    public static implicit operator int(Integer32 x) => x.Value;
}
}
