using System.Formats.Asn1;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Legacy compatibility wrapper for SNMP GET-NEXT request messages.
/// </summary>
public sealed class GetNextRequestMessage : ISnmpMessage, ILegacyV3Request
{
    private readonly ISnmpMessage _message;

    /// <summary>
    /// Initializes a new instance of GetNextRequestMessage.
    /// </summary>
    public GetNextRequestMessage(int requestId, VersionCode version, OctetString community, IList<Variable> variables)
    {
        _message = LegacyRequestBuilder.BuildCommunityRequest(
            requestId,
            version,
            community,
            variables,
            () => new GetNextRequestPdu());

        Privacy = new DefaultPrivacyProvider();
    }

    /// <summary>
    /// Initializes a new instance of GetNextRequestMessage.
    /// </summary>
    public GetNextRequestMessage(
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
            throw new NotSupportedException("Only SNMP v3 is supported by this constructor.");
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
            () => new GetNextRequestPdu());
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
    /// Represents scope.
    /// </summary>
    public IScope? Scope => _message.Scope;

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
    /// Returns a <see cref="string"/> that represents this <see cref="GetNextRequestMessage"/>.
    /// </summary>
    public override string ToString()
    {
        return _message.ToString() ?? base.ToString()!;
    }
}
