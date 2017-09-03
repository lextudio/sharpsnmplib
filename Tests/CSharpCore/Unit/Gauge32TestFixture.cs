/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/10
 * Time: 16:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using Xunit;

#pragma warning disable 1591, 0618, 1718
namespace Lextm.SharpSnmpLib.Unit
{
    public class Gauge32TestFixture
    {
        [Fact]
        public void TestEqual()
        {
            var left = new Gauge32(200);
            var right = new Gauge32(200);
            Assert.Equal(left, right);
// ReSharper disable EqualExpressionComparison
            Assert.True(left == left);
// ReSharper restore EqualExpressionComparison
            Assert.False(left == null);
            Assert.False(null == right);
            Assert.True(left != null);
            Assert.True(left.Equals(right));
            Assert.Equal(((uint)200).GetHashCode(), left.GetHashCode());
            Assert.Equal("200", left.ToString());

            Assert.False(left.Equals(1));

            Assert.Throws<ArgumentNullException>(() => left.AppendBytesTo(null));
            Assert.Throws<ArgumentNullException>(() => new Gauge32(new Tuple<int, byte[]>(0, new byte[] { 0 }), null));
            Assert.Throws<ArgumentNullException>(() => new Gauge32(null, new MemoryStream()));
        }
        
        [Fact]
        public void TestConstructor()
        {
            byte[] buffer2 = new byte[] {01, 44};
            Gauge32 c2 = new Gauge32(buffer2);
            Assert.Equal(300U, c2.ToUInt32());

            
            byte[] buffer1 = new byte[] {13};
            Gauge32 c1 = new Gauge32(buffer1);
            Assert.Equal(13U, c1.ToUInt32());
            
            byte[] buffer3 = new byte[] {1, 17, 112};
            Gauge32 c3 = new Gauge32(buffer3);
            Assert.Equal(70000U, c3.ToUInt32());
            
            byte[] buffer4 = new byte[] {1, 201, 195, 128};
            Gauge32 c4 = new Gauge32(buffer4);
            Assert.Equal(30000000U, c4.ToUInt32());
            
            byte[] buffer5 = new byte[] {0, 255, 255, 255, 255};
            Gauge32 c5 = new Gauge32(buffer5);
            Assert.Equal(uint.MaxValue, c5.ToUInt32());
            
            byte[] buffer0 = new byte[] {0};
            Gauge32 c0 = new Gauge32(buffer0);
            Assert.Equal(uint.MinValue, c0.ToUInt32());
        }
        
        [Fact]
        public void TestToBytes()
        {
            Gauge32 c0 = new Gauge32(0);
            Gauge32 r0 = (Gauge32)DataFactory.CreateSnmpData(c0.ToBytes());
            Assert.Equal(r0, c0);
            
            Gauge32 c5 = new Gauge32(uint.MaxValue);
            Gauge32 r5 = (Gauge32)DataFactory.CreateSnmpData(c5.ToBytes());
            Assert.Equal(r5, c5);
            
            Gauge32 c4 = new Gauge32(30000000);
            Gauge32 r4 = (Gauge32)DataFactory.CreateSnmpData(c4.ToBytes());
            Assert.Equal(r4, c4);
            
            Gauge32 c3 = new Gauge32(70000);
            Gauge32 r3 = (Gauge32)DataFactory.CreateSnmpData(c3.ToBytes());
            Assert.Equal(r3, c3);
            
            Gauge32 c1 = new Gauge32(13);
            Gauge32 r1 = (Gauge32)DataFactory.CreateSnmpData(c1.ToBytes());
            Assert.Equal(r1, c1);
            
            Gauge32 c2 = new Gauge32(300);
            Gauge32 r2 = (Gauge32)DataFactory.CreateSnmpData(c2.ToBytes());
            Assert.Equal(r2, c2);
            
            Assert.Equal(new byte[] {0x42, 0x01, 0x03}, new Gauge32(3).ToBytes());
            Assert.Equal(new byte[] {0x42, 0x05, 0x00, 0x80, 0x00, 0x00, 0x00}, new Gauge32(2147483648).ToBytes());
        }
    }
}
#pragma warning restore 1591,0618,1718
