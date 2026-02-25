using DotNetSnmp.Asn1;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V1;
using DotNetSnmp.Protocol.V3;
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
    }

    /// <summary>
    /// Gets message.
    /// </summary>
    public SnmpV3Message Message { get; }

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
