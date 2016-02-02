/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2010/12/5
 * Time: 15:23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Xunit;

namespace Lextm.SharpSnmpLib.Security.Tests
{
    public class DefaultAuthenticationProviderTestFixture
    {
        [Fact]
        public void Test()
        {
            var provider = DefaultAuthenticationProvider.Instance;
            Assert.Equal("Default authentication provider", provider.ToString());
            Assert.Throws<ArgumentNullException>(() => provider.PasswordToKey(null, null));
            Assert.Throws<ArgumentNullException>(() => provider.PasswordToKey(new byte[0], null));
            Assert.Equal(new byte[0], provider.PasswordToKey(new byte[0], new byte[0]));
            
            Assert.Throws<ArgumentNullException>(() => provider.ComputeHash(VersionCode.V1, null, null, null, null));
            Assert.Throws<ArgumentNullException>(() => provider.ComputeHash(VersionCode.V1, Header.Empty, null, null, null));
            Assert.Throws<ArgumentNullException>(() => provider.ComputeHash(VersionCode.V1, Header.Empty, SecurityParameters.Create(new OctetString("test")), null, null));
            //Assert.Throws<ArgumentNullException>(() => provider.ComputeHash(VersionCode.V1, Header.Empty, SecurityParameters.Create(new OctetString("test")), OctetString.Empty, null));
            //Assert.Equal(OctetString.Empty, provider.ComputeHash(VersionCode.V1, Header.Empty, SecurityParameters.Create(new OctetString("test")), OctetString.Empty, DefaultPrivacyProvider.DefaultPair));
        }
    }
}
