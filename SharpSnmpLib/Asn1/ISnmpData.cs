using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Defines the contract for values that can serialize themselves as ASN.1.
    /// </summary>
    public interface ISnmpData
    {
        /// <summary>
        /// Writes this value to the supplied ASN.1 writer.
        /// </summary>
        /// <param name="writer">The writer that receives the encoded value.</param>
        public void WriteTo(AsnWriter writer);

        /// <summary>Returns a string representation of this SNMP data value.</summary>
        public string? ToString() => null;

        /// <summary>
        /// Gets SNMP type code (legacy compatibility member).
        /// </summary>
        public SnmpType TypeCode => this switch
        {
            Integer32 => SnmpType.Integer32,
            OctetString => SnmpType.OctetString,
            Null => SnmpType.Null,
            ObjectIdentifier => SnmpType.ObjectIdentifier,
            IP => SnmpType.IPAddress,
            Counter32 => SnmpType.Counter32,
            Gauge32 => SnmpType.Gauge32,
            TimeTicks => SnmpType.TimeTicks,
            Opaque => SnmpType.Opaque,
            Counter64 => SnmpType.Counter64,
            Unsigned32 => SnmpType.Unsigned32,
            NoSuchObject => SnmpType.NoSuchObject,
            NoSuchInstance => SnmpType.NoSuchInstance,
            EndOfMibView => SnmpType.EndOfMibView,
            _ => SnmpType.Unknown
        };
    }
}
