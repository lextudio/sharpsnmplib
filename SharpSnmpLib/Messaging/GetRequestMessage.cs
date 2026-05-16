using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Legacy compatibility wrapper for SNMP v3 GET request message.
/// </summary>
public sealed class GetRequestMessage : ISnmpMessage, ILegacyV3Request
{
    private readonly ISnmpMessage _message;

    /// <summary>
    /// Initializes a new instance of GetRequestMessage.
    /// </summary>
    public GetRequestMessage(int requestId, VersionCode version, OctetString community, IList<Variable> variables)
    {
        _message = LegacyRequestBuilder.BuildCommunityRequest(
            requestId,
            version,
            community,
            variables,
            () => new GetRequestPdu());

        Privacy = new DefaultPrivacyProvider();
    }

    /// <summary>
    /// Initializes a new instance of GetRequestMessage.
    /// </summary>
    public GetRequestMessage(
        VersionCode version,
        int messageId,
        int requestId,
        OctetString user,
        OctetString contextName,
        IList<Variable> variables,
        IPrivacyProvider privacy,
        int maxMessageSize,
        ISnmpMessage report)
    {
        if (version != VersionCode.V3)
        {
            throw new NotSupportedException("Only SNMP v3 is supported by this compatibility API.");
        }

        Privacy = privacy ?? throw new ArgumentNullException(nameof(privacy));
        _message = LegacyRequestBuilder.BuildV3Request(
            messageId,
            requestId,
            user,
            contextName,
            variables,
            privacy,
            maxMessageSize,
            report,
            () => new GetRequestPdu());
    }

    /// <summary>
    /// Initializes a new instance of GetRequestMessage.
    /// </summary>
    [Obsolete("Please use overloads with contextName.")]
    public GetRequestMessage(
        VersionCode version,
        int messageId,
        int requestId,
        OctetString user,
        IList<Variable> variables,
        IPrivacyProvider privacy,
        ISnmpMessage report)
        : this(
            version,
            messageId,
            requestId,
            user,
            OctetString.Empty,
            variables,
            privacy,
            Messenger.MaxMessageSize,
            report)
    {
    }

    /// <summary>
    /// Initializes a new instance of GetRequestMessage.
    /// </summary>
    [Obsolete("Please use overloads with contextName.")]
    public GetRequestMessage(
        VersionCode version,
        int messageId,
        int requestId,
        OctetString user,
        IList<Variable> variables,
        IPrivacyProvider privacy,
        int maxMessageSize,
        ISnmpMessage report)
        : this(
            version,
            messageId,
            requestId,
            user,
            OctetString.Empty,
            variables,
            privacy,
            maxMessageSize,
            report)
    {
    }

    /// <summary>
    /// Gets privacy.
    /// </summary>
    public IPrivacyProvider Privacy { get; }

    /// <summary>
    /// Represents protocol Version.
    /// </summary>
    public VersionCode ProtocolVersion => _message.ProtocolVersion;

    /// <summary>
    /// Gets the version (legacy alias for ProtocolVersion).
    /// </summary>
    public VersionCode Version => _message.ProtocolVersion;

    /// <summary>
    /// Represents scope.
    /// </summary>
    IScope? ISnmpMessage.Scope => _message.Scope;

    /// <summary>Gets the v3 scope (legacy compatibility).</summary>
    public Scope Scope => (_message.Scope as Scope) ?? new Scope();

    /// <summary>
    /// Gets the v3 header (legacy compatibility).
    /// </summary>
    public Header Header => Header.FromMessage(_message);

    /// <summary>
    /// Gets the security parameters (legacy compatibility).
    /// </summary>
    public SecurityParameters Parameters => SecurityParameters.FromMessage(_message);

    /// <summary>
    /// Serializes the message to a byte array.
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

    /// <summary>
    /// Returns a <see cref="string"/> that represents this <see cref="GetRequestMessage"/>.
    /// </summary>
    public override string ToString()
    {
        return _message.ToString() ?? base.ToString()!;
    }
}
