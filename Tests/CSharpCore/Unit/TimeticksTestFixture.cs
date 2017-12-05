/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 11:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using Xunit;

#pragma warning disable 1591, 0618, 1718
namespace Lextm.SharpSnmpLib.Unit
{
    public class TimeticksTestFixture
    {
        [Fact]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => new TimeTicks(new Tuple<int, byte[]>(0, new byte[] { 0 }), null));   
            Assert.Throws<ArgumentNullException>(() => new TimeTicks(0).AppendBytesTo(null));
        }
        
        [Fact]
        public void TestConstructor3()
        {
            TimeTicks time = new TimeTicks(800);
            uint count = time.ToUInt32();
            Assert.Equal(time, new TimeTicks(count));
        }

        [Fact]
        public void TestConstructor()
        {
            TimeTicks time = new TimeTicks(15);
            Assert.Equal(15U, time.ToUInt32());
            Assert.Equal("00:00:00.1500000", time.ToString());
        }
        
        [Fact]
        public void TestConstructor2()
        {
            TimeTicks time2 = new TimeTicks(new byte[] { 0x3F, 0xE0 });
            Assert.Equal(16352U, time2.ToUInt32());
            Assert.Equal(16352.GetHashCode(), time2.GetHashCode());
        }
        [Fact]
        public void TestToBytes()
        {
            TimeTicks time = new TimeTicks(16352);
            ISnmpData data = DataFactory.CreateSnmpData(time.ToBytes());
            Assert.Equal(SnmpType.TimeTicks, data.TypeCode);
            Assert.Equal(16352U, ((TimeTicks)data).ToUInt32());
            
            Assert.Equal(new byte[] {0x43, 0x05, 0x00, 0x93, 0xA3, 0x41, 0x4B}, new TimeTicks(2476949835).ToBytes());
        }
        
        [Fact]
        public void TestToTimeSpan()
        {
            TimeTicks time = new TimeTicks(171447);
            TimeSpan result = time.ToTimeSpan();
            Assert.Equal(0, result.Hours);
            Assert.Equal(28, result.Minutes);
            Assert.Equal(34, result.Seconds);
            Assert.Equal(470, result.Milliseconds);
        }
        
        [Fact]
        public void TestConstructor4()
        {
        	var result = new TimeTicks(171447);
        	var ticks = new TimeTicks(new TimeSpan(0, 0, 28, 34, 470));
        	Assert.Equal(result, ticks);
        }
        
        [Fact]
        public void TestEqual()
        {
            var left = new TimeTicks(800);
            var right = new TimeTicks(800);
            Assert.Equal(left, right);
// ReSharper disable EqualExpressionComparison
            Assert.True(left == left);
// ReSharper restore EqualExpressionComparison
            Assert.True(left != null);
            Assert.True(left.Equals(right));

            Assert.False(left.Equals(1));
        }
    }
}
#pragma warning restore 1591,0618,1718
