using System;
using System.Collections.Generic;
using Lextm.SharpSnmpLib.Security;
using NUnit.Framework;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class TestTrapV2Message
    {
        [Test]
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
            Assert.AreEqual(ByteTool.Convert(Resources.trapv3), ByteTool.Convert(trap.ToBytes()));
        }
    }
}
