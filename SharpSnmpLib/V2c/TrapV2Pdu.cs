using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// TRAP v2 PDU.
    /// </summary>
    public class TrapV2Pdu : Pdu
    {
        private const string TimeId = "1.3.6.1.2.1.1.3.0";
        private const string EnterpriseId = "1.3.6.1.6.3.1.1.4.1.0";

        /// <summary>
        /// Initializes a new instance of <see cref="TrapV2Pdu"/>.
        /// </summary>
        public TrapV2Pdu()
        {
            Enterprise = new ObjectIdentifier("0.0");
        }

        /// <summary>
        /// Initializes a legacy-compatible instance of <see cref="TrapV2Pdu"/>.
        /// </summary>
        [System.CLSCompliant(false)]
        public TrapV2Pdu(int requestId, ObjectIdentifier enterprise, uint timeStamp, IList<Variable> variables)
        {
            RequestId = requestId;
            Enterprise = enterprise;
            TimeStamp = timeStamp;
            VariableBindings = new VarBindList(variables.ToArray());
        }

        /// <summary>
        /// Gets time Stamp.
        /// </summary>
        [System.CLSCompliant(false)]
        public uint TimeStamp { get; set; } = 0;

        /// <summary>
        /// Enterprise.
        /// </summary>
        public ObjectIdentifier Enterprise { get; set; }

        /// <summary>
        /// Represents trap2 Msg.
        /// </summary>
        public override Asn1Tag PduType => SnmpAsnTags.Trap2Msg;

        /// <summary>
        /// Reads a value from an ASN.1 reader.
        /// </summary>
        public static TrapV2Pdu ReadFrom(AsnReader reader)
        {
            var seq = reader.ReadSequence(
                expectedTag: SnmpAsnTags.Trap2Msg);

            seq.TryReadInt32(out var requestId);
            seq.TryReadInt32(out _);
            seq.TryReadInt32(out _);

            var bindings = VarBindList.ReadFrom(seq);
            var time = bindings.Remove(0);
            var enterprise = bindings.Remove(0);

            return new TrapV2Pdu
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
            using (_ = writer.PushSequence(tag: SnmpAsnTags.Trap2Msg))
            {
                writer.WriteInteger(RequestId);
                writer.WriteInteger(0);
                writer.WriteInteger(0);

                VariableBindings ??= new VarBindList();
                if (!VariableBindings.Any() || VariableBindings.First().Id.Oid != "1.3.6.1.2.1.1.3.0")
                {
                    VariableBindings.Insert(0, new Variable("1.3.6.1.2.1.1.3.0", new TimeTicks(TimeStamp)));
                }

                if (!VariableBindings.Skip(1).Any() || VariableBindings.ElementAt(1).Id.Oid != "1.3.6.1.6.3.1.1.4.1.0")
                {
                    VariableBindings.Insert(1, new Variable("1.3.6.1.6.3.1.1.4.1.0", Enterprise));
                }

                VariableBindings.WriteTo(writer);
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"TrapV2Pdu: enterprise={Enterprise}; timestamp={TimeStamp}";
        }

        /// <summary>
        /// Decorates variable bindings with timestamp and enterprise metadata.
        /// </summary>
        public IList<Variable> Decorate(IList<Variable> variables)
        {
            var result = new List<Variable>(variables);
            result.Insert(0, new Variable(TimeId, new TimeTicks(TimeStamp)));
            result.Insert(1, new Variable(EnterpriseId, Enterprise));
            return result;
        }
    }
}
