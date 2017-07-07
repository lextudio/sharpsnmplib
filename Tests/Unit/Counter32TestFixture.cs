using System;
using System.IO;
using Xunit;

#pragma warning disable 1591,0618, 1718
namespace Lextm.SharpSnmpLib.Unit
{
    public class Counter32TestFixture
    {
        [Fact]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(()=> new Counter32(0).AppendBytesTo(null));
            Assert.Throws<ArgumentNullException>(()=> new Counter32(null, new MemoryStream()));
        }

        [Fact]
        public void TestOverflow()
        {
            Assert.Equal(new Counter32((uint)32), new Counter32((long)32));
            Assert.Equal(new Counter32((uint)32), new Counter32((long)uint.MaxValue + 33));
        }
        
        [Fact]
        public void TestEqual()
        {
            var left = new Counter32(100);
            var right = new Counter32(100);
            Assert.Equal(left, right);
            Assert.False(left.Equals(1));
        }
        
        [Fact]
        public void TestConstructor()
        {
            byte[] buffer2 = {01, 44};
            Counter32 c2 = new Counter32(buffer2);
            Assert.Equal(300U, c2.ToUInt32());
            Assert.Equal("300", c2.ToString());
            Counter32 sec = new Counter32(300);
            Counter32 thr = new Counter32(301);
            Assert.True(c2.Equals(sec));
            Assert.Equal(300.GetHashCode(), c2.GetHashCode());
            Assert.True(c2 == sec);
            Assert.True(c2 != thr);
            Assert.False(c2 == null);
            Assert.False(null == sec);
            Assert.True(c2 == c2);
            
            byte[] buffer1 = {13};
            Counter32 c1 = new Counter32(buffer1);
            Assert.Equal(13U, c1.ToUInt32());
            
            byte[] buffer3 = {1, 17, 112};
            Counter32 c3 = new Counter32(buffer3);
            Assert.Equal(70000U, c3.ToUInt32());
            
            byte[] buffer4 = {1, 201, 195, 128};
            Counter32 c4 = new Counter32(buffer4);
            Assert.Equal(30000000U, c4.ToUInt32());
            
            byte[] buffer5 = {0, 255, 255, 255, 255};
            Counter32 c5 = new Counter32(buffer5);
            Assert.Equal(uint.MaxValue, c5.ToUInt32());
            
            byte[] buffer0 = {0};
            Counter32 c0 = new Counter32(buffer0);
            Assert.Equal(uint.MinValue, c0.ToUInt32());
        }
        
        [Fact]
        public void TestConstructor2()
        {
            Counter32 test = new Counter32(300);
            Assert.Equal(300U, test.ToUInt32());
        }
        
        [Fact]
        public void TestContructor3()
        {
            Assert.Throws<ArgumentNullException>(() => new Counter32(new Tuple<int, byte[]>(1, new byte[] { 1 }), null));
        }
        
        [Fact]
        public void TestConstructor4()
        {
            Assert.Throws<ArgumentException>(() => new Counter32(new Tuple<int, byte[]>(0, new byte[] { 0 }), new MemoryStream()));
        }
        
        [Fact]
        public void TestConstructor5()
        {
            Assert.Throws<ArgumentException>(() => new Counter32(new Tuple<int, byte[]>(6, new byte[] { 6 }), new MemoryStream()));
        }
        
        [Fact]
        public void TestConstructor6()
        {
            byte[] buffer5 = {3, 255, 255, 255, 255};
            Assert.Throws<ArgumentException>(() => new Counter32(buffer5));
        }
        
        [Fact]
        public void TestToBytes()
        {
            Counter32 c0 = new Counter32(0);
            Counter32 r0 = (Counter32)DataFactory.CreateSnmpData(c0.ToBytes());
            Assert.Equal(r0, c0);
            
            Counter32 c5 = new Counter32(uint.MaxValue);
            Counter32 r5 = (Counter32)DataFactory.CreateSnmpData(c5.ToBytes());
            Assert.Equal(r5, c5);
            
            Counter32 c4 = new Counter32(30000000);
            Counter32 r4 = (Counter32)DataFactory.CreateSnmpData(c4.ToBytes());
            Assert.Equal(r4, c4);
            
            Counter32 c3 = new Counter32(70000);
            Counter32 r3 = (Counter32)DataFactory.CreateSnmpData(c3.ToBytes());
            Assert.Equal(r3, c3);
            
            Counter32 c1 = new Counter32(13);
            Counter32 r1 = (Counter32)DataFactory.CreateSnmpData(c1.ToBytes());
            Assert.Equal(r1, c1);
            
            Counter32 c2 = new Counter32(300);
            Counter32 r2 = (Counter32)DataFactory.CreateSnmpData(c2.ToBytes());
            Assert.Equal(r2, c2);
            
            Counter32 c255 = new Counter32(255);
            Assert.Equal(new byte[] {0x41, 0x02, 0x00, 0xff}, c255.ToBytes());
            
            Assert.Equal(new byte[] {0x41, 0x01, 0x04}, new Counter32(4).ToBytes());
        }
    }
}
#pragma warning restore 1591,0618, 1718
