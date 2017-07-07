/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2010/12/5
 * Time: 14:45
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using Lextm.SharpSnmpLib.Security;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit.Security
{
    public class UserRegistryTestFixture
    {
        [Fact]
        public void Test()
        {
            var users = new UserRegistry(new User[] {new User(new OctetString("test"), DefaultPrivacyProvider.DefaultPair)});
            Assert.Equal(1, users.Count);
            Assert.NotNull(users.Find(new OctetString("test")));
            Assert.Equal(1, users.Add(null).Count);
            Assert.Equal(2, users.Add(new User(new OctetString("test2"), DefaultPrivacyProvider.DefaultPair)).Count);
            Assert.Equal(2, users.Add(new User(new OctetString("test2"), DefaultPrivacyProvider.DefaultPair)).Count);
            Assert.Throws<ArgumentNullException>(() => users.Find(null));
            Assert.Equal("User registry: count: 2", users.ToString());
        }
    }
}
