using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Represents the ResponsePdu type.
    /// </summary>
    public class ResponsePdu : Pdu
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ResponsePdu"/>.
        /// </summary>
        public ResponsePdu()
        {
        }

        /// <summary>
        /// Initializes a legacy-compatible instance of <see cref="ResponsePdu"/>.
        /// </summary>
        public ResponsePdu(int requestId, ErrorCode errorStatus, int errorIndex, IList<Variable> variables)
        {
            RequestId = requestId;
            ErrorStatus = (int)errorStatus;
            ErrorIndex = errorIndex;
            VariableBindings = new VarBindList(variables.ToArray());
        }

        /// <summary>
        /// Represents get Response Msg.
        /// </summary>
        public override Asn1Tag PduType => SnmpAsnTags.GetResponseMsg;

        /// <inheritdoc/>
        public override void WriteTo(AsnWriter writer)
        {
            using (_ = writer.PushSequence(tag: SnmpAsnTags.GetResponseMsg))
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

        /// <summary>Returns a string representation.</summary>
        public override string ToString()
        {
            return $"ResponsePdu: requestId={RequestId}; errorStatus={ErrorStatus}; errorIndex={ErrorIndex}";
        }

        /// <summary>
        /// Reads a value from an ASN.1 reader.
        /// </summary>
        public static ResponsePdu ReadFrom(AsnReader reader)
        {
            var seq = reader.ReadSequence(
                expectedTag: SnmpAsnTags.GetResponseMsg);

            seq.TryReadInt32(out var requestId);
            seq.TryReadInt32(out var errorStatus);
            seq.TryReadInt32(out var errorIndex);

            var bindings = VarBindList.ReadFrom(seq);

            return new ResponsePdu
            {
                RequestId = requestId,
                ErrorStatus = errorStatus,
                ErrorIndex = errorIndex,
                VariableBindings = bindings
            };
        }
    }
}
