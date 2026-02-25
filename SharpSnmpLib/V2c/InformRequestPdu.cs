using DotNetSnmp.Asn1.Serialization;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using System.Formats.Asn1;

namespace DotNetSnmp.Protocol.V2
{
    /// <summary>
    /// INFORM request PDU.
    /// </summary>
    public class InformRequestPdu : Pdu
    {
        /// <summary>
        /// Gets time Stamp.
        /// </summary>
        [System.CLSCompliant(false)]
        public uint TimeStamp { get; set; } = 0;

        /// <summary>
        /// Gets enterprise.
        /// </summary>
        public ObjectIdentifier Enterprise { get; set; }

        /// <summary>
        /// Represents inform Msg.
        /// </summary>
        public override Asn1Tag PduType => SnmpAsnTags.InformMsg;

        /// <summary>
        /// Reads a value from an ASN.1 reader.
        /// </summary>
        public static InformRequestPdu ReadFrom(AsnReader reader)
        {
            var seq = reader.ReadSequence(
                expectedTag: SnmpAsnTags.InformMsg);

            seq.TryReadInt32(out var requestId);
            seq.TryReadInt32(out var errorStatus);
            seq.TryReadInt32(out var errorIndex);

            var bindings = VarBindList.ReadFrom(seq);
            var time = bindings.Remove(0);
            var enterprise = bindings.Remove(0);

            return new InformRequestPdu
            {
                RequestId = requestId,
                TimeStamp = ((TimeTicks)time.Data).Value,
                Enterprise = (ObjectIdentifier)enterprise.Data,
                VariableBindings = bindings
            };
        }

        /// <inheritdoc/>
        public override void WriteTo(AsnWriter writer)
        {
            using (_ = writer.PushSequence(tag: SnmpAsnTags.InformMsg))
            {
                writer.WriteInteger(RequestId);
                writer.WriteInteger(0);
                writer.WriteInteger(0);

                if (VariableBindings != null)
                {
                    var first = VariableBindings.First();
                    if (first.Id.Oid != "1.3.6.1.2.1.1.3.0")
                    {
                        VariableBindings.Insert(0, new Variable("1.3.6.1.2.1.1.3.0", new TimeTicks(TimeStamp)));
                    }

                    var second = VariableBindings.ElementAt(1);
                    if (second.Id.Oid != "1.3.6.1.6.3.1.1.4.1.0")
                    {
                        VariableBindings.Insert(1, new Variable("1.3.6.1.6.3.1.1.4.1.0", Enterprise));
                    }

                    VariableBindings.WriteTo(writer);
                }
            }
        }
    }
}
