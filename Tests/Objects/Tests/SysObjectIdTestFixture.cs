using Lextm.SharpSnmpLib.Pipeline;
using NUnit.Framework;

namespace Lextm.SharpSnmpLib.Objects.Tests
{
    [TestFixture]
    [Category("Default")]
    public class SysObjectIdTestFixture
    {
        [Test]
        public void Test()
        {
            var sys = new SysObjectId();
            Assert.Throws<AccessFailureException>(() => sys.Data = OctetString.Empty);
        }
    }
}
