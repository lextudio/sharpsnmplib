using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Represents the GetBulkRequestPdu type.
    /// </summary>
    public class GetBulkRequestPdu : Pdu
    {
        /// <summary>
        /// how many OIDs in the request should be treated as GET request variables
        /// </summary>
        public int NonRepeaters { get; set; } = 0;

        /// <summary>
        /// how many GET_NEXT operations to perform on each variable
        /// </summary>
        public int MaxRepetitions { get; set; } = 0;

        /// <summary>
        /// Represents bulk Msg.
        /// </summary>
        public override Asn1Tag PduType => SnmpAsnTags.BulkMsg;

        /// <summary>
        /// Initializes a new instance of GetBulkRequestPdu.
        /// </summary>
        public GetBulkRequestPdu() { }

        /// <summary>
        /// Initializes a new instance of GetBulkRequestPdu (legacy compatibility overload).
        /// </summary>
        public GetBulkRequestPdu(int requestId, int nonRepeaters, int maxRepetitions, IList<Variable> variables)
        {
            RequestId = requestId;
            NonRepeaters = nonRepeaters;
            MaxRepetitions = maxRepetitions;
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
        public static GetBulkRequestPdu ReadFrom(AsnReader reader)
        {
            var seq = reader.ReadSequence(
                expectedTag: SnmpAsnTags.BulkMsg);

            seq.TryReadInt32(out var requestId);
            seq.TryReadInt32(out var nonRepeaters);
            seq.TryReadInt32(out var maxRepetitions);

            var bindings = VarBindList.ReadFrom(seq);

            return new GetBulkRequestPdu
            {
                RequestId = requestId,
                NonRepeaters = nonRepeaters,
                MaxRepetitions = maxRepetitions,
                VariableBindings = bindings
            };
        }

        /// <inheritdoc/>
        public override void WriteTo(AsnWriter writer)
        {
            using (_ = writer.PushSequence(tag: SnmpAsnTags.BulkMsg))
            {
                writer.WriteInteger(RequestId);
                writer.WriteInteger(NonRepeaters);
                writer.WriteInteger(MaxRepetitions);

                if (VariableBindings != null)
                {
                    VariableBindings.WriteTo(writer);
                }
            }
        }
    }
}
