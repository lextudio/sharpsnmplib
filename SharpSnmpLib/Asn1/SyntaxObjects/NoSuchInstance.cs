using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Represents the NoSuchInstance type.
    /// </summary>
    public readonly record struct NoSuchInstance : ISnmpData
    {
        /// <summary>Initializes a new instance.</summary>
        public NoSuchInstance() { }

        /// <summary>Gets the SNMP type code.</summary>
        public SnmpType TypeCode => SnmpType.NoSuchInstance;

        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            writer.WriteNull(tag: SnmpAsnTags.NoSuchInstance);
        }
    }
}
