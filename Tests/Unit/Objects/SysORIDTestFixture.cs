using Lextm.SharpSnmpLib.Pipeline;
using Xunit;

namespace Lextm.SharpSnmpLib.Objects.Tests
{
    public class SysORIDTestFixture
    {
        [Fact]
        public void Test()
        {
            var sys = new SysORID(3, new ObjectIdentifier(".1.3.6.1.2.1.1.9.1.2.3"));
            Assert.Equal(".1.3.6.1.2.1.1.9.1.2.3", sys.Variable.Id.ToString());
            Assert.Equal(".1.3.6.1.2.1.1.9.1.2.3", sys.Data.ToString());
            Assert.Throws<AccessFailureException>(() => sys.Data = OctetString.Empty);
        }
    }
}
