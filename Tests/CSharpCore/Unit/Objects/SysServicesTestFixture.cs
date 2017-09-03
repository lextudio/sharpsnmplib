using Lextm.SharpSnmpLib.Objects;
using Lextm.SharpSnmpLib.Pipeline;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit.Objects
{
    public class SysServicesTestFixture
    {
        [Fact]
        public void Test()
        {
            var sys = new SysServices();
            Assert.Equal(new Integer32(72), sys.Data);
            Assert.Throws<AccessFailureException>(() => sys.Data = new TimeTicks(0));
        }
    }
}
