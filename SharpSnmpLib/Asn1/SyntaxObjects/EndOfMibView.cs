using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Represents the EndOfMibView type.
    /// </summary>
    public readonly record struct EndOfMibView : ISnmpData
    {
        /// <summary>
        /// Initializes a new instance of EndOfMibView.
        /// </summary>
        public EndOfMibView() { }

        /// <summary>
        /// Gets the SNMP type code.
        /// </summary>
        public new SnmpType TypeCode => SnmpType.EndOfMibView;

        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            writer.WriteNull(tag: SnmpAsnTags.EndOfMibView);
        }
    }
}
