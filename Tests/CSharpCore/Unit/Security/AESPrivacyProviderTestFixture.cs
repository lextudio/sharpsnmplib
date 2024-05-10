/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2010/12/5
 * Time: 15:33
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

#pragma warning disable CS0618 // Type or member is obsolete
namespace Lextm.SharpSnmpLib.Unit.Security
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Lextm.SharpSnmpLib.Security;

    public class AESPrivacyProviderTestFixture
    {
        [Fact]
        public void TestException()
        {
            var provider = new AESPrivacyProvider(new OctetString("longlongago"),
                new MD5AuthenticationProvider(new OctetString("verylonglongago")));
            Assert.Throws<ArgumentNullException>(() => new AESPrivacyProvider(null, null));
            Assert.Throws<ArgumentNullException>(() => new AESPrivacyProvider(OctetString.Empty, null));
            Assert.Throws<ArgumentNullException>(() => provider.Encrypt(null, null));
            Assert.Throws<ArgumentNullException>(() => provider.Encrypt(OctetString.Empty, null));

            {
                var exception = Assert.Throws<ArgumentException>(() =>
                    provider.Encrypt(new Null(), SecurityParameters.Create(OctetString.Empty)));
                Assert.Contains("Invalid data type.", exception.Message);
            }

            Assert.Throws<ArgumentNullException>(() => provider.Decrypt(null, null));
            Assert.Throws<ArgumentNullException>(() => provider.Decrypt(OctetString.Empty, null));
            {
                var exception = Assert.Throws<ArgumentException>(() =>
                    provider.Decrypt(new Null(), SecurityParameters.Create(OctetString.Empty)));
                Assert.Contains($"Cannot decrypt the scope data: Null.", exception.Message);
            }
        }

        [Fact]
        public void TestIsSupported()
        {
#if NET6_0
            Assert.True(AESPrivacyProviderBase.IsSupported);
#elif NET5_0
                Assert.True(AESPrivacyProviderBase.IsSupported);
#elif NETCOREAPP3_1
                Assert.False(AESPrivacyProviderBase.IsSupported);
#elif NET471
                Assert.True(AESPrivacyProviderBase.IsSupported);
#endif
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
            byte[] fake = new AESPrivacyProvider(OctetString.Empty, new MD5AuthenticationProvider(new OctetString("anything"))).Encrypt(decrypted,
                new byte[]
                {
                    0x37, 0xc6, 0x4c, 0xad, 0x49, 0x37, 0xfe, 0xda, 0x57, 0xc8, 0x48, 0x53, 0x47, 0x2a, 0x2e, 0xc0
                },
                0, 0, new byte[] { 0x00, 0x00, 0x00, 0x01, 0x44, 0x2c, 0xa3, 0xb5 });
            byte[] expected =
                ByteTool.Convert(
                    "36 0A 04 BB A8 9A 37 C1 28 2E 9C B6 30 A1  AB 7E 1E 60 60 EF D2 91 3A 26 B0 1C D5  55 B7 16 78 FB A4 D1 9A 2C E4 30 9A 86  EC E1 83 EE 72 C2 68 BC");
            Assert.Equal(ByteTool.Convert(expected), ByteTool.Convert(fake));
        }

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
                priv = new AESPrivacyProvider(new OctetString("passtest"),
                    new MD5AuthenticationProvider(new OctetString("testpass")));
            }
            else
            {
                return;
            }

            Scope scope = new Scope(engineId, OctetString.Empty,
                new GetRequestPdu(0x3A25,
                    new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.3.0")) }));
            SecurityParameters parameters = new SecurityParameters(engineId, new Integer32(0x14), new Integer32(0x35),
                new OctetString("lexmark"), new OctetString(new byte[12]),
                new OctetString(ByteTool.Convert("00 00 00  01 44 2C A3 B5")));
            var original = scope.GetData(VersionCode.V3);
            ISnmpData data = priv.Encrypt(original, parameters);
            Assert.Equal(SnmpType.OctetString, data.TypeCode);

            ISnmpData decrypted = priv.Decrypt(data, parameters);
            Assert.Equal(ByteTool.Convert(original.ToBytes()), ByteTool.Convert(decrypted.ToBytes()));
        }

        [Fact]
        public void TestEncrypt3()
        {
            byte[] received =
                ByteTool.Convert(
                    "04 35 CC AE 0C FA 1E 41  CC DD F8 BA  49 27 7E 47  C9 8D 73 63 3B 1A CE 56  97 2D CB 0A  2D DF A1 AC  0F B0 8E 3B 25 EF F1 B6  3B 76 3F 74  84 7C E6 C0  DC AE DE EC D9 9E 5F");
            OctetString engineId = new OctetString(ByteTool.Convert("80 00 1F 88 80  38 92 B3 6C  6C 89 40 65  00 00 00 00"));

            IPrivacyProvider priv;
            if (AESPrivacyProviderBase.IsSupported)
            {
                priv = new AESPrivacyProvider(new OctetString("privkey1"),
                    new SHA1AuthenticationProvider(new OctetString("authkey1")));
            }
            else
            {
                return;
            }

            Scope scope = new Scope(engineId, OctetString.Empty,
                new GetRequestPdu(282716518,
                    new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) }));
            SecurityParameters parameters = new SecurityParameters(engineId, new Integer32(0x0B), new Integer32(0x176),
                new OctetString("usr-sha-aes"), new OctetString(new byte[12]),
                new OctetString(ByteTool.Convert("FA 24 36 97  A4 79 E6 35")));
            var original = scope.GetData(VersionCode.V3);
            ISnmpData data = priv.Encrypt(original, parameters);
            Assert.Equal(SnmpType.OctetString, data.TypeCode);
            var encrypted = ByteTool.Convert(data.ToBytes());
            Assert.Equal(ByteTool.Convert(received), encrypted);

            ISnmpData decrypted_received = priv.Decrypt(DataFactory.CreateSnmpData(received), parameters);
            var recovered_received = ByteTool.Convert(decrypted_received.ToBytes());
            var recovered_encrypted = ByteTool.Convert(priv.Decrypt(data, parameters).ToBytes());
            Assert.Equal(recovered_received, recovered_encrypted);
        }

#if NET6_0
        [Theory]
        [MemberData(nameof(Data))]
        public void CompatibilityTest(int length)
        {
            var generator = new Random();
            var data = new byte[length];
            generator.NextBytes(data);
            var iv = new byte[16];
            generator.NextBytes(iv);
            var key = new byte[16];
            generator.NextBytes(key);

            var provider = new AESPrivacyProvider(OctetString.Empty, new MD5AuthenticationProvider(new OctetString("authentication")));
            {
                var encrypted = provider.LegacyEncrypt(key, iv, data);
                Assert.Equal(length, encrypted.Length);
                var decrypted = AESPrivacyProviderBase.Net6Decrypt(key, iv, encrypted);
                Assert.Equal(data, decrypted);
            }

            {
                var encrypted = AESPrivacyProviderBase.Net6Encrypt(key, iv, data);
                Assert.Equal(length, encrypted.Length);
                var decrypted = provider.LegacyDecrypt(key, iv, encrypted);
                Assert.Equal(data, decrypted);
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                for (int start = 1; start <= 1; start++)
                {
                    yield return new object[] { start * 8 };
                }
            }
        }
#endif
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
