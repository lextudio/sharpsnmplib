using DotNetSnmp.Asn1.Serialization;
using DotNetSnmp.Asn1.SyntaxObjects;
using System.Formats.Asn1;

namespace DotNetSnmp.Common.Definitions
{
    /// <summary>
    /// The base class for all SNMP PDU types.
    /// </summary>
    /// <remarks>
    /// The PDU is the protocol data unit. It is the block of data that is
    /// exchanged between the SNMP manager and agent. The PDU contains
    /// the request ID, error status, error index, and variable bindings.
    /// 
    /// Here we treat it as a simplified scope as SNMP v3 scopes are more
    /// complex than the PDU itself.
    /// </remarks>
    public abstract class Pdu : IScope
    {
        /// <summary>
        /// Gets pdu Type.
        /// </summary>
        public abstract Asn1Tag PduType { get; }

        /// <summary>
        /// Gets request Id.
        /// </summary>
        public int RequestId { get; set; } = 0;

        /// <summary>
        /// Gets error Status.
        /// </summary>
        public ErrorCode ErrorStatus { get; set; } = 0;

        /// <summary>
        /// Gets error Index.
        /// </summary>
        public int ErrorIndex { get; set; } = 0;

        /// <summary>
        /// Gets variable Bindings.
        /// </summary>
        public VarBindList? VariableBindings { get; set; }

        /// <summary>
        /// Gets a value indicating whether variable bindings are present.
        /// </summary>
        public bool HasData => VariableBindings?.IsEmpty == false;

        /// <inheritdoc/>
        Pdu IScope.Pdu => this;

        /// <summary>
        /// Returns true if this PDU might trigger a REPORT message.
        /// </summary>
        /// <returns></returns>
        public bool IsConfirmed()
        {
            return PduType != SnmpAsnTags.ReportMsg
                && PduType != SnmpAsnTags.GetResponseMsg
                && PduType != SnmpAsnTags.TrapMsg
                && PduType != SnmpAsnTags.Trap2Msg;
        }

        /// <summary>
        /// Returns true if this PDU is a response PDU.
        /// </summary>
        public bool IsResponse() =>
            PduType == SnmpAsnTags.GetResponseMsg;

        /// <inheritdoc/>
        public abstract void WriteTo(AsnWriter writer);

        /// <summary>
        /// Throws an exception if the ErrorStatus property for the SNMP response PDU is != 0.
        /// </summary>
        /// <see cref="ErrorCode"/>
        /// <exception cref="SnmpRequestException"></exception>
        public void EnsureNoError()
        {
            if (ErrorStatus != ErrorCode.NoError)
            {
                throw new SnmpRequestException(ErrorStatus, ErrorIndex);
            }
        }

        /// <summary>
        /// Copies the PDU data to another PDU.
        /// </summary>
        /// <param name="pdu">The PDU to copy to.</param>
        /// <remarks>
        /// This is used to copy the PDU data from one PDU to another.
        /// </remarks>
        protected void CopyTo(Pdu pdu)
        {
            pdu.VariableBindings = new(VariableBindings!);
            pdu.RequestId = RequestId;
            pdu.ErrorIndex = ErrorIndex;
            pdu.ErrorStatus = ErrorStatus;
        }
    }
}
