using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Represents the SnmpV1Message type.
    /// </summary>
    public class SnmpV1Message : ISnmpMessage
    {
        /// <summary>
        /// Represents v1.
        /// </summary>
        public VersionCode ProtocolVersion => VersionCode.V1;

        /// <summary>
        /// Gets community.
        /// </summary>
        public OctetString Community { get; set; }

        /// <summary>
        /// Gets the message scope.
        /// </summary>
        public IScope? Scope { get; init; }

        /// <summary>
        /// Represents pdu.
        /// </summary>
        public Pdu Pdu => Scope!.Pdu;

        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            using (_ = writer.PushSequence())
            {
                // versiom
                writer.WriteInteger((int)VersionCode.V1);

                // community
                writer.WriteOctetString(Community.Octets);

                // pdu
                Scope?.WriteTo(writer);
            }
        }

        /// <summary>
        /// Reads a value from an ASN.1 reader.
        /// </summary>
        public static SnmpV1Message ReadFrom(AsnReader reader)
        {
            var rootSeq = reader.ReadSequence();

            rootSeq.TryReadInt32(out var version);

            if (version != 0)
            {
                throw new SnmpDecodeException(
                    $"Expected version V1(0) found {version}");
            }

            var community = rootSeq.ReadOctetString();

            var pduType = rootSeq.PeekTag();

            Pdu? pdu = null;

            if (pduType == SnmpAsnTags.GetResponseMsg)
            {
                pdu = ResponsePdu.ReadFrom(rootSeq);
            }
            else if (pduType == SnmpAsnTags.GetMsg)
            {
                pdu = GetRequestPdu.ReadFrom(rootSeq);
            }
            else if (pduType == SnmpAsnTags.GetNextMsg)
            {
                pdu = GetNextRequestPdu.ReadFrom(rootSeq);
            }
            else if (pduType == SnmpAsnTags.SetMsg)
            {
                pdu = SetRequestPdu.ReadFrom(rootSeq);
            }
            else if (pduType == SnmpAsnTags.TrapMsg)
            {
                pdu = TrapPdu.ReadFrom(rootSeq);
            }

            return new SnmpV1Message
            {
                Community = new OctetString(community),
                Scope = pdu!
            };
        }
    }
}
