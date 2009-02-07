/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/8/3
 * Time: 12:25
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
    public class TestOpaque
    {
        [Test]
        public void TestConstructor()
        {
            Gauge32 e = new Gauge32(3);
            Opaque test = new Opaque(e.ToBytes());
            Assert.AreEqual(new byte[] {0x44, 0x03, 0x42, 0x01, 0x03}, test.ToBytes());
        }
    }
}
#pragma warning restore 1591,0618