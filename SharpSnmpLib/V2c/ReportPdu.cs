using DotNetSnmp.Asn1.Serialization;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using System.Formats.Asn1;

namespace DotNetSnmp.Protocol.V1
{
    /// <summary>
    /// Report PDU.
    /// </summary>
    public class ReportPdu : Pdu
    {
        /// <summary>
        /// Represents report Msg.
        /// </summary>
        public override Asn1Tag PduType => SnmpAsnTags.ReportMsg;

        /// <inheritdoc/>
        public override void WriteTo(AsnWriter writer)
        {
            throw new NotImplementedException();
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
                ErrorStatus = (ErrorCode)errorStatus,
                ErrorIndex = errorIndex,
                VariableBindings = bindings
            };
        }
    }
}
