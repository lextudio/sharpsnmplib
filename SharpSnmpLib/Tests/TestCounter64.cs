/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/8/3
 * Time: 12:57
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#pragma warning disable 1591
namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class TestCounter64
    {
        [Test]
        public void TestContructor()
        {
            Assert.AreEqual(14532202884452442569, new Counter64(new byte[] {0x00, 0xC9, 0xAC, 0xC1, 0x87, 0x4B, 0xB1, 0xE1, 0xC9}).ToUInt64());
        }
        
        [Test]
        public void TestToBytes()
        {
            Assert.AreEqual(new byte[] {0x46, 0x09, 0x00, 0xC9, 0xAC, 0xC1, 0x87, 0x4B, 0xB1, 0xE1, 0xC9}, new Counter64(14532202884452442569).ToBytes());
        }
    }
}
#pragma warning restore 1591