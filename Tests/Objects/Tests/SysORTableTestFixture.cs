/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2010/12/11
 * Time: 11:04
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using Xunit;

namespace Lextm.SharpSnmpLib.Objects.Tests
{
    public class SysORTableTestFixture
    {
        [Fact]
        public void Test()
        {
            var table = new SysORTable();
            Assert.Equal(null, table.MatchGet(new ObjectIdentifier("1.3.6")));
            var id = new ObjectIdentifier("1.3.6.1.2.1.1.9.1.1.1");
            Assert.Equal(id, table.MatchGet(id).Variable.Id);
            Assert.Equal(new ObjectIdentifier("1.3.6.1.2.1.1.9.1.1.2"), table.MatchGetNext(id).Variable.Id);
        }
    }
}
