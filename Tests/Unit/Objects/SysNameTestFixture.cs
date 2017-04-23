using System;
using Xunit;

namespace Lextm.SharpSnmpLib.Objects.Tests
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
