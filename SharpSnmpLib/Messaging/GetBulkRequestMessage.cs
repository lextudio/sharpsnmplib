using System.Formats.Asn1;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Legacy compatibility wrapper for SNMP GET-BULK request messages.
/// </summary>
public sealed class GetBulkRequestMessage : ISnmpMessage, ILegacyV3Request
{
    private readonly ISnmpMessage _message;

    /// <summary>
    /// Initializes a new instance of GetBulkRequestMessage.
    /// </summary>
    public GetBulkRequestMessage(
        int requestId,
        VersionCode version,
        OctetString community,
        int nonRepeaters,
        int maxRepetitions,
        IList<Variable> variables)
    {
        if (version != VersionCode.V2)
        {
            throw new NotSupportedException("GET-BULK is only supported by SNMP v2c/v3.");
        }

        _message = LegacyRequestBuilder.BuildCommunityRequest(
            requestId,
            version,
            community,
            variables,
            () => new GetBulkRequestPdu
            {
                NonRepeaters = nonRepeaters,
                MaxRepetitions = maxRepetitions
            });

        Privacy = new DefaultPrivacyProvider();
    }

    /// <summary>
    /// Initializes a new instance of GetBulkRequestMessage (legacy, no contextName).
    /// </summary>
    [Obsolete("Please use overloads with contextName.")]
    public GetBulkRequestMessage(
        VersionCode version,
        int messageId,
        int requestId,
        OctetString user,
        int nonRepeaters,
        int maxRepetitions,
        IList<Variable> variables,
        IPrivacyProvider privacy,
        ISnmpMessage report)
        : this(version, messageId, requestId, user, OctetString.Empty, nonRepeaters, maxRepetitions, variables, privacy, Messenger.MaxMessageSize, report)
    {
    }

    /// <summary>
    /// Initializes a new instance of GetBulkRequestMessage (legacy, no contextName, with maxMessageSize).
    /// </summary>
    [Obsolete("Please use overloads with contextName.")]
    public GetBulkRequestMessage(
        VersionCode version,
        int messageId,
        int requestId,
        OctetString user,
        int nonRepeaters,
        int maxRepetitions,
        IList<Variable> variables,
        IPrivacyProvider privacy,
        int maxMessageSize,
        ISnmpMessage report)
        : this(version, messageId, requestId, user, OctetString.Empty, nonRepeaters, maxRepetitions, variables, privacy, maxMessageSize, report)
    {
    }

    /// <summary>
    /// Initializes a new instance of GetBulkRequestMessage.
    /// </summary>
    public GetBulkRequestMessage(
        VersionCode version,
        int messageId,
        int requestId,
        OctetString user,
        OctetString contextName,
        int nonRepeaters,
        int maxRepetitions,
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
            () => new GetBulkRequestPdu
            {
                NonRepeaters = nonRepeaters,
                MaxRepetitions = maxRepetitions
            });
    }

    /// <summary>
    /// Gets privacy.
    /// </summary>
    public IPrivacyProvider Privacy { get; }

    /// <summary>
    /// Represents protocol Version.
    /// </summary>
    public VersionCode ProtocolVersion => _message.ProtocolVersion;

    /// <summary>Gets the version (legacy alias for ProtocolVersion).</summary>
    public VersionCode Version => _message.ProtocolVersion;

    /// <summary>
    /// Represents scope.
    /// </summary>
    IScope? ISnmpMessage.Scope => _message.Scope;

    /// <summary>Gets the v3 scope (legacy compatibility).</summary>
    public Scope Scope => (_message.Scope as Scope) ?? new Scope();

    /// <summary>Gets the v3 header (legacy compatibility).</summary>
    public Header Header => Header.FromMessage(_message);

    /// <summary>Gets the security parameters (legacy compatibility).</summary>
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
    /// Returns a <see cref="string"/> that represents this <see cref="GetBulkRequestMessage"/>.
    /// </summary>
    public override string ToString()
    {
        return _message.ToString() ?? base.ToString()!;
    }
}
