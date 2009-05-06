using NUnit.Framework;
using Lextm.SharpSnmpLib.Security;
using System;

namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class TestSecurityRecord
    {
        [Test]
        public void TestToSecurityLevel()
        {
            Assert.AreEqual(SecurityLevel.None, SecurityRecord.Default.ToSecurityLevel());
        }

        [Test]
        public void TestConstrucetor()
        {
            Assert.AreEqual(SecurityLevel.None, new SecurityRecord(DefaultAuthenticationProvider.Instance, DefaultPrivacyProvider.Instance).ToSecurityLevel());
        }

        [Test]
        public void TestException()
        {
            Assert.Throws<ArgumentException>(delegate { new SecurityRecord(DefaultAuthenticationProvider.Instance, DESPrivacyProvider.Instance); });
        }

        [Test]
        public void TestAuthenticationOnly()
        {
            Assert.AreEqual(SecurityLevel.Authentication, new SecurityRecord(MD5AuthenticationProvider.Instance, DefaultPrivacyProvider.Instance).ToSecurityLevel());
        }
    }
}
