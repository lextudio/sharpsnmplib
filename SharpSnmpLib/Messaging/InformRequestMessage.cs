using DotNetSnmp.Asn1;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V1;
using DotNetSnmp.Protocol.V2;
using DotNetSnmp.Protocol.V3.Security.Privacy;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Legacy compatibility wrapper for SNMP INFORM request messages.
/// </summary>
public sealed class InformRequestMessage : ISnmpMessage
{
    private readonly ISnmpMessage _message;
    private readonly IList<Variable> _variables;

    /// <summary>
    /// Creates a <see cref="InformRequestMessage"/> with all contents (v1/v2c).
    /// </summary>
    /// <param name="requestId">The request id.</param>
    /// <param name="version">Protocol version.</param>
    /// <param name="community">Community name.</param>
    /// <param name="enterprise">Enterprise.</param>
    /// <param name="time">Time ticks.</param>
    /// <param name="variables">Variables.</param>
    [System.CLSCompliant(false)]
    public InformRequestMessage(
        int requestId,
        VersionCode version,
        OctetString community,
        ObjectIdentifier enterprise,
        uint time,
        IList<Variable> variables)
    {
        if (version == VersionCode.V3)
        {
            throw new ArgumentException("Only v1 and v2c are supported by this constructor.", nameof(version));
        }

        _variables = variables ?? throw new ArgumentNullException(nameof(variables));
        Version = version;
        Enterprise = enterprise;
        TimeStamp = time;

        var pdu = new InformRequestPdu
        {
            RequestId = requestId,
            Enterprise = enterprise,
            TimeStamp = time,
            VariableBindings = new VarBindList(variables.ToArray())
        };

        _message = new SnmpV2Message
        {
            Community = community,
            Scope = pdu
        };
    }

    /// <summary>
    /// Creates a <see cref="InformRequestMessage"/> from a parsed message.
    /// </summary>
    internal InformRequestMessage(ISnmpMessage message)
    {
        _message = message ?? throw new ArgumentNullException(nameof(message));
        Version = message.ProtocolVersion;

        if (message.Scope?.Pdu is InformRequestPdu informPdu)
        {
            Enterprise = informPdu.Enterprise;
            TimeStamp = informPdu.TimeStamp;
            _variables = informPdu.VariableBindings?.ToList() ?? new List<Variable>();
        }
        else
        {
            Enterprise = new ObjectIdentifier("0.0");
            TimeStamp = 0;
            _variables = message.Scope?.VariableBindings?.ToList() ?? new List<Variable>();
        }
    }

    /// <summary>
    /// Protocol version.
    /// </summary>
    public VersionCode Version { get; }

    /// <summary>
    /// Enterprise.
    /// </summary>
    public ObjectIdentifier Enterprise { get; }

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
        return $"InformRequestMessage: version={Version}; enterprise={Enterprise}; vars={_variables.Count}";
    }
}
