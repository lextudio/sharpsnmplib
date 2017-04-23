using System;
using Xunit;

namespace Lextm.SharpSnmpLib.Tests
{
    public class SnmpDataExtensionTestFixture
    {
        [Fact]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => SnmpDataExtension.ToBytes(null));
        }
    }
}
