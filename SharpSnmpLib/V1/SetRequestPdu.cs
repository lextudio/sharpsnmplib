using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// SET request PDU.
    /// </summary>
    public class SetRequestPdu : Pdu
    {
        /// <summary>
        /// Represents set Msg.
        /// </summary>
        public override Asn1Tag PduType => SnmpAsnTags.SetMsg;

        /// <summary>
        /// Initializes a new instance of SetRequestPdu.
        /// </summary>
        public SetRequestPdu()
        {
            ErrorIndex = 0;
            ErrorStatus = (int)ErrorCode.NoError;
        }

        /// <summary>Initializes a new instance with request ID and variables.</summary>
        public SetRequestPdu(int requestId, IList<Variable> variables)
        {
            RequestId = requestId;
            ErrorIndex = 0;
            ErrorStatus = (int)ErrorCode.NoError;
            VariableBindings = new VarBindList(variables.ToArray());
        }

        /// <summary>Returns a string representation.</summary>
        public override string ToString()
        {
            return $"SetRequestPdu: requestId={RequestId}; vars={VariableBindings?.Count() ?? 0}";
        }

        /// <inheritdoc/>
        public override void WriteTo(AsnWriter writer)
        {
            using (_ = writer.PushSequence(tag: PduType))
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
        public static SetRequestPdu ReadFrom(AsnReader reader)
        {
            var seq = reader.ReadSequence(
                expectedTag: SnmpAsnTags.SetMsg);

            seq.TryReadInt32(out var requestId);
            seq.TryReadInt32(out var errorStatus);
            seq.TryReadInt32(out var errorIndex);

            var bindings = VarBindList.ReadFrom(seq);

            return new SetRequestPdu
            {
                RequestId = requestId,
                ErrorStatus = errorStatus,
                ErrorIndex = errorIndex,
                VariableBindings = bindings
            };
        }
    }
}
