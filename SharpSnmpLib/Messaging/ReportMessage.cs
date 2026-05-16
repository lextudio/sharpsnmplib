using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// REPORT message wrapper for compatibility APIs.
/// </summary>
public sealed class ReportMessage : ISnmpMessage
{
    /// <summary>
    /// Initializes a new instance of ReportMessage.
    /// </summary>
    /// <param name="message">The parsed SNMP v3 message.</param>
    public ReportMessage(SnmpV3Message message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        if (message.ProtocolVersion != VersionCode.V3)
        {
            throw new ArgumentException("Only v3 report messages are supported.", nameof(message));
        }

        if (message.Scope?.Pdu is not ReportPdu)
        {
            throw new ArgumentException("The supplied message is not a REPORT response.", nameof(message));
        }

        Message = message;
        Header = new Header(message.Header);
        Parameters = new SecurityParameters(message.SecurityParameters);
        Privacy = new Lextm.SharpSnmpLib.Security.DefaultPrivacyProvider();
    }

    /// <summary>
    /// Initializes a new instance of ReportMessage from legacy v3 constructor arguments.
    /// </summary>
    public ReportMessage(
        VersionCode version,
        Header header,
        SecurityParameters parameters,
        Scope scope,
        IPrivacyProvider privacy,
        byte[]? length)
    {
        if (version != VersionCode.V3)
        {
            throw new ArgumentException("Only v3 report messages are supported.", nameof(version));
        }

        if (header == null)
        {
            throw new ArgumentNullException(nameof(header));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        if (scope == null)
        {
            throw new ArgumentNullException(nameof(scope));
        }

        if (privacy == null)
        {
            throw new ArgumentNullException(nameof(privacy));
        }

        _ = length; // preserved for source compatibility

        var message = new SnmpV3Message
        {
            Header = header.ToHeaderData(),
            SecurityParameters = parameters.ToUsmSecurityParameters(),
            Scope = scope
        };

        if (message.Header.MsgFlags.HasFlag(MsgFlag.Priv))
        {
            privacy.EncryptMessage(message);
        }

        if (message.Header.MsgFlags.HasFlag(MsgFlag.Auth))
        {
            privacy.AuthenticationProvider.AuthenticateOutgoingMsg(message, message.SecurityParameters.AuthParams);
        }

        Message = message;
        Header = header;
        Parameters = parameters;
        Privacy = privacy;
    }

    /// <summary>
    /// Gets message.
    /// </summary>
    public SnmpV3Message Message { get; }

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
    /// Represents protocol Version.
    /// </summary>
    public VersionCode Version => Message.ProtocolVersion;

    /// <summary>
    /// Represents protocol Version.
    /// </summary>
    public VersionCode ProtocolVersion => Message.ProtocolVersion;

    /// <summary>
    /// Represents scope.
    /// </summary>
    public IScope? Scope => Message.Scope;

    /// <summary>
    /// Serializes the message to a byte array.
    /// </summary>
    /// <returns>Encoded SNMP message bytes.</returns>
    public byte[] ToBytes()
    {
        return Message.Encode();
    }

    /// <inheritdoc/>
    public void WriteTo(AsnWriter writer)
    {
        Message.WriteTo(writer);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"REPORT request message: version: {Version}; {Message.SecurityParameters.SecurityName}; {Message.Scope?.Pdu}";
    }
}
