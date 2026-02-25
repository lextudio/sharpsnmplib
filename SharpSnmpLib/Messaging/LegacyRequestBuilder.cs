using System.Text;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V1;
using DotNetSnmp.Protocol.V2;
using DotNetSnmp.Protocol.V3;
using DotNetSnmp.Protocol.V3.Security;
using DotNetSnmp.Protocol.V3.Security.Privacy;

namespace Lextm.SharpSnmpLib.Messaging;

internal interface ILegacyV3Request
{
    IPrivacyProvider Privacy { get; }
}

internal static class LegacyRequestBuilder
{
    internal static ISnmpMessage BuildCommunityRequest(
        int requestId,
        VersionCode version,
        OctetString community,
        IList<Variable> variables,
        Func<Pdu> pduFactory)
    {
        if (variables == null)
        {
            throw new ArgumentNullException(nameof(variables));
        }

        var pdu = pduFactory();
        pdu.RequestId = requestId;
        pdu.VariableBindings = new VarBindList(variables.ToArray());

        return version switch
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
            _ => throw new NotSupportedException("Only SNMP v1/v2c are supported by this constructor.")
        };
    }

    internal static SnmpV3Message BuildV3Request(
        int messageId,
        int requestId,
        OctetString user,
        OctetString contextName,
        IList<Variable> variables,
        IPrivacyProvider privacy,
        int maxMessageSize,
        ISnmpMessage report,
        Func<Pdu> pduFactory)
    {
        if (variables == null)
        {
            throw new ArgumentNullException(nameof(variables));
        }

        if (privacy == null)
        {
            throw new ArgumentNullException(nameof(privacy));
        }

        var reportMessage = ExtractReportMessage(report);
        var contextEngineId = reportMessage.SecurityParameters.EngineId;

        var flags = MsgFlag.Reportable;
        if (privacy.AuthenticationProvider.DigestSize > 0)
        {
            flags |= MsgFlag.Auth;
        }

        if (privacy is not DefaultPrivacyProvider)
        {
            flags |= MsgFlag.Priv;
        }

        var pdu = pduFactory();
        pdu.RequestId = requestId;
        pdu.VariableBindings = new VarBindList(variables.ToArray());

        var message = new SnmpV3Message
        {
            Header = new HeaderData
            {
                MsgId = messageId,
                MsgMaxSize = maxMessageSize,
                MsgFlags = flags,
                MsgSecurityModel = SecurityModel.Usm
            },
            SecurityParameters = new UsmSecurityParameters
            {
                EngineId = contextEngineId,
                EngineBoots = reportMessage.SecurityParameters.EngineBoots,
                EngineTime = reportMessage.SecurityParameters.EngineTime,
                SecurityName = user,
                AuthParams = privacy.AuthenticationProvider.DigestSize > 0
                    ? new byte[privacy.AuthenticationProvider.TruncatedDigestSize]
                    : Memory<byte>.Empty,
                PrivParams = privacy is not DefaultPrivacyProvider
                    ? new byte[privacy.PrivacyParametersLength]
                    : Memory<byte>.Empty
            },
            Scope = new Scope
            {
                ContextEngineId = contextEngineId,
                ContextName = Encoding.UTF8.GetString(contextName.Octets),
                Pdu = pdu
            }
        };

        if (message.Header.MsgFlags.HasFlag(MsgFlag.Priv))
        {
            privacy.EncryptMessage(message);
        }

        if (message.Header.MsgFlags.HasFlag(MsgFlag.Auth))
        {
            privacy.AuthenticationProvider.AuthenticateOutgoingMsg(message, message.SecurityParameters.AuthParams);
        }

        return message;
    }

    private static SnmpV3Message ExtractReportMessage(ISnmpMessage report)
    {
        if (report is ReportMessage rm)
        {
            return rm.Message;
        }

        if (report is SnmpV3Message v3 && v3.Scope?.Pdu is ReportPdu)
        {
            return v3;
        }

        throw new ArgumentException("A v3 REPORT message is required.", nameof(report));
    }
}
