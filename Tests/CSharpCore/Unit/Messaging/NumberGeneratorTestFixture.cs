using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lextm.SharpSnmpLib.Messaging;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit.Messaging
{
    public class NumberGeneratorTestFixture
    {
        //[Theory]
        //[InlineData(1000000)]
        public void Test(int count)
        {
            var ng = new NumberGenerator(int.MinValue, int.MaxValue);
            ng.SetSalt(0);
            var list = Enumerable.Range(1, count).AsParallel().Select(x => ng.NextId);
            var dupes = list.GroupBy(x => x).Where(g => g.Count() > 1).Select(x => x.Key);
            Assert.Equal(0, dupes.Count());
        }

        //[Theory]
        //[InlineData(1000)]
        //[InlineData(10000)]
        public void TestOverflow(int count)
        {
            var overallDupes = 0;
            Parallel.ForEach(Enumerable.Range(0, count), i =>
            {
                var ng = new NumberGenerator(int.MinValue, int.MaxValue);
                ng.SetSalt(int.MaxValue - 5);
                var list = Enumerable.Range(1, 20).AsParallel().Select(x => ng.NextId);
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
