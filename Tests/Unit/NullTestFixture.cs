/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/3
 * Time: 20:26
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using Xunit;

#pragma warning disable 1591, 0618, 1718
namespace Lextm.SharpSnmpLib.Unit
{
    public class NullTestFixture
    {
        [Fact]
        public void TestConstructors()
        {
            Assert.Throws<ArgumentNullException>(() => new Null(null, null));
            Assert.Throws<ArgumentNullException>(() => new Null(new Tuple<int, byte[]>(0, new byte[0]), null));
        }
        
        [Fact]
        public void TestMethod()
        {
            Assert.Equal(false, new Null().Equals(null));
        }
        
        [Fact]
        public void TestToBytes()
        {
            Assert.Equal(new byte[] { 0x05, 0x00 }, new Null().ToBytes());
            Assert.Equal(0, new Null().GetHashCode());
        }
        
        [Fact]
        public void TestEqual()
        {
            var left = new Null();
            var right = new Null();
            Assert.Equal(left, right);
            Assert.True(left == right);
            Assert.True(left.Equals(right));
            Assert.True(left != null);
            // ReSharper disable EqualExpressionComparison
            Assert.True(left == left);
            // ReSharper restore EqualExpressionComparison
            Assert.Throws<ArgumentNullException>(() => left.AppendBytesTo(null));
            Assert.Equal("Null", left.ToString());

            Assert.False(left.Equals(1));
        }
    }
}
#pragma warning restore 1591, 0618,1718
