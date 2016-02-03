using System.Linq;
using Xunit;

namespace Lextm.SharpSnmpLib.Messaging.Tests
{
    public class NumberGeneratorTestFixture
    {
        [Fact]
        public void Test()
        {
            var ng = new NumberGenerator(int.MinValue, int.MaxValue);
            var list = Enumerable.Range(1, 1000000).AsParallel().Select(x => ng.NextId);

            var dupes = list.GroupBy(x => x).Where(g => g.Count() > 1).Select(x => x.Key);

            Assert.Equal(0, dupes.Count());
        }
    }
}
