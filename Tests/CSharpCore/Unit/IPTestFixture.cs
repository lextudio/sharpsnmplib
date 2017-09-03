/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 11:39
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using System.Net;
using Xunit;

#pragma warning disable 1591, 0618, 1718
namespace Lextm.SharpSnmpLib.Unit
{
    public class IPTestFixture
    {
        [Fact]
        public void TestException()
        {
            var test = new IP(IPAddress.Any.GetAddressBytes());
            Assert.Throws<ArgumentNullException>(() => test.AppendBytesTo(null));
            Assert.Throws<ArgumentNullException>(() => new IP(new Tuple<int, byte[]>(0, new byte[] { 0 }), null));
            Assert.Throws<ArgumentNullException>(() => new IP((byte[]) null));
            Assert.Throws<ArgumentException>(() => new IP(new Tuple<int, byte[]>(1, new byte[] { 1 }), new MemoryStream()));
            Assert.Throws<FormatException>(() => new IP("test"));
            Assert.Throws<ArgumentNullException>(() => new IP(null, new MemoryStream()));
        }

        [Fact]
        public void TestConstructor()
        {
            const string expected = "127.0.0.1";
            IP ip = new IP(expected);
            Assert.Equal(expected, ip.ToString());
            var test = new IP(IPAddress.Any.GetAddressBytes());
        }
        
        [Fact]
        public void TestToBytes()
        {
            IP ip = new IP("129.213.224.111");
            Assert.Equal(new byte[] {0x40, 0x04, 0x81, 0xD5, 0xE0, 0x6F}, ip.ToBytes());
        }
        
        [Fact]
        public void TestEquals()
        {
            IP actual = new IP("172.0.0.1");
            IP target = new IP("172.0.0.1");
            IP another = new IP("172.0.0.0");

            Assert.True(actual.Equals(target));
            Assert.True(actual == target);
// ReSharper disable EqualExpressionComparison
            Assert.True(actual == actual);
// ReSharper restore EqualExpressionComparison
            Assert.Equal(actual, target);
            Assert.False(actual == another);
            Assert.True(actual != another);
            Assert.NotEqual(actual, another);

            Assert.False(actual.Equals(1));
        }
    }
}
#pragma warning restore 1591,0618,1718
