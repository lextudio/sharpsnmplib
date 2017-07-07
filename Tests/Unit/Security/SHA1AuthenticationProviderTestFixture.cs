/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2010/12/5
 * Time: 15:23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using Lextm.SharpSnmpLib.Security;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit.Security
{
    public class SHA1AuthenticationProviderTestFixture
    {
        [Fact]
        public void Test()
        {
            var provider = new SHA1AuthenticationProvider(new OctetString("longlongago"));
            Assert.Equal("SHA-1 authentication provider", provider.ToString());
            Assert.Throws<ArgumentNullException>(() => new SHA1AuthenticationProvider(null));
            Assert.Throws<ArgumentNullException>(() => provider.PasswordToKey(null, null));
            Assert.Throws<ArgumentNullException>(() => provider.PasswordToKey(new byte[0], null));
            Assert.Throws<ArgumentException>(() => provider.PasswordToKey(new byte[0], new byte[0]));
        }
    }
}
