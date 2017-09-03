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
    public class NoSuchInstanceTestFixture
    {
        [Fact]
        public void TestToBytes()
        {
            NoSuchInstance obj = new NoSuchInstance();
            Assert.Equal(new byte[] { 0x81, 0x00 }, obj.ToBytes());
            Assert.Equal(0, obj.GetHashCode());
        }
        
        [Fact]
        public void TestEqual()
        {
            var left = new NoSuchInstance();
            var right = new NoSuchInstance();
            Assert.Equal(left, right);
            Assert.True(left == right);
            Assert.True(left.Equals(right));
            Assert.True(left != null);
            // ReSharper disable EqualExpressionComparison
            Assert.True(left == left);
            // ReSharper restore EqualExpressionComparison

            Assert.Throws<ArgumentNullException>(() => left.AppendBytesTo(null));
            Assert.Equal("NoSuchInstance", left.ToString());
        }
    }
}
#pragma warning restore 1591, 0618,1718
