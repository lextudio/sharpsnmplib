using DotNetSnmp.Asn1.Serialization;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using System.Formats.Asn1;

namespace DotNetSnmp.Protocol.V1
{
    public class SnmpV1Message : ISnmpMessage
    {
        public VersionCode ProtocolVersion => VersionCode.V1;

        public OctetString Community { get; set; }

        public IScope? Scope { get; init; }

        public Pdu Pdu => Scope!.Pdu;

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
