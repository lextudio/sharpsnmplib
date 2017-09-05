/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/8/3
 * Time: 10:01
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using Xunit;

#pragma warning disable 1591, 0618
namespace Lextm.SharpSnmpLib.Unit
{
    /// <summary>
    /// Description of TestInteger32.
    /// </summary>
    public class Integer32TestFixture
    {
        [Fact]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => new Integer32(new Tuple<int, byte[]>(0, new byte[] { 0 }), null));
            Assert.Throws<ArgumentException>(() => new Integer32(new Tuple<int, byte[]>(0, new byte[] { 0 }), new MemoryStream()));
            Assert.Throws<ArgumentException>(() => new Integer32(new Tuple<int, byte[]>(-1, new[] { (byte)255 }), new MemoryStream()));
            Assert.Throws<ArgumentException>(() => new Integer32(new Tuple<int, byte[]>(6, new byte[] { 6 }), new MemoryStream()));
            Assert.Throws<ArgumentNullException>(() => new Integer32(6).AppendBytesTo(null));
            Assert.Throws<ArgumentNullException>(() => new Integer32(null, new MemoryStream()));
        }

        [Fact]
        public void TestEqual()
        {
            var left = new Integer32(599);
            var right = new Integer32(599);
            Assert.Equal(left, right);
// ReSharper disable RedundantCast
// ReSharper disable EqualExpressionComparison
            Assert.True((Integer32)null == (Integer32)null);
// ReSharper restore EqualExpressionComparison
// ReSharper restore RedundantCast
            Assert.NotEqual(null, right);
            Assert.NotEqual(left, null);
            Assert.True(left.Equals(right));
            Assert.True(left != null);
        }
        
        [Fact]
        public void TestNegative()
        {
            const int i = -2147418240;
            Integer32 data = new Integer32(i);
            byte[] bytes = data.ToBytes();
            Integer32 other = (Integer32)DataFactory.CreateSnmpData(bytes);
            Assert.Equal(i, other.ToInt32());
        }
        
        [Fact]
        public void TestNegative3()
        {
            // #7240
            const int i = -237053658;
            Integer32 data = new Integer32(i);
            byte[] bytes = data.ToBytes();
            Assert.Equal(6, bytes.Length);
            var exception = Assert.Throws<SnmpException>(() => DataFactory.CreateSnmpData(new byte[] { 0x02, 0x05, 0xFF, 0xF1, 0xDE, 0xD9, 0x26 }));
            Assert.Equal($"Truncation error for 32-bit integer coding.{Environment.NewLine}Parameter name: length", exception.InnerException.Message);
        }
        
        [Fact]
        public void TestConstructor()
        {
            Integer32 test = new Integer32(100);
            Assert.Equal(100, test.ToInt32());
            Assert.Throws<InvalidCastException>(() => test.ToErrorCode());
            Assert.Throws<InvalidCastException>(() => new Integer32(-1).ToErrorCode());
            
            Integer32 test2 = new Integer32(new byte[] {0x00});
            Assert.Equal(0, test2.ToInt32());
            
            Integer32 test3 = new Integer32(new byte[] {0xFF});
            Assert.Equal(-1, test3.ToInt32());
        }
        
        [Fact]
        public void TestToInt32()
        {
            const int result = -26955;
            byte[] expected = new byte[] {0x96, 0xB5};
            Integer32 test = new Integer32(expected);
            Assert.Equal(result, test.ToInt32());
            
            Assert.Equal(255, new Integer32(new byte[] {0x00, 0xFF}).ToInt32());
        }
        
        [Fact]
        public void TestToBytes()
        {
            byte[] bytes = new byte[] {0x02, 0x02, 0x96, 0xB5};
            Integer32 test = new Integer32(-26955);
            Assert.Equal(bytes, test.ToBytes());
            
            Assert.Equal(new byte[] {0x02, 0x02, 0x00, 0xFF}, new Integer32(255).ToBytes());
            
            Assert.Equal(6, new Integer32(2147483647).ToBytes().Length);
        }
        
        [Fact]
        public void TestToBytes2()
        {
            Integer32 i = new Integer32(-1);
            Assert.Equal(new byte[] {0x02, 0x01, 0xFF}, i.ToBytes());
        }
        
        [Fact]
        public void TestNegative2()
        {
            // bug 7217 https://sharpsnmplib.codeplex.com/workitem/7217
            Integer32 i = new Integer32(-250);
            var result = DataFactory.CreateSnmpData(i.ToBytes());
            Assert.Equal(SnmpType.Integer32, result.TypeCode);
            Assert.Equal(-250, ((Integer32)result).ToInt32());
        }
    }
}
#pragma warning restore 1591,0618
