using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// The TimeTicks type represents a non-negative integer which represents
    /// the time, modulo 2^32 (4294967296 decimal), in hundredths of a second
    /// between two epochs.When objects are defined which use this ASN.1
    /// type, the description of the object identifies both of the reference
    /// epochs.
    /// </summary>
    /// <param name="Value"></param>
    [System.CLSCompliant(false)]
    public readonly record struct TimeTicks(uint Value) : ISnmpData
    {
        /// <summary>Initializes a new instance from a TimeSpan.</summary>
        public TimeTicks(TimeSpan span) : this(unchecked((uint)(span.Ticks / 100000L))) { }

        /// <summary>Gets the SNMP type code.</summary>
        public SnmpType TypeCode => SnmpType.TimeTicks;

        /// <summary>Converts to uint (legacy compatibility).</summary>
        [System.CLSCompliant(false)]
        public uint ToUInt32() => Value;

        /// <summary>Converts to TimeSpan.</summary>
        public TimeSpan ToTimeSpan() => new(Value * 100000L);

        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            writer.WriteInteger(
                Value, tag: AsnTypes.TimeTicks);
        }

        /// <summary>
        /// Reads a value from an ASN.1 reader.
        /// </summary>
        public static TimeTicks ReadFrom(AsnReader reader)
        {
            reader.TryReadUInt32(
                out var ticks,
                expectedTag: AsnTypes.TimeTicks);
            return new(ticks);
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="TimeTicks"/>.
        /// </summary>
        public override string ToString()
        {
            return new TimeSpan(Value * 100000L).ToString();
        }

        /// <summary>
        /// Performs a conversion to uint.
        /// </summary>
        [System.CLSCompliant(false)]
        public static implicit operator uint(TimeTicks t) => t.Value;
    }
}
