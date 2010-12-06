/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2010/12/5
 * Time: 15:33
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NUnit.Framework;

namespace Lextm.SharpSnmpLib.Security.Tests
{
    [TestFixture]
    public class DESPrivacyProviderTestFixture
    {
        [Test]
        public void Test()
        {
            var provider = new DESPrivacyProvider(new OctetString("longlongago"), new MD5AuthenticationProvider(new OctetString("verylonglongago")));
            Assert.Throws<ArgumentNullException>(() => new DESPrivacyProvider(null, null));
            Assert.Throws<ArgumentNullException>(() => new DESPrivacyProvider(OctetString.Empty, null));
            Assert.Throws<ArgumentNullException>(() => provider.Encrypt(null, null));
            Assert.Throws<ArgumentNullException>(() => provider.Encrypt(OctetString.Empty, null));
            Assert.Throws<ArgumentException>(() => provider.Encrypt(new Null(), SecurityParameters.Create(OctetString.Empty)));
             
            Assert.Throws<ArgumentNullException>(() => provider.Decrypt(null, null));
            Assert.Throws<ArgumentNullException>(() => provider.Decrypt(OctetString.Empty, null));
            Assert.Throws<ArgumentException>(() => provider.Decrypt(new Null(), SecurityParameters.Create(OctetString.Empty)));
        }
    }
}
