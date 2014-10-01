using System;
using NUnit.Framework;

namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    [Category("Default")]
    public class SnmpDataExtensionTestFixture
    {
        [Test]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => SnmpDataExtension.ToBytes(null));
        }
    }
}
