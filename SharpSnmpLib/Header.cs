using DotNetSnmp.Asn1;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V3;
using DotNetSnmp.Protocol.V3.Security;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib;

/// <summary>
/// Legacy compatibility wrapper for SNMP v3 header data.
/// </summary>
[Obsolete("This type is for internal use only and may be removed in a future release.")]
public sealed class Header
{
    /// <summary>
    /// Max message size used by legacy #SNMP APIs.
    /// </summary>
    public const int MaxMessageSize = 0xFFE3;

    private readonly int? _messageId;
    private readonly int _maxSize;

    private Header()
    {
        _maxSize = MaxMessageSize;
        SecurityLevel = 0;
    }

    internal Header(HeaderData header)
    {
        if (header == null)
        {
            throw new ArgumentNullException(nameof(header));
        }

        _messageId = header.MsgId;
        _maxSize = header.MsgMaxSize;
        SecurityLevel = ToSecurityLevel(header.MsgFlags);
    }

    /// <summary>
    /// Initializes a new instance of <see cref="Header"/>.
    /// </summary>
    public Header(Integer32? messageId, Integer32 maxMessageSize, Levels securityLevel)
    {
        _messageId = messageId?.Value;
        _maxSize = maxMessageSize.Value;
        SecurityLevel = securityLevel;
    }

    /// <summary>
    /// Empty header for v1/v2c messages.
    /// </summary>
    public static Header Empty { get; } = new();

    /// <summary>
    /// Gets security flags.
    /// </summary>
    public Levels SecurityLevel { get; }

    /// <summary>
    /// Gets message id.
    /// </summary>
    public int MessageId => _messageId ?? throw new InvalidOperationException("MessageId is only available for SNMP v3 headers.");

    /// <summary>
    /// Gets maximum message size.
    /// </summary>
    public int MaxSize => _maxSize;

    internal HeaderData ToHeaderData()
    {
        return new HeaderData
        {
            MsgId = _messageId ?? 0,
            MsgMaxSize = _maxSize,
            MsgFlags = ToMsgFlags(SecurityLevel),
            MsgSecurityModel = SecurityModel.Usm
        };
    }

    internal static Header FromMessage(ISnmpMessage message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        if (message.ProtocolVersion != VersionCode.V3)
        {
            return Empty;
        }

        if (message is SnmpV3Message v3)
        {
            return new Header(v3.Header);
        }

        if (message is Messaging.ReportMessage report)
        {
            return new Header(report.Message.Header);
        }

        var parsed = SnmpV3Message.ReadFrom(new AsnReader(message.Encode(), AsnEncodingRules.BER));
        return new Header(parsed.Header);
    }

    internal static Levels ToSecurityLevel(MsgFlag flags)
    {
        Levels result = 0;
        if (flags.HasFlag(MsgFlag.Auth))
        {
            result |= Levels.Authentication;
        }

        if (flags.HasFlag(MsgFlag.Priv))
        {
            result |= Levels.Privacy;
        }

        if (flags.HasFlag(MsgFlag.Reportable))
        {
            result |= Levels.Reportable;
        }

        return result;
    }

    internal static MsgFlag ToMsgFlags(Levels level)
    {
        MsgFlag result = MsgFlag.NoAuthNoPriv;
        if ((level & Levels.Authentication) == Levels.Authentication)
        {
            result |= MsgFlag.Auth;
        }

        if ((level & Levels.Privacy) == Levels.Privacy)
        {
            result |= MsgFlag.Priv;
        }

        if ((level & Levels.Reportable) == Levels.Reportable)
        {
            result |= MsgFlag.Reportable;
        }

        return result;
    }
}
