using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Legacy compatibility wrapper for SNMP response messages.
/// </summary>
public sealed class ResponseMessage : ISnmpMessage
{
    private readonly ISnmpMessage _message;

    /// <summary>
    /// Creates a v1/v2c response message.
    /// </summary>
    public ResponseMessage(int requestId, VersionCode version, OctetString community, ErrorCode error, int index, IList<Variable> variables)
    {
        if (community.Octets == null)
        {
            throw new ArgumentNullException(nameof(community));
        }

        if (variables == null)
        {
            throw new ArgumentNullException(nameof(variables));
        }

        if (version == VersionCode.V3)
        {
            throw new ArgumentException("Please use the v3 constructor for SNMP v3 responses.", nameof(version));
        }

        var pdu = new ResponsePdu(requestId, error, index, variables);
        _message = version switch
        {
            VersionCode.V1 => new SnmpV1Message
            {
                Community = community,
                Scope = pdu
            },
            VersionCode.V2 => new SnmpV2Message
            {
                Community = community,
                Scope = pdu
            },
            _ => throw new NotSupportedException($"Unsupported version {version}")
        };

        Header = Header.Empty;
        Parameters = SecurityParameters.Create(community);
        Privacy = Lextm.SharpSnmpLib.Security.DefaultPrivacyProvider.DefaultPair;
    }

    /// <summary>
    /// Creates a v3 response message.
    /// </summary>
    public ResponseMessage(
        VersionCode version,
        Header header,
        SecurityParameters parameters,
        Scope scope,
        IPrivacyProvider privacy,
        bool needAuthentication,
        byte[]? length)
    {
        if (version != VersionCode.V3)
        {
            throw new ArgumentException("Only SNMP v3 is supported by this constructor.", nameof(version));
        }

        Header = header ?? throw new ArgumentNullException(nameof(header));
        Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        Privacy = privacy ?? throw new ArgumentNullException(nameof(privacy));
        _ = length; // preserved for source compatibility

        var message = new SnmpV3Message
        {
            Header = header.ToHeaderData(),
            SecurityParameters = parameters.ToUsmSecurityParameters(),
            Scope = scope ?? throw new ArgumentNullException(nameof(scope))
        };

        if (message.Header.MsgFlags.HasFlag(MsgFlag.Priv))
        {
            privacy.EncryptMessage(message);
        }

        if (needAuthentication && message.Header.MsgFlags.HasFlag(MsgFlag.Auth))
        {
            privacy.AuthenticationProvider.AuthenticateOutgoingMsg(message, message.SecurityParameters.AuthParams);
        }

        _message = message;
    }

    /// <summary>
    /// Gets legacy version alias.
    /// </summary>
    public VersionCode Version => ProtocolVersion;

    /// <summary>
    /// Gets protocol version.
    /// </summary>
    public VersionCode ProtocolVersion => _message.ProtocolVersion;

    /// <summary>
    /// Gets scope.
    /// </summary>
    public IScope? Scope => _message.Scope;

    /// <summary>
    /// Gets header.
    /// </summary>
    public Header Header { get; }

    /// <summary>
    /// Gets security parameters.
    /// </summary>
    public SecurityParameters Parameters { get; }

    /// <summary>
    /// Gets privacy provider.
    /// </summary>
    public IPrivacyProvider Privacy { get; }

    /// <summary>
    /// Gets response error status.
    /// </summary>
    public ErrorCode ErrorStatus => (ErrorCode)(Scope?.Pdu.ErrorStatus.Value ?? (int)ErrorCode.NoError);

    /// <summary>
    /// Gets response error index.
    /// </summary>
    public int ErrorIndex => Scope?.Pdu.ErrorIndex.Value ?? 0;

    /// <summary>
    /// Serializes message to bytes.
    /// </summary>
    public byte[] ToBytes()
    {
        return _message.Encode();
    }

    /// <inheritdoc/>
    public void WriteTo(AsnWriter writer)
    {
        _message.WriteTo(writer);
    }
}
