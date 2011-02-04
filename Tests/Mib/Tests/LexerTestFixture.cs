using System;
using System.IO;
using NUnit.Framework;

#pragma warning disable 1591
namespace Lextm.SharpSnmpLib.Mib.Tests
{
    [TestFixture]
    public class LexerTestFixture
    {
        [Test]
        public void TestMethod()
        {
            Lexer lexer = new Lexer();
            lexer.Parse(new StringReader("{ iso org(3) dod(6) 1 }"));
            string parent;
            uint value;
            lexer.ParseOidValue(out parent, out value);
            Assert.AreEqual("iso.org(3).dod(6)", parent);
            Assert.AreEqual(1, value);
        }

        [Test]
        public void TestMethod1()
        {
            Lexer lexer = new Lexer();
            lexer.Parse(new StringReader("{ iso(1) std(0) iso8802(8802) ieee802dot1(1)" + Environment.NewLine +
                                         "     ieee802dot1mibs(1) 1 }"));
            string parent;
            uint value;
            lexer.ParseOidValue(out parent, out value);
            Assert.AreEqual("iso(1).std(0).iso8802(8802).ieee802dot1(1).ieee802dot1mibs(1)", parent);
            Assert.AreEqual(1, value);
        }

        [Test]
        public void TestParse()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Properties.Resources.SNMPv2_SMI);
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