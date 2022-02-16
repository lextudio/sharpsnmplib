using System;
using Lextm.SharpSnmpLib.Security;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit.Security
{
    public class TsmAuthenticationProviderTestFixture
    {
        [Fact]
        public void Test()
        {
            var provider = TsmAuthenticationProvider.Instance;
            Assert.Equal("TSM authentication provider", provider.ToString());
            Assert.Throws<ArgumentNullException>(() => provider.PasswordToKey(null, null));
            Assert.Throws<ArgumentNullException>(() => provider.PasswordToKey(new byte[0], null));
            Assert.Equal(new byte[0], provider.PasswordToKey(new byte[0], new byte[0]));
        }
    }
}
