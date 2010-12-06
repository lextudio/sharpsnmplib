/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2010/12/5
 * Time: 15:23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NUnit.Framework;

namespace Lextm.SharpSnmpLib.Security.Tests
{
    [TestFixture]
    public class MD5AuthenticationProviderTestFixture
    {
        [Test]
        public void Test()
        {
            var provider = new MD5AuthenticationProvider(new OctetString("longlongago"));
            Assert.AreEqual("MD5 authentication provider", provider.ToString());
            Assert.Throws<ArgumentNullException>(() => new MD5AuthenticationProvider(null));
            Assert.Throws<ArgumentNullException>(() => provider.PasswordToKey(null, null));
            Assert.Throws<ArgumentNullException>(() => provider.PasswordToKey(new byte[0], null));
            Assert.Throws<ArgumentException>(() => provider.PasswordToKey(new byte[0], new byte[0]));
            
            Assert.Throws<ArgumentNullException>(() => provider.ComputeHash(VersionCode.V1, null, null, null, null));
            Assert.Throws<ArgumentNullException>(() => provider.ComputeHash(VersionCode.V1, Header.Empty, null, null, null));
            Assert.Throws<ArgumentNullException>(() => provider.ComputeHash(VersionCode.V1, Header.Empty, SecurityParameters.Create(new OctetString("test")), null, null));
            Assert.Throws<ArgumentNullException>(() => provider.ComputeHash(VersionCode.V1, Header.Empty, SecurityParameters.Create(new OctetString("test")), OctetString.Empty, null));
        }
    }
}