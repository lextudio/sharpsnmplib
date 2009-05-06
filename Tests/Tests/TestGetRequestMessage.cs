/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/28
 * Time: 18:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

using NUnit.Framework;

#pragma warning disable 1591

namespace Lextm.SharpSnmpLib.Tests
{
    /// <summary>
    /// Description of TestGetMessage.
    /// </summary>
    [TestFixture]
    public class TestGetRequestMessage
    {
        [Test]
        public void Test()
        {
            byte[] expected = Resources.get;
            ISnmpMessage message = MessageFactory.ParseMessages(expected)[0];
            Assert.AreEqual(SnmpType.GetRequestPdu, message.Pdu.TypeCode);
            GetRequestPdu pdu = (GetRequestPdu)message.Pdu;
            Assert.AreEqual(1, pdu.Variables.Count);
            Variable v = pdu.Variables[0];
            Assert.AreEqual(new uint[] { 1, 3, 6, 1, 2, 1, 1, 6, 0 }, v.Id.ToNumerical());
            Assert.AreEqual(typeof(Null), v.Data.GetType());
            Assert.GreaterOrEqual(expected.Length, message.ToBytes().Length);
        }

        [Test]
        public void TestConstructor()
        {
            List<Variable> list = new List<Variable>(1);
            list.Add(new Variable(new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 1, 6, 0 }), new Null()));
            GetRequestMessage message = new GetRequestMessage(0, VersionCode.V2, new OctetString("public"), list);
            Assert.GreaterOrEqual(Resources.get.Length, message.ToBytes().Length);
        }
        
        [Test]
        public void TestConstructorV3()
        {
            string bytes = "30 3A 02 01 03 30 0F 02 02 6A 09 02 03 00 FF E3" +
                " 04 01 04 02 01 03 04 10 30 0E 04 00 02 01 00 02" +
                " 01 00 04 00 04 00 04 00 30 12 04 00 04 00 A0 0C" +
                " 02 02 2C 6B 02 01 00 02 01 00 30 00";
            GetRequestMessage request = new GetRequestMessage(
                VersionCode.V3, 
                new Header(
                    new Integer32(0x6A09),
                    new Integer32(0xFFE3), 
                    new OctetString(new byte[] { 0x4 }),
                    new Integer32(3)),
                new SecurityParameters(
                    OctetString.Empty, 
                    new Integer32(0),
                    new Integer32(0),
                    OctetString.Empty,
                    OctetString.Empty,
                    OctetString.Empty),
                new Scope(
                    OctetString.Empty,
                    OctetString.Empty,
                    new GetRequestPdu(new Integer32(0x2C6B), ErrorCode.NoError, new Integer32(0), new List<Variable>())),
                    Security.SecurityRecord.Default
               );
            Assert.AreEqual(bytes, ByteTool.ConvertByteSting(request.ToBytes()));
        }
    }
}
#pragma warning restore 1591