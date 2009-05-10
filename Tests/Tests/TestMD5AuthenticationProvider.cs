using System.Text;
using Lextm.SharpSnmpLib.Security;
using NUnit.Framework;

namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class TestMD5AuthenticationProvider
    {
        [Test]
        public void TestPasswordToKey()
        {
            byte[] password = Encoding.ASCII.GetBytes("testpass");
            byte[] engineId = new byte[] { 0x80, 0x00, 0x1F, 0x88, 0x80, 0xE9, 0x63, 0x00, 0x00, 0xD6, 0x1F, 0xF4, 0x49 };

            byte[] key = new MD5AuthenticationProvider(new OctetString("")).PasswordToKey(password, engineId);
            Assert.AreEqual(new byte[] { 226, 221, 44, 186, 149, 93, 73, 79, 237, 69, 120, 155, 145, 7, 44, 255 }, key);
        }
    }
}
