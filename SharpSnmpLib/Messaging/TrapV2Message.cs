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

    public VersionCode Version { get; }

    public int MessageId { get; }

    public int MaxMessageSize { get; }

    public OctetString EngineId { get; }

    public int EngineBoots { get; }

    public int EngineTime { get; }

    public ObjectIdentifier Enterprise { get; }

    public uint TimeStamp { get; }

    private readonly IList<Variable> _variables;

    public IList<Variable> Variables()
    {
        return _variables;
    }

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

    public override string ToString()
    {
        return $"TrapV2Message: version={Version}; enterprise={Enterprise}; vars={_variables.Count}";
    }
}
