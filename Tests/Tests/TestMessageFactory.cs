/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/8/8
 * Time: 19:32
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.IO;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using NUnit.Framework;

#pragma warning disable 1591, 0618
namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class TestMessageFactory
    {      
        [Test]
        public void TestInform()
        {
            byte[] data = new byte[] { 0x30, 0x5d, 0x02, 0x01, 0x01, 0x04, 0x06, 0x70, 0x75, 0x62, 0x6c, 0x69, 0x63, 0xa6, 0x50, 0x02, 0x01, 0x01, 0x02, 0x01,
                0x00, 0x02, 0x01, 0x00, 0x30, 0x45, 0x30, 0x0e, 0x06, 0x08, 0x2b, 0x06, 0x01, 0x02, 0x01, 0x01, 0x03, 0x00, 0x43, 0x02,
                0x3f, 0xe0, 0x30, 0x18, 0x06, 0x0a, 0x2b, 0x06, 0x01, 0x06, 0x03, 0x01, 0x01, 0x04, 0x01, 0x00, 0x06, 0x0a, 0x2b, 0x06,
                0x01, 0x04, 0x01, 0x90, 0x72, 0x87, 0x68, 0x02, 0x30, 0x19, 0x06, 0x0b, 0x2b, 0x06, 0x01, 0x04, 0x01, 0x90, 0x72, 0x87,
                0x69, 0x15, 0x00, 0x04, 0x0a, 0x49, 0x6e, 0x66, 0x6f, 0x72, 0x6d, 0x54, 0x65, 0x73, 0x74 };

            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(data, new UserRegistry());
            Assert.AreEqual(SnmpType.InformRequestPdu, messages[0].Pdu.TypeCode);
            //Assert.AreEqual(4, messages[0].TimeStamp);
        }
        
        [Test]
        public void TestString()
        {
            string bytes = "30 29 02 01 00 04 06 70 75 62 6c 69 63 a0 1c 02 04 4f 89 fb dd" + Environment.NewLine +
                "02 01 00 02 01 00 30 0e 30 0c 06 08 2b 06 01 02 01 01 05 00 05 00";
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, new UserRegistry());
            Assert.AreEqual(1, messages.Count);
            GetRequestMessage m = (GetRequestMessage)messages[0];
            Variable v = m.Variables[0];
            string i = v.Id.ToString();
            Assert.AreEqual(".1.3.6.1.2.1.1.5.0", i);
        }
        
        [Test]
        public void TestBrokenString()
        {
            bool hasException = false;
            try
            {
                const string bytes = "30 39 02 01 01 04 06 70 75 62 6C 69 63 A7 2C 02 01 01 02 01 00 02 01 00 30 21 30 0D 06 08 2B 06 01 02 01 01";
                IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, new UserRegistry());
                Assert.AreEqual(1, messages.Count);
            }
            catch (Exception)
            {
                hasException = true;
            }
            
            Assert.IsTrue(hasException);
        }

        [Test]
        public void TestDiscovery()
        {
            const string bytes = "30 3A 02 01  03 30 0F 02  02 6A 09 02  03 00 FF E3" +
                                 "04 01 04 02  01 03 04 10  30 0E 04 00  02 01 00 02" +
                                 "01 00 04 00  04 00 04 00  30 12 04 00  04 00 A0 0C" +
                                 "02 02 2C 6B  02 01 00 02  01 00 30 00";
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, new UserRegistry());
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(OctetString.Empty, messages[0].Parameters.UserName);
        }

        [Test]
        public void TestGetV3()
        {
            const string bytes = "30 68 02 01  03 30 0F 02  02 6A 08 02  03 00 FF E3" +
                                 "04 01 04 02  01 03 04 23  30 21 04 0D  80 00 1F 88" +
                                 "80 E9 63 00  00 D6 1F F4  49 02 01 05  02 02 0F 1B" +
                                 "04 05 6C 65  78 74 6D 04  00 04 00 30  2D 04 0D 80" +
                                 "00 1F 88 80  E9 63 00 00  D6 1F F4 49  04 00 A0 1A" +
                                 "02 02 2C 6A  02 01 00 02  01 00 30 0E  30 0C 06 08" +
                                 "2B 06 01 02  01 01 03 00  05 00";
            UserRegistry registry = new UserRegistry();
            registry.Add(new OctetString("lextm"), DefaultPrivacyProvider.DefaultPair);
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.AreEqual(1, messages.Count);
            GetRequestMessage get = (GetRequestMessage)messages[0];
            Assert.AreEqual(27144, get.MessageId);
            //Assert.AreEqual(SecurityLevel.None | SecurityLevel.Reportable, get.Level);
            Assert.AreEqual("lextm", get.Community.ToString());
        }

        [Test]
        public void TestGetRequestV3AuthPriv()
        {
            const string bytes = "30 81 80 02  01 03 30 0F  02 02 6C 99  02 03 00 FF" +
                                 "E3 04 01 07  02 01 03 04  38 30 36 04  0D 80 00 1F" +
                                 "88 80 E9 63  00 00 D6 1F  F4 49 02 01  14 02 01 35" +
                                 "04 07 6C 65  78 6D 61 72  6B 04 0C 80  50 D9 A1 E7" +
                                 "81 B6 19 80  4F 06 C0 04  08 00 00 00  01 44 2C A3" +
                                 "B5 04 30 4B  4F 10 3B 73  E1 E4 BD 91  32 1B CB 41" +
                                 "1B A1 C1 D1  1D 2D B7 84  16 CA 41 BF  B3 62 83 C4" +
                                 "29 C5 A4 BC  32 DA 2E C7  65 A5 3D 71  06 3C 5B 56" +
                                 "FB 04 A4";
            MD5AuthenticationProvider auth = new MD5AuthenticationProvider(new OctetString("testpass"));
            var registry = new UserRegistry();
            registry.Add(new OctetString("lexmark"), new DESPrivacyProvider(new OctetString("passtest"), auth));
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.AreEqual(1, messages.Count);
            GetRequestMessage get = (GetRequestMessage)messages[0];
            Assert.AreEqual(27801, get.MessageId);
            //Assert.AreEqual(SecurityLevel.None | SecurityLevel.Reportable, get.Level);
            Assert.AreEqual("lexmark", get.Community.ToString());
            //OctetString digest = new MD5AuthenticationProvider(new OctetString("testpass")).ComputeHash(get);

            //Assert.AreEqual(digest, get.Parameters.AuthenticationParameters);
        }

        [Test]
        public void TestGetRequestV3Auth()
        {
            const string bytes = "30 73"+
                                 "02 01  03 "+
                                 "30 0F "+
                                 "02  02 35 41 "+
                                 "02  03 00 FF E3"+
                                 "04 01 05"+
                                 "02  01 03"+
                                 "04 2E  "+
                                 "30 2C"+
                                 "04 0D  80 00 1F 88 80 E9 63 00  00 D6 1F F4  49 "+
                                 "02 01 0D  "+
                                 "02 01 57 "+
                                 "04 05 6C 65 78  6C 69 "+
                                 "04 0C  1C 6D 67 BF  B2 38 ED 63 DF 0A 05 24  "+
                                 "04 00 "+
                                 "30 2D  "+
                                 "04 0D 80 00  1F 88 80 E9 63 00 00 D6  1F F4 49 "+
                                 "04  00 "+
                                 "A0 1A 02  02 01 AF 02 01 00 02 01  00 30 0E 30  0C 06 08 2B  06 01 02 01 01 03 00 05  00";
            UserRegistry registry = new UserRegistry();
            registry.Add(new OctetString("lexli"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("testpass"))));
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.AreEqual(1, messages.Count);
            GetRequestMessage get = (GetRequestMessage)messages[0];
            Assert.AreEqual(13633, get.MessageId);
            //Assert.AreEqual(SecurityLevel.None | SecurityLevel.Reportable, get.Level);
            Assert.AreEqual("lexli", get.Community.ToString());
            //OctetString digest = new MD5AuthenticationProvider(new OctetString("testpass")).ComputeHash(get);

            //Assert.AreEqual(digest, get.Parameters.AuthenticationParameters);
        }

        [Test]
        public void TestGetResponseV3()
        {
            const string bytes = "30 6B 02 01  03 30 0F 02  02 6A 08 02  03 00 FF E3" +
                                 "04 01 00 02  01 03 04 23  30 21 04 0D  80 00 1F 88" +
                                 "80 E9 63 00  00 D6 1F F4  49 02 01 05  02 02 0F 1C" +
                                 "04 05 6C 65  78 74 6D 04  00 04 00 30  30 04 0D 80" +
                                 "00 1F 88 80  E9 63 00 00  D6 1F F4 49  04 00 A2 1D" +
                                 "02 02 2C 6A  02 01 00 02  01 00 30 11  30 0F 06 08" +
                                 "2B 06 01 02  01 01 03 00  43 03 05 E7  14";
            UserRegistry registry = new UserRegistry();
            registry.Add(new OctetString("lextm"), DefaultPrivacyProvider.DefaultPair);
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.AreEqual(1, messages.Count);
            ISnmpMessage message = messages[0];
            Assert.AreEqual("80001F8880E9630000D61FF449", message.Parameters.EngineId.ToHexString());
            Assert.AreEqual(5, message.Parameters.EngineBoots.ToInt32());
            Assert.AreEqual(3868, message.Parameters.EngineTime.ToInt32());
            Assert.AreEqual("lextm", message.Parameters.UserName.ToString());
            Assert.AreEqual("", message.Parameters.AuthenticationParameters.ToHexString());
            Assert.AreEqual("", message.Parameters.PrivacyParameters.ToHexString());
            Assert.AreEqual("80001F8880E9630000D61FF449", message.Scope.ContextEngineId.ToHexString());
            Assert.AreEqual("", message.Scope.ContextName.ToHexString());
        }

        [Test]
        public void TestDiscoveryResponse()
        {
            const string bytes = "30 66 02 01  03 30 0F 02  02 6A 09 02  03 00 FF E3" +
                                 "04 01 00 02  01 03 04 1E  30 1C 04 0D  80 00 1F 88" +
                                 "80 E9 63 00  00 D6 1F F4  49 02 01 05  02 02 0F 1B" +
                                 "04 00 04 00  04 00 30 30  04 0D 80 00  1F 88 80 E9" +
                                 "63 00 00 D6  1F F4 49 04  00 A8 1D 02  02 2C 6B 02" +
                                 "01 00 02 01  00 30 11 30  0F 06 0A 2B  06 01 06 03" +
                                 "0F 01 01 04  00 41 01 03";
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, new UserRegistry());
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(5, messages[0].Parameters.EngineBoots.ToInt32());
            Assert.AreEqual(new byte[] {4, 13, 128, 0, 31, 136, 128, 233, 99, 0, 0, 214, 31, 244, 73}, messages[0].Parameters.EngineId.ToBytes());
            Assert.AreEqual(3867, messages[0].Parameters.EngineTime.ToInt32());
            Assert.AreEqual(ErrorCode.NoError, messages[0].Pdu.ErrorStatus.ToErrorCode());
            Assert.AreEqual(1, messages[0].Pdu.Variables.Count);
            
            Variable v = messages[0].Pdu.Variables[0];
            Assert.AreEqual(".1.3.6.1.6.3.15.1.1.4.0", v.Id.ToString());
            ISnmpData data = v.Data;
            Assert.AreEqual(SnmpType.Counter32, data.TypeCode);
            Assert.AreEqual(3, ((Counter32)data).ToUInt32());

            Assert.AreEqual("80001F8880E9630000D61FF449", messages[0].Scope.ContextEngineId.ToHexString());
            Assert.AreEqual("", messages[0].Scope.ContextName.ToHexString());
        }
        
        [Test]
        public void TestTrapV3()
        {
            byte[] bytes = Resources.trapv3;
            UserRegistry registry = new UserRegistry();
            registry.Add(new OctetString("lextm"), DefaultPrivacyProvider.DefaultPair);
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.AreEqual(1, messages.Count);
            ISnmpMessage message = messages[0];
            Assert.AreEqual("80001F8880E9630000D61FF449", message.Parameters.EngineId.ToHexString());
            Assert.AreEqual(0, message.Parameters.EngineBoots.ToInt32());
            Assert.AreEqual(0, message.Parameters.EngineTime.ToInt32());
            Assert.AreEqual("lextm", message.Parameters.UserName.ToString());
            Assert.AreEqual("", message.Parameters.AuthenticationParameters.ToHexString());
            Assert.AreEqual("", message.Parameters.PrivacyParameters.ToHexString());
            Assert.AreEqual("", message.Scope.ContextEngineId.ToHexString()); // SNMP#NET returns string.Empty here.
            Assert.AreEqual("", message.Scope.ContextName.ToHexString());
            Assert.AreEqual(528732060, message.MessageId);
            Assert.AreEqual(1905687779, message.RequestId);
            Assert.AreEqual(".1.3.6", ((TrapV2Message)message).Enterprise.ToString());
        }

        [Test]
        public void TestTrapV3Auth()
        {
            byte[] bytes = Resources.trapv3auth;
            UserRegistry registry = new UserRegistry();
            registry.Add(new OctetString("lextm"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication"))));
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.AreEqual(1, messages.Count);
            ISnmpMessage message = messages[0];
            Assert.AreEqual("80001F8880E9630000D61FF449", message.Parameters.EngineId.ToHexString());
            Assert.AreEqual(0, message.Parameters.EngineBoots.ToInt32());
            Assert.AreEqual(0, message.Parameters.EngineTime.ToInt32());
            Assert.AreEqual("lextm", message.Parameters.UserName.ToString());
            Assert.AreEqual("84433969457707152C289A3E", message.Parameters.AuthenticationParameters.ToHexString());
            Assert.AreEqual("", message.Parameters.PrivacyParameters.ToHexString());
            Assert.AreEqual("", message.Scope.ContextEngineId.ToHexString()); // SNMP#NET returns string.Empty here.
            Assert.AreEqual("", message.Scope.ContextName.ToHexString());
            Assert.AreEqual(318463383, message.MessageId);
            Assert.AreEqual(1276263065, message.RequestId);
        }

        public void TestTrapV3AuthPriv()
        {
            // The message body generated by snmp#net is problematic.
            byte[] bytes = Resources.trapv3authpriv;
            UserRegistry registry = new UserRegistry();
            registry.Add(new OctetString("lextm"), new DESPrivacyProvider(new OctetString("privacyphrase"), new MD5AuthenticationProvider(new OctetString("authentication"))));
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.AreEqual(1, messages.Count);
            ISnmpMessage message = messages[0];
            Assert.AreEqual("80001F8880E9630000D61FF449", message.Parameters.EngineId.ToHexString());
            Assert.AreEqual(0, message.Parameters.EngineBoots.ToInt32());
            Assert.AreEqual(0, message.Parameters.EngineTime.ToInt32());
            Assert.AreEqual("lextm", message.Parameters.UserName.ToString());
            Assert.AreEqual("89D351891A55829243617F2C", message.Parameters.AuthenticationParameters.ToHexString());
            Assert.AreEqual("0000000069D39B2A", message.Parameters.PrivacyParameters.ToHexString());
            Assert.AreEqual("", message.Scope.ContextEngineId.ToHexString()); // SNMP#NET returns string.Empty here.
            Assert.AreEqual("", message.Scope.ContextName.ToHexString());
            Assert.AreEqual(0, message.Scope.Pdu.Variables.Count);
            Assert.AreEqual(1004947569, message.MessageId);
            Assert.AreEqual(234419641, message.RequestId);
        }
    }
}
#pragma warning restore 1591, 0618