using System;
using Lextm.SharpSnmpLib.Security;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit.Security
{
    public class TsmPrivacyProviderTestFixture
    {
        [Fact]
        public void Test()
        {
            var provider = TsmPrivacyProvider.DefaultPair;
            Assert.Throws<ArgumentNullException>(() => provider.Encrypt(null, null));
            Assert.Throws<ArgumentNullException>(() => provider.Encrypt(OctetString.Empty, null));
            Assert.Throws<ArgumentException>(() => provider.Encrypt(new Null(), SecurityParameters.Create(OctetString.Empty)));

            var expected = new Sequence((byte[])null);
            Assert.Equal(expected, provider.Encrypt(expected, SecurityParameters.Create(OctetString.Empty)));

            Assert.Throws<ArgumentNullException>(() => provider.Decrypt(null, null));
            Assert.Throws<ArgumentNullException>(() => provider.Decrypt(OctetString.Empty, null));
            var result = provider.Decrypt(new Sequence((byte[])null), SecurityParameters.Create(OctetString.Empty));
            Assert.NotNull(result);
        }
    }
}
