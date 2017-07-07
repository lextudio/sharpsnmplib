using Lextm.SharpSnmpLib.Security;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit.Security
{
    public class LevelsTestFixture
    {
        [Fact]
        public void TestToSecurityLevel()
        {
            Assert.Equal((Levels)0, DefaultPrivacyProvider.DefaultPair.ToSecurityLevel());
        }
#if !NETSTANDARD
        [Fact]
        public void TestException()
        {
            Assert.Throws<ArgumentException>(delegate { new DESPrivacyProvider(new OctetString(""), DefaultAuthenticationProvider.Instance); });
        }
#endif
        [Fact]
        public void TestAuthenticationOnly()
        {
            Assert.Equal(Levels.Authentication, new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("test"))).ToSecurityLevel());
        }
    }
}
