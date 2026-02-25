using System.Text;
using System.Net;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V3.Security.Privacy;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Legacy compatibility wrapper for TrapV2 message sending.
/// </summary>
public sealed class TrapV2Message
{
    private readonly int _requestId;
    private readonly OctetString _securityName;
    private readonly IPrivacyProvider? _privacy;

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

        if (Version == VersionCode.V2)
        {
            Messenger.SendTrapV2(_requestId, Version, endpoint, _securityName, Enterprise, TimeStamp, _variables);
            return;
        }

        if (Version == VersionCode.V3)
        {
            var username = Encoding.UTF8.GetString(_securityName.Octets);
            Messenger.SendInformV3Async(endpoint, username, _privacy ?? new DefaultPrivacyProvider(), Enterprise, TimeStamp, _variables)
                .GetAwaiter()
                .GetResult();
            return;
        }

        throw new NotSupportedException("TrapV2Message only supports v2c and v3 in this compatibility layer.");
    }

    /// <summary>
    /// Returns a <see cref="string"/> that represents the current <see cref="TrapV2Message"/>.
    /// </summary>
    public override string ToString()
    {
        return $"TrapV2Message: version={Version}; enterprise={Enterprise}; vars={_variables.Count}";
    }
}
