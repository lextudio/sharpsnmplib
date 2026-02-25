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

public sealed class AgentFoundEventArgs : EventArgs
{
    public AgentFoundEventArgs(IPEndPoint agent, AgentVariable? variable)
    {
        Agent = agent ?? throw new ArgumentNullException(nameof(agent));
        Variable = variable;
    }

    public IPEndPoint Agent { get; }

    public AgentVariable? Variable { get; }
}

public sealed class AgentVariable
{
    public AgentVariable(ObjectIdentifier id, IAsnSerializable data)
    {
        Id = id;
        Data = data;
    }

    public ObjectIdentifier Id { get; }

    public IAsnSerializable Data { get; }

    public override string ToString()
    {
        return $"{Id} = {Data}";
    }
}

public sealed class Discoverer
{
    public event EventHandler<AgentFoundEventArgs>? AgentFound;

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
                        return Convert(msg.Scope?.VariableBindings?.ToArray().FirstOrDefault());
                    }
                case VersionCode.V2:
                    {
                        var msg = SnmpV2Message.ReadFrom(new AsnReader(packet, AsnEncodingRules.BER));
                        return Convert(msg.Scope?.VariableBindings?.ToArray().FirstOrDefault());
                    }
                case VersionCode.V3:
                    {
                        var msg = SnmpV3Message.ReadFrom(new AsnReader(packet, AsnEncodingRules.BER));
                        if (msg.Header.MsgFlags.HasFlag(MsgFlags.Priv))
                        {
                            return null;
                        }

                        return Convert(msg.Scope?.VariableBindings?.ToArray().FirstOrDefault());
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
