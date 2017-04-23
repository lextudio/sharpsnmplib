using System;
using Xunit;

namespace Lextm.SharpSnmpLib.Objects.Tests
{
    public class SysContactTestFixture
    {
        [Fact]
        public void Test()
        {
            var sys = new SysContact();
            Assert.Throws<ArgumentNullException>(() => sys.Data = null);
            Assert.Throws<ArgumentException>(() => sys.Data = new TimeTicks(0));
        }
    }
}
