/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/8/3
 * Time: 12:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using Xunit;

#pragma warning disable 1591, 0618
namespace Lextm.SharpSnmpLib.Unit
{
    public class OpaqueTestFixture
    {
        [Fact]
        public void TestException()
        {
            Assert.Throws<NullReferenceException>(()=>new Opaque(null));
            Assert.Throws<ArgumentNullException>(() => new Opaque(null, null));
            Assert.Throws<ArgumentNullException>(() => new Opaque(new Tuple<int, byte[]>(1, new byte[0]), null));
        }

        [Fact]
        public void TestConstructor()
        {
            Gauge32 e = new Gauge32(3);
            Opaque test = new Opaque(e.ToBytes());
            Assert.Equal(new byte[] {0x44, 0x03, 0x42, 0x01, 0x03}, test.ToBytes());
            Assert.Equal("42 01 03", test.ToString());
            Assert.Throws<ArgumentNullException>(() => test.AppendBytesTo(null));
        }
        
        [Fact]
        public void TestEqual()
        {
            var left = new Opaque(new byte[] { 0x80, 0x77 });
            var right = new Opaque(new byte[] { 0x80, 0x77 });
            Assert.Equal(left, right);
// ReSharper disable RedundantCast
            Assert.Equal((Opaque)null, (Opaque)null);
// ReSharper restore RedundantCast
            Assert.NotEqual(null, right);
            Assert.NotEqual(left, null);
            Assert.True(left != null);
            Assert.True(null != right);
// ReSharper disable EqualExpressionComparison
            Assert.True((Opaque)null == (Opaque)null);
// ReSharper restore EqualExpressionComparison
            Assert.True(left.Equals(right));

            Assert.False(left.Equals(1));
        }
    }
}
#pragma warning restore 1591,0618
