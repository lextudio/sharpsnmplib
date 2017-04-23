using Lextm.SharpSnmpLib.Pipeline;
using Xunit;

namespace Lextm.SharpSnmpLib.Objects.Tests
{
    public class SysObjectIdTestFixture
    {
        [Fact]
        public void Test()
        {
            var sys = new SysObjectId();
            Assert.Throws<AccessFailureException>(() => sys.Data = OctetString.Empty);
        }
    }
}
