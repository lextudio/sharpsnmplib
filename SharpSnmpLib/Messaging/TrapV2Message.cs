using System.Text;
using System.Net;
using DotNetSnmp.Asn1;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V2;
using DotNetSnmp.Protocol.V3.Security.Privacy;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Legacy compatibility wrapper for TrapV2 message sending.
/// </summary>
public sealed class TrapV2Message : ISnmpMessage
{
    private readonly int _requestId;
    private readonly OctetString _securityName;
    private readonly IPrivacyProvider? _privacy;
    private readonly ISnmpMessage? _message;

    /// <summary>
    /// Initializes a new instance of TrapV2Message.
    /// </summary>
    [System.CLSCompliant(false)]
    public TrapV2Message(
        int requestId,
        VersionCode version,
        OctetString community,
        ObjectIdentifier enterprise,
        uint timestamp,
        IList<Variable> variables)
    {
        _requestId = requestId;
        Version = version;
        _securityName = community;
        Enterprise = enterprise;
        TimeStamp = timestamp;
        _variables = variables ?? throw new ArgumentNullException(nameof(variables));
    }

    /// <summary>
    /// Initializes a new instance of TrapV2Message.
    /// </summary>
    [System.CLSCompliant(false)]
    public TrapV2Message(
        VersionCode version,
        int messageId,
        int requestId,
        OctetString user,
        ObjectIdentifier enterprise,
        uint timestamp,
        IList<Variable> variables,
        IPrivacyProvider privacy,
        int maxMessageSize,
        OctetString engineId,
        int engineBoots,
        int engineTime)
    {
        Version = version;
        MessageId = messageId;
        _requestId = requestId;
        _securityName = user;
        Enterprise = enterprise;
        TimeStamp = timestamp;
        _variables = variables ?? throw new ArgumentNullException(nameof(variables));
        _privacy = privacy ?? throw new ArgumentNullException(nameof(privacy));
        MaxMessageSize = maxMessageSize;
        EngineId = engineId;
        EngineBoots = engineBoots;
        EngineTime = engineTime;
    }

    internal TrapV2Message(ISnmpMessage message)
    {
        _message = message ?? throw new ArgumentNullException(nameof(message));
        Version = message.ProtocolVersion;
        _securityName = message.Parameters.UserName;
        _privacy = null;

        if (message.Scope?.Pdu is not TrapV2Pdu trap)
        {
            throw new ArgumentException("The supplied message is not a trap v2 message.", nameof(message));
        }

        _requestId = trap.RequestId;
        Enterprise = trap.Enterprise;
        TimeStamp = trap.TimeStamp;
        _variables = trap.VariableBindings?.ToList() ?? new List<Variable>();

        if (Version == VersionCode.V3)
        {
            MessageId = message.MessageId();
            MaxMessageSize = message.Header.MaxSize;
            EngineId = message.Parameters.EngineId;
            EngineBoots = message.Parameters.EngineBoots.Value;
            EngineTime = message.Parameters.EngineTime.Value;
        }
    }

    /// <summary>
    /// Gets version.
    /// </summary>
    public VersionCode Version { get; }

    /// <summary>
    /// Gets message Id.
    /// </summary>
    public int MessageId { get; }

    /// <summary>
    /// Gets max Message Size.
    /// </summary>
    public int MaxMessageSize { get; }

    /// <summary>
    /// Gets engine Id.
    /// </summary>
    public OctetString EngineId { get; }

    /// <summary>
    /// Gets engine Boots.
    /// </summary>
    public int EngineBoots { get; }

    /// <summary>
    /// Gets engine Time.
    /// </summary>
    public int EngineTime { get; }

    /// <summary>
    /// Enterprise.
    /// </summary>
    public ObjectIdentifier Enterprise { get; }

    /// <summary>
    /// Time stamp.
    /// </summary>
    [System.CLSCompliant(false)]
    public uint TimeStamp { get; }

    private readonly IList<Variable> _variables;

    /// <summary>
    /// Gets the variable bindings carried by this trap message.
    /// </summary>
    public IList<Variable> Variables()
    {
        return _variables;
    }

    /// <summary>
    /// Sends the trap to the specified endpoint.
    /// </summary>
    public void Send(IPEndPoint endpoint)
    {
        if (endpoint == null)
        {
            throw new ArgumentNullException(nameof(endpoint));
        }

        SendAsync(endpoint).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Sends the trap to the specified endpoint asynchronously.
    /// </summary>
    public Task SendAsync(IPEndPoint endpoint)
    {
        if (endpoint == null)
        {
            throw new ArgumentNullException(nameof(endpoint));
        }

        if (Version == VersionCode.V2)
        {
            return Messenger.SendTrapV2Async(_requestId, Version, endpoint, _securityName, Enterprise, TimeStamp, _variables);
        }

        if (Version == VersionCode.V3)
        {
            var username = Encoding.UTF8.GetString(_securityName.Octets);
            return Messenger.SendTrapV2V3Async(endpoint, username, _privacy ?? new DefaultPrivacyProvider(), Enterprise, TimeStamp, _variables);
        }

        throw new NotSupportedException("TrapV2Message only supports v2c and v3 in this compatibility layer.");
    }

    /// <summary>
    /// Sends the trap to the specified endpoint asynchronously.
    /// </summary>
    public async Task SendAsync(IPEndPoint endpoint, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await SendAsync(endpoint).WaitAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Returns a <see cref="string"/> that represents the current <see cref="TrapV2Message"/>.
    /// </summary>
    public override string ToString()
    {
        return $"TrapV2Message: version={Version}; enterprise={Enterprise}; vars={_variables.Count}";
    }

    /// <inheritdoc/>
    public VersionCode ProtocolVersion => _message?.ProtocolVersion ?? Version;

    /// <inheritdoc/>
    public IScope? Scope => _message?.Scope;

    /// <summary>
    /// Serializes the message to bytes when this instance wraps a parsed SNMP message.
    /// </summary>
    public byte[] ToBytes()
    {
        if (_message != null)
        {
            return _message.Encode();
        }

        if (Version == VersionCode.V2)
        {
            var message = new DotNetSnmp.Protocol.V2.SnmpV2Message
            {
                Community = _securityName,
                Scope = new TrapV2Pdu
                {
                    RequestId = _requestId,
                    Enterprise = Enterprise,
                    TimeStamp = TimeStamp,
                    VariableBindings = new VarBindList(_variables.ToArray())
                }
            };

            return message.Encode();
        }

        throw new NotSupportedException("Serialization is only available for parsed TrapV2 messages or v2c trap senders.");
    }

    /// <inheritdoc/>
    public void WriteTo(AsnWriter writer)
    {
        if (_message == null)
        {
            throw new NotSupportedException("WriteTo is only available for parsed TrapV2 messages.");
        }

        _message.WriteTo(writer);
    }
}
