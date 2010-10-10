using System;
using Lextm.SharpSnmpLib.Security;
using NUnit.Framework;

namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class TestSecurityRecord
    {
        [Test]
        public void TestToSecurityLevel()
        {
            Assert.AreEqual((Levels)0, PrivacyProviderExtension.ToSecurityLevel(DefaultPrivacyProvider.Default));
        }

        [Test]
        public void TestException()
        {
            Assert.Throws<ArgumentException>(delegate { new DESPrivacyProvider(new OctetString(""), DefaultAuthenticationProvider.Instance); });
        }

        [Test]
        public void TestAuthenticationOnly()
        {
            Assert.AreEqual(Levels.Authentication, PrivacyProviderExtension.ToSecurityLevel(new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("test")))));
        }
    }
}
