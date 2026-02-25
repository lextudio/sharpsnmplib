using System.Formats.Asn1;
using System.Net;
using System.Net.Sockets;
using DotNetSnmp.Asn1;
using DotNetSnmp.Asn1.Serialization;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V1;
using DotNetSnmp.Protocol.V2;
using DotNetSnmp.Protocol.V3;
using DotNetSnmp.Protocol.V3.Security;

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
    public AgentVariable(ObjectIdentifier id, IAsnSerializable data)
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
    public IAsnSerializable Data { get; }

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
    public async Task DiscoverAsync(VersionCode version, IPEndPoint broadcastAddress, OctetString? community, int timeout)
    {
        if (broadcastAddress == null)
        {
            throw new ArgumentNullException(nameof(broadcastAddress));
        }

        if (timeout < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout));
        }

        var any = broadcastAddress.AddressFamily == AddressFamily.InterNetworkV6
            ? IPAddress.IPv6Any
            : IPAddress.Any;

        using var udp = new UdpClient(new IPEndPoint(any, 0));
        if (broadcastAddress.AddressFamily == AddressFamily.InterNetwork)
        {
            udp.EnableBroadcast = true;
        }

        var probe = CreateProbe(version, community);
        await udp.SendAsync(probe, probe.Length, broadcastAddress).ConfigureAwait(false);

        using var cts = new CancellationTokenSource(timeout);
        while (!cts.IsCancellationRequested)
        {
            UdpReceiveResult received;
            try
            {
                received = await udp.ReceiveAsync(cts.Token).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (SocketException)
            {
                break;
            }

            var variable = TryExtractVariable(received.Buffer);
            AgentFound?.Invoke(this, new AgentFoundEventArgs(received.RemoteEndPoint, variable));
        }
    }

    private static byte[] CreateProbe(VersionCode version, OctetString? community)
    {
        if (version == VersionCode.V3)
        {
            return Messenger.GetNextDiscovery(SnmpType.GetRequestPdu).ToBytes();
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

    private static AgentVariable? TryExtractVariable(byte[] packet)
    {
        if (packet == null || packet.Length == 0)
        {
            return null;
        }

        try
        {
            var reader = new AsnReader(packet, AsnEncodingRules.BER);
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
                        var msg = SnmpV1Message.ReadFrom(new AsnReader(packet, AsnEncodingRules.BER));
                        return Convert(msg.Scope?.VariableBindings?.FirstOrDefault());
                    }
                case VersionCode.V2:
                    {
                        var msg = SnmpV2Message.ReadFrom(new AsnReader(packet, AsnEncodingRules.BER));
                        return Convert(msg.Scope?.VariableBindings?.FirstOrDefault());
                    }
                case VersionCode.V3:
                    {
                        var msg = SnmpV3Message.ReadFrom(new AsnReader(packet, AsnEncodingRules.BER));
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
