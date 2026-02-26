using DotNetSnmp.Asn1.Serialization;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Protocol.V3;
using System.Text;

namespace DotNetSnmp.Common.Definitions
{
    /// <summary>
    /// Defines the contract for IScope.
    /// </summary>
    /// <remarks>
    /// The scope of an SNMP PDU contains the request ID, variable bindings, and PDU type.
    /// It represents the payload portion of an SNMP message that contains the actual operation
    /// and data being requested or responded to. IScope implementations are serializable to ASN.1
    /// format for network transmission.
    /// </remarks>
    public interface IScope : IAsnSerializable
    {
        /// <summary>
        /// Gets request Id.
        /// </summary>
        /// <value>
        /// An integer value that uniquely identifies this SNMP request or response.
        /// </value>
        /// <remarks>
        /// The request ID is used to match responses with their corresponding requests
        /// when multiple requests are outstanding.
        /// </remarks>
        int RequestId { get; }

        /// <summary>
        /// Gets variable Bindings.
        /// </summary>
        /// <value>
        /// A collection of OID-value pairs representing the variables being requested,
        /// set, or returned in a response.
        /// </value>
        /// <remarks>
        /// Variable bindings are the fundamental data elements in SNMP operations, consisting
        /// of an Object Identifier (OID) and its corresponding value or status.
        /// </remarks>
        VarBindList? VariableBindings { get; set; }

        /// <summary>
        /// Gets the protocol data unit (PDU).
        /// </summary>
        /// <value>
        /// An enumeration value indicating the type of PDU (e.g., Get, GetNext, Set, Response).
        /// </value>
        /// <remarks>
        /// The PDU type determines how the SNMP operation should be interpreted by the recipient.
        /// </remarks>
        Pdu Pdu { get; }

        /// <summary>
        /// Determines whether response.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this scope is a response; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method helps distinguish request PDUs from response PDUs without having to
        /// examine the PDU type directly.
        /// </remarks>
        bool IsResponse();

        /// <summary>
        /// Gets context engine id (legacy compatibility member).
        /// </summary>
        OctetString ContextEngineId
            => this is Scope scoped ? new OctetString(scoped.ContextEngineId.ToArray()) : OctetString.Empty;

        /// <summary>
        /// Gets context name (legacy compatibility member).
        /// </summary>
        OctetString ContextName
            => this is Scope scoped ? new OctetString(Encoding.UTF8.GetBytes(scoped.ContextName)) : OctetString.Empty;

        /// <summary>
        /// Gets serialized scope data for a target protocol version (legacy compatibility member).
        /// </summary>
        IAsnSerializable GetData(VersionCode version)
        {
            return version == VersionCode.V3 ? this : Pdu;
        }
    }
}
