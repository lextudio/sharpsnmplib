using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib;

/// <summary>TRAP v1 PDU (legacy compatibility stub).</summary>
public class TrapV1Pdu : ISnmpData
{
    /// <summary>Initializes a new instance.</summary>
    public TrapV1Pdu(
        ObjectIdentifier enterprise,
        IP agentAddress,
        Integer32 generic,
        Integer32 specific,
        TimeTicks timeStamp,
        IList<Variable> variables)
    {
        Enterprise = enterprise;
        AgentAddress = agentAddress;
        Generic = (GenericCode)generic.Value;
        Specific = specific.Value;
        TimeStamp = timeStamp;
        Variables = variables;
    }

    /// <summary>Initializes a new instance with uint[] OID.</summary>
    [CLSCompliant(false)]
    public TrapV1Pdu(
        uint[] enterprise,
        IP agentAddress,
        Integer32 generic,
        Integer32 specific,
        TimeTicks timeStamp,
        IList<Variable> variables)
        : this(new ObjectIdentifier(enterprise), agentAddress, generic, specific, timeStamp, variables)
    {
    }

    /// <summary>Gets request ID (always 0 for TRAP v1).</summary>
    public Integer32 RequestId => Integer32.Zero;

    /// <summary>Gets error index (always 0).</summary>
    public Integer32 ErrorIndex => Integer32.Zero;

    /// <summary>Gets error status (always 0).</summary>
    public Integer32 ErrorStatus => Integer32.Zero;

    /// <summary>Gets the type code.</summary>
    public SnmpType TypeCode => SnmpType.TrapV1Pdu;

    /// <summary>Gets the enterprise OID.</summary>
    public ObjectIdentifier Enterprise { get; }

    /// <summary>Gets the agent address.</summary>
    public IP AgentAddress { get; }

    /// <summary>Gets the generic trap code.</summary>
    public GenericCode Generic { get; }

    /// <summary>Gets the specific trap code.</summary>
    public int Specific { get; }

    /// <summary>Gets the timestamp.</summary>
    public TimeTicks TimeStamp { get; }

    /// <summary>Gets the variable bindings.</summary>
    public IList<Variable> Variables { get; }

    /// <inheritdoc/>
    public void WriteTo(AsnWriter writer) => throw new NotImplementedException();

    /// <inheritdoc/>
    public override string ToString() => $"TrapV1Pdu: enterprise={Enterprise}";
}
