using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Report PDU.
    /// </summary>
    public class ReportPdu : Pdu
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ReportPdu"/>.
        /// </summary>
        public ReportPdu()
        {
        }

        /// <summary>
        /// Initializes a legacy-compatible instance of <see cref="ReportPdu"/>.
        /// </summary>
        public ReportPdu(int requestId, ErrorCode errorStatus, int errorIndex, IList<Variable> variables)
        {
            RequestId = requestId;
            ErrorStatus = (int)errorStatus;
            ErrorIndex = errorIndex;
            VariableBindings = new VarBindList(variables.ToArray());
        }

        /// <summary>
        /// Represents report Msg.
        /// </summary>
        public override Asn1Tag PduType => SnmpAsnTags.ReportMsg;

        /// <inheritdoc/>
        public override void WriteTo(AsnWriter writer)
        {
            using (_ = writer.PushSequence(tag: SnmpAsnTags.ReportMsg))
            {
                writer.WriteInteger(RequestId);
                writer.WriteInteger(ErrorStatus.Value);
                writer.WriteInteger(ErrorIndex);
                if (VariableBindings != null)
                {
                    VariableBindings.WriteTo(writer);
                }
            }
        }

        /// <summary>
        /// Reads a value from an ASN.1 reader.
        /// </summary>
        public static ReportPdu ReadFrom(AsnReader reader)
        {
            var seq = reader.ReadSequence(
                expectedTag: SnmpAsnTags.ReportMsg);

            seq.TryReadInt32(out var requestId);
            seq.TryReadInt32(out var errorStatus);
            seq.TryReadInt32(out var errorIndex);

            var bindings = VarBindList.ReadFrom(seq);

            return new ReportPdu
            {
                RequestId = requestId,
                ErrorStatus = errorStatus,
                ErrorIndex = errorIndex,
                VariableBindings = bindings
            };
        }
    }
}
