using System.Threading;
using Lextm.Common;
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
            WatchDog dog = new WatchDog(100d);
            dog.Bark += delegate
                            {
                                count++;
                            };
            dog.Enabled = true;
            dog.KeepBarking = false;
            dog.Feed();
            dog.Feed();
            dog.Feed();
            dog.Feed();
            Thread.Sleep(120);
            Assert.AreEqual(1, count);
            Thread.Sleep(120);
            dog.Enabled = false;
            Assert.AreEqual(1, count);
        }
    }
}
