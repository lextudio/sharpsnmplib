using System.Buffers;
using System.Text;
using DotNetSnmp.Asn1;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V1;
using DotNetSnmp.Protocol.V3;
using DotNetSnmp.Protocol.V3.Security;
using DotNetSnmp.Protocol.V3.Security.Authentication;
using DotNetSnmp.Protocol.V3.Security.Privacy;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using Xunit;

namespace DotNetSnmp.Test
{
    public class MessageParsingTest
    {
        [Fact]
        public void FullMessageTest_SHA1()
        {
            const string hexBytes = "30 77 02 01  03 30 0F 02  02 47 21 02  03 00 FF E3" +
                     "04 01 05 02  01 03 04 32  30 30 04 0D  80 00 1F 88" +
                     "80 E9 63 00  00 D6 1F F4  49 02 01 15  02 02 01 5B" +
                     "04 08 6C 65  78 74 75 64  69 6F 04 0C  7B 62 65 AE" +
                     "D3 8F E3 7D  58 45 5C 6C  04 00 30 2D  04 0D 80 00" +
                     "1F 88 80 E9  63 00 00 D6  1F F4 49 04  00 A0 1A 02" +
                     "02 56 FF 02  01 00 02 01  00 30 0E 30  0C 06 08 2B" +
                     "06 01 02 01  01 03 00 05  00";

            // Convert hex string to byte array
            var messageBytes = Utils.Dump.BytesFromHexString(hexBytes);

            // Parse the message
            var reader = new System.Formats.Asn1.AsnReader(messageBytes, System.Formats.Asn1.AsnEncodingRules.BER);
            var message = Protocol.V3.SnmpV3Message.ReadFrom(reader);

            // Verify message basic properties
            Assert.NotNull(message);
            Assert.Equal(VersionCode.V3, message.ProtocolVersion);

            // Verify Header Data
            Assert.Equal(0x4721, message.Header.MsgId);
            Assert.Equal(0xFFE3, message.Header.MsgMaxSize);
            Assert.Equal(MsgFlags.Auth | MsgFlags.Reportable, message.Header.MsgFlags);
            Assert.Equal(SecurityModel.Usm, message.Header.MsgSecurityModel);

            // Verify Security Parameters
            var secParams = message.SecurityParameters;
            Assert.NotNull(secParams);

            var expectedEngineId = Utils.Dump.BytesFromHexString("80 00 1F 88 80 E9 63 00 00 D6 1F F4 49");
            Assert.True(expectedEngineId.SequenceEqual(secParams.EngineId.ToArray()));

            Assert.Equal(0x15, secParams.EngineBoots);
            Assert.Equal(0x015B, secParams.EngineTime);

            var expectedSecurityName = Encoding.ASCII.GetBytes("lextudio");
            Assert.True(expectedSecurityName.SequenceEqual(secParams.SecurityName.Octets));

            var expectedAuthParams = Utils.Dump.BytesFromHexString("7B 62 65 AE D3 8F E3 7D 58 45 5C 6C");
            Assert.True(expectedAuthParams.SequenceEqual(secParams.AuthParams.ToArray()));

            Assert.Equal(0, secParams.PrivParams.Length);

            // Verify Scoped PDU
            Assert.NotNull(message.Scope);
            Assert.NotNull(message.Scope.Pdu);

            // Context information
            var expectedContextEngineId = Utils.Dump.BytesFromHexString("80 00 1F 88 80 E9 63 00 00 D6 1F F4 49");
            var scope = (Scope)message.Scope;
            Assert.True(expectedContextEngineId.SequenceEqual(scope.ContextEngineId.ToArray()));
            Assert.Empty(scope.ContextName.ToArray());

            // PDU details
            var pdu = message.Scope.Pdu;
            //Assert.Equal(PduType.GetRequest, pdu.PduType);
            Assert.Equal(0x56FF, pdu.RequestId);
            Assert.Equal(ErrorCode.NoError, pdu.ErrorStatus);
            Assert.Equal(0, pdu.ErrorIndex);

            // Verify Variable Bindings
            Assert.NotNull(pdu.VariableBindings);
            Assert.Single(pdu.VariableBindings);

            // Verify the OID and value
            var varBind = pdu.VariableBindings?.First();
            Assert.Equal("1.3.6.1.2.1.1.3.0", varBind!.Value.Id.ToString());
            Assert.IsType<Null>(varBind.Value.Data);

            // Verify we can re-encode the message to the same bytes
            var reEncodedBytes = message.Encode();

            // Compare original and re-encoded bytes
            Assert.Equal(messageBytes.Length, reEncodedBytes.Length);
            Assert.True(messageBytes.SequenceEqual(reEncodedBytes));

            var sha1 = new DotNetSnmp.Protocol.V3.Security.Authentication.SHA1AuthenticationProvider(Encoding.UTF8.GetBytes("password"));
            var authenticated = sha1.AuthenticateIncomingMsg(message);
            Assert.True(authenticated);
        }

        [Fact]
        public void FullMessageTest_SHA1_AES()
        {
            const string hexBytes = "30 81 8A 02  01 03 30 11  02 04 6C F5  81 EC 02 03" +
                                 "00 FF E3 04  01 07 02 01  03 04 3E 30  3C 04 0E 80" +
                                 "00 4F B8 05  63 6C 6F 75  64 4D AB 22  CC 02 01 00" +
                                 "02 02 00 E6  04 0B 75 73  72 2D 73 68  61 2D 61 65" +
                                 "73 04 0C EA  A6 81 2E 66  30 BD 2B 15  7E DE 3D 04" +
                                 "08 F2 D7 27  56 34 71 83  21 04 32 0B  DB 8F 71 60" +
                                 "14 BE F3 7E  3A 56 C0 6F  CD 80 4D 73  3B 09 07 6D" +
                                 "4F 29 7E 6E  2C A2 8B 37  C4 E4 E1 8D  20 63 B7 05" +
                                 "CF F6 2F AA  FC 78 59 5C  21 1F 09 0F  96";

            // Convert hex string to byte array
            var messageBytes = Utils.Dump.BytesFromHexString(hexBytes);

            // Parse the message
            var reader = new System.Formats.Asn1.AsnReader(messageBytes, System.Formats.Asn1.AsnEncodingRules.BER);
            var message = Protocol.V3.SnmpV3Message.ReadFrom(reader);

            // Verify message basic properties
            Assert.NotNull(message);
            Assert.Equal(VersionCode.V3, message.ProtocolVersion);

            // Verify Header Data
            Assert.Equal(0x6CF581EC, message.Header.MsgId);
            Assert.Equal(0xFFE3, message.Header.MsgMaxSize);
            Assert.Equal(MsgFlags.Auth | MsgFlags.Priv | MsgFlags.Reportable, message.Header.MsgFlags);
            Assert.Equal(SecurityModel.Usm, message.Header.MsgSecurityModel);

            // Verify Security Parameters
            var secParams = message.SecurityParameters;
            Assert.NotNull(secParams);

            var expectedEngineId = Utils.Dump.BytesFromHexString("80 00 4F B8 05 63 6C 6F 75 64 4D AB 22 CC");
            Assert.True(expectedEngineId.SequenceEqual(secParams.EngineId.ToArray()), $"Unexpected engine ID: {Utils.Dump.BytesToHexString(secParams.EngineId.ToArray())}");

            Assert.Equal(0x0, secParams.EngineBoots);
            Assert.Equal(0x00E6, secParams.EngineTime);

            var expectedSecurityName = Encoding.ASCII.GetBytes("usr-sha-aes");
            Assert.True(expectedSecurityName.SequenceEqual(secParams.SecurityName.Octets), $"Unexpected security name: {Utils.Dump.BytesToHexString(secParams.SecurityName.Octets)}");

            var expectedAuthParams = Utils.Dump.BytesFromHexString("EA A6 81 2E 66 30 BD 2B 15 7E DE 3D");
            Assert.True(expectedAuthParams.SequenceEqual(secParams.AuthParams.ToArray()), $"Unexpected auth params: {Utils.Dump.BytesToHexString(secParams.AuthParams.ToArray())}");

            var expectedPrivParams = Utils.Dump.BytesFromHexString("F2 D7 27 56 34 71 83 21");
            Assert.True(expectedPrivParams.SequenceEqual(secParams.PrivParams.ToArray()), $"Unexpected priv params: {Utils.Dump.BytesToHexString(secParams.PrivParams.ToArray())}");

            // Verify Scoped PDU
            Assert.Null(message.Scope);
            Assert.True(message.EncryptedScopedPdu.Length > 0, "Scope PDU is not encrypted");

            var sha1 = new DotNetSnmp.Protocol.V3.Security.Authentication.SHA1AuthenticationProvider(Encoding.UTF8.GetBytes("authkey1"));

            // Decrypt the Scoped PDU using AES privacy and passcode "privkey1"
            var engineId = message.SecurityParameters.EngineId.ToArray();
            var privPassword = "privkey1";

            // Create the AES privacy provider
            var aesPrivacyService = new DotNetSnmp.Protocol.V3.Security.Privacy.AESPrivacyProvider(sha1, privPassword.GetBytesMemoryOrDefault(Encoding.UTF8));

            // Decrypt the Scoped PDU
            aesPrivacyService.DecryptMessage(message);

            // Verify the Scoped PDU was decrypted
            Assert.NotNull(message.Scope);

            // Context information
            var expectedContextEngineId = Utils.Dump.BytesFromHexString("80 00 4F B8 05 63 6C 6F 75 64 4D AB 22 CC");
            var scope = (Scope)message.Scope;
            Assert.True(expectedContextEngineId.SequenceEqual(scope.ContextEngineId.ToArray()), $"Unexpected context engine ID: {Utils.Dump.BytesToHexString(scope.ContextEngineId.ToArray())}");
            Assert.Empty(scope.ContextName.ToArray());

            // PDU details
            var pdu = message.Scope.Pdu;
            Assert.Equal(0x746173F8, pdu.RequestId);
            Assert.Equal(ErrorCode.NoError, pdu.ErrorStatus);
            Assert.Equal(0, pdu.ErrorIndex);

            // Verify Variable Bindings
            Assert.NotNull(pdu.VariableBindings);
            Assert.Single(pdu.VariableBindings);

            // Verify the OID and value
            var varBind = pdu.VariableBindings?.First();
            Assert.Equal("1.3.6.1.2.1.1.1.0", varBind!.Value.Id.ToString());
            Assert.IsType<Null>(varBind.Value.Data);

            // Verify we can re-encode the message to the same bytes
            var reEncodedBytes = message.Encode();

            // Compare original and re-encoded bytes
            Assert.Equal(messageBytes.Length, reEncodedBytes.Length);
            Assert.True(messageBytes.SequenceEqual(reEncodedBytes));

            var authenticated = sha1.AuthenticateIncomingMsg(message);
            Assert.True(authenticated);
        }

        [Fact]
        public void FullMessageTest_MD5_TripleDES()
        {
            // This is a sample SNMPv3 message with TripleDES encryption
            const string hexBytes = "30 81 9A 02 01 03 30 11 02 04 16 39 99 3C 02 03 00 FF E3 04 01 07 02 01 03 04 40 30 3E 04 0E 80 00 4F B8 05 63 6C 6F 75 64 4D AB 22 CC 02 01 00 02 03 75 6B 5D 04 0C 75 73 72 2D 6D 64 35 2D 33 64 65 73 04 0C 88 3A 31 F2 4D FA 37 1D 40 43 51 EC 04 08 00 00 00 00 3A 90 A4 49 04 40 77 0E B3 60 5E 97 7B 53 3E 21 FD B1 74 6B 73 CF 8A AF C1 14 0B C5 AA EF 2C A3 4F 5E FF 07 BD 37 AF 14 64 91 1B AB 23 E5 C8 52 8E 64 0F FF 67 A3 CB 6D 68 0B 96 67 C5 79 93 AF 02 B2 02 CD B5 CF";

            // Convert hex string to byte array
            var messageBytes = Utils.Dump.BytesFromHexString(hexBytes);

            // Parse the message
            var reader = new System.Formats.Asn1.AsnReader(messageBytes, System.Formats.Asn1.AsnEncodingRules.BER);
            var message = Protocol.V3.SnmpV3Message.ReadFrom(reader);

            // Verify message basic properties
            Assert.NotNull(message);
            Assert.Equal(Common.Definitions.VersionCode.V3, message.ProtocolVersion);

            // Verify Header Data
            Assert.Equal(0x1639993C, message.Header.MsgId);
            Assert.Equal(0xFFE3, message.Header.MsgMaxSize);
            Assert.Equal(MsgFlags.Auth | MsgFlags.Priv | MsgFlags.Reportable, message.Header.MsgFlags);
            Assert.Equal(SecurityModel.Usm, message.Header.MsgSecurityModel);

            // Verify Security Parameters
            var secParams = message.SecurityParameters;
            Assert.NotNull(secParams);

            var expectedEngineId = Utils.Dump.BytesFromHexString("80 00 4F B8 05 63 6C 6F 75 64 4D AB 22 CC");
            Assert.True(expectedEngineId.SequenceEqual(secParams.EngineId.ToArray()), $"Unexpected engine ID: {Utils.Dump.BytesToHexString(secParams.EngineId.ToArray())}");

            Assert.Equal(0x00, secParams.EngineBoots);
            Assert.Equal(0x756B5D, secParams.EngineTime);

            var expectedSecurityName = Encoding.ASCII.GetBytes("usr-md5-3des");
            Assert.True(expectedSecurityName.SequenceEqual(secParams.SecurityName.Octets), $"Unexpected security name: {Utils.Dump.BytesToHexString(secParams.SecurityName.Octets)}");

            var expectedPrivParams = Utils.Dump.BytesFromHexString("00 00 00 00 3A 90 A4 49");
            Assert.True(expectedPrivParams.SequenceEqual(secParams.PrivParams.ToArray()), $"Unexpected priv params: {Utils.Dump.BytesToHexString(secParams.PrivParams.ToArray())}");

            // Verify Scoped PDU is encrypted
            Assert.Null(message.Scope);
            Assert.True(message.EncryptedScopedPdu.Length > 0, "Scope PDU is not encrypted");

            // Set up authentication and privacy providers with the credentials
            var authPassword = new OctetString("authkey1");
            var privPassword = new OctetString("privkey1");

            var auth = new DotNetSnmp.Protocol.V3.Security.Authentication.MD5AuthenticationProvider(authPassword.Octets);
            var priv = new DotNetSnmp.Protocol.V3.Security.Privacy.TripleDESPrivacyProvider(auth, privPassword.Octets);

            // Decrypt the message
            priv.DecryptMessage(message);

            // Verify the Scoped PDU was decrypted successfully
            Assert.NotNull(message.Scope);

            // Verify the contents of the decrypted Scoped PDU
            var scope = (Scope)message.Scope;
            Assert.True(Utils.Dump.BytesFromHexString("80 00 4F B8 05 63 6C 6F 75 64 4D AB 22 CC").SequenceEqual(scope.ContextEngineId.ToArray()), $"Unexpected context engine ID: {Utils.Dump.BytesToHexString(scope.ContextEngineId.ToArray())}");
            Assert.Empty(scope.ContextName.ToArray());

            // Check PDU details
            var pdu = scope.Pdu;
            //Assert.Equal(Protocol.PduType.GetRequest, pdu.PduType);
            Assert.Equal(-1133661761, pdu.RequestId);
            Assert.Equal(ErrorCode.NoError, pdu.ErrorStatus);
            Assert.Equal(0, pdu.ErrorIndex);

            // Check variable binding
            Assert.NotNull(pdu.VariableBindings);
            Assert.Equal(2, pdu.VariableBindings.Count());
            //Assert.Equal("1.3.6.1.2.1.1.3.0", pdu.VariableBindings.First().Value.Id.ToString());

            // Verify we can re-encode the message to the same bytes
            var reEncodedBytes = message.Encode();
            Assert.Equal(messageBytes.Length, reEncodedBytes.Length);
            Assert.True(messageBytes.SequenceEqual(reEncodedBytes), "Re-encoded bytes do not match original bytes");
            // Verify authentication
            var authenticated = auth.AuthenticateIncomingMsg(message);
            Assert.True(authenticated, "Message authentication failed");
        }

        [Fact]
        public void TestBadResponseFromPrinter()
        {
            // This test verifies handling of invalid data from a printer (#7241)
            const string data = "30 2B 02 01 00 04 06 70 75 62 6C 69 63 A2 1E 02 04 32 FA 7A 02 02 01 00 02 01 00 30 10 30 0E 06 0A 2B 06 01 02 01 02 02 01 16 01 06 00";

            // The original data had numerous zeros padded at the end, which we'll represent with this comment
            // We're using a truncated version that's sufficient to trigger the same validation error

            // Convert the hex string to bytes
            var bytes = Utils.Dump.BytesFromHexString(data);

            // Attempt to parse the message with invalid data
            // This should throw an exception due to the malformed data
            var exception = Assert.Throws<System.Formats.Asn1.AsnContentException>(() =>
            {
                try
                {
                    // Try to parse the message
                    var reader = new System.Formats.Asn1.AsnReader(bytes, System.Formats.Asn1.AsnEncodingRules.BER);
                    var message = SnmpV1Message.ReadFrom(reader);
                }
                catch (Exception)
                {
                    // Re-throw the exception to be caught by Assert.Throws
                    throw;
                }
            });

            // Verify the exception contains an appropriate message
            // The original test checked for "Byte length cannot be 0" in the inner exception
            Assert.Contains("The ASN.1 value is invalid.", exception.Message);
        }

        [Fact]
        public void MessageFactoryTest()
        {
            // This test verifies the MessageFactory's ability to parse messages
            const string hexBytes = "30 77 02 01  03 30 0F 02  02 47 21 02  03 00 FF E3" +
                     "04 01 05 02  01 03 04 32  30 30 04 0D  80 00 1F 88" +
                     "80 E9 63 00  00 D6 1F F4  49 02 01 15  02 02 01 5B" +
                     "04 08 6C 65  78 74 75 64  69 6F 04 0C  7B 62 65 AE" +
                     "D3 8F E3 7D  58 45 5C 6C  04 00 30 2D  04 0D 80 00" +
                     "1F 88 80 E9  63 00 00 D6  1F F4 49 04  00 A0 1A 02" +
                     "02 56 FF 02  01 00 02 01  00 30 0E 30  0C 06 08 2B" +
                     "06 01 02 01  01 03 00 05  00";

            // Convert the hex string to bytes
            var bytes = Utils.Dump.BytesFromHexString(hexBytes);

            // Create a UserRegistry (if needed)
            var registry = new UserRegistry();
            // Add a user to the registry (if needed)
            registry.Add(new User(new OctetString("lextudio"), new DotNetSnmp.Protocol.V3.Security.Privacy.DefaultPrivacyProvider(new DotNetSnmp.Protocol.V3.Security.Authentication.SHA1AuthenticationProvider(Encoding.UTF8.GetBytes("password")))));

            // Parse the messages using the MessageFactory
            var messages = MessageFactory.ParseMessages(bytes, registry);

            // Verify that we have at least one message
            Assert.NotEmpty(messages);
        }

        [Fact]
        public void MessageFactoryParseMessagesWithEnumerableChars()
        {
            const string hexBytes = "30 77 02 01  03 30 0F 02  02 47 21 02  03 00 FF E3" +
                     "04 01 05 02  01 03 04 32  30 30 04 0D  80 00 1F 88" +
                     "80 E9 63 00  00 D6 1F F4  49 02 01 15  02 02 01 5B" +
                     "04 08 6C 65  78 74 75 64  69 6F 04 0C  7B 62 65 AE" +
                     "D3 8F E3 7D  58 45 5C 6C  04 00 30 2D  04 0D 80 00" +
                     "1F 88 80 E9  63 00 00 D6  1F F4 49 04  00 A0 1A 02" +
                     "02 56 FF 02  01 00 02 01  00 30 0E 30  0C 06 08 2B" +
                     "06 01 02 01  01 03 00 05  00";

            var registry = CreateRegistry();
            var messages = MessageFactory.ParseMessages(hexBytes.AsEnumerable(), registry);

            Assert.NotEmpty(messages);
        }

        [Fact]
        public void MessageFactoryParseMessagesWithBufferSlice()
        {
            const string hexBytes = "30 77 02 01  03 30 0F 02  02 47 21 02  03 00 FF E3" +
                     "04 01 05 02  01 03 04 32  30 30 04 0D  80 00 1F 88" +
                     "80 E9 63 00  00 D6 1F F4  49 02 01 15  02 02 01 5B" +
                     "04 08 6C 65  78 74 75 64  69 6F 04 0C  7B 62 65 AE" +
                     "D3 8F E3 7D  58 45 5C 6C  04 00 30 2D  04 0D 80 00" +
                     "1F 88 80 E9  63 00 00 D6  1F F4 49 04  00 A0 1A 02" +
                     "02 56 FF 02  01 00 02 01  00 30 0E 30  0C 06 08 2B" +
                     "06 01 02 01  01 03 00 05  00";

            var bytes = Utils.Dump.BytesFromHexString(hexBytes);
            var withPadding = new byte[bytes.Length + 6];
            Array.Copy(bytes, 0, withPadding, 3, bytes.Length);

            var registry = CreateRegistry();
            var messages = MessageFactory.ParseMessages(withPadding, 3, bytes.Length, registry);

            Assert.NotEmpty(messages);
        }

        [Fact]
        public void MessageFactoryParseMessagesNullChecks()
        {
            var registry = CreateRegistry();
            var bytes = Utils.Dump.BytesFromHexString("30 03 02 01 00");

            Assert.Throws<ArgumentNullException>(() => MessageFactory.ParseMessages((IEnumerable<char>)null!, registry));
            Assert.Throws<ArgumentNullException>(() => MessageFactory.ParseMessages((byte[])null!, registry));
            Assert.Throws<ArgumentNullException>(() => MessageFactory.ParseMessages((byte[])null!, 0, 0, registry));
            Assert.Throws<ArgumentNullException>(() => MessageFactory.ParseMessages(bytes, (UserRegistry)null!));
        }

        private static UserRegistry CreateRegistry()
        {
            var registry = new UserRegistry();
            registry.Add(new User(new OctetString("lextudio"), new DotNetSnmp.Protocol.V3.Security.Privacy.DefaultPrivacyProvider(new DotNetSnmp.Protocol.V3.Security.Authentication.SHA1AuthenticationProvider(Encoding.UTF8.GetBytes("password")))));
            return registry;
        }
    }
}
