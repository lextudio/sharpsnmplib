using System.Net;
using System.Text;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V3.Security.Authentication;
using DotNetSnmp.Protocol.V3.Security.Privacy;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Xunit;

namespace DotNetSnmp.Test;

public sealed class MessengerCompatibilityTest
{
    [Fact]
    public void GetWithInvalidTimeoutThrows()
    {
        var variables = new List<Variable> { new(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) };
        var endpoint = new IPEndPoint(IPAddress.Loopback, 161);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Messenger.Get(VersionCode.V2, endpoint, new OctetString("public"), variables, -2));
    }

    [Fact]
    public async Task SendTrapV2AsyncWithNonIPEndPointThrows()
    {
        var variables = new List<Variable>();
        var receiver = new DnsEndPoint("localhost", 162);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            Messenger.SendTrapV2Async(
                requestId: 1,
                version: VersionCode.V2,
                receiver: receiver,
                community: new OctetString("public"),
                enterprise: new ObjectIdentifier("1.3.6.1.6.3.1.1.5.2"),
                timestamp: 1,
                variables: variables));
    }

    [Fact]
    public async Task SendTrapV2AsyncRejectsNonV2()
    {
        var variables = new List<Variable>();
        var endpoint = new IPEndPoint(IPAddress.Loopback, 162);

        await Assert.ThrowsAsync<NotSupportedException>(() =>
            Messenger.SendTrapV2Async(
                requestId: 1,
                version: VersionCode.V1,
                endpoint: endpoint,
                community: new OctetString("public"),
                contextName: OctetString.Empty,
                enterprise: new ObjectIdentifier("1.3.6.1.6.3.1.1.5.2"),
                timestamp: 1,
                variables: variables));
    }

    [Fact]
    public async Task SendTrapV1AsyncWithNonIPEndPointThrows()
    {
        var variables = new List<Variable>();
        var receiver = new DnsEndPoint("localhost", 162);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            Messenger.SendTrapV1Async(
                receiver: receiver,
                agent: IPAddress.Loopback,
                community: new OctetString("public"),
                enterprise: new ObjectIdentifier("1.3.6.1.6.3.1.1.5.2"),
                generic: GenericCode.EnterpriseSpecific,
                specific: 1,
                timestamp: 1,
                variables: variables));
    }

    [Fact]
    public async Task SendTrapV1AsyncNullGuardsArePresent()
    {
        var receiver = new IPEndPoint(IPAddress.Loopback, 162);
        var community = new OctetString("public");
        var enterprise = new ObjectIdentifier("1.3.6.1.6.3.1.1.5.2");
        var variables = new List<Variable>();

        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            Messenger.SendTrapV1Async(receiver, null!, community, enterprise, GenericCode.ColdStart, 0, 1, variables));
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            Messenger.SendTrapV1Async(receiver, IPAddress.Loopback, community, enterprise, GenericCode.ColdStart, 0, 1, null!));
    }

    [Fact]
    public void SendTrapV2WithNonIPEndPointThrows()
    {
        var variables = new List<Variable>();
        var receiver = new DnsEndPoint("localhost", 162);

        Assert.Throws<ArgumentException>(() =>
            Messenger.SendTrapV2(
                requestId: 1,
                version: VersionCode.V2,
                receiver: receiver,
                community: new OctetString("public"),
                enterprise: new ObjectIdentifier("1.3.6.1.6.3.1.1.5.2"),
                timestamp: 1,
                variables: variables));
    }

    [Fact]
    public void SendTrapV1WithNonIPEndPointThrows()
    {
        var variables = new List<Variable>();
        var receiver = new DnsEndPoint("localhost", 162);

        Assert.Throws<ArgumentException>(() =>
            Messenger.SendTrapV1(
                receiver: receiver,
                agent: IPAddress.Loopback,
                community: new OctetString("public"),
                enterprise: new ObjectIdentifier("1.3.6.1.6.3.1.1.5.2"),
                generic: GenericCode.EnterpriseSpecific,
                specific: 1,
                timestamp: 1,
                variables: variables));
    }

    [Fact]
    public void SendTrapV2RejectsNonV2()
    {
        var variables = new List<Variable>();
        var endpoint = new IPEndPoint(IPAddress.Loopback, 162);

        Assert.Throws<NotSupportedException>(() =>
            Messenger.SendTrapV2(
                requestId: 1,
                version: VersionCode.V1,
                receiver: endpoint,
                community: new OctetString("public"),
                enterprise: new ObjectIdentifier("1.3.6.1.6.3.1.1.5.2"),
                timestamp: 1,
                variables: variables));
    }

    [Fact]
    public void GetTableRejectsV3()
    {
        var endpoint = new IPEndPoint(IPAddress.Loopback, 161);
#pragma warning disable CS0618 // Exercising compatibility API surface intentionally.
        Assert.Throws<NotSupportedException>(() =>
            Messenger.GetTable(
                version: VersionCode.V3,
                endpoint: endpoint,
                community: new OctetString("public"),
                table: new ObjectIdentifier("1.3.6.1.2.1.1"),
                timeout: 1000,
                maxRepetitions: 10));
#pragma warning restore CS0618
    }

    [Fact]
    public void SendInformWithInvalidTimeoutThrows()
    {
        var privacy = new DefaultPrivacyProvider(new SHA1AuthenticationProvider(Encoding.UTF8.GetBytes("password")));
        var variables = new List<Variable>();
        var endpoint = new IPEndPoint(IPAddress.Loopback, 162);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Messenger.SendInform(
                requestId: 1,
                version: VersionCode.V2,
                receiver: endpoint,
                community: new OctetString("public"),
                contextName: OctetString.Empty,
                enterprise: new ObjectIdentifier("1.3.6.1.6.3.1.1.5.2"),
                timestamp: 1,
                variables: variables,
                timeout: -2,
                privacy: privacy,
                report: null!));
    }
}
