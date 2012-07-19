using System;
using NUnit.Framework;

namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class SecurityParametersTestFixture
    {
        [Test]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => new SecurityParameters(null));
            Assert.Throws<ArgumentNullException>(() => new SecurityParameters(null, null, null, null, null, null));
        }
        
        [Test]
        public void TestToString()
        {
            var obj = SecurityParameters.Create(new OctetString("test"));
            Assert.AreEqual("Security parameters: engineId: ;engineBoots: ;engineTime: ;userName: test; authen hash: ; privacy hash: ", obj.ToString());
            
            obj.AuthenticationParameters = new OctetString("one");
            Assert.Throws<ArgumentException>(() => obj.AuthenticationParameters = new OctetString("me"));
        }
    }
}
