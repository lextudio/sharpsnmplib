using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Represents the Null type.
    /// </summary>
    public readonly record struct Null : ISnmpData
    {
        /// <summary>Initializes a new instance.</summary>
        public Null() { }

        /// <summary>
        /// A reusable SNMP NULL value instance.
        /// </summary>
        public readonly static Null Instance = new();

        /// <summary>Gets the SNMP type code.</summary>
        public SnmpType TypeCode => SnmpType.Null;

        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            writer.WriteNull();
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="Null"/>.
        /// </summary>
        public override string ToString()
        {
            return "NULL";
        }
    }
}
