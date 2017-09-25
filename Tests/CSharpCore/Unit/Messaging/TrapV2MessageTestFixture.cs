namespace Lextm.SharpSnmpLib.Unit.Messaging
{
    using System.Collections.Generic;
    using Xunit;
    using Lextm.SharpSnmpLib.Messaging;
    using Lextm.SharpSnmpLib.Security;
    using System.IO;

    public class TrapV2MessageTestFixture
    {
        [Fact]
        public void TestToBytes()
        {
            var trap = new TrapV2Message(
                VersionCode.V3,
                528732060,
                1905687779,
                new OctetString("lextm"),
                new ObjectIdentifier("1.3.6"),
                0,
                new List<Variable>(),
                DefaultPrivacyProvider.DefaultPair,
                0x10000,
                new OctetString(ByteTool.Convert("80001F8880E9630000D61FF449")),
                0,
                0
               );
            Assert.Equal(ByteTool.Convert(File.ReadAllBytes(Path.Combine("Resources", "trapv3"))), ByteTool.Convert(trap.ToBytes()));
        }
        
        [Fact]
        public void TestToBytes2()
        {
            var privacy = new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication")));
            var trap = new TrapV2Message(
                VersionCode.V3,
                318463383,
                1276263065,
                new OctetString("lextm"),
                new ObjectIdentifier("1.3.6"),
                0,
                new List<Variable>(),
                privacy,
                0x10000,
                new OctetString(ByteTool.Convert("80001F8880E9630000D61FF449")),
                0,
                0
               );
            Assert.Equal(ByteTool.Convert(File.ReadAllBytes(Path.Combine("Resources", "trapv3auth"))), ByteTool.Convert(trap.ToBytes()));
        }


        [Fact]
        public void TestToBytes3()
        {
            if (!DESPrivacyProvider.IsSupported)
            {
                return;
            }

            var privacy = new DESPrivacyProvider(new OctetString("privacyphrase"), new MD5AuthenticationProvider(new OctetString("authentication")));
            var trap = new TrapV2Message(
                VersionCode.V3,
                new Header(
                    new Integer32(1004947569),
                    new Integer32(0x10000),
                    privacy.ToSecurityLevel()),
                new SecurityParameters(
                    new OctetString(ByteTool.Convert("80001F8880E9630000D61FF449")),
                    Integer32.Zero,
                    Integer32.Zero,
                    new OctetString("lextm"),
                    new OctetString(ByteTool.Convert("61A9A486AF4A861BD5C0BB1F")),
                    new OctetString(ByteTool.Convert("0000000069D39B2A"))),
                new Scope(OctetString.Empty, OctetString.Empty,
                          new TrapV2Pdu(
                              234419641,
                              new ObjectIdentifier("1.3.6"),
                              0,
                              new List<Variable>())),
                privacy,
                null);
            byte[] bytes = trap.ToBytes();
            UserRegistry registry = new UserRegistry();
            registry.Add(new OctetString("lextm"), privacy);
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.Equal(1, messages.Count);
            ISnmpMessage message = messages[0];
            Assert.Equal("80001F8880E9630000D61FF449", message.Parameters.EngineId.ToHexString());
            Assert.Equal(0, message.Parameters.EngineBoots.ToInt32());
            Assert.Equal(0, message.Parameters.EngineTime.ToInt32());
            Assert.Equal("lextm", message.Parameters.UserName.ToString());
            Assert.Equal("61A9A486AF4A861BD5C0BB1F", message.Parameters.AuthenticationParameters.ToHexString());
            Assert.Equal("0000000069D39B2A", message.Parameters.PrivacyParameters.ToHexString());
            Assert.Equal("", message.Scope.ContextEngineId.ToHexString()); // SNMP#NET returns string.Empty here.
            Assert.Equal("", message.Scope.ContextName.ToHexString());
            Assert.Equal(0, message.Scope.Pdu.Variables.Count);
            Assert.Equal(1004947569, message.MessageId());
            Assert.Equal(234419641, message.RequestId());
        }
    }
}
