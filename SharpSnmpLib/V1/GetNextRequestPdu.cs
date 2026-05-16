using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// GETNEXT request PDU.
    /// </summary>
    public class GetNextRequestPdu : Pdu
    {
        /// <summary>
        /// Represents get Next Msg.
        /// </summary>
        public override Asn1Tag PduType => SnmpAsnTags.GetNextMsg;

        /// <summary>
        /// Initializes a new instance of GetNextRequestPdu.
        /// </summary>
        public GetNextRequestPdu()
        {
            ErrorIndex = 0;
            ErrorStatus = (int)ErrorCode.NoError;
        }

        /// <summary>
        /// Initializes a new instance of GetNextRequestPdu (legacy compatibility overload).
        /// </summary>
        public GetNextRequestPdu(int requestId, IList<Variable> variables)
            : this()
        {
            RequestId = requestId;
            if (variables == null) throw new ArgumentNullException(nameof(variables));
            var bindings = new VarBindList();
            foreach (var variable in variables) bindings.Add(variable);
            VariableBindings = bindings;
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        public override string ToString() => throw new NotImplementedException();

        /// <summary>
        /// Reads a value from an ASN.1 reader.
        /// </summary>
        public static GetNextRequestPdu ReadFrom(AsnReader reader)
        {
            var seq = reader.ReadSequence(
                expectedTag: SnmpAsnTags.GetNextMsg);

            seq.TryReadInt32(out var requestId);
            seq.TryReadInt32(out var errorStatus);
            seq.TryReadInt32(out var errorIndex);

            var bindings = VarBindList.ReadFrom(seq);

            return new GetNextRequestPdu
            {
                RequestId = requestId,
                ErrorStatus = errorStatus,
                ErrorIndex = errorIndex,
                VariableBindings = bindings
            };
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
    }
}
