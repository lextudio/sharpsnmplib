using DotNetSnmp.Asn1.Serialization;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V1;
using DotNetSnmp.Protocol.V2;
using Lextm.SharpSnmpLib;
using System.Formats.Asn1;
using System.Text;

namespace DotNetSnmp.Protocol.V3
{
    /// <summary>
    /// Represents an SNMP v3 scoped PDU, which is a block of data containing a ContextEngineId,
    /// a ContextName, and a PDU.
    /// </summary>
    /// <remarks>
    /// The Scope class implements the IScope interface for SNMP v3 messages. In SNMPv3,
    /// the scoped PDU provides contextual information (engine ID and context name)
    /// along with the actual protocol data unit, allowing for more fine-grained access control
    /// and multiple independent SNMP entities within a single device.
    /// </remarks>
    public class Scope : IScope
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Scope"/>.
        /// </summary>
        public Scope()
        {
            ContextName = string.Empty;
            Pdu = new GetRequestPdu();
        }

        /// <summary>
        /// Initializes a legacy-compatible instance of <see cref="Scope"/>.
        /// </summary>
        public Scope(OctetString contextEngineId, OctetString contextName, Pdu pdu)
        {
            ContextEngineId = contextEngineId.Octets;
            ContextName = contextName.ToString();
            Pdu = pdu;
        }

        /// <summary>
        /// Gets context Engine Id.
        /// </summary>
        /// <value>
        /// A byte array containing the context engine ID.
        /// </value>
        /// <remarks>
        /// The context engine ID uniquely identifies an SNMP engine within an administrative domain.
        /// It is part of the addressing and message validation process in SNMP v3.
        /// </remarks>
        public ReadOnlyMemory<byte> ContextEngineId { get; set; }

        /// <summary>
        /// Gets context Name.
        /// </summary>
        /// <value>
        /// A string containing the context name.
        /// </value>
        /// <remarks>
        /// The context name identifies a particular context within an SNMP entity.
        /// Different contexts can provide access to different subsets of managed objects.
        /// </remarks>
        public string ContextName { get; set; }

        /// <summary>
        /// Gets the protocol data unit (PDU).
        /// </summary>
        /// <value>
        /// The PDU object representing an SNMP operation.
        /// </value>
        /// <remarks>
        /// The PDU contains the actual SNMP operation (Get, Set, GetNext, etc.) and
        /// the associated variable bindings.
        /// </remarks>
        public Pdu Pdu { get; set; }

        /// <summary>
        /// Legacy type code compatibility for scoped PDUs.
        /// </summary>
        public SnmpType TypeCode => SnmpType.Sequence;

        /// <inheritdoc/>
        public int RequestId => Pdu.RequestId;

        /// <inheritdoc/>
        public VarBindList? VariableBindings
        {
            get { return Pdu.VariableBindings; }
            set { Pdu.VariableBindings = value; }
        }

        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            using (_ = writer.PushSequence())
            {
                var enc = OctetString.DefaultEncoding;

                writer.WriteOctetString(ContextEngineId.Span);

                writer.WriteOctetString(
                    ContextName.GetBytesSpanOrDefault(enc));

                Pdu.WriteTo(writer);
            }
        }

        /// <summary>
        /// Deserializes a Scope object from an ASN.1 encoded representation.
        /// </summary>
        /// <param name="reader">The ASN.1 reader containing the encoded scope data.</param>
        /// <returns>A new instance of the Scope class populated with the deserialized data.</returns>
        /// <exception cref="AsnContentException">Thrown when an unexpected PDU type is encountered.</exception>
        /// <exception cref="NotImplementedException">Thrown when a PDU type is recognized but not yet implemented.</exception>
        /// <remarks>
        /// This method handles the deserialization of various PDU types within the scope
        /// and constructs the appropriate PDU object based on the detected type.
        /// </remarks>
        public static Scope ReadFrom(AsnReader reader)
        {
            var seq = reader.ReadSequence();

            var ctxEngineId = seq.ReadOctetString();

            var ctxName = seq.ReadOctetString();

            var encoding = OctetString.DefaultEncoding;

            var pduType = seq.PeekTag();

            Pdu? pdu = null;
            if (pduType == SnmpAsnTags.GetResponseMsg)
            {
                pdu = ResponsePdu.ReadFrom(seq);
            }
            else if (pduType == SnmpAsnTags.GetMsg)
            {
                pdu = GetRequestPdu.ReadFrom(seq);
            }
            else if (pduType == SnmpAsnTags.GetNextMsg)
            {
                pdu = GetNextRequestPdu.ReadFrom(seq);
            }
            else if (pduType == SnmpAsnTags.SetMsg)
            {
                pdu = SetRequestPdu.ReadFrom(seq);
            }
            else if (pduType == SnmpAsnTags.BulkMsg)
            {
                pdu = GetBulkRequestPdu.ReadFrom(seq);
            }
            else if (pduType == SnmpAsnTags.InformMsg)
            {
                pdu = InformRequestPdu.ReadFrom(seq);
            }
            else if (pduType == SnmpAsnTags.Trap2Msg)
            {
                pdu = TrapV2Pdu.ReadFrom(seq);
            }
            else if (pduType == SnmpAsnTags.ReportMsg)
            {
                pdu = ReportPdu.ReadFrom(seq);
            }
            else
            {
                throw new AsnContentException(
                    $"Unexpected PDU type: {pduType}");
            }

            return new()
            {
                ContextEngineId = ctxEngineId,
                ContextName = encoding.GetString(ctxName),
                Pdu = pdu
            };
        }

        /// <inheritdoc/>
        public bool IsResponse()
        {
            return Pdu.IsResponse();
        }

        /// <summary>
        /// Gets serialized scope data for the specified protocol version (legacy compatibility member).
        /// </summary>
        public IAsnSerializable GetData(VersionCode version)
        {
            return version == VersionCode.V3 ? this : Pdu;
        }
    }
}
