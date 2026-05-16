using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Represents the NoSuchObject type.
    /// </summary>
    public readonly record struct NoSuchObject : ISnmpData
    {
        /// <summary>Initializes a new instance.</summary>
        public NoSuchObject() { }

        /// <summary>Gets the SNMP type code.</summary>
        public SnmpType TypeCode => SnmpType.NoSuchObject;

        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            writer.WriteNull(tag: SnmpAsnTags.NoSuchObject);
        }
    }
}
