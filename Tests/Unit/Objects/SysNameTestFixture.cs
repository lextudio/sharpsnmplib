using System;
using Lextm.SharpSnmpLib.Objects;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit.Objects
{
    public class SysNameTestFixture
    {
        [Fact]
        public void Test()
        {
            var sys = new SysName();
            Assert.Throws<ArgumentNullException>(() => sys.Data = null);
            Assert.Throws<ArgumentException>(() => sys.Data = new TimeTicks(0));
            sys.Data = OctetString.Empty;
            Assert.Equal(OctetString.Empty, sys.Data);
        }
    }
}
