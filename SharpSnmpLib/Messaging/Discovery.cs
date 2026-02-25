using System.Formats.Asn1;
using System.Globalization;
using System.Net;
using DotNetSnmp.Asn1;
using DotNetSnmp.Asn1.Serialization;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V1;
using DotNetSnmp.Protocol.V2;
using DotNetSnmp.Protocol.V3;
using DotNetSnmp.Protocol.V3.Security;
using DotNetSnmp.Transport;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Discovery class that participates in SNMP v3 discovery process.
/// </summary>
public sealed class Discovery
{
    private readonly int _messageId;
    private readonly int _requestId;
    private readonly int _maxMessageSize;
    private readonly SnmpType _type;

    /// <summary>
    /// Initializes a new instance of Discovery.
    /// </summary>
    /// <param name="messageId">The message id.</param>
    /// <param name="requestId">The request id.</param>
    /// <param name="maxMessageSize">The max size of message.</param>
    public Discovery(int messageId, int requestId, int maxMessageSize)
        : this(messageId, requestId, maxMessageSize, SnmpType.GetRequestPdu)
    {
    }

    /// <summary>
    /// Initializes a new instance of Discovery.
    /// </summary>
    /// <param name="messageId">The message id.</param>
    /// <param name="requestId">The request id.</param>
    /// <param name="maxMessageSize">The max size of message.</param>
    /// <param name="type">Message type.</param>
    public Discovery(int messageId, int requestId, int maxMessageSize, SnmpType type)
    {
        _messageId = messageId;
        _requestId = requestId;
        _maxMessageSize = maxMessageSize;
        _type = type;

        // validate immediately so constructor behavior matches legacy expectations.
        _ = CreateDiscoveryPdu(type, requestId);
    }

    /// <summary>
    /// Gets the response message.
    /// </summary>
    /// <param name="timeout">
    /// The timeout value in milliseconds. 0 and -1 indicate infinite timeout.
    /// </param>
    /// <param name="receiver">The receiver endpoint.</param>
    /// <returns>A parsed report message.</returns>
    public ReportMessage GetResponse(int timeout, IPEndPoint receiver)
    {
        if (timeout < -1)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout));
        }

        if (timeout == 0 || timeout == -1)
        {
            return GetResponseAsync(receiver).GetAwaiter().GetResult();
        }

        try
        {
            return GetResponseAsync(receiver)
                .WaitAsync(TimeSpan.FromMilliseconds(timeout))
                .GetAwaiter()
                .GetResult();
        }
        catch (TimeoutException ex)
        {
            throw new TimeoutException($"Discovery timed out after {timeout} milliseconds.", ex);
        }
    }

    /// <summary>
    /// Gets response Async.
    /// </summary>
    /// <param name="receiver">The receiver endpoint.</param>
    /// <returns>A parsed report message.</returns>
    public async Task<ReportMessage> GetResponseAsync(IPEndPoint receiver)
    {
        if (receiver == null)
        {
            throw new ArgumentNullException(nameof(receiver));
        }

        using var transport = new BasicUdpTransport(receiver);
        var outbound = ToBytes();
        await transport.SendAsync(outbound, receiver).ConfigureAwait(false);

        var incoming = await transport.ReceiveAsync(receiver, CancellationToken.None).ConfigureAwait(false);
        var reader = new AsnReader(incoming, AsnEncodingRules.BER);
        var response = SnmpV3Message.ReadFrom(reader);

        return new ReportMessage(response);
    }

    /// <summary>
    /// Serializes the message to a byte array.
    /// </summary>
    /// <returns>Encoded bytes.</returns>
    public byte[] ToBytes()
    {
        return CreateMessage().Encode();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            "discovery class: message id: {0}; request id: {1}",
            _messageId,
            _requestId);
    }

    private SnmpV3Message CreateMessage()
    {
        var scope = new Scope
        {
            ContextEngineId = ReadOnlyMemory<byte>.Empty,
            ContextName = string.Empty,
            Pdu = CreateDiscoveryPdu(_type, _requestId)
        };

        return new SnmpV3Message
        {
            Header = new HeaderData
            {
                MsgId = _messageId,
                MsgMaxSize = _maxMessageSize,
                MsgFlags = MsgFlags.Reportable,
                MsgSecurityModel = SecurityModel.Usm
            },
            SecurityParameters = new UsmSecurityParameters
            {
                SecurityName = OctetString.Empty,
                EngineId = Memory<byte>.Empty,
                EngineBoots = 0,
                EngineTime = 0,
                AuthParams = Memory<byte>.Empty,
                PrivParams = Memory<byte>.Empty
            },
            Scope = scope
        };
    }

    private static Pdu CreateDiscoveryPdu(SnmpType type, int requestId)
    {
        return type switch
        {
            SnmpType.GetRequestPdu => new GetRequestPdu
            {
                RequestId = requestId,
                VariableBindings = new VarBindList()
            },
            SnmpType.GetNextRequestPdu => new GetNextRequestPdu
            {
                RequestId = requestId,
                VariableBindings = new VarBindList()
            },
            SnmpType.GetBulkRequestPdu => new GetBulkRequestPdu
            {
                RequestId = requestId,
                NonRepeaters = 0,
                MaxRepetitions = 0,
                VariableBindings = new VarBindList()
            },
            SnmpType.SetRequestPdu => new SetRequestPdu
            {
                RequestId = requestId,
                VariableBindings = new VarBindList()
            },
            SnmpType.InformRequestPdu => new InformRequestPdu
            {
                RequestId = requestId,
                TimeStamp = 0,
                Enterprise = new ObjectIdentifier("1.3.6.1.6.3.1.1.5.1"),
                VariableBindings = new VarBindList(
                    new Variable("1.3.6.1.2.1.1.3.0", new TimeTicks(0)),
                    new Variable("1.3.6.1.6.3.1.1.4.1.0", new ObjectIdentifier("1.3.6.1.6.3.1.1.5.1")))
            },
            _ => throw new ArgumentException("Discovery message must be a request.", nameof(type))
        };
    }
}
