using System.Formats.Asn1;
using System.Net;
using System.Net.Sockets;
using DotNetSnmp.Asn1;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V1;
using DotNetSnmp.Protocol.V2;
using DotNetSnmp.Protocol.V3;
using DotNetSnmp.Protocol.V3.Security;
using DotNetSnmp.Transport;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Legacy PDU facade exposed by compatibility APIs.
/// </summary>
public sealed class LegacyPdu : ISnmpPdu
{
    private readonly Pdu _pdu;

    internal LegacyPdu(Pdu pdu)
    {
        _pdu = pdu ?? throw new ArgumentNullException(nameof(pdu));
        Variables = pdu.VariableBindings?.ToList() ?? new List<Variable>();

        if (TryGetBulkRequestParameters(pdu, out var nonRepeaters, out var maxRepetitions))
        {
            ErrorStatus = new Integer32(nonRepeaters);
            ErrorIndex = new Integer32(maxRepetitions);
        }
        else
        {
            ErrorStatus = new Integer32((int)pdu.ErrorStatus);
            ErrorIndex = new Integer32(pdu.ErrorIndex);
        }
    }

    private static bool TryGetBulkRequestParameters(Pdu pdu, out int nonRepeaters, out int maxRepetitions)
    {
        nonRepeaters = 0;
        maxRepetitions = 0;

        if (pdu.TypeCode != SnmpType.GetBulkRequestPdu)
        {
            return false;
        }

        if (pdu is GetBulkRequestPdu bulk)
        {
            nonRepeaters = bulk.NonRepeaters;
            maxRepetitions = bulk.MaxRepetitions;
            return true;
        }

        // Some compatibility builds can surface a different CLR type for bulk PDUs.
        // Read by property name to preserve v12 semantics in those mixed-type scenarios.
        var type = pdu.GetType();
        var nonRepeatersProperty = type.GetProperty("NonRepeaters");
        var maxRepetitionsProperty = type.GetProperty("MaxRepetitions");
        if (nonRepeatersProperty?.PropertyType == typeof(int) && maxRepetitionsProperty?.PropertyType == typeof(int))
        {
            nonRepeaters = (int)(nonRepeatersProperty.GetValue(pdu) ?? 0);
            maxRepetitions = (int)(maxRepetitionsProperty.GetValue(pdu) ?? 0);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Gets variables.
    /// </summary>
    public IList<Variable> Variables { get; }

    /// <summary>
    /// Gets error Status.
    /// </summary>
    public Integer32 ErrorStatus { get; }

    /// <summary>
    /// Gets error Index.
    /// </summary>
    public Integer32 ErrorIndex { get; }

    /// <summary>
    /// Gets request id.
    /// </summary>
    public Integer32 RequestId => new(_pdu.RequestId);

    /// <summary>
    /// Gets PDU type code.
    /// </summary>
    public SnmpType TypeCode => _pdu switch
    {
        GetRequestPdu => SnmpType.GetRequestPdu,
        GetNextRequestPdu => SnmpType.GetNextRequestPdu,
        GetBulkRequestPdu => SnmpType.GetBulkRequestPdu,
        SetRequestPdu => SnmpType.SetRequestPdu,
        ResponsePdu => SnmpType.ResponsePdu,
        TrapPdu => SnmpType.TrapV1Pdu,
        TrapV2Pdu => SnmpType.TrapV2Pdu,
        InformRequestPdu => SnmpType.InformRequestPdu,
        ReportPdu => SnmpType.ReportPdu,
        _ => SnmpType.Unknown
    };

    /// <summary>
    /// Casts to <see cref="TrapV2Pdu"/> when possible.
    /// </summary>
    public static explicit operator TrapV2Pdu(LegacyPdu pdu)
    {
        if (pdu == null)
        {
            throw new ArgumentNullException(nameof(pdu));
        }

        if (pdu._pdu is TrapV2Pdu trap)
        {
            return trap;
        }

        throw new InvalidCastException("Legacy PDU is not a TrapV2Pdu.");
    }

    /// <summary>
    /// Casts to <see cref="InformRequestPdu"/> when possible.
    /// </summary>
    public static explicit operator InformRequestPdu(LegacyPdu pdu)
    {
        if (pdu == null)
        {
            throw new ArgumentNullException(nameof(pdu));
        }

        if (pdu._pdu is InformRequestPdu inform)
        {
            return inform;
        }

        throw new InvalidCastException("Legacy PDU is not an InformRequestPdu.");
    }
}

/// <summary>
/// Provides helper methods for SnmpMessageCompatibilityExtensions.
/// </summary>
public static class SnmpMessageCompatibilityExtensions
{
    /// <summary>
    /// Gets message PDU type.
    /// </summary>
    public static SnmpType TypeCode(this ISnmpMessage message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        var pdu = message.Scope?.Pdu;
        return pdu?.TypeCode ?? SnmpType.Unknown;
    }

    /// <summary>
    /// Gets message variable bindings.
    /// </summary>
    public static IList<Variable> Variables(this ISnmpMessage message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        return message.TypeCode() == SnmpType.Unknown
            ? new List<Variable>(0)
            : message.Pdu().Variables;
    }

    /// <summary>
    /// Gets request id.
    /// </summary>
    public static int RequestId(this ISnmpMessage message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        return message.Scope?.RequestId ?? 0;
    }

    /// <summary>
    /// Gets message id.
    /// </summary>
    public static int MessageId(this ISnmpMessage message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        return message.ProtocolVersion == VersionCode.V3
            ? message.Header.MessageId
            : message.RequestId();
    }

    /// <summary>
    /// Serializes the message to a byte array.
    /// </summary>
    public static byte[] ToBytes(this ISnmpMessage message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        if (message is ReportMessage report)
        {
            return report.ToBytes();
        }

        return message.Encode();
    }

    /// <summary>
    /// Returns the message PDU wrapped in the legacy compatibility facade.
    /// </summary>
    public static LegacyPdu Pdu(this ISnmpMessage message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        var pdu = message.Scope?.Pdu;
        if (pdu == null)
        {
            throw new SnmpException("The SNMP message does not contain a scoped PDU.");
        }

        return new LegacyPdu(pdu);
    }

    /// <summary>
    /// Gets the response message.
    /// </summary>
    public static ISnmpMessage GetResponse(this ISnmpMessage request, int timeout, IPEndPoint receiver)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (receiver == null)
        {
            throw new ArgumentNullException(nameof(receiver));
        }

        if (timeout < -1)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout));
        }

        if (timeout == 0 || timeout == -1)
        {
            return GetResponseAsync(request, receiver).GetAwaiter().GetResult();
        }

        try
        {
            return GetResponseAsync(request, receiver)
                .WaitAsync(TimeSpan.FromMilliseconds(timeout))
                .GetAwaiter()
                .GetResult();
        }
        catch (System.TimeoutException ex)
        {
            throw new TimeoutException($"SNMP request timed out after {timeout} milliseconds.", ex);
        }
    }

    /// <summary>
    /// Sends an SNMP request and handles the response using a provided socket.
    /// </summary>
    public static ISnmpMessage GetResponse(this ISnmpMessage request, int timeout, IPEndPoint receiver, Socket udpSocket)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (receiver == null)
        {
            throw new ArgumentNullException(nameof(receiver));
        }

        if (udpSocket == null)
        {
            throw new ArgumentNullException(nameof(udpSocket));
        }

        var registry = new UserRegistry();
        if (request.ProtocolVersion == VersionCode.V3)
        {
            var privacy = request is ILegacyV3Request v3Request
                ? v3Request.Privacy
                : new Lextm.SharpSnmpLib.Security.DefaultPrivacyProvider();
            registry.Add(request.Parameters.UserName, privacy);
        }

        return request.GetResponse(timeout, receiver, registry, udpSocket);
    }

    /// <summary>
    /// Sends an SNMP request and handles the response using a provided socket and user registry.
    /// </summary>
    public static ISnmpMessage GetResponse(this ISnmpMessage request, int timeout, IPEndPoint receiver, UserRegistry registry, Socket udpSocket)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (receiver == null)
        {
            throw new ArgumentNullException(nameof(receiver));
        }

        if (registry == null)
        {
            throw new ArgumentNullException(nameof(registry));
        }

        if (udpSocket == null)
        {
            throw new ArgumentNullException(nameof(udpSocket));
        }

        if (timeout < -1)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout));
        }

        if (timeout == 0 || timeout == -1)
        {
            return GetResponseAsync(request, receiver, registry, udpSocket).GetAwaiter().GetResult();
        }

        try
        {
            return GetResponseAsync(request, receiver, registry, udpSocket)
                .WaitAsync(TimeSpan.FromMilliseconds(timeout))
                .GetAwaiter()
                .GetResult();
        }
        catch (System.TimeoutException ex)
        {
            throw new TimeoutException($"SNMP request timed out after {timeout} milliseconds.", ex);
        }
    }

    /// <summary>
    /// Sends an SNMP request and handles the response asynchronously using a provided socket.
    /// </summary>
    public static Task<ISnmpMessage> GetResponseAsync(this ISnmpMessage request, IPEndPoint receiver, Socket udpSocket)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (receiver == null)
        {
            throw new ArgumentNullException(nameof(receiver));
        }

        if (udpSocket == null)
        {
            throw new ArgumentNullException(nameof(udpSocket));
        }

        var registry = new UserRegistry();
        if (request.ProtocolVersion == VersionCode.V3)
        {
            var privacy = request is ILegacyV3Request v3Request
                ? v3Request.Privacy
                : new Lextm.SharpSnmpLib.Security.DefaultPrivacyProvider();
            registry.Add(request.Parameters.UserName, privacy);
        }

        return request.GetResponseAsync(receiver, registry, udpSocket);
    }

    /// <summary>
    /// Sends an SNMP request and handles the response asynchronously using a provided socket.
    /// </summary>
    public static async Task<ISnmpMessage> GetResponseAsync(this ISnmpMessage request, IPEndPoint receiver, Socket udpSocket, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await request.GetResponseAsync(receiver, udpSocket).WaitAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends an SNMP request and handles the response asynchronously using a provided socket and user registry.
    /// </summary>
    public static async Task<ISnmpMessage> GetResponseAsync(this ISnmpMessage request, IPEndPoint receiver, UserRegistry registry, Socket udpSocket)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (receiver == null)
        {
            throw new ArgumentNullException(nameof(receiver));
        }

        if (registry == null)
        {
            throw new ArgumentNullException(nameof(registry));
        }

        if (udpSocket == null)
        {
            throw new ArgumentNullException(nameof(udpSocket));
        }

        var requestCode = request.TypeCode();
        if (requestCode == SnmpType.TrapV1Pdu || requestCode == SnmpType.TrapV2Pdu || requestCode == SnmpType.ReportPdu)
        {
            throw new InvalidOperationException($"not a request message: {requestCode}");
        }

        var payload = request.ToBytes();
        udpSocket.ReceiveBufferSize = Messenger.MaxMessageSize;

        try
        {
            await udpSocket.SendToAsync(payload, SocketFlags.None, receiver).ConfigureAwait(false);
        }
        catch (SocketException ex) when (ex.SocketErrorCode == SocketError.TimedOut || ex.SocketErrorCode == SocketError.WouldBlock)
        {
            throw TimeoutException.Create(receiver.Address, 0);
        }

        var reply = new byte[udpSocket.ReceiveBufferSize];
        EndPoint remote = udpSocket.AddressFamily == AddressFamily.InterNetworkV6
            ? new IPEndPoint(IPAddress.IPv6Any, 0)
            : new IPEndPoint(IPAddress.Any, 0);

        SocketReceiveFromResult result;
        try
        {
            result = await udpSocket.ReceiveFromAsync(reply, SocketFlags.None, remote).ConfigureAwait(false);
        }
        catch (SocketException ex) when (ex.SocketErrorCode == SocketError.TimedOut || ex.SocketErrorCode == SocketError.WouldBlock)
        {
            throw TimeoutException.Create(receiver.Address, 0);
        }

        var response = MessageFactory.ParseMessages(reply, 0, result.ReceivedBytes, registry, throwOnV3SecurityError: true)[0];
        var responseCode = response.TypeCode();
        if (responseCode != SnmpType.ResponsePdu && responseCode != SnmpType.ReportPdu)
        {
            throw new SnmpException($"wrong response type: {responseCode}");
        }

        var requestId = request.MessageId();
        var responseId = response.MessageId();
        if (responseId != requestId)
        {
            throw new SnmpException($"wrong response sequence: expected {requestId}, received {responseId}");
        }

        return response;
    }

    /// <summary>
    /// Sends an SNMP request and handles the response asynchronously using a provided socket and user registry.
    /// </summary>
    public static async Task<ISnmpMessage> GetResponseAsync(this ISnmpMessage request, IPEndPoint receiver, UserRegistry registry, Socket udpSocket, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await request.GetResponseAsync(receiver, registry, udpSocket).WaitAsync(cancellationToken).ConfigureAwait(false);
    }

    private static async Task<ISnmpMessage> GetResponseAsync(ISnmpMessage request, IPEndPoint receiver)
    {
        using var transport = new BasicUdpTransport(receiver);
        await transport.SendAsync(request.ToBytes(), receiver).ConfigureAwait(false);

        var incoming = await transport.ReceiveAsync(receiver, CancellationToken.None).ConfigureAwait(false);
        return ParseResponse(incoming, request);
    }

    private static ISnmpMessage ParseResponse(ReadOnlyMemory<byte> incoming, ISnmpMessage request)
    {
        var versionReader = new AsnReader(incoming, AsnEncodingRules.BER);
        var root = versionReader.ReadSequence();
        if (!root.TryReadInt32(out var versionRaw))
        {
            throw new SnmpException("Cannot parse response version.");
        }

        var version = (VersionCode)versionRaw;
        return version switch
        {
            VersionCode.V1 => SnmpV1Message.ReadFrom(new AsnReader(incoming, AsnEncodingRules.BER)),
            VersionCode.V2 => SnmpV2Message.ReadFrom(new AsnReader(incoming, AsnEncodingRules.BER)),
            VersionCode.V3 => ParseV3Response(incoming, request),
            _ => throw new SnmpException($"Unsupported SNMP version in response: {versionRaw}.")
        };
    }

    private static ISnmpMessage ParseV3Response(ReadOnlyMemory<byte> incoming, ISnmpMessage request)
    {
        var response = SnmpV3Message.ReadFrom(new AsnReader(incoming, AsnEncodingRules.BER));

        if (request is ILegacyV3Request v3Request)
        {
            if (response.Header.MsgFlags.HasFlag(MsgFlag.Auth))
            {
                var authenticated = v3Request.Privacy.AuthenticationProvider.AuthenticateIncomingMsg(response);
                if (!authenticated)
                {
                    throw new SnmpException("Authentication failed for incoming response.");
                }
            }

            if (response.Header.MsgFlags.HasFlag(MsgFlag.Priv))
            {
                v3Request.Privacy.DecryptMessage(response);
            }
        }

        if (response.Scope?.Pdu is ReportPdu)
        {
            return new ReportMessage(response);
        }

        return response;
    }
}
