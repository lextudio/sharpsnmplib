using System.Net;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Test;
using Lextm.SharpSnmpLib.Security;
using Lextm.SharpSnmpLib.Messaging;
using Xunit;

namespace DotNetSnmp.Test
{
    public class IntegrationTest
    {
        private static readonly IPEndPoint _dummyEndPoint = new(IPAddress.Loopback, 161);
        private static readonly OctetString _publicCommunity = new("public");

        [Fact]
        public async Task TestMessengerGetAsyncV1_Mock()
        {
            var version = VersionCode.V1;
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
                new(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")),
                new(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))
            };

            using var mock = new MockTransport();
            // Queue a response that will be built after the request is captured
            mock.ResponseBuilder = sentBytes => MockTransport.BuildGetResponse(
                sentBytes, version, community,
                new Variable("1.3.6.1.2.1.1.1.0", new OctetString("Test system")),
                new Variable("1.3.6.1.2.1.1.2.0", new ObjectIdentifier("1.3.6.1.4.1.123")));

            var result = await Messenger.GetAsync(version, _dummyEndPoint, community, variables, mock);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());
            Assert.Equal("Test system", result[0].Data.ToString());
            Assert.Equal("1.3.6.1.4.1.123", result[1].Data.ToString());
        }

        [Fact]
        public async Task TestMessengerGetAsyncV2_Mock()
        {
            var version = VersionCode.V2;
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
                new(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")),
                new(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))
            };

            using var mock = new MockTransport();
            mock.ResponseBuilder = sentBytes => MockTransport.BuildGetResponse(
                sentBytes, version, community,
                new Variable("1.3.6.1.2.1.1.1.0", new OctetString("Test system")),
                new Variable("1.3.6.1.2.1.1.2.0", new ObjectIdentifier("1.3.6.1.4.1.123")));

            var result = await Messenger.GetAsync(version, _dummyEndPoint, community, variables, mock);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());
            Assert.Equal("Test system", result[0].Data.ToString());
            Assert.Equal("1.3.6.1.4.1.123", result[1].Data.ToString());
        }

        [Fact]
        public async Task TestMessengerSetAsyncV1_Mock()
        {
            var expected = "Test" + new Random().Next(1, 1000);
            var version = VersionCode.V1;
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
                new(new ObjectIdentifier("1.3.6.1.2.1.1.5.0"), new OctetString(expected)),
            };

            using var mock = new MockTransport();
            mock.ResponseBuilder = sentBytes => MockTransport.BuildGetResponse(
                sentBytes, version, community,
                new Variable("1.3.6.1.2.1.1.5.0", new OctetString(expected)));

            var result = await Messenger.SetAsync(version, _dummyEndPoint, community, variables, mock);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("1.3.6.1.2.1.1.5.0", result[0].Id.ToString());
            Assert.Equal(expected, result[0].Data.ToString());
        }

        [Fact]
        public async Task TestMessengerSetAsyncV2_Mock()
        {
            var expected = "Test" + new Random().Next(1, 1000);
            var version = VersionCode.V2;
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
                new(new ObjectIdentifier("1.3.6.1.2.1.1.5.0"), new OctetString(expected)),
            };

            using var mock = new MockTransport();
            mock.ResponseBuilder = sentBytes => MockTransport.BuildGetResponse(
                sentBytes, version, community,
                new Variable("1.3.6.1.2.1.1.5.0", new OctetString(expected)));

            var result = await Messenger.SetAsync(version, _dummyEndPoint, community, variables, mock);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("1.3.6.1.2.1.1.5.0", result[0].Id.ToString());
            Assert.Equal(expected, result[0].Data.ToString());
        }

        [Fact]
        public async Task TestMessengerGetAsyncV1()
        {
            var version = VersionCode.V1;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
                new(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")),
                new(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))
            };

            var result = await Messenger.GetAsync(version, endpoint, community, variables);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());
        }

        [Fact]
        public async Task TestMessengerGetAsyncV2()
        {
            var version = VersionCode.V2;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
                new(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")),
                new(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))
            };

            var result = await Messenger.GetAsync(version, endpoint, community, variables);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());
        }

        [Fact]
        public async Task TestMessengerSetAsyncV1()
        {
            var expected = "Test" + new Random().Next(1, 1000);
            var version = VersionCode.V1;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
                new(new ObjectIdentifier("1.3.6.1.2.1.1.5.0"), new OctetString(expected)),
            };

            var result = await Messenger.SetAsync(version, endpoint, community, variables);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("1.3.6.1.2.1.1.5.0", result[0].Id.ToString());
            Assert.Equal(expected, result[0].Data.ToString());
        }

        [Fact]
        public async Task TestMessengerSetAsyncV2()
        {
            var expected = "Test" + new Random().Next(1, 1000);
            var version = VersionCode.V2;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
                new(new ObjectIdentifier("1.3.6.1.2.1.1.5.0"), new OctetString(expected)),
            };

            var result = await Messenger.SetAsync(version, endpoint, community, variables);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("1.3.6.1.2.1.1.5.0", result[0].Id.ToString());
            Assert.Equal(expected, result[0].Data.ToString());
        }

        [Fact]
        public async Task TestMessengerWalkAsyncV1()
        {
            var version = VersionCode.V1;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
            };

            var result = await Messenger.WalkAsync(version, endpoint, community, new ObjectIdentifier("1.3.6.1.2.1.1"), variables, WalkMode.WithinSubtree, TestContext.Current.CancellationToken);

            Assert.Equal(16, variables.Count);
        }

        [Fact]
        public async Task TestMessengerWalkAsyncV1_Full()
        {
            var version = VersionCode.V1;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
            };

            var result = await Messenger.WalkAsync(version, endpoint, community, new ObjectIdentifier("1.3.6.1.2.1.1"), variables, WalkMode.Default, TestContext.Current.CancellationToken);

            Assert.True(variables.Count > 16);
        }

        [Fact]
        public async Task TestMessengerWalkAsyncV2()
        {
            var version = VersionCode.V2;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
            };

            var result = await Messenger.WalkAsync(version, endpoint, community, new ObjectIdentifier("1.3.6.1.2.1.1"), variables, WalkMode.WithinSubtree, TestContext.Current.CancellationToken);

            Assert.Equal(16, variables.Count);
        }

        [Fact]
        public async Task TestMessengerWalkAsyncV2_Full()
        {
            var version = VersionCode.V2;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
            };

            var result = await Messenger.WalkAsync(version, endpoint, community, new ObjectIdentifier("1.3.6.1.2.1.1"), variables, WalkMode.Default, TestContext.Current.CancellationToken);

            Assert.True(variables.Count > 16);
        }

        [Fact]
        public async Task TestMessengerBulkWalkAsyncV2()
        {
            var version = VersionCode.V2;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
            };

            var result = await Messenger.BulkWalkAsync(version, endpoint, community, OctetString.Empty, new ObjectIdentifier("1.3.6.1.2.1.1"), variables, 10, WalkMode.WithinSubtree);

            Assert.Equal(16, variables.Count);
        }

        [Fact]
        public async Task TestMessengerBulkWalkAsyncV2_Full()
        {
            var version = VersionCode.V2;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
            };

            var result = await Messenger.BulkWalkAsync(version, endpoint, community, OctetString.Empty, new ObjectIdentifier("1.3.6.1.2.1.1"), variables, 10, WalkMode.Default);

            Assert.True(variables.Count > 16);
        }

        [Fact]
        public async Task TestMessengerInformAsyncV2()
        {
            var version = VersionCode.V2;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 162);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
                new(new ObjectIdentifier("1.3.6.1.2.1.1.1"), new OctetString("Test system")),
            };

            await Messenger.SendInformAsync(848, version, endpoint, community, OctetString.Empty, new ObjectIdentifier("1.3.6.1.6.3.1.1.5.2"), 454, variables);
        }

        [Fact]
        public async Task TestMessengerTrapV2Async()
        {
            var version = VersionCode.V2;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 162);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
                new(new ObjectIdentifier("1.3.6.1.2.1.1.1"), new OctetString("Test system")),
            };

            await Messenger.SendTrapV2Async(848, version, endpoint, community, OctetString.Empty, new ObjectIdentifier("1.3.6.1.6.3.1.1.5.2"), 454, variables);
        }

        [Fact]
        public async Task TestMessengerGetAsyncV3()
        {
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);

            var username = "usr-none-none";

            var variables = new List<Variable>
            {
                new(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")),
                new(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))
            };

            var result = await Messenger.GetV3Async(
                endpoint,
                username,
                variables);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());

            Assert.False(string.IsNullOrEmpty(result[0].Data.ToString()));
            Assert.False(string.IsNullOrEmpty(result[1].Data.ToString()));
        }

        [Fact]
        public async Task TestMessengerGetAsyncV3_SHA1()
        {
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);

            var username = "usr-sha-none";
            var authPassword = new OctetString("authkey1");

            var variables = new List<Variable>
            {
                new(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")),
                new(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))
            };

            var result = await Messenger.GetV3Async(
                endpoint,
                username,
                new DefaultPrivacyProvider(new SHA1AuthenticationProvider(authPassword.Octets)),
                variables);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());

            Assert.False(string.IsNullOrEmpty(result[0].Data.ToString()));
            Assert.False(string.IsNullOrEmpty(result[1].Data.ToString()));
        }

        [Fact]
        public async Task TestMessengerGetAsyncV3_MD5()
        {
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);

            var username = "usr-md5-none";
            var authPassword = new OctetString("authkey1");

            var variables = new List<Variable>
            {
                new(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")),
                new(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))
            };

            var result = await Messenger.GetV3Async(
                endpoint,
                username,
                new DefaultPrivacyProvider(new MD5AuthenticationProvider(authPassword.Octets)),
                variables);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());

            Assert.False(string.IsNullOrEmpty(result[0].Data.ToString()));
            Assert.False(string.IsNullOrEmpty(result[1].Data.ToString()));
        }


        [Fact]
        public async Task TestMessengerGetAsyncV3_MD5_DES()
        {
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);

            var username = "usr-md5-des";
            var authPassword = new OctetString("authkey1");
            var privPassword = new OctetString("privkey1");

            var variables = new List<Variable>
            {
                new(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")),
                new(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))
            };

            var result = await Messenger.GetV3Async(
                endpoint,
                username,
                new DESPrivacyProvider(new MD5AuthenticationProvider(authPassword.Octets), privPassword.Octets),
                variables);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());

            Assert.False(string.IsNullOrEmpty(result[0].Data.ToString()));
            Assert.False(string.IsNullOrEmpty(result[1].Data.ToString()));
        }

        [Fact]
        public async Task TestMessengerGetAsyncV3_MD5_TripleDES()
        {
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);

            var username = "usr-md5-3des";
            var authPassword = new OctetString("authkey1");
            var privPassword = new OctetString("privkey1");

            var variables = new List<Variable>
            {
                new(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")),
                new(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))
            };

            var result = await Messenger.GetV3Async(
                endpoint,
                username,
                new TripleDESPrivacyProvider(new MD5AuthenticationProvider(authPassword.Octets), privPassword.Octets),
                variables);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());

            Assert.False(string.IsNullOrEmpty(result[0].Data.ToString()));
            Assert.False(string.IsNullOrEmpty(result[1].Data.ToString()));
        }


        [Fact]
        public async Task TestMessengerGetAsyncV3_SHA1_AES()
        {
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);

            var username = "usr-sha-aes";
            var authPassword = new OctetString("authkey1");
            var privPassword = new OctetString("privkey1");

            var variables = new List<Variable>
            {
                new(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")),
                new(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))
            };

            var result = await Messenger.GetV3Async(
                endpoint,
                username,
                new AESPrivacyProvider(new SHA1AuthenticationProvider(authPassword.Octets), privPassword.Octets),
                variables);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());

            Assert.False(string.IsNullOrEmpty(result[0].Data.ToString()));
            Assert.False(string.IsNullOrEmpty(result[1].Data.ToString()));
        }

        [Fact]
        public async Task TestMessengerGetAsyncV3_SHA1_AES192()
        {
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);

            var username = "usr-sha-aes192";
            var authPassword = new OctetString("authkey1");
            var privPassword = new OctetString("privkey1");

            var variables = new List<Variable>
            {
                new(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")),
                new(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))
            };

            var result = await Messenger.GetV3Async(
                endpoint,
                username,
                new AES192PrivacyProvider(new SHA1AuthenticationProvider(authPassword.Octets), privPassword.Octets),
                variables);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());

            Assert.False(string.IsNullOrEmpty(result[0].Data.ToString()));
            Assert.False(string.IsNullOrEmpty(result[1].Data.ToString()));
        }

        [Fact]
        public async Task TestMessengerGetAsyncV3_SHA1_AES256()
        {
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);

            var username = "usr-sha-aes256";
            var authPassword = new OctetString("authkey1");
            var privPassword = new OctetString("privkey1");

            var variables = new List<Variable>
            {
                new(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")),
                new(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))
            };

            var result = await Messenger.GetV3Async(
                endpoint,
                username,
                new AES256PrivacyProvider(new SHA1AuthenticationProvider(authPassword.Octets), privPassword.Octets),
                variables);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());

            Assert.False(string.IsNullOrEmpty(result[0].Data.ToString()));
            Assert.False(string.IsNullOrEmpty(result[1].Data.ToString()));
        }

        [Fact]
        public async Task TestMessengerSetAsyncV3_SHA1_AES()
        {
            var expected = "Test" + new Random().Next(1, 1000);
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);

            var username = "usr-sha-aes";
            var authPassword = new OctetString("authkey1");
            var privPassword = new OctetString("privkey1");

            var variables = new List<Variable>
            {
                new(new ObjectIdentifier("1.3.6.1.2.1.1.5.0"), new OctetString(expected)),
            };

            var result = await Messenger.SetV3Async(
                endpoint,
                username,
                new AESPrivacyProvider(new SHA1AuthenticationProvider(authPassword.Octets), privPassword.Octets),
                variables);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("1.3.6.1.2.1.1.5.0", result[0].Id.ToString());
            Assert.Equal(expected, result[0].Data.ToString());
        }

        [Fact]
        public async Task TestMessengerBulkWalkAsyncV3_SHA1_AES()
        {
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);

            var username = "usr-sha-aes";
            var authPassword = new OctetString("authkey1");
            var privPassword = new OctetString("privkey1");

            var variables = new List<Variable>();

            var result = await Messenger.BulkWalkV3Async(
                endpoint,
                username,
                new AESPrivacyProvider(new SHA1AuthenticationProvider(authPassword.Octets), privPassword.Octets),
                new ObjectIdentifier("1.3.6.1.2.1.1"),
                variables,
                10,
                WalkMode.WithinSubtree);

            Assert.NotNull(variables);
            Assert.True(variables.Count >= 16, $"Expected at least 16 variables, got {variables.Count}");

            Assert.Contains(variables, v => v.Id.ToString().StartsWith("1.3.6.1.2.1.1."));

            Assert.False(string.IsNullOrEmpty(variables[0].Data.ToString()));
        }

        [Fact]
        public async Task TestMessengerInformAsyncV3_SHA1_AES()
        {
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 162);

            var username = "usr-sha-aes";
            var authPassword = new OctetString("authkey1");
            var privPassword = new OctetString("privkey1");

            var variables = new List<Variable>
            {
                new(new ObjectIdentifier("1.3.6.1.2.1.1.1.0"), new OctetString("Test system"))
            };

            await Messenger.SendInformV3Async(
                endpoint,
                username,
                new AESPrivacyProvider(new SHA1AuthenticationProvider(authPassword.Octets), privPassword.Octets),
                new ObjectIdentifier("1.3.6.1.6.3.1.1.5.2"),
                (uint)DateTimeOffset.Now.ToUnixTimeSeconds(),
                variables);
        }

        [Fact]
        public async Task TestMessengerTrapV2AsyncV3_SHA1_AES()
        {
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 162);

            var username = "usr-sha-aes";
            var authPassword = new OctetString("authkey1");
            var privPassword = new OctetString("privkey1");

            var sysUpTimeOid = new ObjectIdentifier("1.3.6.1.2.1.1.3.0");
            var sysUpTimeValue = new TimeTicks(12345);
            var trapOid = new ObjectIdentifier("1.3.6.1.6.3.1.1.5.2");
            var snmpTrapOid = new ObjectIdentifier("1.3.6.1.6.3.1.1.4.1.0");

            var ifIndexOid = new ObjectIdentifier("1.3.6.1.2.1.2.2.1.1.0");
            var ifIndexValue = new Integer32(1);
            var ifDescriptionOid = new ObjectIdentifier("1.3.6.1.2.1.2.2.1.2.0");
            var ifDescriptionValue = new OctetString("Test Interface");

            var variables = new List<Variable>
            {
                new(sysUpTimeOid, sysUpTimeValue),
                new(snmpTrapOid, trapOid),
                new(ifIndexOid, ifIndexValue),
                new(ifDescriptionOid, ifDescriptionValue)
            };

            await Messenger.SendTrapV2V3Async(
                endpoint,
                username,
                new AESPrivacyProvider(new SHA1AuthenticationProvider(authPassword.Octets), privPassword.Octets),
                new ObjectIdentifier("1.3.6.1.4.1.12345"),
                (uint)DateTimeOffset.Now.ToUnixTimeSeconds(),
                variables);
        }
    }
}
