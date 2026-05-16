using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// The Counter32 type represents a non-negative integer which
    /// monotonically increases until it reaches a maximum value of 2^32-1
    /// (4294967295 decimal), when it wraps around and starts increasing
    /// again from zero. (wraps)
    /// </summary>
    /// <param name="Value"></param>
    [System.CLSCompliant(false)]
    public readonly record struct Counter32(uint Value) : ISnmpData
    {
        /// <summary>
        /// Initializes a new instance from a legacy long value.
        /// </summary>
        [System.CLSCompliant(false)]
        public Counter32(long value)
            : this(unchecked((uint)value))
        {
        }

        /// <inheritdoc/>
        public SnmpType TypeCode => SnmpType.Counter32;

        /// <summary>Returns the value as UInt32.</summary>
        [System.CLSCompliant(false)]
        public uint ToUInt32() => Value;

        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            writer.WriteInteger(
                Value,
                tag: AsnTypes.Counter32);
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
        public static implicit operator uint(Counter32 x) => x.Value;

        /// <summary>
        /// Performs a compatibility conversion from long.
        /// </summary>
        [System.CLSCompliant(false)]
        public static implicit operator Counter32(long value) => new(unchecked((uint)value));
    }
}
