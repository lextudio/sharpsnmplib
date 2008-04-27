using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SharpSnmpLib.Tests
{
    [TestFixture]
    public class TestObjectIdentifier
    {
        [Test]
        public void TestToOid()
        {
            ObjectIdentifier oid = new ObjectIdentifier(new byte[] { 0x2B, 0x06, 0x01, 0x04, 0x01, 0x90, 0x72, 0x87, 0x68, 0x02 });
            Assert.AreEqual(new uint[] { 1, 3, 6, 1, 4, 1, 2162, 1000, 2 }, oid.ToOid());
        }
    }
}
