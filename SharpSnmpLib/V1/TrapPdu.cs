using Lextm.SharpSnmpLib;
using System.Formats.Asn1;
using System.Net;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Represents the TrapPdu type.
    /// </summary>
    public class TrapPdu : Pdu
    {
        /// <summary>
        /// Gets enterprise.
        /// </summary>
        public ObjectIdentifier Enterprise { get; set; }
        /// <summary>
        /// Gets agent Address.
        /// </summary>
        public IPAddress? AgentAddress { get; set; }
        /// <summary>
        /// Gets generic Trap.
        /// </summary>
        public int GenericTrap { get; set; }
        /// <summary>
        /// Gets specific Trap.
        /// </summary>
        public int SpecificTrap { get; set; }
        /// <summary>
        /// Gets time Stamp.
        /// </summary>
        [System.CLSCompliant(false)]
        public uint TimeStamp { get; set; }

        /// <summary>
        /// Represents trap Msg.
        /// </summary>
        public override Asn1Tag PduType => SnmpAsnTags.TrapMsg;

        /// <inheritdoc/>
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

        /// <summary>
        /// Reads a value from an ASN.1 reader.
        /// </summary>
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
