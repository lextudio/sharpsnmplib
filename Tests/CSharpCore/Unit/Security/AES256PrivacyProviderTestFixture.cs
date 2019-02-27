/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2010/12/5
 * Time: 15:33
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace Lextm.SharpSnmpLib.Unit.Security
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Lextm.SharpSnmpLib.Security;

    public class AES256PrivacyProviderTestFixture
    {
        [Fact]
        public void TestException()
        {
            var provider = new AES256PrivacyProvider(new OctetString("longlongago"),
                new MD5AuthenticationProvider(new OctetString("verylonglongago")));
            Assert.Throws<ArgumentNullException>(() => new AES256PrivacyProvider(null, null));
            Assert.Throws<ArgumentNullException>(() => new AES256PrivacyProvider(OctetString.Empty, null));
            Assert.Throws<ArgumentNullException>(() => provider.Encrypt(null, null));
            Assert.Throws<ArgumentNullException>(() => provider.Encrypt(OctetString.Empty, null));

            {
                var exception = Assert.Throws<ArgumentException>(() =>
                    provider.Encrypt(new Null(), SecurityParameters.Create(OctetString.Empty)));
                Assert.Equal($"Invalid data type.{Environment.NewLine}Parameter name: data", exception.Message);
            }

            Assert.Throws<ArgumentNullException>(() => provider.Decrypt(null, null));
            Assert.Throws<ArgumentNullException>(() => provider.Decrypt(OctetString.Empty, null));
            {
                var exception = Assert.Throws<ArgumentException>(() =>
                    provider.Decrypt(new Null(), SecurityParameters.Create(OctetString.Empty)));
                Assert.Equal($"Cannot decrypt the scope data: Null.{Environment.NewLine}Parameter name: data", exception.Message);
            }
        }

        /*
        [Fact]
        public void TestDecrypt2()
        {
            // TODO: copied from DES. Need to change the data to match AES.
            byte[] encrypted =
                ByteTool.Convert(
                    "04 38 A4 F9 78 15 2B 14 45 F7 4F C5 B2 1C 82 72 9A 0B D9 EE C1 17 3E E1 26 0D 8B D4 7B 0F D7 35 06 1B E2 14 0D 4A 9B CA BF EF 18 6B 53 B9 FA 70 95 D0 15 38 C5 77 96 85 61 40");
            var privacy = new AES256PrivacyProvider(new OctetString("privacyphrase"),
                new MD5AuthenticationProvider(new OctetString("authentication")));
            var parameters = new SecurityParameters(
                new OctetString(ByteTool.Convert("80001F8880E9630000D61FF449")),
                Integer32.Zero,
                Integer32.Zero,
                new OctetString("lextm"),
                OctetString.Empty,
                new OctetString(ByteTool.Convert("0000000069D39B2A")));
            var data = privacy.Decrypt(DataFactory.CreateSnmpData(encrypted),
                parameters);
            Assert.Equal(SnmpType.Sequence, data.TypeCode);

            byte[] net =
                ByteTool.Convert(
                    "04 38 A4 F9 78 15 2B 14 45 F7 4F C5 B2 1C 82 72 9A 0B D9 EE C1 17 3E E1 26 0D 8B D4 7B 0F D7 35 06 1B E2 14 0D 4A 9B CA BF EF 18 6B 53 B9 FA 70 95 D0 5D AF 04 5A 68 B5 DA 73");
            var netData = privacy.Decrypt(DataFactory.CreateSnmpData(net), parameters);
            Assert.Equal(SnmpType.Sequence, netData.TypeCode);

            Assert.Equal(ByteTool.Convert(netData.ToBytes()), ByteTool.Convert(data.ToBytes()));
        }


        [Fact]
        public void TestDecrypt()
        {
            // TODO: copied from DES. Need to change the data to match AES.
            byte[] encrypted = ByteTool.Convert("4B  4F 10 3B 73  E1 E4 BD 91  32 1B CB 41" +
                                                "1B A1 C1 D1  1D 2D B7 84  16 CA 41 BF  B3 62 83 C4" +
                                                "29 C5 A4 BC  32 DA 2E C7  65 A5 3D 71  06 3C 5B 56" +
                                                "FB 04 A4");
            byte[] real = AES256PrivacyProvider.Decrypt(encrypted,
                new byte[]
                {
                    0x37, 0xc6, 0x4c, 0xad, 0x49, 0x37, 0xfe, 0xda, 0x57, 0xc8, 0x48, 0x53, 0x47, 0x2a, 0x2e, 0xc0
                },
                0, 0, new byte[] {0x00, 0x00, 0x00, 0x01, 0x44, 0x2c, 0xa3, 0xb5});
            byte[] expected =
                ByteTool.Convert(
                    "30  2D  04 0D 80 00 1F 88 80  E9 63 00 00  D6 1F F4 49 04 00 A0 1A 02 02 3A 25  02 01 00 02  01 00 30 0E  30 0C 06 08 2B 06 01 02  01 01 03 00  05 00 01");
            Assert.Equal(expected, real);
        }

        [Fact]
        public void TestEncrypt()
        {
            if (!AESPrivacyProviderBase.IsSupported)
            {
                return;
            }

            byte[] decrypted =
                ByteTool.Convert(
                    "30  2D  04 0D 80 00 1F 88 80  E9 63 00 00  D6 1F F4 49 04 00 A0 1A 02 02 3A 25  02 01 00 02  01 00 30 0E  30 0C 06 08 2B 06 01 02  01 01 03 00  05 00 01");
            byte[] fake = new AES256PrivacyProvider(OctetString.Empty, new MD5AuthenticationProvider(new OctetString("anything"))).Encrypt(decrypted,
                new byte[]
                {
                    0x37, 0xc6, 0x4c, 0xad, 0x49, 0x37, 0xfe, 0xda, 0x57, 0xc8, 0x48, 0x53, 0x47, 0x2a, 0x2e, 0xc0
                },
                0, 0, new byte[] {0x00, 0x00, 0x00, 0x01, 0x44, 0x2c, 0xa3, 0xb5});
            byte[] expected =
                ByteTool.Convert(
                    "36 0A 04 BB A8 9A 37 C1 28 2E 9C B6 30 A1  AB 7E 1E 60 60 EF D2 91 3A 26 B0 1C D5  55 B7 16 78 FB A4 D1 9A 2C E4 30 9A 86  EC E1 83 EE 72 C2 68 BC");
            Assert.Equal(ByteTool.Convert(expected), ByteTool.Convert(fake));
        }
//*/
        [Fact]
        public void TestEncrypt2()
        {
            byte[] expected =
                ByteTool.Convert(
                    "04 30 9D 13 04 9C 7E D9 84 8B 33 C3 26 5C 1F 91 30 27 D3 56 B0 FD 81 36 50 3A EF 80 1C B9 25 D6 38 84 A7 07 45 FE E8 D7 01 83 A1 CE 04 79 9D 5F 9E 2F");
            OctetString engineId = new OctetString(ByteTool.Convert("80 00 1F 88 80  E9 63 00 00  D6 1F F4 49"));
            IPrivacyProvider priv;
            if (AESPrivacyProviderBase.IsSupported)
            {
                priv = new AES256PrivacyProvider(new OctetString("passtest"),
                    new MD5AuthenticationProvider(new OctetString("testpass")));
            }
            else
            {
                return;
            }

            Scope scope = new Scope(engineId, OctetString.Empty,
                new GetRequestPdu(0x3A25,
                    new List<Variable> {new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.3.0"))}));
            SecurityParameters parameters = new SecurityParameters(engineId, new Integer32(0x14), new Integer32(0x35),
                new OctetString("lexmark"), new OctetString(new byte[12]),
                new OctetString(ByteTool.Convert("00 00 00  01 44 2C A3 B5")));
            var original = scope.GetData(VersionCode.V3);
            ISnmpData data = priv.Encrypt(original, parameters);
            Assert.Equal(SnmpType.OctetString, data.TypeCode);
            //Assert.Equal(ByteTool.Convert(expected), ByteTool.Convert(data.ToBytes()));

            ISnmpData decrypted = priv.Decrypt(data, parameters);
            Assert.Equal(ByteTool.Convert(original.ToBytes()), ByteTool.Convert(decrypted.ToBytes()));
        }
    }
}
