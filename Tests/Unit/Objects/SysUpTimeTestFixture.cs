using Lextm.SharpSnmpLib.Objects;
using Lextm.SharpSnmpLib.Pipeline;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit.Objects
{
    public class SysUpTimeTestFixture
    {
        [Fact]
        public void Test()
        {
            var sys = new SysUpTime();
            Assert.Throws<AccessFailureException>(() => sys.Data = OctetString.Empty);
        }
    }
}
