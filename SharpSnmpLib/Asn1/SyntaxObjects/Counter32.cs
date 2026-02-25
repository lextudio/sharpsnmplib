using DotNetSnmp.Asn1.Serialization;
using System.Formats.Asn1;

namespace DotNetSnmp.Asn1.SyntaxObjects
{
    /// <summary>
    /// The Counter32 type represents a non-negative integer which
    /// monotonically increases until it reaches a maximum value of 2^32-1
    /// (4294967295 decimal), when it wraps around and starts increasing
    /// again from zero. (wraps)
    /// </summary>
    /// <param name="Value"></param>
    public readonly record struct Counter32(uint Value) : IAsnSerializable
    {
        public void WriteTo(AsnWriter writer)
    {
        writer.WriteInteger(
            Value,
            tag: AsnTypes.Counter32);
    }

    public void Deconstruct(out uint value)
    {
        value = Value;
    }

    public static implicit operator uint(Counter32 x) => x.Value;
}
}
