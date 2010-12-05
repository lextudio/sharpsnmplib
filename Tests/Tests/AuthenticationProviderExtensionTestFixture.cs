/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2010/12/5
 * Time: 15:04
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Lextm.SharpSnmpLib.Security;
using NUnit.Framework;

namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class AuthenticationProviderExtensionTestFixture
    {
        [Test]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => AuthenticationProviderExtension.ComputeHash(null, VersionCode.V1, null, null, null, null));
            Assert.Throws<ArgumentNullException>(() => AuthenticationProviderExtension.ComputeHash(DefaultAuthenticationProvider.Instance, VersionCode.V1, null, null, null, null));
            Assert.Throws<ArgumentNullException>(() => AuthenticationProviderExtension.ComputeHash(DefaultAuthenticationProvider.Instance, VersionCode.V1, Header.Empty, null, null, null));
            Assert.Throws<ArgumentNullException>(() => AuthenticationProviderExtension.ComputeHash(DefaultAuthenticationProvider.Instance, VersionCode.V1, Header.Empty, SecurityParameters.Create(new OctetString("test")), null, null));
            Assert.Throws<ArgumentNullException>(() => AuthenticationProviderExtension.ComputeHash(DefaultAuthenticationProvider.Instance, VersionCode.V1, Header.Empty, SecurityParameters.Create(new OctetString("test")), new Scope(OctetString.Empty, OctetString.Empty, MalformedPdu.Instance), null));
            Assert.Throws<ArgumentNullException>(() => AuthenticationProviderExtension.VerifyHash(null, VersionCode.V1, null, null, null, null));
            Assert.Throws<ArgumentNullException>(() => AuthenticationProviderExtension.VerifyHash(DefaultAuthenticationProvider.Instance, VersionCode.V1, null, null, null, null));
            Assert.Throws<ArgumentNullException>(() => AuthenticationProviderExtension.VerifyHash(DefaultAuthenticationProvider.Instance, VersionCode.V1, Header.Empty, null, null, null));
            Assert.Throws<ArgumentNullException>(() => AuthenticationProviderExtension.VerifyHash(DefaultAuthenticationProvider.Instance, VersionCode.V1, Header.Empty, SecurityParameters.Create(new OctetString("test")), null, null));
            Assert.Throws<ArgumentNullException>(() => AuthenticationProviderExtension.VerifyHash(DefaultAuthenticationProvider.Instance, VersionCode.V1, Header.Empty, SecurityParameters.Create(new OctetString("test")), MalformedPdu.Instance, null));
        }
    }
}
