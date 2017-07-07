/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2010/12/5
 * Time: 15:00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using Lextm.SharpSnmpLib.Security;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit.Security
{
    public class PrivacyProviderExtensionTestFixture
    {
        [Fact]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => PrivacyProviderExtension.ToSecurityLevel(null));
            Assert.Throws<ArgumentNullException>(() => PrivacyProviderExtension.GetScopeData(null, null, null, null));
            Assert.Throws<ArgumentNullException>(() => DefaultPrivacyProvider.DefaultPair.GetScopeData(null, null, null));
        }
    }
}
