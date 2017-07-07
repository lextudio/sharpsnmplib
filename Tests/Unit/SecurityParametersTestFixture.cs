using System;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit
{
    public class SecurityParametersTestFixture
    {
        [Fact]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => new SecurityParameters(null));
            Assert.Throws<ArgumentNullException>(() => new SecurityParameters(null, null, null, null, null, null));
        }
        
        [Fact]
        public void TestToString()
        {
            var obj = SecurityParameters.Create(new OctetString("test"));
            Assert.Equal("Security parameters: engineId: ;engineBoots: ;engineTime: ;userName: test; authen hash: ; privacy hash: ", obj.ToString());
            
            obj.AuthenticationParameters = new OctetString("one");
            Assert.Throws<ArgumentException>(() => obj.AuthenticationParameters = new OctetString("me"));
        }
    }
}
