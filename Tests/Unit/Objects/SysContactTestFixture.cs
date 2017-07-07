using System;
using Lextm.SharpSnmpLib.Objects;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit.Objects
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
