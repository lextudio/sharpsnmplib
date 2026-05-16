using Lextm.SharpSnmpLib;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;

namespace Lextm.SharpSnmpLib
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
        public Integer32 RequestId { get; set; } = Integer32.Zero;

        /// <summary>
        /// Gets error Status.
        /// </summary>
        public Integer32 ErrorStatus { get; set; } = Integer32.Zero;

        /// <summary>
        /// Gets error Index.
        /// </summary>
        public Integer32 ErrorIndex { get; set; } = Integer32.Zero;

        /// <summary>
        /// Gets variable Bindings.
        /// </summary>
        public VarBindList? VariableBindings { get; set; }

        /// <summary>
        /// Legacy alias for PDU type code.
        /// </summary>
        public SnmpType TypeCode =>
            PduType == SnmpAsnTags.GetMsg ? SnmpType.GetRequestPdu :
            PduType == SnmpAsnTags.GetNextMsg ? SnmpType.GetNextRequestPdu :
            PduType == SnmpAsnTags.GetResponseMsg ? SnmpType.ResponsePdu :
            PduType == SnmpAsnTags.SetMsg ? SnmpType.SetRequestPdu :
            PduType == SnmpAsnTags.TrapMsg ? SnmpType.TrapV1Pdu :
            PduType == SnmpAsnTags.BulkMsg ? SnmpType.GetBulkRequestPdu :
            PduType == SnmpAsnTags.InformMsg ? SnmpType.InformRequestPdu :
            PduType == SnmpAsnTags.Trap2Msg ? SnmpType.TrapV2Pdu :
            PduType == SnmpAsnTags.ReportMsg ? SnmpType.ReportPdu :
            SnmpType.Unknown;

        /// <summary>
        /// Legacy alias for variable bindings.
        /// </summary>
        public IList<Variable> Variables => VariableBindings?.ToList() ?? new List<Variable>();

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
            if (ErrorStatus.Value != (int)ErrorCode.NoError)
            {
                throw new SnmpRequestException((ErrorCode)ErrorStatus.Value, ErrorIndex.Value);
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
