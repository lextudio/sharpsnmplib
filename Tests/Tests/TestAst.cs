/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 11/7/2009
 * Time: 8:29 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
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
        public void TestLexer()
        {
            string test = "my";
            SmiLexer lex = new SmiLexer(new ANTLRStringStream(test));
            CommonTokenStream tokens = new CommonTokenStream(lex);
            Assert.AreEqual(1, tokens.GetTokens().Count);
            SmiParser parser = new SmiParser(tokens);
            CommonErrorNode error = (CommonErrorNode)parser.module_definition().Tree;
            Assert.AreEqual(1, tokens.Size());
            Assert.AreEqual("my", error.Text);
        }
        
        [Test]
        public void TestLexerOK()
        {
            string test = "ADSL-LINE-MIB DEFINITIONS ::= BEGIN END";
            SmiLexer lex = new SmiLexer(new ANTLRStringStream(test));
            CommonTokenStream tokens = new CommonTokenStream(lex);
            Assert.AreEqual(5, tokens.GetTokens().Count);
            SmiParser parser = new SmiParser(tokens);
            object tree = parser.module_definition().Tree;
            Assert.AreEqual(5, tokens.Size());
            Assert.AreEqual(typeof(CommonTree), tree.GetType());
        }
    }
}
