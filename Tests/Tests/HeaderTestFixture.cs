using System;
using NUnit.Framework;

namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class HeaderTestFixture
    {
        [Test]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => new Header(null));
            Assert.Throws<ArgumentNullException>(() => new Header(null, null, null));
            Assert.Throws<ArgumentNullException>(() => new Header(new Integer32(0), null, null));
            Assert.Throws<ArgumentNullException>(() => new Header(new Integer32(0), new Integer32(0), null));
            Assert.AreEqual("Header: messageId: 0;maxMessageSize: 0;securityBits: ;securityModel: 3", new Header(new Integer32(0), new Integer32(0), OctetString.Empty).ToString());
        }
    }
}
