using Lextm.SharpSnmpLib.Pipeline;
using Xunit;

namespace Lextm.SharpSnmpLib.Objects.Tests
{
    public class SysDescrTestFixture
    {
        [Fact]
        public void Test()
        {
            var sys = new SysDescr();
            Assert.Throws<AccessFailureException>(() => sys.Data = OctetString.Empty);
        }
    }
}
