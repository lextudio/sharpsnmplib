/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2010/12/5
 * Time: 14:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lextm.SharpSnmpLib.Security;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit.Security
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

        [Theory]
        [InlineData(1000000)]
        public void TestNormal(int count)
        {
            var ng = new SaltGenerator();
            ng.SetSalt(0);
            var list = Enumerable.Range(1, count).AsParallel().Select(x => ng.NextSalt);
            var dupes = list.GroupBy(x => x).Where(g => g.Count() > 1).Select(x => x.Key);
            Assert.Equal(0, dupes.Count());
        }

        [Theory]
        [InlineData(1000)]
        //[InlineData(10000)]
        public void TestOverflow(int count)
        {
            var overallDupes = 0;
            Parallel.ForEach(Enumerable.Range(0, count), i =>
            {
                var ng = new SaltGenerator();
                ng.SetSalt(long.MaxValue - 5);
                var list = Enumerable.Range(1, 20).AsParallel().Select(x => ng.NextSalt);
                var dupes = list.GroupBy(x => x).Where(g => g.Count() > 1).Select(x => x.Key);
                if (dupes.Count() > 0)
                {
                    Interlocked.Increment(ref overallDupes);
                }
            });
            Assert.Equal(0, overallDupes);
        }
    }
}
