using Lextm.SharpSnmpLib.Pipeline;
using Xunit;

namespace Lextm.SharpSnmpLib.Objects.Tests
{
    public class SysORDescrTestFixture
    {
        [Fact]
        public void Test()
        {
            var sys = new SysORDescr(3, new OctetString("test"));
            Assert.Equal("1.3.6.1.2.1.1.9.1.3.3", sys.Variable.Id.ToString());
            Assert.Equal("test", sys.Data.ToString());
            Assert.Throws<AccessFailureException>(() => sys.Data = OctetString.Empty);
        }
    }
}
