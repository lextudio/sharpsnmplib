/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 11/7/2009
 * Time: 8:29 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using Antlr.Runtime;
using Lextm.SharpSnmpLib.Mib.Ast.ANTLR;
using NUnit.Framework;

namespace Lextm.SharpSnmpLib.Tests
{
    /// <summary>
    /// Description of TestAst.
    /// </summary>
    [TestFixture]
    public class TestAst
    {  
        [Test]
        public void TestSNMPv2_SMI()
        {
            var stream = new ANTLRInputStream(new MemoryStream(Resources.SNMPv2_SMI));
            var lexer = new SmiLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new SmiParser(tokens);
            var document = parser.GetDocument();
            Assert.AreEqual(1, document.Modules.Count);
            Assert.AreEqual(34, document.Modules[0].Constructs.Count);
            Assert.AreEqual(16, document.Modules[0].Entities.Count);
        }

        [Test]
        public void TestLexerOK()
        {
            string test = "ADSL-LINE-MIB DEFINITIONS ::= BEGIN" + Environment.NewLine +
                "IMPORTS" + Environment.NewLine +
                "MODULE-IDENTITY, OBJECT-TYPE," + Environment.NewLine +
                "Counter32, Gauge32, Integer32," + Environment.NewLine +
                "NOTIFICATION-TYPE," + Environment.NewLine +
                "transmission           FROM SNMPv2-SMI;" + Environment.NewLine +
                "END";
            var lex = new SmiLexer(new ANTLRStringStream(test));
            var tokens = new CommonTokenStream(lex);
            var parser = new SmiParser(tokens);
            var document = parser.GetDocument();
            Assert.AreEqual(1, document.Modules.Count);
            const string expected = "ADSL-LINE-MIB";
            Assert.AreEqual(expected, document.Modules[0].Name);
            var module = document.Modules[0];
            Assert.AreEqual(false, module.Exports.AllExported);
            Assert.AreEqual(0, module.Exports.Symbols.Count);
            Assert.AreEqual(1, module.Imports.Clauses.Count);
            var import = module.Imports.Clauses[0];
            Assert.AreEqual("SNMPv2-SMI", import.Module);
            Assert.AreEqual(7, import.Symbols.Count);
            Assert.AreEqual("MODULE-IDENTITY", import.Symbols[0]);
            Assert.AreEqual("OBJECT-TYPE", import.Symbols[1]);
            Assert.AreEqual("Counter32", import.Symbols[2]);
            Assert.AreEqual("Gauge32", import.Symbols[3]);
            Assert.AreEqual("Integer32", import.Symbols[4]);
            Assert.AreEqual("NOTIFICATION-TYPE", import.Symbols[5]);
            Assert.AreEqual("transmission", import.Symbols[6]);
        }
    }
}
