using System.Net;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V3.Security.Authentication;
using DotNetSnmp.Protocol.V3.Security.Privacy;
using DotNetSnmp.Transport.Targets;
using Lextm.SharpSnmpLib.Messaging;
using Xunit;

namespace DotNetSnmp.Test
{
    public class IntegrationTest
    {
        [Fact]
        public async Task TestMessengerGetAsyncV1()
        {
            // Arrange
            var version = VersionCode.V1;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")), // sysDescr
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))  // sysObjectID
            };

            // Act
            var result = await Messenger.GetAsync(version, endpoint, community, variables);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());
        }

        [Fact]
        public async Task TestMessengerGetAsyncV2()
        {
            // Arrange
            var version = VersionCode.V2;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")), // sysDescr
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))  // sysObjectID
            };

            // Act
            var result = await Messenger.GetAsync(version, endpoint, community, variables);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());
        }

        [Fact]
        public async Task TestMessengerSetAsyncV1()
        {
            // Arrange
            var expected = "Test" + new Random().Next(1, 1000);
            var version = VersionCode.V1;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.5.0"), new OctetString(expected)), // sysContact
            };

            // Act
            var result = await Messenger.SetAsync(version, endpoint, community, variables);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("1.3.6.1.2.1.1.5.0", result[0].Id.ToString());
            Assert.Equal(expected, result[0].Data.ToString());
        }

        [Fact]
        public async Task TestMessengerSetAsyncV2()
        {
            // Arrange
            var expected = "Test" + new Random().Next(1, 1000);
            var version = VersionCode.V2;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.5.0"), new OctetString(expected)), // sysContact
            };

            // Act
            var result = await Messenger.SetAsync(version, endpoint, community, variables);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("1.3.6.1.2.1.1.5.0", result[0].Id.ToString());
            Assert.Equal(expected, result[0].Data.ToString());
        }

        [Fact]
        public async Task TestMessengerWalkAsyncV1()
        {
            // Arrange
            var version = VersionCode.V1;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
            };

            // Act
            var result = await Messenger.WalkAsync(version, endpoint, community, new ObjectIdentifier("1.3.6.1.2.1.1"), variables, WalkMode.WithinSubtree);

            // Assert
            // Assert.Equal(1, result);
            Assert.Equal(16, variables.Count);
        }

        [Fact]
        public async Task TestMessengerWalkAsyncV1_Full()
        {
            // Arrange
            var version = VersionCode.V1;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
            };

            // Act
            var result = await Messenger.WalkAsync(version, endpoint, community, new ObjectIdentifier("1.3.6.1.2.1.1"), variables, WalkMode.Default);

            // Assert
            // Assert.Equal(1, result);
            Assert.True(variables.Count > 16);
        }

        [Fact]
        public async Task TestMessengerWalkAsyncV2()
        {
            // Arrange
            var version = VersionCode.V2;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
            };

            // Act
            var result = await Messenger.WalkAsync(version, endpoint, community, new ObjectIdentifier("1.3.6.1.2.1.1"), variables, WalkMode.WithinSubtree);

            // Assert
            // Assert.Equal(1, result);
            Assert.Equal(16, variables.Count);
        }

        [Fact]
        public async Task TestMessengerWalkAsyncV2_Full()
        {
            // Arrange
            var version = VersionCode.V2;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
            };

            // Act
            var result = await Messenger.WalkAsync(version, endpoint, community, new ObjectIdentifier("1.3.6.1.2.1.1"), variables, WalkMode.Default);

            // Assert
            // Assert.Equal(1, result);
            Assert.True(variables.Count > 16);
        }

        [Fact]
        public async Task TestMessengerBulkWalkAsyncV2()
        {
            // Arrange
            var version = VersionCode.V2;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
            };

            // Act
            var result = await Messenger.BulkWalkAsync(version, endpoint, community, OctetString.Empty, new ObjectIdentifier("1.3.6.1.2.1.1"), variables, 10, WalkMode.WithinSubtree);

            // Assert
            // Assert.Equal(1, result);
            Assert.Equal(16, variables.Count);
        }

        [Fact]
        public async Task TestMessengerBulkWalkAsyncV2_Full()
        {
            // Arrange
            var version = VersionCode.V2;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
            };

            // Act
            var result = await Messenger.BulkWalkAsync(version, endpoint, community, OctetString.Empty, new ObjectIdentifier("1.3.6.1.2.1.1"), variables, 10, WalkMode.Default);

            // Assert
            // Assert.Equal(1, result);
            Assert.True(variables.Count > 16);
        }

        [Fact]
        public async Task TestMessengerInformAsyncV2()
        {
            // Arrange
            var version = VersionCode.V2;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 162);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1"), new OctetString("Test system")), // sysDescr
            };

            // Act
            await Messenger.SendInformAsync(848, version, endpoint, community, OctetString.Empty, new ObjectIdentifier("1.3.6.1.6.3.1.1.5.2"), 454, variables);
        }

        [Fact]
        public async Task TestMessengerTrapV2Async()
        {
            // Arrange
            var version = VersionCode.V2;
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 162);
            var community = new OctetString("public");
            var variables = new List<Variable>
            {
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1"), new OctetString("Test system")), // sysDescr
            };

            // Act
            await Messenger.SendTrapV2Async(848, version, endpoint, community, OctetString.Empty, new ObjectIdentifier("1.3.6.1.6.3.1.1.5.2"), 454, variables);
        }

        [Fact]
        public async Task TestMessengerGetAsyncV3()
        {
            // Arrange
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);

            // SNMPv3 credentials
            var username = "usr-none-none";

            var variables = new List<Variable>
            {
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")), // sysDescr
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))  // sysObjectID
            };

            // Act - Use the new GetV3Async method
            var result = await Messenger.GetV3Async(
                endpoint,
                username,
                variables);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());

            // Optional: Verify that we actually got some data back (not empty values)
            Assert.False(string.IsNullOrEmpty(result[0].Data.ToString()));
            Assert.False(string.IsNullOrEmpty(result[1].Data.ToString()));
        }

        [Fact]
        public async Task TestMessengerGetAsyncV3_SHA1()
        {
            // Arrange
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);

            // SNMPv3 credentials
            var username = "usr-sha-none";
            var authPassword = new OctetString("authkey1");

            var variables = new List<Variable>
            {
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")), // sysDescr
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))  // sysObjectID
            };

            // Act - Use the new GetV3Async method
            var result = await Messenger.GetV3Async(
                endpoint,
                username,
                new DefaultPrivacyProvider(new SHA1AuthenticationProvider(authPassword.Octets)),
                variables);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());

            // Optional: Verify that we actually got some data back (not empty values)
            Assert.False(string.IsNullOrEmpty(result[0].Data.ToString()));
            Assert.False(string.IsNullOrEmpty(result[1].Data.ToString()));
        }

        [Fact]
        public async Task TestMessengerGetAsyncV3_MD5()
        {
            // Arrange
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);

            // SNMPv3 credentials
            var username = "usr-md5-none";
            var authPassword = new OctetString("authkey1");

            var variables = new List<Variable>
            {
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")), // sysDescr
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))  // sysObjectID
            };

            // Act - Use the new GetV3Async method
            var result = await Messenger.GetV3Async(
                endpoint,
                username,
                new DefaultPrivacyProvider(new MD5AuthenticationProvider(authPassword.Octets)),
                variables);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());

            // Optional: Verify that we actually got some data back (not empty values)
            Assert.False(string.IsNullOrEmpty(result[0].Data.ToString()));
            Assert.False(string.IsNullOrEmpty(result[1].Data.ToString()));
        }


        [Fact]
        public async Task TestMessengerGetAsyncV3_MD5_DES()
        {
            // Arrange
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);

            // SNMPv3 credentials
            var username = "usr-md5-des";
            var authPassword = new OctetString("authkey1");
            var privPassword = new OctetString("privkey1");

            var variables = new List<Variable>
            {
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")), // sysDescr
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))  // sysObjectID
            };

            // Act - Use the new GetV3Async method
            var result = await Messenger.GetV3Async(
                endpoint,
                username,
                new DESPrivacyProvider(new MD5AuthenticationProvider(authPassword.Octets), privPassword.Octets),
                variables);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());

            // Optional: Verify that we actually got some data back (not empty values)
            Assert.False(string.IsNullOrEmpty(result[0].Data.ToString()));
            Assert.False(string.IsNullOrEmpty(result[1].Data.ToString()));
        }

        [Fact]
        public async Task TestMessengerGetAsyncV3_MD5_TripleDES()
        {
            // Arrange
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);

            // SNMPv3 credentials
            var username = "usr-md5-3des";
            var authPassword = new OctetString("authkey1");
            var privPassword = new OctetString("privkey1");

            var variables = new List<Variable>
            {
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")), // sysDescr
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))  // sysObjectID
            };

            // Act - Use the new GetV3Async method
            var result = await Messenger.GetV3Async(
                endpoint,
                username,
                new TripleDESPrivacyProvider(new MD5AuthenticationProvider(authPassword.Octets), privPassword.Octets),
                variables);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());

            // Optional: Verify that we actually got some data back (not empty values)
            Assert.False(string.IsNullOrEmpty(result[0].Data.ToString()));
            Assert.False(string.IsNullOrEmpty(result[1].Data.ToString()));
        }


        [Fact]
        public async Task TestMessengerGetAsyncV3_SHA1_AES()
        {
            // Arrange
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);

            // SNMPv3 credentials
            var username = "usr-sha-aes";
            var authPassword = new OctetString("authkey1");
            var privPassword = new OctetString("privkey1");

            var variables = new List<Variable>
            {
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")), // sysDescr
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))  // sysObjectID
            };

            // Act - Use the new GetV3Async method
            var result = await Messenger.GetV3Async(
                endpoint,
                username,
                new AESPrivacyProvider(new SHA1AuthenticationProvider(authPassword.Octets), privPassword.Octets),
                variables);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());

            // Optional: Verify that we actually got some data back (not empty values)
            Assert.False(string.IsNullOrEmpty(result[0].Data.ToString()));
            Assert.False(string.IsNullOrEmpty(result[1].Data.ToString()));
        }

        [Fact]
        public async Task TestMessengerGetAsyncV3_SHA1_AES192()
        {
            // Arrange
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);

            // SNMPv3 credentials
            var username = "usr-sha-aes192";
            var authPassword = new OctetString("authkey1");
            var privPassword = new OctetString("privkey1");

            var variables = new List<Variable>
            {
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")), // sysDescr
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))  // sysObjectID
            };

            // Act - Use the GetV3Async method
            var result = await Messenger.GetV3Async(
                endpoint,
                username,
                new AES192PrivacyProvider(new SHA1AuthenticationProvider(authPassword.Octets), privPassword.Octets),
                variables);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());

            // Optional: Verify that we actually got some data back (not empty values)
            Assert.False(string.IsNullOrEmpty(result[0].Data.ToString()));
            Assert.False(string.IsNullOrEmpty(result[1].Data.ToString()));
        }

        [Fact]
        public async Task TestMessengerGetAsyncV3_SHA1_AES256()
        {
            // Arrange
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);

            // SNMPv3 credentials
            var username = "usr-sha-aes256";
            var authPassword = new OctetString("authkey1");
            var privPassword = new OctetString("privkey1");

            var variables = new List<Variable>
            {
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")), // sysDescr
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.2.0"))  // sysObjectID
            };

            // Act - Use the GetV3Async method
            var result = await Messenger.GetV3Async(
                endpoint,
                username,
                new AES256PrivacyProvider(new SHA1AuthenticationProvider(authPassword.Octets), privPassword.Octets),
                variables);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1.3.6.1.2.1.1.1.0", result[0].Id.ToString());
            Assert.Equal("1.3.6.1.2.1.1.2.0", result[1].Id.ToString());

            // Optional: Verify that we actually got some data back (not empty values)
            Assert.False(string.IsNullOrEmpty(result[0].Data.ToString()));
            Assert.False(string.IsNullOrEmpty(result[1].Data.ToString()));
        }

        [Fact]
        public async Task TestMessengerSetAsyncV3_SHA1_AES()
        {
            // Arrange
            var expected = "Test" + new Random().Next(1, 1000);
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);

            // SNMPv3 credentials
            var username = "usr-sha-aes";
            var authPassword = new OctetString("authkey1");
            var privPassword = new OctetString("privkey1");

            var variables = new List<Variable>
            {
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.5.0"), new OctetString(expected)), // sysName
            };

            // Act - Use the SetV3Async method
            var result = await Messenger.SetV3Async(
                endpoint,
                username,
                new AESPrivacyProvider(new SHA1AuthenticationProvider(authPassword.Octets), privPassword.Octets),
                variables);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("1.3.6.1.2.1.1.5.0", result[0].Id.ToString());
            Assert.Equal(expected, result[0].Data.ToString());
        }

        [Fact]
        public async Task TestMessengerBulkWalkAsyncV3_SHA1_AES()
        {
            // Arrange
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 161);

            // SNMPv3 credentials
            var username = "usr-sha-aes";
            var authPassword = new OctetString("authkey1");
            var privPassword = new OctetString("privkey1");

            var variables = new List<Variable>();

            // Act - Use the BulkWalkV3Async method
            var result = await Messenger.BulkWalkV3Async(
                endpoint,
                username,
                new AESPrivacyProvider(new SHA1AuthenticationProvider(authPassword.Octets), privPassword.Octets),
                new ObjectIdentifier("1.3.6.1.2.1.1"), // system MIB
                variables,
                10,
                WalkMode.WithinSubtree);

            // Assert
            Assert.NotNull(variables);
            Assert.True(variables.Count >= 16, $"Expected at least 16 variables, got {variables.Count}");

            // Verify we have system group variables
            Assert.Contains(variables, v => v.Id.ToString().StartsWith("1.3.6.1.2.1.1."));

            // Verify that we actually got some data back (not empty values)
            Assert.False(string.IsNullOrEmpty(variables[0].Data.ToString()));
        }

        [Fact]
        public async Task TestMessengerInformAsyncV3_SHA1_AES()
        {
            // Arrange
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 162); // Notice the trap port 162

            // SNMPv3 credentials
            var username = "usr-sha-aes";
            var authPassword = new OctetString("authkey1");
            var privPassword = new OctetString("privkey1");

            var variables = new List<Variable>
            {
                new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0"), new OctetString("Test system")) // sysDescr
            };

            // Act - Send an SNMPv3 Inform request
            await Messenger.SendInformV3Async(
                endpoint,
                username,
                new AESPrivacyProvider(new SHA1AuthenticationProvider(authPassword.Octets), privPassword.Octets),
                new ObjectIdentifier("1.3.6.1.6.3.1.1.5.2"), // linkDown
                (uint)DateTimeOffset.Now.ToUnixTimeSeconds(),
                variables);

            // No assertion needed as the method will throw an exception if there's an error
        }

        [Fact]
        public async Task TestMessengerTrapV2AsyncV3_SHA1_AES()
        {
            // Arrange
            var host = "demo.pysnmp.com";
            var addresses = await Dns.GetHostAddressesAsync(host, TestContext.Current.CancellationToken);
            var endpoint = new IPEndPoint(addresses[0], 162); // Notice the trap port 162

            // SNMPv3 credentials
            var username = "usr-sha-aes";
            var authPassword = new OctetString("authkey1");
            var privPassword = new OctetString("privkey1");

            // Standard trap variables
            var sysUpTimeOid = new ObjectIdentifier("1.3.6.1.2.1.1.3.0"); // sysUpTime
            var sysUpTimeValue = new TimeTicks(12345);
            var trapOid = new ObjectIdentifier("1.3.6.1.6.3.1.1.5.2"); // linkDown
            var snmpTrapOid = new ObjectIdentifier("1.3.6.1.6.3.1.1.4.1.0"); // snmpTrapOID.0

            // Custom trap data
            var ifIndexOid = new ObjectIdentifier("1.3.6.1.2.1.2.2.1.1.0"); // ifIndex.0
            var ifIndexValue = new Integer32(1); // Interface index
            var ifDescriptionOid = new ObjectIdentifier("1.3.6.1.2.1.2.2.1.2.0"); // ifDescription.0
            var ifDescriptionValue = new OctetString("Test Interface");

            var variables = new List<Variable>
            {
                new Variable(sysUpTimeOid, sysUpTimeValue),
                new Variable(snmpTrapOid, trapOid),
                new Variable(ifIndexOid, ifIndexValue),
                new Variable(ifDescriptionOid, ifDescriptionValue)
            };

            // Act - Send an SNMPv3 TRAPv2
            await Messenger.SendTrapV2V3Async(
                endpoint,
                username,
                new AESPrivacyProvider(new SHA1AuthenticationProvider(authPassword.Octets), privPassword.Octets),
                new ObjectIdentifier("1.3.6.1.4.1.12345"), // enterprise OID (example)
                (uint)DateTimeOffset.Now.ToUnixTimeSeconds(),
                variables);

            // No assertion needed as the method will not expect a response and would throw an exception if there's an error sending
        }
    }
}
