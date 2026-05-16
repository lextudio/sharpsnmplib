using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Represents the Counter64 type.
    /// </summary>
    [System.CLSCompliant(false)]
    public readonly record struct Counter64(ulong Value) : ISnmpData
    {
        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            writer.WriteInteger(
                Value,
                tag: AsnTypes.Counter64);
        }

        /// <summary>
        /// Reads a value from an ASN.1 reader.
        /// </summary>
        public static Counter64 ReadFrom(AsnReader reader)
        {
            reader.TryReadUInt64(
                out var value,
                expectedTag: AsnTypes.Counter64);
            return new(value);
        }

        /// <summary>
        /// Deconstructs the value into its components.
        /// </summary>
        public void Deconstruct(out ulong value)
        {
            value = Value;
        }

        /// <summary>
        /// Gets the SNMP type code.
        /// </summary>
        public new SnmpType TypeCode => SnmpType.Counter64;

        /// <summary>
        /// Returns value as ulong.
        /// </summary>
        [System.CLSCompliant(false)]
        public ulong ToUInt64() => Value;

        /// <summary>
        /// Performs a conversion to ulong.
        /// </summary>
        [System.CLSCompliant(false)]
        public static implicit operator ulong(Counter64 x) => x.Value;
    }
}
