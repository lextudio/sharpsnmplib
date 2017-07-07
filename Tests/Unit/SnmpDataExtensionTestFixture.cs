using System;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit
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
