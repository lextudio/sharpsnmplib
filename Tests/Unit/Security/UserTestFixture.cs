/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2010/12/5
 * Time: 11:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using Lextm.SharpSnmpLib.Security;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit.Security
{
    public class UserTestFixture
    {
        [Fact]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => new User(null, null));
            Assert.Throws<ArgumentNullException>(() => new User(new OctetString("test"), null));
            
            var user = new User(new OctetString("test"), DefaultPrivacyProvider.DefaultPair);
            Assert.Equal("User: name: test; provider: Default privacy provider", user.ToString());
        }
    }
}
