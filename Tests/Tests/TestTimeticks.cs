/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 11:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using Lextm.SharpSnmpLib.Messaging;
using NUnit.Framework;

#pragma warning disable 1591,0618
namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class TestTimeticks
    {
        [Test]
        public void TestConstructor()
        {
            TimeTicks time = new TimeTicks(15);
            Assert.AreEqual(15, time.ToUInt32());
        }
        
        [Test]
        public void TestConstructor2()
        {
            TimeTicks time2 = new TimeTicks(new byte[] { 0x3F, 0xE0 });
            Assert.AreEqual(16352, time2.ToUInt32());
        }
        [Test]
        public void TestToBytes()
        {
            TimeTicks time = new TimeTicks(16352);
            ISnmpData data = DataFactory.CreateSnmpData(time.ToBytes());
            Assert.AreEqual(data.TypeCode, SnmpType.TimeTicks);
            Assert.AreEqual(16352, ((TimeTicks)data).ToUInt32());
            
            Assert.AreEqual(new byte[] {0x43, 0x05, 0x00, 0x93, 0xA3, 0x41, 0x4B}, new TimeTicks(2476949835).ToBytes());
        }
        
        [Test]
        public void TestToTimeSpan()
        {
            TimeTicks time = new TimeTicks(171447);
            TimeSpan result = time.ToTimeSpan();
            Assert.AreEqual(0, result.Hours);
            Assert.AreEqual(28, result.Minutes);
            Assert.AreEqual(34, result.Seconds);
            Assert.AreEqual(470, result.Milliseconds);
        }
    }
}
#pragma warning restore 1591,0618