using System.Threading;
using NUnit.Framework;

namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class TestWatchDog
    {
        [Test]
        public void Test()
        {
            int count = 0;
            WatchDog dog = new WatchDog(1000d);
            dog.Bark += delegate
                            {
                                count++;
                            };
            dog.Enabled = true;
            dog.Feed();
            dog.Feed();
            dog.Feed();
            dog.Feed();
            Thread.Sleep(1200);
            dog.Enabled = false;
            Assert.AreEqual(1, count);
        }
    }
}
