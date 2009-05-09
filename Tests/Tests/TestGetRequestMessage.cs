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
using Lextm.SharpSnmpLib.Security;

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
        public void TestConstructorV3Auth1()
        {
            string bytes = "30 73" +
"02 01  03 " +
"30 0F " +
"02  02 35 41 " +
"02  03 00 FF E3" +
"04 01 05" +
"02  01 03" +
"04 2E  " +
"30 2C" +
"04 0D  80 00 1F 88 80 E9 63 00  00 D6 1F F4  49 " +
"02 01 0D  " +
"02 01 57 " +
"04 05 6C 65 78  6C 69 " +
"04 0C  1C 6D 67 BF  B2 38 ED 63 DF 0A 05 24  " +
"04 00 " +
"30 2D  " +
"04 0D 80 00  1F 88 80 E9 63 00 00 D6  1F F4 49 " +
"04  00 " +
"A0 1A 02  02 01 AF 02 01 00 02 01  00 30 0E 30  0C 06 08 2B  06 01 02 01 01 03 00 05  00";
            GetRequestMessage request = new GetRequestMessage(
                VersionCode.V3,
                13633,
                0x01AF,
                new OctetString("lexli"),
                new List<Variable>(1) { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.3.0"))},
                new SecurityRecord(new MD5AuthenticationProvider(new OctetString("testpass")), DefaultPrivacyProvider.Instance));
                //new Header(
                //    new Integer32(13633),
                //    new Integer32(0xFFE3),
                //    new OctetString(new byte[] { 0x5 }),
                //    new Integer32(3)),
                //new SecurityParameters(
                //    new OctetString(ByteTool.ConvertByteString("80 00 1F 88 80 E9 63 00  00 D6 1F F4  49")),
                //    new Integer32(0x0d),
                //    new Integer32(0x57),
                //    new OctetString("lexli"),
                //    new OctetString(new byte[12]),
                //    OctetString.Empty),
                //new Scope(
                //    new OctetString(ByteTool.ConvertByteString("80 00 1F 88 80 E9 63 00  00 D6 1F F4  49")),
                //    OctetString.Empty,
                //    new GetRequestPdu(
                //        new Integer32(0x01AF),
                //        ErrorCode.NoError,
                //        new Integer32(0),
                //        new List<Variable>(1) { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.3.0"), new Null()) })),
                //new SecurityRecord(new MD5AuthenticationProvider(new OctetString("testpass")), DefaultPrivacyProvider.Instance));
            Assert.AreEqual(SecurityLevel.Authentication, request.Level);
            request.Authenticate();
            string test = ByteTool.ConvertByteString(request.ToBytes());
            Assert.AreEqual(ByteTool.ConvertByteString(bytes), request.ToBytes());
        }

        [Test]
        public void TestConstructorV3Auth()
        {
            string bytes = "30 73" +
"02 01  03 " +
"30 0F " +
"02  02 35 41 " +
"02  03 00 FF E3" +
"04 01 05" +
"02  01 03" +
"04 2E  " +
"30 2C" +
"04 0D  80 00 1F 88 80 E9 63 00  00 D6 1F F4  49 " +
"02 01 0D  " +
"02 01 57 " +
"04 05 6C 65 78  6C 69 " +
"04 0C  1C 6D 67 BF  B2 38 ED 63 DF 0A 05 24  " +
"04 00 " +
"30 2D  " +
"04 0D 80 00  1F 88 80 E9 63 00 00 D6  1F F4 49 " +
"04  00 " +
"A0 1A 02  02 01 AF 02 01 00 02 01  00 30 0E 30  0C 06 08 2B  06 01 02 01 01 03 00 05  00";
            GetRequestMessage request = new GetRequestMessage(
                VersionCode.V3,
                new Header(
                    new Integer32(13633),
                    new Integer32(0xFFE3),
                    new OctetString(new byte[] { 0x5 }),
                    new Integer32(3)),
                new SecurityParameters(
                    new OctetString(ByteTool.ConvertByteString("80 00 1F 88 80 E9 63 00  00 D6 1F F4  49")),
                    new Integer32(0x0d),
                    new Integer32(0x57),
                    new OctetString("lexli"),
                    new OctetString(new byte[12]),
                    OctetString.Empty),
                new Scope(
                    new OctetString(ByteTool.ConvertByteString("80 00 1F 88 80 E9 63 00  00 D6 1F F4  49")),
                    OctetString.Empty,
                    new GetRequestPdu(
                        new Integer32(0x01AF),
                        ErrorCode.NoError,
                        new Integer32(0),
                        new List<Variable>(1) { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.3.0"), new Null() )})),
                new SecurityRecord(new MD5AuthenticationProvider(new OctetString("testpass")), DefaultPrivacyProvider.Instance));
            Assert.AreEqual(SecurityLevel.Authentication, request.Level);
            request.Authenticate();
            string test = ByteTool.ConvertByteString(request.ToBytes());
            Assert.AreEqual(ByteTool.ConvertByteString(bytes), request.ToBytes());
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
            Assert.AreEqual(bytes, ByteTool.ConvertByteString(request.ToBytes()));
        }
    }
}
#pragma warning restore 1591