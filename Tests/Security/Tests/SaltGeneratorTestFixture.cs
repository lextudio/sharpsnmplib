/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2010/12/5
 * Time: 14:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using Xunit;

namespace Lextm.SharpSnmpLib.Security.Tests
{
    public class SaltGeneratorTestFixture
    {
        [Fact]
        public void Test()
        {
            var gen = new SaltGenerator();
            var first = gen.GetSaltBytes();
            var second = gen.GetSaltBytes();
            Assert.NotEqual(first, second);
            Assert.Equal("Salt generator", gen.ToString());
            
            gen.SetSalt(long.MaxValue);
            Assert.Equal(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, gen.GetSaltBytes());
        }
    }
}
