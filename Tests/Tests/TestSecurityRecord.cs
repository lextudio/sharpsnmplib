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
            Assert.AreEqual(Levels.None, ProviderPair.Default.ToSecurityLevel());
        }

        [Test]
        public void TestConstrucetor()
        {
            Assert.AreEqual(Levels.None, new ProviderPair(DefaultAuthenticationProvider.Instance, DefaultPrivacyProvider.Instance).ToSecurityLevel());
        }

        [Test]
        public void TestException()
        {
            Assert.Throws<ArgumentException>(delegate { new ProviderPair(DefaultAuthenticationProvider.Instance, new DESPrivacyProvider(new OctetString(""), DefaultAuthenticationProvider.Instance)); });
        }

        [Test]
        public void TestAuthenticationOnly()
        {
            Assert.AreEqual(Levels.Authentication, new ProviderPair(new MD5AuthenticationProvider(new OctetString("test")), DefaultPrivacyProvider.Instance).ToSecurityLevel());
        }
    }
}
