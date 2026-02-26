using DotNetSnmp.Asn1;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V1;
using DotNetSnmp.Protocol.V2;
using DotNetSnmp.Protocol.V3;
using DotNetSnmp.Protocol.V3.Security;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib;

/// <summary>
/// Legacy compatibility wrapper for SNMP security parameters.
/// </summary>
[Obsolete("This type is for internal use only and may be removed in a future release.")]
public sealed class SecurityParameters
{
    /// <summary>
    /// Gets engine id.
    /// </summary>
    public OctetString EngineId { get; }

    /// <summary>
    /// Gets engine boots.
    /// </summary>
    public Integer32 EngineBoots { get; }

    /// <summary>
    /// Gets engine time.
    /// </summary>
    public Integer32 EngineTime { get; }

    /// <summary>
    /// Gets user name (or community for v1/v2c).
    /// </summary>
    public OctetString UserName { get; }

    /// <summary>
    /// Gets authentication parameters.
    /// </summary>
    public OctetString? AuthenticationParameters { get; }

    /// <summary>
    /// Gets privacy parameters.
    /// </summary>
    public OctetString? PrivacyParameters { get; }

    /// <summary>
    /// Gets or sets a value indicating whether hashes are invalid.
    /// </summary>
    public bool IsInvalid { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="SecurityParameters"/>.
    /// </summary>
    public SecurityParameters(
        OctetString? engineId,
        Integer32? engineBoots,
        Integer32? engineTime,
        OctetString userName,
        OctetString? authenticationParameters,
        OctetString? privacyParameters)
    {
        UserName = userName;
        EngineId = engineId ?? OctetString.Empty;
        EngineBoots = engineBoots ?? Integer32.Zero;
        EngineTime = engineTime ?? Integer32.Zero;
        AuthenticationParameters = authenticationParameters;
        PrivacyParameters = privacyParameters;
    }

    internal SecurityParameters(UsmSecurityParameters parameters)
    {
        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        EngineId = new OctetString(parameters.EngineId.ToArray());
        EngineBoots = new Integer32(parameters.EngineBoots);
        EngineTime = new Integer32(parameters.EngineTime);
        UserName = parameters.SecurityName;
        AuthenticationParameters = new OctetString(parameters.AuthParams.ToArray());
        PrivacyParameters = new OctetString(parameters.PrivParams.ToArray());
    }

    /// <summary>
    /// Creates community-based security parameters.
    /// </summary>
    public static SecurityParameters Create(OctetString userName)
    {
        return new SecurityParameters(null, null, null, userName, null, null);
    }

    internal UsmSecurityParameters ToUsmSecurityParameters()
    {
        return new UsmSecurityParameters
        {
            EngineId = EngineId.Octets,
            EngineBoots = EngineBoots.Value,
            EngineTime = EngineTime.Value,
            SecurityName = UserName,
            AuthParams = AuthenticationParameters?.Octets ?? Memory<byte>.Empty,
            PrivParams = PrivacyParameters?.Octets ?? Memory<byte>.Empty
        };
    }

    internal static SecurityParameters FromMessage(ISnmpMessage message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        if (message is SnmpV1Message v1)
        {
            return Create(v1.Community);
        }

        if (message is SnmpV2Message v2)
        {
            return Create(v2.Community);
        }

        if (message is SnmpV3Message v3)
        {
            var result = new SecurityParameters(v3.SecurityParameters);
            if (Messaging.MessageFactory.TryGetV3SecurityState(message, out var state))
            {
                result.IsInvalid = state.AuthenticationFailed;
            }

            return result;
        }

        if (message is Messaging.ReportMessage report)
        {
            return new SecurityParameters(report.Message.SecurityParameters);
        }

        if (message.ProtocolVersion == VersionCode.V3)
        {
            var parsed = SnmpV3Message.ReadFrom(new AsnReader(message.Encode(), AsnEncodingRules.BER));
            return new SecurityParameters(parsed.SecurityParameters);
        }

        if (message.ProtocolVersion == VersionCode.V2)
        {
            var parsed = SnmpV2Message.ReadFrom(new AsnReader(message.Encode(), AsnEncodingRules.BER));
            return Create(parsed.Community);
        }

        if (message.ProtocolVersion == VersionCode.V1)
        {
            var parsed = SnmpV1Message.ReadFrom(new AsnReader(message.Encode(), AsnEncodingRules.BER));
            return Create(parsed.Community);
        }

        return Create(OctetString.Empty);
    }
}
