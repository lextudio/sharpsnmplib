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
                Assert.Contains("Invalid data type.", exception.Message);
            }

            Assert.Throws<ArgumentNullException>(() => provider.Decrypt(null, null));
            Assert.Throws<ArgumentNullException>(() => provider.Decrypt(OctetString.Empty, null));
            {
                var exception = Assert.Throws<ArgumentException>(() =>
                    provider.Decrypt(new Null(), SecurityParameters.Create(OctetString.Empty)));
                Assert.Contains("Cannot decrypt the scope data: Null.", exception.Message);
            }
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
            var key = new byte[32];
            generator.NextBytes(key);

            var provider = new AES256PrivacyProvider(OctetString.Empty, new MD5AuthenticationProvider(new OctetString("authentication")));
            {
                var encrypted = provider.LegacyEncrypt(key, iv, data);
                var decrypted = provider.Net6Decrypt(key, iv, encrypted);
                Assert.Equal(data, decrypted);
            }

            {
                var encrypted = provider.Net6Encrypt(key, iv, data);
                var decrypted = provider.LegacyDecrypt(key, iv, encrypted);
                Assert.Equal(data, decrypted);
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                for (int start = 1; start <= 256; start++)
                {
                    yield return new object[] { start * 8 };
                }
            }
        }
#endif
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
