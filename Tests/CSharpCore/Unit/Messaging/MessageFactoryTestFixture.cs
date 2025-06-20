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
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using Xunit;
using System.IO;
using System.Net.Security;

#pragma warning disable 1591, 0618
namespace Lextm.SharpSnmpLib.Unit.Messaging
{
    public class MessageFactoryTestFixture
    {
        [Fact]
        public void TrapV1InSNMPV2()
        {
            var data = "30 42 02 01 01 04 06 70 75 62 6C 69 63 A4 35 06 08 2B 06 01 04 01 81 AB 34 40 04 C0 A8 01 14 02 01 06 02 02 03 E8 43 04 00 00 03 63 30 16 30 14 06 0C 2B 06 01 04 01 81 AB 34 02 01 01 00 02 04 00 00 00 01";
            var bytes = ByteTool.Convert(data);
            Assert.Throws<SnmpException>(() => MessageFactory.ParseMessages(bytes, new UserRegistry()));
        }

        [Fact]
        public void TestReportFailure()
        {
            if (!DESPrivacyProvider.IsSupported)
            {
                return;
            }

            const string data = "30 70 02 01 03 30" +
                                "11 02 04 76 EB 6A 22 02 03 00 FF F0 04 01 01 02 01 03 04 33 30 31 04 09" +

                                "80 00 05 23 01 C1 4D BB 83 02 01 5B 02 03 1C 93 9D 04 0C 4D 44 35 5F 44" +
                                "45 53 5F 55 73 65 72 04 0C E5 C7 C5 2E 17 7E 87 62 AB 56 D6 C7 04 00 30" +
                                "23 04 00 04 00 A8 1D 02 01 00 02 01 00 02 01 00 30 12 30 10 06 0A 2B 06" +

                                "01 06 03 0F 01 01 02 00 41 02 05 EE";
            var bytes = ByteTool.Convert(data);
            const string userName = "MD5_DES_User";
            const string phrase = "AuthPassword";
            const string privatePhrase = "PrivPassword";
            IAuthenticationProvider auth = new MD5AuthenticationProvider(new OctetString(phrase));
            IPrivacyProvider priv = new DESPrivacyProvider(new OctetString(privatePhrase), auth);
            var users = new UserRegistry();
            users.Add(new User(new OctetString(userName), priv));
            var messages = MessageFactory.ParseMessages(bytes, users);
            Assert.Single(messages);
            var message = messages[0];
            Assert.Single(message.Variables());
        }

        [Fact]
        public void TestReportFailure2()
        {
            const string data = "30780201033010020462d4a37602020578040101020103042f302d040b800000090340f4ecf2b113020124020200a4040762696c6c696e67040c62bc133ef237922dfa8ca39a04003030040b800000090340f4ecf2b1130400a81f02049d2b5c8c0201000201003011300f060a2b060106030f01010200410105";
            var bytes = ByteTool.Convert(data);
            const string userName = "billing";
            IAuthenticationProvider auth = new MD5AuthenticationProvider(new OctetString("testing345"));
            IPrivacyProvider priv = new DefaultPrivacyProvider(auth);
            var users = new UserRegistry();
            users.Add(new User(new OctetString(userName), priv));
            var messages = MessageFactory.ParseMessages(bytes, users);
            Assert.Single(messages);
            var message = messages[0];
            Assert.Single(message.Variables());
            Assert.Equal("not in time window", message.Variables()[0].Id.GetErrorMessage());
        }

        [Fact]
        public void TestInform()
        {
            byte[] data = new byte[] { 0x30, 0x5d, 0x02, 0x01, 0x01, 0x04, 0x06, 0x70, 0x75, 0x62, 0x6c, 0x69, 0x63, 0xa6, 0x50, 0x02, 0x01, 0x01, 0x02, 0x01,
                0x00, 0x02, 0x01, 0x00, 0x30, 0x45, 0x30, 0x0e, 0x06, 0x08, 0x2b, 0x06, 0x01, 0x02, 0x01, 0x01, 0x03, 0x00, 0x43, 0x02,
                0x3f, 0xe0, 0x30, 0x18, 0x06, 0x0a, 0x2b, 0x06, 0x01, 0x06, 0x03, 0x01, 0x01, 0x04, 0x01, 0x00, 0x06, 0x0a, 0x2b, 0x06,
                0x01, 0x04, 0x01, 0x90, 0x72, 0x87, 0x68, 0x02, 0x30, 0x19, 0x06, 0x0b, 0x2b, 0x06, 0x01, 0x04, 0x01, 0x90, 0x72, 0x87,
                0x69, 0x15, 0x00, 0x04, 0x0a, 0x49, 0x6e, 0x66, 0x6f, 0x72, 0x6d, 0x54, 0x65, 0x73, 0x74 };

            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(data, new UserRegistry());
            Assert.Equal(SnmpType.InformRequestPdu, messages[0].TypeCode());
        }

        [Fact]
        public void TestString()
        {
            string bytes = "30 29 02 01 00 04 06 70 75 62 6c 69 63 a0 1c 02 04 4f 89 fb dd" + Environment.NewLine +
                "02 01 00 02 01 00 30 0e 30 0c 06 08 2b 06 01 02 01 01 05 00 05 00";
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, new UserRegistry());
            Assert.Single(messages);
            GetRequestMessage m = (GetRequestMessage)messages[0];
            Variable v = m.Variables()[0];
            string i = v.Id.ToString();
            Assert.Equal("1.3.6.1.2.1.1.5.0", i);
        }

        [Fact]
        public void TestBrokenString()
        {
            bool hasException = false;
            try
            {
                const string bytes = "30 39 02 01 01 04 06 70 75 62 6C 69 63 A7 2C 02 01 01 02 01 00 02 01 00 30 21 30 0D 06 08 2B 06 01 02 01 01";
                IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, new UserRegistry());
                Assert.Single(messages);
            }
            catch (Exception)
            {
                hasException = true;
            }

            Assert.True(hasException);
        }

        [Fact]
        public void TestDiscovery()
        {
            const string bytes = "30 3A 02 01  03 30 0F 02  02 6A 09 02  03 00 FF E3" +
                                 "04 01 04 02  01 03 04 10  30 0E 04 00  02 01 00 02" +
                                 "01 00 04 00  04 00 04 00  30 12 04 00  04 00 A0 0C" +
                                 "02 02 2C 6B  02 01 00 02  01 00 30 00";
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, new UserRegistry());
            Assert.Single(messages);
            Assert.Equal(OctetString.Empty, messages[0].Parameters.UserName);
        }

        [Fact]
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
            Assert.Single(messages);
            GetRequestMessage get = (GetRequestMessage)messages[0];
            Assert.Equal(27144, get.MessageId());
            //Assert.Equal(SecurityLevel.None | SecurityLevel.Reportable, get.Level);
            Assert.Equal("lextm", get.Community().ToString());
        }

        [Fact]
        public void TestGetRequestV3AuthPriv_MD5_DES()
        {
            if (!DESPrivacyProvider.IsSupported)
            {
                return;
            }

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
            registry.Add(new OctetString("lexmark"), new DefaultPrivacyProvider(auth));
            var messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.Single(messages);
            Assert.Equal(SnmpType.Unknown, messages[0].TypeCode());

            //registry.Add(new OctetString("lexmark"),
            // new DESPrivacyProvider(
            //     new OctetString("veryverylonglongago"),
            //     auth));

            //Assert.Throws<SnmpException>(() => MessageFactory.ParseMessages(bytes, registry));

            registry.Add(new OctetString("lexmark"), new DESPrivacyProvider(new OctetString("passtest"), auth));
            messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.Single(messages);
            GetRequestMessage get = (GetRequestMessage)messages[0];
            Assert.Equal(27801, get.MessageId());
            //Assert.Equal(SecurityLevel.None | SecurityLevel.Reportable, get.Level);
            Assert.Equal("lexmark", get.Community().ToString());
            //OctetString digest = new MD5AuthenticationProvider(new OctetString("testpass")).ComputeHash(get);

            //Assert.Equal(digest, get.Parameters.AuthenticationParameters);
        }

        [Fact]
        public void TestGetRequestV3AuthPriv_SHA1_AES()
        {
            if (!AESPrivacyProviderBase.IsSupported)
            {
                return;
            }

            const string bytes = "30 81 8A 02  01 03 30 11  02 04 6C F5  81 EC 02 03" +
                                 "00 FF E3 04  01 07 02 01  03 04 3E 30  3C 04 0E 80" +
                                 "00 4F B8 05  63 6C 6F 75  64 4D AB 22  CC 02 01 00" +
                                 "02 02 00 E6  04 0B 75 73  72 2D 73 68  61 2D 61 65" +
                                 "73 04 0C EA  A6 81 2E 66  30 BD 2B 15  7E DE 3D 04" +
                                 "08 F2 D7 27  56 34 71 83  21 04 32 0B  DB 8F 71 60" +
                                 "14 BE F3 7E  3A 56 C0 6F  CD 80 4D 73  3B 09 07 6D" +
                                 "4F 29 7E 6E  2C A2 8B 37  C4 E4 E1 8D  20 63 B7 05" +
                                 "CF F6 2F AA  FC 78 59 5C  21 1F 09 0F  96";
            var auth = new SHA1AuthenticationProvider(new OctetString("authkey1"));
            var privacy = new AESPrivacyProvider(new OctetString("privkey1"), auth);
            var registry = new UserRegistry();
            registry.Add(new OctetString("usr-sha-aes"), privacy);
            var messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.Single(messages);
            GetRequestMessage get = (GetRequestMessage)messages[0];
            Assert.Equal(1828028908, get.MessageId());
            Assert.Equal("usr-sha-aes", get.Community().ToString());
        }

        [Fact]
        public void TestGetRequestV3AuthPrivAES2()
        {
            if (!AESPrivacyProviderBase.IsSupported)
            {
                return;
            }

            SaltGenerator.LockSalt = true;
            var auth = new SHA1AuthenticationProvider(new OctetString("authkey1"));
            var privacy = new AESPrivacyProvider(new OctetString("privkey1"), auth);
            var getRequestMessage = new GetRequestMessage(
                VersionCode.V3,
                1828028908,
                1952543736,
                new OctetString("usr-sha-aes"),
                new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) },
                privacy,
                new ReportMessage(
                    VersionCode.V3,
                    Header.Empty,
                    new SecurityParameters(
                        new OctetString("80004fb805636c6f75644dab22cc"),
                        new Integer32(0),
                        new Integer32(230),
                        OctetString.Empty,
                        OctetString.Empty,
                        OctetString.Empty),
                    new Scope(
                        OctetString.Empty,
                        OctetString.Empty,
                        new ReportPdu(
                            1952543737,
                            ErrorCode.NoError,
                            0,
                            new List<Variable> {
                                new Variable(
                                    new ObjectIdentifier("1.3.6.1.6.3.15.1.1.4.0"),
                                    new Counter32(3))
                            })),
                    privacy,
                    Array.Empty<byte>())
            );
            var output = ByteTool.Convert(getRequestMessage.ToBytes());
            Console.WriteLine(output);
            Assert.Equal(
                "30 81 A6 02 01 03 30 11 02 04 6C F5 81 EC 02 03 " +
                "00 FF E3 04 01 07 02 01 03 04 4C 30 4A 04 1C 38 " +
                "30 30 30 34 66 62 38 30 35 36 33 36 63 36 66 37 " +
                "35 36 34 34 64 61 62 32 32 63 63 02 01 00 02 02 " +
                "00 E6 04 0B 75 73 72 2D 73 68 61 2D 61 65 73 04 " +
                "0C 77 B7 F3 1C 3A DB EE E1 B6 1E 5D 89 04 08 00 " +
                "00 00 00 00 00 08 00 04 40 C1 0E 53 6C 51 CF 83 " +
                "E5 10 21 12 CF 51 F8 5F 26 DD 3F 5D 73 FB 04 7D " +
                "08 45 63 3D EA A9 43 5E 0E B3 5D 29 91 92 6A 4E " +
                "AF 9D D0 75 DB 6B AC 3D A3 1B 10 5C 9F 50 13 A6 " +
                "01 5D 01 F1 F4 EF 70 53 CC", output);
        }

        [Fact]
        public void TestGetRequestV3AuthPriv_MD5_3DES()
        {
            const string bytes = "30 81 9A 02 01 03 30 11 02 04 16 39 99 3C 02 03 00 FF E3 04 01 07 02 01 03 04 40 30 3E 04 0E 80 00 4F B8 05 63 6C 6F 75 64 4D AB 22 CC 02 01 00 02 03 75 6B 5D 04 0C 75 73 72 2D 6D 64 35 2D 33 64 65 73 04 0C 88 3A 31 F2 4D FA 37 1D 40 43 51 EC 04 08 00 00 00 00 3A 90 A4 49 04 40 77 0E B3 60 5E 97 7B 53 3E 21 FD B1 74 6B 73 CF 8A AF C1 14 0B C5 AA EF 2C A3 4F 5E FF 07 BD 37 AF 14 64 91 1B AB 23 E5 C8 52 8E 64 0F FF 67 A3 CB 6D 68 0B 96 67 C5 79 93 AF 02 B2 02 CD B5 CF";
            var auth = new MD5AuthenticationProvider(new OctetString("authkey1"));
            var privacy = new TripleDESPrivacyProvider(new OctetString("privkey1"), auth);

            var registry = new UserRegistry();
            registry.Add(new OctetString("usr-md5-3des"), privacy);
            var messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.Single(messages);
        }

        [Fact]
        public void TestGetRequestV3Auth()
        {
            const string bytes = "30 73" +
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
            UserRegistry registry = new UserRegistry();
            registry.Add(new OctetString("lexli"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("testpass"))));
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.Single(messages);
            GetRequestMessage get = (GetRequestMessage)messages[0];
            Assert.Equal(13633, get.MessageId());
            //Assert.Equal(SecurityLevel.None | SecurityLevel.Reportable, get.Level);
            Assert.Equal("lexli", get.Community().ToString());
            //OctetString digest = new MD5AuthenticationProvider(new OctetString("testpass")).ComputeHash(get);

            //Assert.Equal(digest, get.Parameters.AuthenticationParameters);
        }

        [Fact]
        public void TestGetRequestV3AuthPriv_LocalizedKeys_Request()
        {
            if (!DESPrivacyProvider.IsSupported)
            {
                return;
            }

            var auth_passphrase = new OctetString("authkey1");
            var auth = new SHA1AuthenticationProvider(auth_passphrase);

            const string bytes = "30 81 82 02  01 03 30 11  02 04 41 DE  D6 81 02 03" +
                                 "00 FF E3 04  01 07 02 01  03 04 39 30  37 04 0A 80" +
                                 "00 00 00 04  73 6E 6D 70  31 02 01 00  02 01 00 04" +
                                 "0B 75 73 72  2D 73 68 61  2D 61 65 73  04 0C 45 03" +
                                 "6B 5C 42 5D  B4 0F 59 73  25 3E 04 08  68 63 B6 B7" +
                                 "E5 51 65 E7  04 2F 0A 61  C3 64 0C 4D  A9 19 D9 21" +
                                 "54 2D 84 3D  1A 25 B5 3D  C8 23 DD 90  51 E3 75 38" +
                                 "E2 C6 01 1D  5D B6 3C BE  23 40 49 FE  DC 0D CF C2" +
                                 "6F 28 2B 92  9E";

            var privacy = new AESPrivacyProvider(new OctetString("privkey1"), auth);
            var registry = new UserRegistry();
            registry.Add(new OctetString("usr-sha-aes"), privacy);
            var messages = MessageFactory.ParseMessages(bytes, registry);
            var message = messages[0];
            Assert.Single(messages);
            Assert.Equal(SnmpType.GetRequestPdu, message.TypeCode());
        }

        [Fact]
        public void TestGetRequestV3AuthPriv_LocalizedKeys_Response()
        {
            if (!DESPrivacyProvider.IsSupported)
            {
                return;
            }

            var auth_passphrase = new OctetString("authkey1");
            var auth = new SHA1AuthenticationProvider(auth_passphrase);

            const string bytes = "30 81 82 02  01 03 30 11  02 04 41 DE  D6 81 02 03" +
                                 "00 FF E3 04  01 03 02 01  03 04 39 30  37 04 0A 80" +
                                 "00 00 00 04  73 6E 6D 70  31 02 01 00  02 01 00 04" +
                                 "0B 75 73 72  2D 73 68 61  2D 61 65 73  04 0C 17 00" +
                                 "91 B1 11 A0  B7 38 BF 79  0B B6 04 08  45 FF 6B C8" +
                                 "C7 A1 9E 93  04 2F DC 9E  35 31 C3 E6  5A 68 A0 F5" +
                                 "06 B0 9B D6  97 16 46 23  C2 D9 F6 EE  9C 8F F4 00" +
                                 "31 AC CE 32  43 6A FC C5  56 6D 02 7A  49 A5 33 46" +
                                 "AB 7A 56 7F  B6";

            var privacy = new AESPrivacyProvider(new OctetString("privkey1"), auth);
            var registry = new UserRegistry();
            registry.Add(new OctetString("usr-sha-aes"), privacy);
            var messages = MessageFactory.ParseMessages(bytes, registry);
            var message = messages[0];
            Assert.Single(messages);
            Assert.Equal(SnmpType.ResponsePdu, message.TypeCode());
        }

#if !NETSTANDARD
        [Fact]
        public void TestResponseV1()
        {
            ISnmpMessage message = MessageFactory.ParseMessages(File.ReadAllBytes(Path.Combine("Resources", "getresponse.dat")), new UserRegistry())[0];
            Assert.Equal(SnmpType.ResponsePdu, message.TypeCode());
            ISnmpPdu pdu = message.Pdu();
            Assert.Equal(SnmpType.ResponsePdu, pdu.TypeCode);
            ResponsePdu response = (ResponsePdu)pdu;
            Assert.Equal(Integer32.Zero, response.ErrorStatus);
            Assert.Equal(0, response.ErrorIndex.ToInt32());
            Assert.Single(response.Variables);
            Variable v = response.Variables[0];
            Assert.Equal(new uint[] { 1, 3, 6, 1, 2, 1, 1, 6, 0 }, v.Id.ToNumerical());
            Assert.Equal("Shanghai", v.Data.ToString());
        }
#endif
        [Fact]
        public void TestGetResponseV3Error()
        {
            // TODO: make use of this test case.
            const string bytes = "30 77 02 01 03 30 11 02 04 1A AE F6 91 02 03 00" +
                                 "FF E3 04 01 04 02 01 03 04 29 30 27 04 0F 04 0D" +
                                 "80 00 1F 88 80 E9 63 00 00 D6 1F F4 49 02 01 00" +
                                 "02 04 01 5C DE E9 04 07 6E 65 69 74 68 65 72 04" +
                                 "00 04 00 30 34 04 0F 04 0D 80 00 1F 88 80 E9 63" +
                                 "00 00 D6 1F F4 49 04 00 A2 1F 02 04 1A AE F6 9E" +
                                 "02 01 00 02 01 00 30 11 30 0F 06 08 2B 06 01 02" +
                                 "01 01 02 00 06 03 2B 06 01";
            UserRegistry registry = new UserRegistry();
            registry.Add(new OctetString("neither"), DefaultPrivacyProvider.DefaultPair);
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.Single(messages);
            ISnmpMessage message = messages[0];
            Assert.Equal("040D80001F8880E9630000D61FF449", message.Parameters.EngineId.ToHexString());
            Assert.Equal(0, message.Parameters.EngineBoots.ToInt32());
            Assert.Equal(22863593, message.Parameters.EngineTime.ToInt32());
            Assert.Equal("neither", message.Parameters.UserName.ToString());
            Assert.Equal("", message.Parameters.AuthenticationParameters.ToHexString());
            Assert.Equal("", message.Parameters.PrivacyParameters.ToHexString());
            Assert.Equal("040D80001F8880E9630000D61FF449", message.Scope.ContextEngineId.ToHexString());
            Assert.Equal("", message.Scope.ContextName.ToHexString());
        }

        [Fact]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => MessageFactory.ParseMessages((byte[])null, null));
            Assert.Throws<ArgumentNullException>(() => MessageFactory.ParseMessages(new byte[0], null));

            Assert.Throws<ArgumentNullException>(() => MessageFactory.ParseMessages((string)null, null));
            Assert.Throws<ArgumentNullException>(() => MessageFactory.ParseMessages(string.Empty, null));

            Assert.Throws<ArgumentNullException>(() => MessageFactory.ParseMessages(null, 0, 0, null));
            Assert.Throws<ArgumentNullException>(() => MessageFactory.ParseMessages(new byte[0], 0, 0, null));
        }

        [Fact]
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
            var messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.Single(messages);
            Assert.Equal(SnmpType.Unknown, messages[0].TypeCode());

            registry.Add(new OctetString("lextm"), DefaultPrivacyProvider.DefaultPair);
            messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.Single(messages);
            ISnmpMessage message = messages[0];
            Assert.Equal("80001F8880E9630000D61FF449", message.Parameters.EngineId.ToHexString());
            Assert.Equal(5, message.Parameters.EngineBoots.ToInt32());
            Assert.Equal(3868, message.Parameters.EngineTime.ToInt32());
            Assert.Equal("lextm", message.Parameters.UserName.ToString());
            Assert.Equal("", message.Parameters.AuthenticationParameters.ToHexString());
            Assert.Equal("", message.Parameters.PrivacyParameters.ToHexString());
            Assert.Equal("80001F8880E9630000D61FF449", message.Scope.ContextEngineId.ToHexString());
            Assert.Equal("", message.Scope.ContextName.ToHexString());
        }

        [Fact]
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
            Assert.Single(messages);
            Assert.Equal(5, messages[0].Parameters.EngineBoots.ToInt32());
            Assert.Equal("80001F8880E9630000D61FF449", messages[0].Parameters.EngineId.ToHexString());
            Assert.Equal(3867, messages[0].Parameters.EngineTime.ToInt32());
            Assert.Equal(ErrorCode.NoError, messages[0].Pdu().ErrorStatus.ToErrorCode());
            Assert.Single(messages[0].Pdu().Variables);

            Variable v = messages[0].Pdu().Variables[0];
            Assert.Equal("1.3.6.1.6.3.15.1.1.4.0", v.Id.ToString());
            ISnmpData data = v.Data;
            Assert.Equal(SnmpType.Counter32, data.TypeCode);
            Assert.Equal(3U, ((Counter32)data).ToUInt32());

            Assert.Equal("80001F8880E9630000D61FF449", messages[0].Scope.ContextEngineId.ToHexString());
            Assert.Equal("", messages[0].Scope.ContextName.ToHexString());
        }

        [Fact]
        public void TestTrapV3()
        {
            byte[] bytes = File.ReadAllBytes(Path.Combine("Resources", "trapv3"));
            UserRegistry registry = new UserRegistry();
            registry.Add(new OctetString("lextm"), DefaultPrivacyProvider.DefaultPair);
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.Single(messages);
            ISnmpMessage message = messages[0];
            Assert.Equal("80001F8880E9630000D61FF449", message.Parameters.EngineId.ToHexString());
            Assert.Equal(0, message.Parameters.EngineBoots.ToInt32());
            Assert.Equal(0, message.Parameters.EngineTime.ToInt32());
            Assert.Equal("lextm", message.Parameters.UserName.ToString());
            Assert.Equal("", message.Parameters.AuthenticationParameters.ToHexString());
            Assert.Equal("", message.Parameters.PrivacyParameters.ToHexString());
            Assert.Equal("", message.Scope.ContextEngineId.ToHexString()); // SNMP#NET returns string.Empty here.
            Assert.Equal("", message.Scope.ContextName.ToHexString());
            Assert.Equal(528732060, message.MessageId());
            Assert.Equal(1905687779, message.RequestId());
            Assert.Equal("1.3.6", ((TrapV2Message)message).Enterprise.ToString());
        }

        [Fact]
        public void TestTrapV3Auth()
        {
            byte[] bytes = File.ReadAllBytes(Path.Combine("Resources", "trapv3auth"));
            UserRegistry registry = new UserRegistry();
            registry.Add(new OctetString("lextm"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication"))));
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.Single(messages);
            ISnmpMessage message = messages[0];
            Assert.Equal("80001F8880E9630000D61FF449", message.Parameters.EngineId.ToHexString());
            Assert.Equal(0, message.Parameters.EngineBoots.ToInt32());
            Assert.Equal(0, message.Parameters.EngineTime.ToInt32());
            Assert.Equal("lextm", message.Parameters.UserName.ToString());
            Assert.Equal("84433969457707152C289A3E", message.Parameters.AuthenticationParameters.ToHexString());
            Assert.Equal("", message.Parameters.PrivacyParameters.ToHexString());
            Assert.Equal("", message.Scope.ContextEngineId.ToHexString()); // SNMP#NET returns string.Empty here.
            Assert.Equal("", message.Scope.ContextName.ToHexString());
            Assert.Equal(318463383, message.MessageId());
            Assert.Equal(1276263065, message.RequestId());
        }

        [Fact]
        public void TestTrapV3AuthBytes()
        {
            byte[] bytes = File.ReadAllBytes(Path.Combine("Resources", "v3authNoPriv_BER_Issue"));
            UserRegistry registry = new UserRegistry();
            SHA1AuthenticationProvider authen = new SHA1AuthenticationProvider(new OctetString("testpass"));
            registry.Add(new OctetString("test"), new DefaultPrivacyProvider(authen));
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.Single(messages);
            ISnmpMessage message = messages[0];
            Assert.Equal("80001299030005B706CF69", message.Parameters.EngineId.ToHexString());
            Assert.Equal(41, message.Parameters.EngineBoots.ToInt32());
            Assert.Equal(877, message.Parameters.EngineTime.ToInt32());
            Assert.Equal("test", message.Parameters.UserName.ToString());
            Assert.Equal("C107F9DAA3FC552960E38936", message.Parameters.AuthenticationParameters.ToHexString());
            Assert.Equal("", message.Parameters.PrivacyParameters.ToHexString());
            Assert.Equal("80001299030005B706CF69", message.Scope.ContextEngineId.ToHexString()); // SNMP#NET returns string.Empty here.
            Assert.Equal("", message.Scope.ContextName.ToHexString());
            Assert.Equal(681323585, message.MessageId());
            Assert.Equal(681323584, message.RequestId());

            Assert.Equal(bytes, message.ToBytes());
        }

        [Fact]
        public void TestTrapV3AuthPriv()
        {
            if (!DESPrivacyProvider.IsSupported)
            {
                return;
            }

            // The message body generated by snmp#net is problematic.
            byte[] bytes = File.ReadAllBytes(Path.Combine("Resources", "trapv3authpriv"));
            UserRegistry registry = new UserRegistry();
            registry.Add(new OctetString("lextm"), new DESPrivacyProvider(new OctetString("privacyphrase"), new MD5AuthenticationProvider(new OctetString("authentication"))));
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.Single(messages);
            ISnmpMessage message = messages[0];
            Assert.Equal("80001F8880E9630000D61FF449", message.Parameters.EngineId.ToHexString());
            Assert.Equal(0, message.Parameters.EngineBoots.ToInt32());
            Assert.Equal(0, message.Parameters.EngineTime.ToInt32());
            Assert.Equal("lextm", message.Parameters.UserName.ToString());
            Assert.Equal("89D351891A55829243617F2C", message.Parameters.AuthenticationParameters.ToHexString());
            Assert.Equal("0000000069D39B2A", message.Parameters.PrivacyParameters.ToHexString());
            Assert.Equal("", message.Scope.ContextEngineId.ToHexString()); // SNMP#NET returns string.Empty here.
            Assert.Equal("", message.Scope.ContextName.ToHexString());
            Assert.Empty(message.Scope.Pdu.Variables);
            Assert.Equal(1004947569, message.MessageId());
            Assert.Equal(234419641, message.RequestId());
        }

        [Fact]
        public void TestBadResponseFromPrinter()
        {
            // #7241
            var data = "30 2B 02 01 00 04 06 70 75 62 6C 69 63 A2 1E 02 04 32 FA 7A 02 02 01 00 02 01 00 30 10 30 0E 06 0A 2B 06 01 02 01 02 02 01 16 01 06 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00";
            var bytes = ByteTool.Convert(data);
            var exception = Assert.Throws<SnmpException>(() => MessageFactory.ParseMessages(bytes, new UserRegistry()));
            Assert.Contains($"Byte length cannot be 0.", exception.InnerException.Message);
        }

        [Fact]
        public void TestIsInvalidWithWrongPassword()
        {
            byte[] bytes = File.ReadAllBytes(Path.Combine("Resources", "trapv3auth"));

            // Create registry with a user that has the wrong authentication password
            UserRegistry registry = new UserRegistry();
            registry.Add(new OctetString("lextm"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("wrongpassword"))));

            // Parse the message with the wrong password
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.Single(messages);
            ISnmpMessage message = messages[0];

            // Verify the message was parsed but the IsInvalid flag was set due to hash verification failure
            Assert.Equal("lextm", message.Parameters.UserName.ToString());
            Assert.True(message.Parameters.IsInvalid, "IsInvalid flag should be true when authentication fails");

            // Verify correct message properties despite invalid authentication
            Assert.Equal("80001F8880E9630000D61FF449", message.Parameters.EngineId.ToHexString());
            Assert.Equal(0, message.Parameters.EngineBoots.ToInt32());
            Assert.Equal(0, message.Parameters.EngineTime.ToInt32());
            Assert.Equal("84433969457707152C289A3E", message.Parameters.AuthenticationParameters.ToHexString());
        }

        [Fact]
        public void TestIsInvalidWithCorrectPassword()
        {
            byte[] bytes = File.ReadAllBytes(Path.Combine("Resources", "trapv3auth"));

            // Create registry with a user that has the correct authentication password
            UserRegistry registry = new UserRegistry();
            registry.Add(new OctetString("lextm"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("authentication"))));

            // Parse the message with the correct password
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.Single(messages);
            ISnmpMessage message = messages[0];

            // The message should be valid when the correct password is used
            Assert.Equal("lextm", message.Parameters.UserName.ToString());
            Assert.False(message.Parameters.IsInvalid, "IsInvalid flag should be false when authentication succeeds");

            // Verify correct message properties with valid authentication
            Assert.Equal("80001F8880E9630000D61FF449", message.Parameters.EngineId.ToHexString());
            Assert.Equal("84433969457707152C289A3E", message.Parameters.AuthenticationParameters.ToHexString());
        }

        [Fact]
        public void TestIsInvalidWithSHA1Authentication()
        {
            byte[] bytes = File.ReadAllBytes(Path.Combine("Resources", "v3authNoPriv_BER_Issue"));

            // Test with wrong password
            UserRegistry registry = new UserRegistry();
            SHA1AuthenticationProvider wrongAuth = new SHA1AuthenticationProvider(new OctetString("wrongpassword"));
            registry.Add(new OctetString("test"), new DefaultPrivacyProvider(wrongAuth));

            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.Single(messages);
            ISnmpMessage message = messages[0];

            // Verify the IsInvalid flag is set when SHA1 authentication fails
            Assert.Equal("test", message.Parameters.UserName.ToString());
            Assert.True(message.Parameters.IsInvalid, "IsInvalid flag should be true when SHA1 authentication fails");

            // Now test with correct password
            registry = new UserRegistry();
            SHA1AuthenticationProvider correctAuth = new SHA1AuthenticationProvider(new OctetString("testpass"));
            registry.Add(new OctetString("test"), new DefaultPrivacyProvider(correctAuth));

            messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.Single(messages);
            message = messages[0];

            // The IsInvalid flag should be false when using correct password
            Assert.False(message.Parameters.IsInvalid, "IsInvalid flag should be false when SHA1 authentication succeeds");
        }

        [Fact]
        public void TestIsInvalidCanBeCheckedByApplication()
        {
            byte[] bytes = File.ReadAllBytes(Path.Combine("Resources", "trapv3auth"));

            // Create registry with a user that has the wrong authentication password
            UserRegistry registry = new UserRegistry();
            registry.Add(new OctetString("lextm"), new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("wrongpassword"))));

            // Parse the message with the wrong password
            IList<ISnmpMessage> messages = MessageFactory.ParseMessages(bytes, registry);
            Assert.Single(messages);
            ISnmpMessage message = messages[0];

            // This demonstrates how an application can check the IsInvalid flag to detect auth failures
            if (message.Parameters.IsInvalid)
            {
                // The application can log or handle the authentication failure here,
                // but still has access to the message content for inspection
                Assert.Equal("lextm", message.Parameters.UserName.ToString());
                Assert.Equal("80001F8880E9630000D61FF449", message.Parameters.EngineId.ToHexString());

                // In real-world applications, you would typically reject the message or log the authentication failure
                // Rather than accepting the potentially tampered contents
            }
            else
            {
                // Should not reach here
                Assert.True(message.Parameters.IsInvalid, "Authentication should have failed with wrong password");
            }
        }
    }
}
#pragma warning restore 1591, 0618
