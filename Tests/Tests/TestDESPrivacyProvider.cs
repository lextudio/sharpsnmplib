using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class TestDESPrivacyProvider
    {
        [Test]
        public void TestDecrypt()
        {
            byte[] encrypted = ByteTool.ConvertByteString("4B  4F 10 3B 73  E1 E4 BD 91  32 1B CB 41" +
                 "1B A1 C1 D1  1D 2D B7 84  16 CA 41 BF  B3 62 83 C4" +
                 "29 C5 A4 BC  32 DA 2E C7  65 A5 3D 71  06 3C 5B 56" +
                 "FB 04 A4");
            byte[] real = DESPrivacyProvider.Decrypt(encrypted, new byte[] { 0x37, 0xc6, 0x4c, 0xad, 0x49, 0x37, 0xfe, 0xda, 0x57, 0xc8, 0x48, 0x53, 0x47, 0x2a, 0x2e, 0xc0 },
                new byte[] { 0x00, 0x00, 0x00, 0x01, 0x44, 0x2c, 0xa3, 0xb5 });
            byte[] expected = ByteTool.ConvertByteString("30  2D  04 0D 80 00 1F 88 80  E9 63 00 00  D6 1F F4 49 04 00 A0 1A 02 02 3A 25  02 01 00 02  01 00 30 0E  30 0C 06 08 2B 06 01 02  01 01 03 00  05 00 01");
            Assert.AreEqual(expected, real);
        }

        [Test]
        public void TestEncrypt()
        {
            byte[] decrypted = ByteTool.ConvertByteString("30  2D  04 0D 80 00 1F 88 80  E9 63 00 00  D6 1F F4 49 04 00 A0 1A 02 02 3A 25  02 01 00 02  01 00 30 0E  30 0C 06 08 2B 06 01 02  01 01 03 00  05 00 01");
            byte[] fake = DESPrivacyProvider.Encrypt(decrypted, new byte[] { 0x37, 0xc6, 0x4c, 0xad, 0x49, 0x37, 0xfe, 0xda, 0x57, 0xc8, 0x48, 0x53, 0x47, 0x2a, 0x2e, 0xc0 },
                new byte[] { 0x00, 0x00, 0x00, 0x01, 0x44, 0x2c, 0xa3, 0xb5 });
            byte[] expected = ByteTool.ConvertByteString("4B  4F 10 3B 73  E1 E4 BD 91  32 1B CB 41" +
                 "1B A1 C1 D1  1D 2D B7 84  16 CA 41 BF  B3 62 83 C4" +
                 "29 C5 A4 BC  32 DA 2E C7  65 A5 3D 71  06 3C 5B 56" +
                 "FB 04 A4");
            Assert.AreEqual(expected, fake);
        }

        [Test]
        public void TestEncrypt2()
        {
            byte[] expected = ByteTool.ConvertByteString("04 30 4B  4F 10 3B 73  E1 E4 BD 91  32 1B CB 41" +
     "1B A1 C1 D1  1D 2D B7 84  16 CA 41 BF  B3 62 83 C4" +
     "29 C5 A4 BC  32 DA 2E C7  65 A5 3D 71  06 3C 5B 56" +
     "FB 04 A4");
            OctetString engineId = new OctetString(ByteTool.ConvertByteString("80 00 1F 88 80  E9 63 00 00  D6 1F F4 49"));
            DESPrivacyProvider priv = new DESPrivacyProvider(new OctetString("passtest"), new MD5AuthenticationProvider(new OctetString("testpass")));
            Scope scope = new Scope(engineId, OctetString.Empty, new GetRequestPdu(0x3A25, ErrorCode.NoError, 0, new List<Variable>() { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.3.0")) }));
            SecurityParameters parameters = new SecurityParameters(engineId, new Integer32(0x14), new Integer32(0x35), new OctetString("lexmark"), new OctetString(new byte[12]), new OctetString(ByteTool.ConvertByteString("00 00 00  01 44 2C A3 B5")));
            ISnmpData data = priv.Encrypt(scope.GetData(VersionCode.V3), parameters);
            Assert.AreEqual(SnmpType.OctetString, data.TypeCode);
            Assert.AreEqual(expected, ByteTool.ToBytes(data));
        }
    }
}
