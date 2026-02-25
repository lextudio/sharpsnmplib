using DotNetSnmp.Asn1.Serialization;
using System.Formats.Asn1;

namespace DotNetSnmp.Asn1.SyntaxObjects
{
    /// <summary>
    /// Represents the Unsigned32 type.
    /// </summary>
    public readonly record struct Unsigned32(uint Value) : IAsnSerializable
    {
        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
    {
        writer.WriteInteger(
            Value,
            tag: AsnTypes.Unsigned32);
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
    public static implicit operator uint(Unsigned32 x) => x.Value;
}
}
