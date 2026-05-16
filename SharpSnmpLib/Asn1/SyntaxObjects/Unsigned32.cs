using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Represents the Unsigned32 type.
    /// </summary>
    [System.CLSCompliant(false)]
    public readonly record struct Unsigned32(uint Value) : ISnmpData
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
        [System.CLSCompliant(false)]
        public static implicit operator uint(Unsigned32 x) => x.Value;
    }
}
