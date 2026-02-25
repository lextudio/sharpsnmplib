using System.Formats.Asn1;

namespace DotNetSnmp.Asn1.Serialization
{
    /// <summary>
    /// Defines ASN.1 tags used by SNMP syntax values, including opaque extension tags.
    /// </summary>
    public static class AsnTypes
    {
        /// <summary>
        /// The application tag for the <c>IpAddress</c> syntax.
        /// </summary>
        public readonly static Asn1Tag IpAddress = new(TagClass.Application, 0);

        /// <summary>
        /// The application tag for the <c>Counter32</c> syntax.
        /// </summary>
        public readonly static Asn1Tag Counter32 = new(TagClass.Application, 1);

        /// <summary>
        /// The application tag for the <c>Gauge32</c> syntax.
        /// </summary>
        public readonly static Asn1Tag Gauge32 = new(TagClass.Application, 2);

        /// <summary>
        /// An alias for <see cref="Gauge32"/> used for the <c>Unsigned32</c> syntax.
        /// </summary>
        public readonly static Asn1Tag Unsigned32 = Gauge32;

        /// <summary>
        /// The application tag for the <c>TimeTicks</c> syntax.
        /// </summary>
        public readonly static Asn1Tag TimeTicks = new(TagClass.Application, 3);

        /// <summary>
        /// The application tag for the <c>Opaque</c> syntax.
        /// </summary>
        public readonly static Asn1Tag Opaque = new(TagClass.Application, 4);

        /// <summary>
        /// The application tag for the <c>Counter64</c> syntax.
        /// </summary>
        public readonly static Asn1Tag Counter64 = new(TagClass.Application, 6);

        /// <summary>
        /// The universal ASN.1 INTEGER tag used for <c>Integer32</c>.
        /// </summary>
        public readonly static Asn1Tag Integer32 = Asn1Tag.Integer;

        /// <summary>
        /// An alias for <see cref="Counter64"/> used for unsigned 64-bit values.
        /// </summary>
        public readonly static Asn1Tag Unsigned64 = Counter64;

        /// <summary>
        /// The application tag for the opaque floating-point extension (<c>Float</c>).
        /// </summary>
        public readonly static Asn1Tag Float = new(TagClass.Application, 8);

        /// <summary>
        /// The application tag for the opaque floating-point extension (<c>Double</c>).
        /// </summary>
        public readonly static Asn1Tag Double = new(TagClass.Application, 9);

        #region draft-perkins-opaque-01.txt Opaque types
        /// <summary>
        /// The ASN.1 context-specific class bit mask used by opaque extension tags.
        /// </summary>
        public const byte AsnContext = (byte)TagClass.ContextSpecific; //0x80, 128

        /// <summary>
        /// The low-bit extension marker for multi-octet tag identifiers.
        /// </summary>
        public const byte AsnExtensionId = 0x1F; // 31

        /// <summary>
        /// First octet of the tag
        /// </summary>
        public const byte AsnOpaqueTag1 = AsnContext | AsnExtensionId; // 159

        /// <summary>
        /// Base value for the second octet of the tag
        /// the second octet is the value for the tag
        /// </summary>
        public const byte AsnOpaqueTag2 = 0x30; // 48

        /// <summary>
        /// Second octet of tag for unions
        /// </summary>
        public const byte AsnOpaqueTag2U = 0x2f; // 47

        // 0x40
        private const byte Application = (byte)TagClass.Application;
        private const byte _counter64 = 6;
        private const byte _float = 8;
        private const byte _double = 9;
        private const byte _integer64 = 10;
        private const byte _unsigned64 = 11;

        /// <summary>
        /// The second-octet tag value for opaque Counter64 extension values.
        /// </summary>
        public const int AsnOpaqueCounter64TagValue // 48 + (64 | 6) = 118
            = AsnOpaqueTag2 + (Application | _counter64);

        /// <summary>
        /// The second-octet tag value for opaque Float extension values.
        /// </summary>
        public const int AsnOpaqueFloatTagValue     // 48 + (64 | 6) = 120
            = AsnOpaqueTag2 + (Application | _float);

        /// <summary>
        /// The second-octet tag value for opaque Double extension values.
        /// </summary>
        public const int AsnOpaqueDoubleTagValue    // 48 + (64 | 6) = 121
            = AsnOpaqueTag2 + (Application | _double);

        /// <summary>
        /// The second-octet tag value for opaque signed 64-bit extension values.
        /// </summary>
        public const int AsnOpaqueInteger64TagValue
            = AsnOpaqueTag2 + (Application | _integer64);

        /// <summary>
        /// The second-octet tag value for opaque unsigned 64-bit extension values.
        /// </summary>
        public const int AsnOpaqueUnsigned64TagValue
            = AsnOpaqueTag2 + (Application | _unsigned64);

        /// <summary>
        /// The ASN.1 tag used to encode opaque Counter64 extension values.
        /// </summary>
        public readonly static Asn1Tag OpaqueCounter64 =
            new(TagClass.ContextSpecific, AsnOpaqueCounter64TagValue);

        /// <summary>
        /// The ASN.1 tag used to encode opaque Float extension values.
        /// </summary>
        public readonly static Asn1Tag OpaqueFloat =
            new(TagClass.ContextSpecific, AsnOpaqueFloatTagValue);

        /// <summary>
        /// The ASN.1 tag used to encode opaque Double extension values.
        /// </summary>
        public readonly static Asn1Tag OpaqueDouble =
            new(TagClass.ContextSpecific, AsnOpaqueDoubleTagValue);

        /// <summary>
        /// The ASN.1 tag used to encode opaque signed 64-bit extension values.
        /// </summary>
        public readonly static Asn1Tag OpaqueInteger64 =
            new(TagClass.ContextSpecific, AsnOpaqueInteger64TagValue);

        /// <summary>
        /// The ASN.1 tag used to encode opaque unsigned 64-bit extension values.
        /// </summary>
        public readonly static Asn1Tag OpaqueUnsigned64 =
            new(TagClass.ContextSpecific, AsnOpaqueUnsigned64TagValue);
        #endregion
    }
}
