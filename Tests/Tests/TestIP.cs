/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 11:39
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#pragma warning disable 1591,0618
namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class TestIP
    {
        [Test]
        public void TestConstructor()
        {
            string expected = "127.0.0.1";
            IP ip = new IP(expected);
            Assert.AreEqual(expected, ip.ToString());
        }
        [Test]
        public void TestToBytes()
        {
            IP ip = new IP("129.213.224.111");
            Assert.AreEqual(new byte[] {0x40, 0x04, 0x81, 0xD5, 0xE0, 0x6F}, ip.ToBytes());
        }
    }
}
#pragma warning restore 1591,0618