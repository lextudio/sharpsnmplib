using System;
using System.IO;
using NUnit.Framework;

namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class StreamExtensionTestFixture
    {
        [Test]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => StreamExtension.AppendBytes(null, SnmpType.Counter32, null));
            Assert.Throws<ArgumentNullException>(() => new MemoryStream().AppendBytes(SnmpType.Counter32, null));
            Assert.Throws<ArgumentNullException>(() => StreamExtension.IgnoreBytes(null, 0));
            Assert.Throws<ArgumentNullException>(() => StreamExtension.ReadPayloadLength(null));
            Assert.Throws<ArgumentNullException>(() => StreamExtension.WritePayloadLength(null, 0));
            Assert.Throws<ArgumentException>(() => StreamExtension.WritePayloadLength(new MemoryStream(), -1));
        }
    }
}
