/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 20:29
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
#pragma warning disable 1591
namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class TestGetResponseMessage
    {
        [Test]
        public void TestMethod()
        {
            MemoryStream m = new MemoryStream(Resource.getresponse, false);
            ISnmpMessage message = MessageFactory.ParseMessages(m)[0];
            Assert.AreEqual(SnmpType.GetResponsePdu, message.Pdu.TypeCode);
            ISnmpPdu pdu = message.Pdu;
            Assert.AreEqual(SnmpType.GetResponsePdu, pdu.TypeCode);
            GetResponsePdu response = (GetResponsePdu)pdu;
            Assert.AreEqual(ErrorCode.NoError, response.ErrorStatus);
            Assert.AreEqual(0, response.ErrorIndex);
            Assert.AreEqual(1, response.Variables.Count);
            Variable v = response.Variables[0];
            Assert.AreEqual(new uint[] { 1, 3, 6, 1, 2, 1, 1, 6, 0 }, v.Id.ToNumerical());
            Assert.AreEqual("Shanghai", v.Data.ToString());
        }
    }
}
#pragma warning restore 1591

