using Lextm.SharpSnmpLib;
using System.IO;
using Lextm.SharpSnmpLib.Mib;
using NUnit.Framework;
#pragma warning disable 1591
namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class TestLexer
    {
        [Test]
        public void TestParse()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.SNMPv2_SMI);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            Assert.AreEqual(893, lexer.SymbolCount);
        }
    }
}
#pragma warning restore 1591