using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// The Gauge32 type represents a non-negative integer, which may
    /// increase or decrease, but shall never exceed a maximum value, nor
    /// fall below a minimum value. (doesn't wrap)
    /// </summary>
    /// <param name="Value"></param>
    [System.CLSCompliant(false)]
    public readonly record struct Gauge32(uint Value) : ISnmpData
    {
        /// <summary>
        /// Initializes a new instance from a legacy long value.
        /// </summary>
        [System.CLSCompliant(false)]
        public Gauge32(long value)
            : this(unchecked((uint)value))
        {
        }

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
        /// Gets the SNMP type code.
        /// </summary>
        public new SnmpType TypeCode => SnmpType.Gauge32;

        /// <summary>
        /// Returns value as uint.
        /// </summary>
        [System.CLSCompliant(false)]
        public uint ToUInt32() => Value;

        /// <summary>
        /// Performs a conversion to uint.
        /// </summary>
        [System.CLSCompliant(false)]
        public static implicit operator uint(Gauge32 x) => x.Value;

        /// <summary>
        /// Performs a compatibility conversion from long.
        /// </summary>
        [System.CLSCompliant(false)]
        public static implicit operator Gauge32(long value) => new(unchecked((uint)value));
    }
}
