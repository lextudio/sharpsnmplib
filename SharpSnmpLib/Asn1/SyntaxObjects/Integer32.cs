using DotNetSnmp.Asn1.Serialization;
using System.Formats.Asn1;

namespace DotNetSnmp.Asn1.SyntaxObjects
{
    /// <summary>
    /// Represents the Integer32 type.
    /// </summary>
    public readonly record struct Integer32(int Value) : IAsnSerializable
    {
        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
    {
        writer.WriteInteger(
            Value,
            tag: AsnTypes.Integer32);
    }

    /// <summary>
    /// Deconstructs the value into its components.
    /// </summary>
    public void Deconstruct(out int value)
    {
        value = Value;
    }

    /// <summary>
    /// Reads a value from an ASN.1 reader.
    /// </summary>
    public static Integer32 ReadFrom(AsnReader reader)
    {
        reader.TryReadInt32(out var value);
        return new(value);
    }

    /// <summary>
    /// Performs a conversion to int.
    /// </summary>
    public static implicit operator int(Integer32 x) => x.Value;
}
}
