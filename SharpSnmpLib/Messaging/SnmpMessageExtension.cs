using System.Formats.Asn1;
using System.Net;
using System.Net.Sockets;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Backward-compatible entry point for legacy extension APIs.
/// </summary>
public static class SnmpMessageExtension
{
    /// <summary>
    /// Gets a value indicating whether current runtime is Windows.
    /// </summary>
    public static bool IsRunningOnWindows => OperatingSystem.IsWindows();

    /// <summary>
    /// Gets a value indicating whether current runtime is macOS.
    /// </summary>
    public static bool IsRunningOnMac => OperatingSystem.IsMacOS();

    /// <summary>
    /// Gets a value indicating whether current runtime is iOS.
    /// </summary>
    public static bool IsRunningOnIOS => OperatingSystem.IsIOS();

    /// <summary>Gets message PDU type.</summary>
    public static SnmpType TypeCode(this ISnmpMessage message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));
        return message.Scope?.Pdu?.TypeCode ?? SnmpType.Unknown;
    }

    /// <summary>Gets message variable bindings.</summary>
    public static IList<Variable> Variables(this ISnmpMessage message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));
        return message.Scope?.Pdu?.VariableBindings?.ToList() ?? new List<Variable>();
    }

    /// <summary>Gets request id.</summary>
    public static int RequestId(this ISnmpMessage message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));
        return message.Scope?.RequestId ?? 0;
    }

    /// <summary>Gets message id.</summary>
    public static int MessageId(this ISnmpMessage message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));
        return message.ProtocolVersion == VersionCode.V3
            ? message.Header.MessageId
            : message.RequestId();
    }

    /// <summary>Gets the community string from the message.</summary>
    public static OctetString Community(this ISnmpMessage message)
    {
        if (message is GetRequestMessage grm)
        {
            return grm.Parameters.UserName;
        }

        return SecurityParameters.FromMessage(message).UserName;
    }

    /// <summary>Gets the PDU from the message wrapped in legacy facade.</summary>
    public static ISnmpPdu Pdu(this ISnmpMessage message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));
        var pdu = message.Scope?.Pdu;
        if (pdu == null) throw new SnmpException("The SNMP message does not contain a scoped PDU.");
        return new LegacyPdu(pdu);
    }

    /// <summary>Synchronously sends the message to the manager.</summary>
    public static void Send(this ISnmpMessage message, EndPoint manager)
    {
        SendAsync(message, manager).GetAwaiter().GetResult();
    }

    /// <summary>Synchronously sends the message to the manager using a socket.</summary>
    public static void Send(this ISnmpMessage message, EndPoint manager, Socket socket)
    {
        SendAsync(message, manager, socket).GetAwaiter().GetResult();
    }

    /// <summary>Asynchronously sends the message to the manager.</summary>
    public static async Task SendAsync(this ISnmpMessage message, EndPoint manager)
    {
        using var socket = new Socket(manager.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
        await SendAsync(message, manager, socket, CancellationToken.None).ConfigureAwait(false);
    }

    /// <summary>Asynchronously sends the message to the manager using a socket.</summary>
    public static async Task SendAsync(this ISnmpMessage message, EndPoint manager, Socket socket)
    {
        await SendAsync(message, manager, socket, CancellationToken.None).ConfigureAwait(false);
    }

    /// <summary>Asynchronously sends the message to the manager with cancellation.</summary>
    public static async Task SendAsync(this ISnmpMessage message, EndPoint manager, CancellationToken token)
    {
        using var socket = new Socket(manager.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
        await SendAsync(message, manager, socket, token).ConfigureAwait(false);
    }

    /// <summary>Asynchronously sends the message to the manager using a socket with cancellation.</summary>
    public static async Task SendAsync(this ISnmpMessage message, EndPoint manager, Socket socket, CancellationToken token)
    {
        var bytes = message.Encode();
        await socket.SendToAsync(bytes, SocketFlags.None, manager, token).ConfigureAwait(false);
    }

    /// <summary>Synchronously sends and receives a response.</summary>
    public static ISnmpMessage GetResponse(this ISnmpMessage request, int timeout, IPEndPoint receiver)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        if (receiver == null) throw new ArgumentNullException(nameof(receiver));
        if (timeout < -1) throw new ArgumentOutOfRangeException(nameof(timeout));

        if (timeout == 0 || timeout == -1)
            return GetResponseAsync(request, receiver).GetAwaiter().GetResult();

        try
        {
            return GetResponseAsync(request, receiver)
                .WaitAsync(TimeSpan.FromMilliseconds(timeout))
                .GetAwaiter().GetResult();
        }
        catch (System.TimeoutException ex)
        {
            throw new TimeoutException($"SNMP request timed out after {timeout} milliseconds.", ex);
        }
    }

    /// <summary>Synchronously sends and receives a response using a registry.</summary>
    public static ISnmpMessage GetResponse(this ISnmpMessage request, int timeout, IPEndPoint receiver, UserRegistry registry)
        => GetResponseAsync(request, receiver, registry, CancellationToken.None).GetAwaiter().GetResult();

    /// <summary>Synchronously sends and receives a response using a socket.</summary>
    public static ISnmpMessage GetResponse(this ISnmpMessage request, int timeout, IPEndPoint receiver, Socket udpSocket)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        if (receiver == null) throw new ArgumentNullException(nameof(receiver));
        if (udpSocket == null) throw new ArgumentNullException(nameof(udpSocket));

        var registry = new UserRegistry();
        if (request.ProtocolVersion == VersionCode.V3)
        {
            var privacy = request is ILegacyV3Request v3Req ? v3Req.Privacy : new DefaultPrivacyProvider();
            registry.Add(request.Parameters.UserName, privacy);
        }

        return GetResponse(request, timeout, receiver, registry, udpSocket);
    }

    /// <summary>Synchronously sends and receives a response using a registry and socket.</summary>
    public static ISnmpMessage GetResponse(this ISnmpMessage request, int timeout, IPEndPoint receiver, UserRegistry registry, Socket udpSocket)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        if (receiver == null) throw new ArgumentNullException(nameof(receiver));
        if (registry == null) throw new ArgumentNullException(nameof(registry));
        if (udpSocket == null) throw new ArgumentNullException(nameof(udpSocket));
        if (timeout < -1) throw new ArgumentOutOfRangeException(nameof(timeout));

        if (timeout == 0 || timeout == -1)
            return GetResponseAsync(request, receiver, registry, udpSocket).GetAwaiter().GetResult();

        try
        {
            return GetResponseAsync(request, receiver, registry, udpSocket)
                .WaitAsync(TimeSpan.FromMilliseconds(timeout))
                .GetAwaiter().GetResult();
        }
        catch (System.TimeoutException ex)
        {
            throw new TimeoutException($"SNMP request timed out after {timeout} milliseconds.", ex);
        }
    }

    /// <summary>Asynchronously sends and receives a response.</summary>
    public static Task<ISnmpMessage> GetResponseAsync(this ISnmpMessage request, IPEndPoint receiver)
        => GetResponseAsync(request, receiver, new UserRegistry(), CancellationToken.None);

    /// <summary>Asynchronously sends and receives a response with cancellation.</summary>
    public static Task<ISnmpMessage> GetResponseAsync(this ISnmpMessage request, IPEndPoint receiver, CancellationToken token)
        => GetResponseAsync(request, receiver, new UserRegistry(), token);

    /// <summary>Asynchronously sends and receives a response using a registry.</summary>
    public static Task<ISnmpMessage> GetResponseAsync(this ISnmpMessage request, IPEndPoint receiver, UserRegistry registry)
        => GetResponseAsync(request, receiver, registry, CancellationToken.None);

    /// <summary>Asynchronously sends and receives a response using a registry with cancellation.</summary>
    public static async Task<ISnmpMessage> GetResponseAsync(this ISnmpMessage request, IPEndPoint receiver, UserRegistry registry, CancellationToken token)
    {
        throw new NotImplementedException("Use SnmpDispatcher.SendPdu instead.");
    }

    /// <summary>Asynchronously sends and receives a response using a socket.</summary>
    public static Task<ISnmpMessage> GetResponseAsync(this ISnmpMessage request, IPEndPoint receiver, Socket udpSocket)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        if (receiver == null) throw new ArgumentNullException(nameof(receiver));
        if (udpSocket == null) throw new ArgumentNullException(nameof(udpSocket));

        var registry = new UserRegistry();
        if (request.ProtocolVersion == VersionCode.V3)
        {
            var privacy = request is ILegacyV3Request v3Req ? v3Req.Privacy : new DefaultPrivacyProvider();
            registry.Add(request.Parameters.UserName, privacy);
        }

        return GetResponseAsync(request, receiver, registry, udpSocket);
    }

    /// <summary>Asynchronously sends and receives a response using a socket with cancellation.</summary>
    public static async Task<ISnmpMessage> GetResponseAsync(this ISnmpMessage request, IPEndPoint receiver, Socket udpSocket, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await GetResponseAsync(request, receiver, udpSocket).WaitAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Asynchronously sends and receives a response using a registry and socket.</summary>
    public static async Task<ISnmpMessage> GetResponseAsync(this ISnmpMessage request, IPEndPoint receiver, UserRegistry registry, Socket udpSocket)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        if (receiver == null) throw new ArgumentNullException(nameof(receiver));
        if (registry == null) throw new ArgumentNullException(nameof(registry));
        if (udpSocket == null) throw new ArgumentNullException(nameof(udpSocket));

        var requestCode = request.TypeCode();
        if (requestCode == SnmpType.TrapV1Pdu || requestCode == SnmpType.TrapV2Pdu || requestCode == SnmpType.ReportPdu)
            throw new InvalidOperationException($"not a request message: {requestCode}");

        var payload = request.Encode();
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
            throw new SnmpException($"wrong response type: {responseCode}");

        var requestId = request.MessageId();
        var responseId = response.MessageId();
        if (responseId != requestId)
            throw new SnmpException($"wrong response sequence: expected {requestId}, received {responseId}");

        return response;
    }

    /// <summary>Asynchronously sends and receives a response using a registry, socket, and cancellation.</summary>
    public static async Task<ISnmpMessage> GetResponseAsync(this ISnmpMessage request, IPEndPoint receiver, UserRegistry registry, Socket udpSocket, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await GetResponseAsync(request, receiver, registry, udpSocket).WaitAsync(cancellationToken).ConfigureAwait(false);
    }
}
