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
    /// <summary>
    /// Description of TestEndOfMibView.
    /// </summary>
    public class EndOfMibViewTestFixture
    {
        [Fact]
        public void TestToBytes()
        {
            EndOfMibView obj = new EndOfMibView();
            Assert.Equal(new byte[] { 0x82, 0x00 }, obj.ToBytes());
            Assert.Equal(0, obj.GetHashCode());

            EndOfMibView right = new EndOfMibView();
            Assert.Equal(obj, right);
            Assert.True(obj.Equals(right));
            Assert.True(obj != null);
// ReSharper disable EqualExpressionComparison
            Assert.True(obj == obj);
// ReSharper restore EqualExpressionComparison

            Assert.Throws<ArgumentNullException>(() => obj.AppendBytesTo(null));
            Assert.Equal("EndOfMibView", obj.ToString());
        }
    }
}
#pragma warning restore 1591, 0618,1718
