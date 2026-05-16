using Lextm.SharpSnmpLib;
using System.Collections.Generic;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// GET request PDU.
    /// </summary>
    public class GetRequestPdu : Pdu
    {
        /// <summary>
        /// Represents get Msg.
        /// </summary>
        public override Asn1Tag PduType => SnmpAsnTags.GetMsg;

        /// <summary>
        /// Initializes a new instance of GetRequestPdu.
        /// </summary>
        public GetRequestPdu()
        {
            ErrorIndex = 0;
            ErrorStatus = (int)ErrorCode.NoError;
        }

        /// <summary>
        /// Initializes a new instance of GetRequestPdu (legacy compatibility overload).
        /// </summary>
        public GetRequestPdu(int requestId, IList<Variable> variables)
            : this()
        {
            RequestId = requestId;
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            var bindings = new VarBindList();
            foreach (var variable in variables)
            {
                bindings.Add(variable);
            }

            VariableBindings = bindings;
        }

        /// <summary>
        /// Returns a string representation.
        /// </summary>
        public override string ToString() => throw new NotImplementedException();

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
        public static GetRequestPdu ReadFrom(AsnReader reader)
        {
            var seq = reader.ReadSequence(
                expectedTag: SnmpAsnTags.GetMsg);

            seq.TryReadInt32(out var requestId);
            seq.TryReadInt32(out var errorStatus);
            seq.TryReadInt32(out var errorIndex);

            var bindings = VarBindList.ReadFrom(seq);

            return new GetRequestPdu
            {
                RequestId = requestId,
                ErrorStatus = errorStatus,
                ErrorIndex = errorIndex,
                VariableBindings = bindings
            };
        }
    }
}
