using System;
using Xunit;

namespace Lextm.SharpSnmpLib.Security.Tests
{
    public class LevelsTestFixture
    {
        [Fact]
        public void TestToSecurityLevel()
        {
            Assert.Equal((Levels)0, DefaultPrivacyProvider.DefaultPair.ToSecurityLevel());
        }

        [Fact]
        public void TestException()
        {
            Assert.Throws<ArgumentException>(delegate { new DESPrivacyProvider(new OctetString(""), DefaultAuthenticationProvider.Instance); });
        }

        [Fact]
        public void TestAuthenticationOnly()
        {
            Assert.Equal(Levels.Authentication, new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("test"))).ToSecurityLevel());
        }
    }
}
