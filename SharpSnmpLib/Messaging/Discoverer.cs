// Discoverer class for SNMP agent discovery.
// Copyright (C) 2008-2018 Malcolm Crowe, Lex Li, and other contributors.
// Copyright (C) 2018-2026 LeXtudio Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System.Buffers;
using System.Formats.Asn1;
using System.Net;
using System.Net.Sockets;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Represents the AgentFoundEventArgs type.
/// </summary>
public sealed class AgentFoundEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of AgentFoundEventArgs.
    /// </summary>
    public AgentFoundEventArgs(IPEndPoint agent, AgentVariable? variable)
    {
        Agent = agent ?? throw new ArgumentNullException(nameof(agent));
        Variable = variable;
    }

    /// <summary>
    /// Gets agent.
    /// </summary>
    public IPEndPoint Agent { get; }

    /// <summary>
    /// Gets variable.
    /// </summary>
    public AgentVariable? Variable { get; }
}

/// <summary>
/// Represents the AgentVariable type.
/// </summary>
public sealed class AgentVariable
{
    /// <summary>
    /// Initializes a new instance of AgentVariable.
    /// </summary>
    public AgentVariable(ObjectIdentifier id, ISnmpData data)
    {
        Id = id;
        Data = data;
    }

    /// <summary>
    /// Gets id.
    /// </summary>
    public ObjectIdentifier Id { get; }

    /// <summary>
    /// Gets data.
    /// </summary>
    public ISnmpData Data { get; }

    /// <summary>
    /// Returns a string representation of the current value.
    /// </summary>
    public override string ToString()
    {
        return $"{Id} = {Data}";
    }
}

/// <summary>
/// Discoverer class to discover SNMP agents in the same network.
/// </summary>
public sealed class Discoverer
{
    /// <summary>
    /// Occurs when an SNMP agent is found.
    /// </summary>
    public event EventHandler<AgentFoundEventArgs>? AgentFound;

    /// <summary>
    /// Discovers agents of the specified version in a specific time interval.
    /// </summary>
    public void Discover(VersionCode version, IPEndPoint broadcastAddress, OctetString? community, int timeout)
    {
        DiscoverAsync(version, broadcastAddress, community, timeout, OctetString.Empty).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Discovers agents of the specified version in a specific time interval.
    /// </summary>
    public void Discover(VersionCode version, IPEndPoint broadcastAddress, OctetString? community, int timeout, OctetString contextName)
    {
        DiscoverAsync(version, broadcastAddress, community, timeout, contextName).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Discovers agents of the specified version using the specified transport.
    /// </summary>
    public void Discover(VersionCode version, IPEndPoint endpoint, OctetString? community, int timeout, ISnmpTransport transport)
    {
        DiscoverAsync(version, endpoint, community, timeout, OctetString.Empty, transport).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Discovers agents of the specified version using the specified transport.
    /// </summary>
    public void Discover(VersionCode version, IPEndPoint endpoint, OctetString? community, int timeout, OctetString contextName, ISnmpTransport transport)
    {
        DiscoverAsync(version, endpoint, community, timeout, contextName, transport).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Discovers agents of the specified version in a specific time interval.
    /// </summary>
    public async Task DiscoverAsync(VersionCode version, IPEndPoint broadcastAddress, OctetString? community, int timeout)
    {
        await DiscoverAsync(version, broadcastAddress, community, timeout, OctetString.Empty).ConfigureAwait(false);
    }

    /// <summary>
    /// Discovers agents of the specified version using the specified transport.
    /// </summary>
    /// <remarks>
    /// This overload is intended for unicast probing scenarios. For UDP broadcast discovery,
    /// use the socket-based overloads.
    /// </remarks>
    public async Task DiscoverAsync(
        VersionCode version,
        IPEndPoint endpoint,
        OctetString? community,
        int timeout,
        ISnmpTransport transport)
    {
        await DiscoverAsync(version, endpoint, community, timeout, OctetString.Empty, transport).ConfigureAwait(false);
    }

    /// <summary>
    /// Discovers agents of the specified version in a specific time interval.
    /// </summary>
    public async Task DiscoverAsync(VersionCode version, IPEndPoint broadcastAddress, OctetString? community, int timeout, OctetString contextName)
    {
        if (broadcastAddress == null)
        {
            throw new ArgumentNullException(nameof(broadcastAddress));
        }

        if (timeout < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout));
        }

        var addressFamily = broadcastAddress.AddressFamily;
        var any = addressFamily == AddressFamily.InterNetworkV6
            ? IPAddress.IPv6Any
            : IPAddress.Any;

        using var socket = new Socket(addressFamily, SocketType.Dgram, ProtocolType.Udp);
        if (addressFamily == AddressFamily.InterNetwork)
        {
            socket.EnableBroadcast = true;
        }

        socket.Bind(new IPEndPoint(any, 0));

        var probe = CreateProbe(version, community, contextName);
        await socket.SendToAsync(probe.AsMemory(), SocketFlags.None, broadcastAddress).ConfigureAwait(false);

        using var cts = new CancellationTokenSource(timeout);
        var senderAddress = new SocketAddress(addressFamily);
        var buffer = ArrayPool<byte>.Shared.Rent(65535);
        try
        {
            while (!cts.IsCancellationRequested)
            {
                int bytesReceived;
                try
                {
                    bytesReceived = await socket.ReceiveFromAsync(
                        buffer.AsMemory(),
                        SocketFlags.None,
                        senderAddress,
                        cts.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionReset)
                {
                    continue;
                }

                var remoteEndPoint = (IPEndPoint)new IPEndPoint(
                    addressFamily == AddressFamily.InterNetworkV6 ? IPAddress.IPv6Any : IPAddress.Any,
                    0).Create(senderAddress);

                var variable = TryExtractVariable(buffer.AsSpan(0, bytesReceived));
                AgentFound?.Invoke(this, new AgentFoundEventArgs(remoteEndPoint, variable));
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    /// <summary>
    /// Discovers agents of the specified version using the specified transport.
    /// </summary>
    /// <remarks>
    /// This overload is intended for unicast probing scenarios. For UDP broadcast discovery,
    /// use the socket-based overloads.
    /// </remarks>
    public async Task DiscoverAsync(
        VersionCode version,
        IPEndPoint endpoint,
        OctetString? community,
        int timeout,
        OctetString contextName,
        ISnmpTransport transport)
    {
        if (endpoint == null)
        {
            throw new ArgumentNullException(nameof(endpoint));
        }

        if (transport == null)
        {
            throw new ArgumentNullException(nameof(transport));
        }

        if (timeout < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout));
        }

        var probe = CreateProbe(version, community, contextName);
        using var cts = new CancellationTokenSource(timeout);

        await transport.SendAsync(probe.AsMemory(), endpoint, cts.Token).ConfigureAwait(false);

        while (!cts.IsCancellationRequested)
        {
            try
            {
                var incoming = await transport.ReceiveAsync(endpoint, cts.Token).ConfigureAwait(false);
                var variable = TryExtractVariable(incoming.Span);
                AgentFound?.Invoke(this, new AgentFoundEventArgs(endpoint, variable));

                // TCP discovery is request/response; a single parsed response is enough.
                if (transport is BasicTcpTransport)
                {
                    break;
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }

    private static byte[] CreateProbe(VersionCode version, OctetString? community, OctetString contextName)
    {
        if (version == VersionCode.V3)
        {
            return Messenger.GetNextDiscovery(SnmpType.GetRequestPdu, contextName).ToBytes();
        }

        var pdu = new GetRequestPdu
        {
            RequestId = Messenger.NextRequestId,
            VariableBindings = new VarBindList(new Variable("1.3.6.1.2.1.1.1.0"))
        };

        ISnmpMessage message = version switch
        {
            VersionCode.V1 => new SnmpV1Message
            {
                Community = community ?? OctetString.Empty,
                Scope = pdu
            },
            VersionCode.V2 => new SnmpV2Message
            {
                Community = community ?? OctetString.Empty,
                Scope = pdu
            },
            _ => throw new NotSupportedException($"Unsupported discovery version: {version}")
        };

        return message.Encode();
    }

    private static AgentVariable? TryExtractVariable(ReadOnlySpan<byte> packet)
    {
        if (packet.Length == 0)
        {
            return null;
        }

        // AsnReader requires byte[], so copy from the span.
        var bytes = packet.ToArray();
        try
        {
            var reader = new AsnReader(bytes, AsnEncodingRules.BER);
            var seq = reader.ReadSequence();
            if (!seq.TryReadInt32(out var versionRaw))
            {
                return null;
            }

            var version = (VersionCode)versionRaw;
            switch (version)
            {
                case VersionCode.V1:
                    {
                        var msg = SnmpV1Message.ReadFrom(new AsnReader(bytes, AsnEncodingRules.BER));
                        return Convert(msg.Scope?.VariableBindings?.FirstOrDefault());
                    }
                case VersionCode.V2:
                    {
                        var msg = SnmpV2Message.ReadFrom(new AsnReader(bytes, AsnEncodingRules.BER));
                        return Convert(msg.Scope?.VariableBindings?.FirstOrDefault());
                    }
                case VersionCode.V3:
                    {
                        var msg = SnmpV3Message.ReadFrom(new AsnReader(bytes, AsnEncodingRules.BER));
                        if (msg.Header.MsgFlags.HasFlag(MsgFlag.Priv))
                        {
                            return null;
                        }

                        return Convert(msg.Scope?.VariableBindings?.FirstOrDefault());
                    }
                default:
                    return null;
            }
        }
        catch
        {
            return null;
        }
    }

    private static AgentVariable? Convert(Variable? variable)
    {
        if (variable == null)
        {
            return null;
        }

        var value = variable.Value;
        return new AgentVariable(value.Id, value.Data);
    }
}
