using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Asn1;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V1;
using DotNetSnmp.Protocol.V3;
using DotNetSnmp.Protocol.V3.Security.Privacy;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Legacy compatibility wrapper for SNMP v3 GET request message.
/// </summary>
public sealed class GetRequestMessage : ISnmpMessage, ILegacyV3Request
{
    private readonly SnmpV3Message _message;

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
