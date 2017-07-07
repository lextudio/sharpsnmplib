/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2009/2/15
 * Time: 20:00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using Xunit;

#pragma warning disable 1591, 0618, 1718
namespace Lextm.SharpSnmpLib.Unit
{
    public class NoSuchObjectTestFixture
    {
        [Fact]
        public void TestToBytes()
        {
            NoSuchObject obj = new NoSuchObject();
            Assert.Equal(new byte[] { 0x80, 0x00 }, obj.ToBytes());
            Assert.Equal(0, obj.GetHashCode());
        }
        
        [Fact]
        public void TestEqual()
        {
            var left = new NoSuchObject();
            var right = new NoSuchObject();
            Assert.Equal(left, right);
            Assert.True(left == right);
// ReSharper disable EqualExpressionComparison
            Assert.True(left == left);
// ReSharper restore EqualExpressionComparison
            Assert.True(left.Equals(right));
            Assert.True(left != null);

            Assert.Throws<ArgumentNullException>(() => left.AppendBytesTo(null));
            Assert.Equal("NoSuchObject", left.ToString());
        }
    }
}
#pragma warning restore 1591, 0618,1718
