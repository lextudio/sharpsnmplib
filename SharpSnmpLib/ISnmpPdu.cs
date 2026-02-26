using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib;

/// <summary>
/// Legacy compatibility abstraction for SNMP PDU data.
/// </summary>
public interface ISnmpPdu
{
    /// <summary>
    /// Gets request id.
    /// </summary>
    Integer32 RequestId { get; }

    /// <summary>
    /// Gets error status.
    /// </summary>
    Integer32 ErrorStatus { get; }

    /// <summary>
    /// Gets error index.
    /// </summary>
    Integer32 ErrorIndex { get; }

    /// <summary>
    /// Gets variable bindings.
    /// </summary>
    IList<Variable> Variables { get; }

    /// <summary>
    /// Gets PDU type code.
    /// </summary>
    SnmpType TypeCode { get; }
}
