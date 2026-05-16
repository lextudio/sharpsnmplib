using System.Formats.Asn1;
using System.Net;
// removed: DotNetSnmp.Asn1.Serialization
using Lextm.SharpSnmpLib;
// removed: DotNetSnmp.Common.Definitions
// removed: DotNetSnmp.Protocol.V1
// removed: DotNetSnmp.Protocol.V3
// removed: DotNetSnmp.Transport
using Lextm.SharpSnmpLib.Messaging;
using System.Text;
using Xunit;

namespace DotNetSnmp.Test;

public sealed class DiscoveryCompatibilityTest
{
    [Theory]
    [InlineData(SnmpType.GetRequestPdu, 0)]
    [InlineData(SnmpType.GetNextRequestPdu, 1)]
    [InlineData(SnmpType.GetBulkRequestPdu, 5)]
    [InlineData(SnmpType.SetRequestPdu, 3)]
    [InlineData(SnmpType.InformRequestPdu, 6)]
    public void DiscoveryToBytesEncodesExpectedPduType(SnmpType type, int expectedTagValue)
    {
        var discovery = new Discovery(101, 202, 1500, type);
        var pduTag = ReadScopedPduTag(discovery.ToBytes());

        Assert.Equal(TagClass.ContextSpecific, pduTag.TagClass);
        Assert.Equal(expectedTagValue, pduTag.TagValue);
        Assert.True(pduTag.IsConstructed);
    }

    [Fact]
    public void DiscoveryConstructorRejectsUnsupportedMessageType()
    {
        Assert.Throws<ArgumentException>(() => new Discovery(1, 2, 1500, SnmpType.ResponsePdu));
    }

    [Fact]
    public async Task DiscoveryNullReceiverGuardsArePresent()
    {
        var discovery = new Discovery(1, 2, 1500);

        Assert.Throws<ArgumentNullException>(() => discovery.GetResponse(1000, null!));
        await Assert.ThrowsAsync<ArgumentNullException>(() => discovery.GetResponseAsync(null!));
    }

    [Fact]
    public async Task DiscoveryNullTransportGuardsArePresent()
    {
        var discovery = new Discovery(1, 2, 1500);
        var endpoint = new IPEndPoint(IPAddress.Loopback, 161);

        Assert.Throws<ArgumentNullException>(() => discovery.GetResponse(1000, endpoint, null!));
        await Assert.ThrowsAsync<ArgumentNullException>(() => discovery.GetResponseAsync(endpoint, null!));
    }

    [Fact]
    public async Task DiscovererNullTransportGuardsArePresent()
    {
        var discoverer = new Discoverer();
        var endpoint = new IPEndPoint(IPAddress.Loopback, 161);

        Assert.Throws<ArgumentNullException>(() =>
            discoverer.Discover(VersionCode.V2, endpoint, new OctetString("public"), 100, null!));
        Assert.Throws<ArgumentNullException>(() =>
            discoverer.Discover(VersionCode.V2, endpoint, new OctetString("public"), 100, OctetString.Empty, null!));

        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            discoverer.DiscoverAsync(VersionCode.V2, endpoint, new OctetString("public"), 100, null!));

        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            discoverer.DiscoverAsync(VersionCode.V2, endpoint, new OctetString("public"), 100, OctetString.Empty, null!));
    }

    [Fact]
    public void DiscoveryToStringContainsIds()
    {
        var discovery = new Discovery(88, 99, 1500);
        var value = discovery.ToString();

        Assert.Contains("message id: 88", value);
        Assert.Contains("request id: 99", value);
    }

    [Fact]
    public void MessengerGetNextDiscoveryUsesCompatibilityEnum()
    {
        var discovery = Messenger.GetNextDiscovery(SnmpType.GetRequestPdu);
        var pduTag = ReadScopedPduTag(discovery.ToBytes());

        Assert.Equal(0, pduTag.TagValue);
    }

    [Fact]
    public void DiscoveryEncodesProvidedContextName()
    {
        var discovery = new Discovery(101, 202, 1500, SnmpType.GetRequestPdu, new OctetString("ctx-v3"));
        var contextName = ReadScopedContextName(discovery.ToBytes());

        Assert.Equal("ctx-v3", contextName);
    }

    [Fact]
    public void MessengerGetNextDiscoveryEncodesProvidedContextName()
    {
        var discovery = Messenger.GetNextDiscovery(SnmpType.GetRequestPdu, new OctetString("ctx-v3-messenger"));
        var contextName = ReadScopedContextName(discovery.ToBytes());

        Assert.Equal("ctx-v3-messenger", contextName);
    }

    [Fact]
    public void ReportMessageRequiresReportPdu()
    {
        var nonReportMessage = new SnmpV3Message
        {
            Header = new HeaderData
            {
                MsgId = 1,
                MsgFlags = 0,
                MsgMaxSize = 1500,
                MsgSecurityModel = SecurityModel.Usm
            },
            SecurityParameters = new Lextm.SharpSnmpLib.Security.UsmSecurityParameters
            {
                SecurityName = OctetString.Empty,
                EngineId = Memory<byte>.Empty,
                AuthParams = Memory<byte>.Empty,
                PrivParams = Memory<byte>.Empty
            },
            Scope = new Scope
            {
                ContextEngineId = ReadOnlyMemory<byte>.Empty,
                ContextName = string.Empty,
                Pdu = new GetRequestPdu
                {
                    RequestId = 5,
                    VariableBindings = new VarBindList()
                }
            }
        };

        Assert.Throws<ArgumentException>(() => new ReportMessage(nonReportMessage));
    }

    private static Asn1Tag ReadScopedPduTag(byte[] encoded)
    {
        var reader = new AsnReader(encoded, AsnEncodingRules.BER);
        var root = reader.ReadSequence();
        _ = root.ReadInteger();
        _ = root.ReadSequence();
        _ = root.ReadOctetString();
        var scope = root.ReadSequence();
        _ = scope.ReadOctetString();
        _ = scope.ReadOctetString();
        return scope.PeekTag();
    }

    private static string ReadScopedContextName(byte[] encoded)
    {
        var reader = new AsnReader(encoded, AsnEncodingRules.BER);
        var root = reader.ReadSequence();
        _ = root.ReadInteger();
        _ = root.ReadSequence();
        _ = root.ReadOctetString();
        var scope = root.ReadSequence();
        _ = scope.ReadOctetString();
        return Encoding.UTF8.GetString(scope.ReadOctetString());
    }
}
