using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Represents the Integer32 type.
    /// </summary>
    public readonly record struct Integer32(int Value) : ISnmpData
    {
        /// <summary>
        /// Zero value.
        /// </summary>
        public static Integer32 Zero { get; } = new(0);

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
        /// Returns a string representation of this value.
        /// </summary>
        public override string ToString() => Value.ToString();

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

        /// <summary>
        /// Performs an implicit conversion from int.
        /// </summary>
        public static implicit operator Integer32(int value) => new(value);
    }
}
