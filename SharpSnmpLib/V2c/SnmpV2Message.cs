using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Represents the SnmpV2Message type.
    /// </summary>
    public class SnmpV2Message : ISnmpMessage
    {
        /// <summary>
        /// Represents v2.
        /// </summary>
        public VersionCode ProtocolVersion => VersionCode.V2;

        /// <summary>
        /// Gets community.
        /// </summary>
        public OctetString Community { get; set; }

        /// <summary>
        /// Gets the message scope.
        /// </summary>
        public IScope? Scope { get; init; }

        /// <summary>
        /// Cached wire bytes set when the message was parsed from the network; empty when built from scratch.
        /// </summary>
        internal byte[]? RawBytes { get; set; }

        /// <inheritdoc/>
        byte[] ISnmpMessage.ToBytes() => RawBytes ?? AsnSerializableExtensions.Encode(this);

        /// <summary>
        /// Represents pdu.
        /// </summary>
        public Pdu Pdu => Scope!.Pdu;

        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            using (_ = writer.PushSequence())
            {
                // version
                writer.WriteInteger((int)VersionCode.V2);

                // community
                writer.WriteOctetString(Community.Octets);

                // pdu
                Scope?.WriteTo(writer);
            }
        }

        /// <summary>
        /// Reads a value from an ASN.1 reader.
        /// </summary>
        public static SnmpV2Message ReadFrom(AsnReader reader)
        {
            var rootSeq = reader.ReadSequence();

            rootSeq.TryReadInt32(out var version);

            if (version != 1)
            {
                throw new SnmpDecodeException(
                    $"Expected version V2c(1) found {version}");
            }

            var community = rootSeq.ReadOctetString();

            var pduType = rootSeq.PeekTag();

            Pdu pdu;

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
            else if (pduType == SnmpAsnTags.BulkMsg)
            {
                pdu = GetBulkRequestPdu.ReadFrom(rootSeq);
            }
            else if (pduType == SnmpAsnTags.InformMsg)
            {
                pdu = InformRequestPdu.ReadFrom(rootSeq);
            }
            else if (pduType == SnmpAsnTags.SetMsg)
            {
                pdu = SetRequestPdu.ReadFrom(rootSeq);
            }
            else if (pduType == SnmpAsnTags.Trap2Msg)
            {
                pdu = TrapV2Pdu.ReadFrom(rootSeq);
            }
            else
            {
                throw new SnmpDecodeException(
                    $"Unexpected PDU type {pduType} found in V2c message");
            }

            return new SnmpV2Message
            {
                Community = new OctetString(community),
                Scope = pdu!
            };
        }
    }
}
