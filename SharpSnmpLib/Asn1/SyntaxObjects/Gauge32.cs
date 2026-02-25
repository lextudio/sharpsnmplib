using DotNetSnmp.Asn1.Serialization;
using System.Formats.Asn1;

namespace DotNetSnmp.Asn1.SyntaxObjects
{
    /// <summary>
    /// The Gauge32 type represents a non-negative integer, which may
    /// increase or decrease, but shall never exceed a maximum value, nor
    /// fall below a minimum value. (doesn't wrap)
    /// </summary>
    /// <param name="Value"></param>
    public readonly record struct Gauge32(uint Value) : IAsnSerializable
    {
        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
    {
        writer.WriteInteger(
            Value,
            tag: AsnTypes.Gauge32);
    }
    /// <summary>
    /// Deconstructs the value into its components.
    /// </summary>
    public void Deconstruct(out uint value)
    {
        value = Value;
    }

    /// <summary>
    /// Performs a conversion to uint.
    /// </summary>
    public static implicit operator uint(Gauge32 x) => x.Value;
}
}
