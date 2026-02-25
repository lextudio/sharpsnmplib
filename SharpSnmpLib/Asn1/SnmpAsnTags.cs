using System.Formats.Asn1;

namespace DotNetSnmp.Asn1.Serialization
{
    public static class SnmpAsnTags
    {
        #region PDU types in SNMPv1, SNMPsec, SNMPv2p, SNMPv2c, SNMPv2u, SNMPv2*, and SNMPv3 
        // (ASN_CONTEXT | ASN_CONSTRUCTOR | 0x0): 0xA0=160
        public readonly static Asn1Tag GetMsg = new(TagClass.ContextSpecific, 0, isConstructed: true);

        // (ASN_CONTEXT | ASN_CONSTRUCTOR | 0x1): 0xA1=161
        public readonly static Asn1Tag GetNextMsg = new(TagClass.ContextSpecific, 1, isConstructed: true);

        // (ASN_CONTEXT | ASN_CONSTRUCTOR | 0x2): 0xA2=162
        public readonly static Asn1Tag GetResponseMsg = new(TagClass.ContextSpecific, 2, isConstructed: true);

        // (ASN_CONTEXT | ASN_CONSTRUCTOR | 0x3): 0xA3=163
        public readonly static Asn1Tag SetMsg = new(TagClass.ContextSpecific, 3, isConstructed: true);
        #endregion

        #region PDU types in SNMPv1 and SNMPsec 

        // (ASN_CONTEXT | ASN_CONSTRUCTOR | 0x4): 0xA4=164
        public readonly static Asn1Tag TrapMsg = new(TagClass.ContextSpecific, 4, isConstructed: true);
        #endregion

        #region PDU types in SNMPv2p, SNMPv2c, SNMPv2u, SNMPv2*, and SNMPv3 

        // (ASN_CONTEXT | ASN_CONSTRUCTOR | 0x5) /* a5=165 */
        public readonly static Asn1Tag BulkMsg = new(TagClass.ContextSpecific, 5, isConstructed: true);

        // (ASN_CONTEXT | ASN_CONSTRUCTOR | 0x6) /* a6=166 */
        public readonly static Asn1Tag InformMsg = new(TagClass.ContextSpecific, 6, isConstructed: true);

        // (ASN_CONTEXT | ASN_CONSTRUCTOR | 0x7) /* a7=167 */
        public readonly static Asn1Tag Trap2Msg = new(TagClass.ContextSpecific, 7, isConstructed: true);
        #endregion

        #region PDU types in SNMPv2u, SNMPv2*, and SNMPv3

        // (ASN_CONTEXT | ASN_CONSTRUCTOR | 0x8) /* a8=168 */
        public readonly static Asn1Tag ReportMsg = new(TagClass.ContextSpecific, 8, isConstructed: true);
        #endregion

        #region Exception values for SNMPv2p, SNMPv2c, SNMPv2u, SNMPv2*, and SNMPv3 
        // (ASN_CONTEXT | ASN_PRIMITIVE | 0x0) /* 80=128 */
        public readonly static Asn1Tag NoSuchObject = new(TagClass.ContextSpecific, 0, isConstructed: false);

        // (ASN_CONTEXT | ASN_PRIMITIVE | 0x1) /* 81=129 */
        public readonly static Asn1Tag NoSuchInstance = new(TagClass.ContextSpecific, 1, isConstructed: false);

        // (ASN_CONTEXT | ASN_PRIMITIVE | 0x2) /* 82=130 */
        public readonly static Asn1Tag EndOfMibView = new(TagClass.ContextSpecific, 2, isConstructed: false);
        #endregion
    }
}
