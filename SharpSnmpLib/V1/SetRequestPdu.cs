using DotNetSnmp.Asn1.Serialization;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using System.Formats.Asn1;

namespace DotNetSnmp.Protocol.V1
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
            ErrorStatus = ErrorCode.NoError;
        }

        /// <inheritdoc/>
        public override void WriteTo(AsnWriter writer)
        {
            using (_ = writer.PushSequence(tag: PduType))
            {
                writer.WriteInteger(RequestId);
                writer.WriteInteger((int)ErrorStatus);
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
                ErrorStatus = (ErrorCode)errorStatus,
                ErrorIndex = errorIndex,
                VariableBindings = bindings
            };
        }
    }
}
