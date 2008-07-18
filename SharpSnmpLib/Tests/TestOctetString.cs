/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/2
 * Time: 12:24
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
    public class TestOctetString
    {
        [Test]
        public void TestMethod()
        {
            byte[] expected = new byte[] {0x04, 0x06, 0x70, 0x75, 0x62, 0x6C, 0x69, 0x63};
            ISnmpData data = SnmpDataFactory.CreateSnmpData(expected);
            Assert.AreEqual(SnmpType.OctetString, data.TypeCode);
            OctetString s = (OctetString)data;
            Assert.AreEqual("public", s.ToString());
        }
    }
}
#pragma warning restore 1591