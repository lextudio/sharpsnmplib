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
        public void TestParse_SNMPv2_SMI()
        {
            var lexer = new Lexer();
            var m = new MemoryStream(Properties.Resources.SNMPv2_SMI);
            using (var reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }

            Assert.AreEqual(1059, lexer.SymbolCount);
        }

        [Test]
        public void TestParse_RFC1157_SNMP_MIB()
        {
            var lexer = new Lexer();
            var m = new MemoryStream(Properties.Resources.RFC1157_SNMP);
            using (var reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }

            Assert.AreEqual(371, lexer.SymbolCount);
        }
        
        [Test]
        public void TestParse2()
        {
            const string test = "Cas::=";
            var lexer = new Lexer();
            var reader = new StringReader(test);
            lexer.Parse(reader);
            Assert.AreEqual("Cas", lexer.GetNextSymbol().ToString());
            Assert.AreEqual("::=", lexer.GetNextSymbol().ToString());
        }
        
        [Test]
        public void TestParse3()
        {
            const string test = "::=BEGIN";
            var lexer = new Lexer();
            var reader = new StringReader(test);
            lexer.Parse(reader);
            Assert.AreEqual("::=", lexer.GetNextSymbol().ToString());
            Assert.AreEqual("BEGIN", lexer.GetNextSymbol().ToString());
        }      
 
        [Test]
        public void TestParseInlineComment()
        {
            string test = "APPNMIB DEFINITIONS     ::= BEGIN" + Environment.NewLine +
                          //* 
                          Environment.NewLine +
                          "appnDirLuLocation OBJECT-TYPE" + Environment.NewLine +
                          "   SYNTAX INTEGER {" + Environment.NewLine +
                          "           local(1),   --Local" + Environment.NewLine +
                          "           domain(2),  --Domain" + Environment.NewLine +
                          "           xdomain(3)  --Cross Domain" + Environment.NewLine +
                          "           }" + Environment.NewLine +
                          "   MAX-ACCESS read-only" + Environment.NewLine +
                          "   STATUS current" + Environment.NewLine +
                          "   DESCRIPTION" + Environment.NewLine +
                          "     \"Specifies the location of the LU with respect to the local" + Environment.NewLine +
                          "     node.\"" + Environment.NewLine +
                          Environment.NewLine +
                          "   ::= { appnDirEntry 4 }" + Environment.NewLine +
                          Environment.NewLine + 
                          //*/
                          "tcpRtoAlgorithm OBJECTTYPE" + Environment.NewLine +
                          "    SYNTAX      INTEGER {" + Environment.NewLine +
                          "                    other(1),    -- none of the following" + Environment.NewLine +
                          "                    constant(2), -- a constant rto" + Environment.NewLine +
                          "                    rsre(3),     -- MIL-STD-1778, Appendix B" + Environment.NewLine +
                          "                    vanj(4),     -- Van Jacobson's algorithm" + Environment.NewLine +
                          "                    rfc2988(5)   -- RFC 2988" + Environment.NewLine +
                          "                }" + Environment.NewLine +
                          "    MAXACCESS readonly" + Environment.NewLine +
                          "    STATUS     current" + Environment.NewLine +
                          "    DESCRIPTION" + Environment.NewLine +
                          "           \"The algorithm used to determine the timeout value used for" +
                          Environment.NewLine +
                          "            retransmitting unacknowledged octets.\"" + Environment.NewLine +
                          "    ::= { tcp 1 }" + Environment.NewLine;

            var lexer = new Lexer();
            var reader = new StringReader(test);
            lexer.Parse(reader);
            Assert.AreEqual(112, lexer.SymbolCount);
        }
    }
}
#pragma warning restore 1591