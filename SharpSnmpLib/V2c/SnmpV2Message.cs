using DotNetSnmp.Asn1.Serialization;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V1;
using System.Formats.Asn1;

namespace DotNetSnmp.Protocol.V2
{
    public class SnmpV2Message : ISnmpMessage
    {
        public VersionCode ProtocolVersion => VersionCode.V2;

        public OctetString Community { get; set; }

        public IScope? Scope { get; init; }

        public Pdu Pdu => Scope!.Pdu;

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
