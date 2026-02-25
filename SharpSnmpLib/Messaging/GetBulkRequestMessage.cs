using System.Formats.Asn1;
using DotNetSnmp.Asn1;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V2;
using DotNetSnmp.Protocol.V3;
using DotNetSnmp.Protocol.V3.Security.Privacy;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Legacy compatibility wrapper for SNMP GET-BULK request messages.
/// </summary>
public sealed class GetBulkRequestMessage : ISnmpMessage, ILegacyV3Request
{
    private readonly ISnmpMessage _message;

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

    public IPrivacyProvider Privacy { get; }

    public VersionCode ProtocolVersion => _message.ProtocolVersion;

    public IScope? Scope => _message.Scope;

    public byte[] ToBytes()
    {
        return _message.Encode();
    }

    public void WriteTo(AsnWriter writer)
    {
        _message.WriteTo(writer);
    }

    public override string ToString()
    {
        return _message.ToString() ?? base.ToString()!;
    }
}
