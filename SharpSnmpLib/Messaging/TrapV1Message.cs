using System.Net;
using DotNetSnmp.Asn1;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V1;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Legacy compatibility wrapper for SNMP TRAP v1 messages.
/// </summary>
public sealed class TrapV1Message : ISnmpMessage
{
    private readonly SnmpV1Message _message;
    private readonly TrapPdu _trapPdu;
    private readonly IList<Variable> _variables;

    /// <summary>
    /// Creates a <see cref="TrapV1Message"/> with all content.
    /// </summary>
    /// <param name="version">Protocol version.</param>
    /// <param name="agent">Agent address.</param>
    /// <param name="community">Community name.</param>
    /// <param name="enterprise">Enterprise.</param>
    /// <param name="generic">Generic code.</param>
    /// <param name="specific">Specific code.</param>
    /// <param name="time">Time stamp.</param>
    /// <param name="variables">Variables.</param>
    [System.CLSCompliant(false)]
    public TrapV1Message(
        VersionCode version,
        IPAddress agent,
        OctetString community,
        ObjectIdentifier enterprise,
        GenericCode generic,
        int specific,
        uint time,
        IList<Variable> variables)
    {
        if (version != VersionCode.V1)
        {
            throw new ArgumentException("TRAP v1 only supports SNMP v1.", nameof(version));
        }

        _variables = variables ?? throw new ArgumentNullException(nameof(variables));
        Enterprise = enterprise;
        AgentAddress = agent ?? throw new ArgumentNullException(nameof(agent));
        Generic = generic;
        Specific = specific;
        TimeStamp = time;

        _trapPdu = new TrapPdu
        {
            Enterprise = enterprise,
            AgentAddress = agent,
            GenericTrap = (int)generic,
            SpecificTrap = specific,
            TimeStamp = time,
            VariableBindings = new VarBindList(variables.ToArray())
        };

        _message = new SnmpV1Message
        {
            Community = community,
            Scope = _trapPdu
        };
    }

    /// <summary>
    /// Creates a <see cref="TrapV1Message"/> from a parsed v1 message with a trap PDU.
    /// </summary>
    internal TrapV1Message(SnmpV1Message message)
    {
        _message = message ?? throw new ArgumentNullException(nameof(message));

        if (message.Pdu is not TrapPdu trapPdu)
        {
            throw new ArgumentException("The message does not contain a TRAP v1 PDU.", nameof(message));
        }

        _trapPdu = trapPdu;
        Enterprise = trapPdu.Enterprise;
        AgentAddress = trapPdu.AgentAddress ?? IPAddress.Any;
        Generic = (GenericCode)trapPdu.GenericTrap;
        Specific = trapPdu.SpecificTrap;
        TimeStamp = trapPdu.TimeStamp;
        _variables = trapPdu.VariableBindings?.ToList() ?? new List<Variable>();
    }

    /// <summary>
    /// Protocol version.
    /// </summary>
    public VersionCode Version => VersionCode.V1;

    /// <summary>
    /// Enterprise.
    /// </summary>
    public ObjectIdentifier Enterprise { get; }

    /// <summary>
    /// Agent address.
    /// </summary>
    public IPAddress AgentAddress { get; }

    /// <summary>
    /// Generic code.
    /// </summary>
    public GenericCode Generic { get; }

    /// <summary>
    /// Specific code.
    /// </summary>
    public int Specific { get; }

    /// <summary>
    /// Time stamp.
    /// </summary>
    [System.CLSCompliant(false)]
    public uint TimeStamp { get; }

    /// <summary>
    /// Gets the variable bindings.
    /// </summary>
    public IList<Variable> Variables()
    {
        return _variables;
    }

    /// <inheritdoc/>
    public VersionCode ProtocolVersion => _message.ProtocolVersion;

    /// <inheritdoc/>
    public IScope? Scope => _message.Scope;

    /// <inheritdoc/>
    public void WriteTo(System.Formats.Asn1.AsnWriter writer)
    {
        _message.WriteTo(writer);
    }

    /// <summary>
    /// Serializes the message to a byte array.
    /// </summary>
    public byte[] ToBytes()
    {
        return _message.Encode();
    }

    /// <summary>
    /// Returns a string representation.
    /// </summary>
    public override string ToString()
    {
        return $"TrapV1Message: enterprise={Enterprise}; generic={Generic}; specific={Specific}; vars={_variables.Count}";
    }
}
