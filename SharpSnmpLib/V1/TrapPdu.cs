using DotNetSnmp.Asn1.Serialization;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using System.Formats.Asn1;
using System.Net;

namespace DotNetSnmp.Protocol.V1
{
    public class TrapPdu : Pdu
    {
        public ObjectIdentifier Enterprise { get; set; }
        public IPAddress? AgentAddress { get; set; }
        public int GenericTrap { get; set; }
        public int SpecificTrap { get; set; }
        public uint TimeStamp { get; set; }

        public override Asn1Tag PduType => SnmpAsnTags.TrapMsg;

        public override void WriteTo(AsnWriter writer)
        {
            using (_ = writer.PushSequence(tag: PduType))
            {
                Enterprise.WriteTo(writer);
                writer.WriteOctetString(AgentAddress!.GetAddressBytes());
                writer.WriteInteger(GenericTrap);
                writer.WriteInteger(SpecificTrap);
                writer.WriteInteger(TimeStamp);
                if (VariableBindings != null)
                {
                    VariableBindings.WriteTo(writer);
                }
            }
        }

        public static TrapPdu ReadFrom(AsnReader reader)
        {
            var pduReader = reader.ReadSequence(SnmpAsnTags.TrapMsg);

            var enterprise = ObjectIdentifier.ReadFrom(pduReader);
            var agentAddress = new IPAddress(pduReader.ReadOctetString());
            pduReader.TryReadInt32(out var genericTrap);
            pduReader.TryReadInt32(out var specificTrap);
            var timeStamp = (uint)pduReader.ReadInteger();
            var variableBindings = VarBindList.ReadFrom(pduReader);

            return new TrapPdu
            {
                Enterprise = enterprise,
                AgentAddress = agentAddress,
                GenericTrap = genericTrap,
                SpecificTrap = specificTrap,
                TimeStamp = timeStamp,
                VariableBindings = variableBindings
            };
        }
    }
}
