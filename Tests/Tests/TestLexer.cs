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
            MemoryStream m = new MemoryStream(Resources.SNMPv2_SMI);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            Assert.AreEqual(925, lexer.SymbolCount);
        }
        
        [Test]
        public void TestParse2()
        {
            const string test = "Cas::=";
            Lexer lexer = new Lexer();
            StringReader reader = new StringReader(test);
            lexer.Parse(reader);
            Assert.AreEqual("Cas", lexer.NextSymbol.ToString());
            Assert.AreEqual("::=", lexer.NextSymbol.ToString());
        }
        
        [Test]
        public void TestParse3()
        {
            const string test = "::=BEGIN";
            Lexer lexer = new Lexer();
            StringReader reader = new StringReader(test);
            lexer.Parse(reader);
            Assert.AreEqual("::=", lexer.NextSymbol.ToString());
            Assert.AreEqual("BEGIN", lexer.NextSymbol.ToString());
        }       
    }
}
#pragma warning restore 1591