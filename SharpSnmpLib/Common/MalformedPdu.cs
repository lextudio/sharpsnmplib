using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Malformed PDU class. Returned when a PDU cannot be parsed.
    /// </summary>
    public sealed class MalformedPdu : Pdu
    {
        /// <inheritdoc/>
        public override Asn1Tag PduType => SnmpAsnTags.GetMsg;

        /// <summary>Gets the SNMP type code.</summary>
        public new SnmpType TypeCode => SnmpType.Unknown;

        /// <inheritdoc/>
        public override void WriteTo(AsnWriter writer)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public override string ToString() => "Malformed PDU";
    }
}
