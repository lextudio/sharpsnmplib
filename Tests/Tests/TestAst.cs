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
using Antlr.Runtime.Tree;
// using Lextm.SharpSnmpLib.Mib.Ast.ANTLR;
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
        public void TestLexerOnMibs()
        {
            TestMib(Properties.Resources.ACTONA_ACTASTOR_MIB);
            TestMib(Properties.Resources.ADSL_LINE_MIB);
            TestMib(Properties.Resources.ADSL_TC_MIB);
            TestMib(Properties.Resources.AGENTX_MIB);
            TestMib(Properties.Resources.AIRPORT_BASESTATION_3_MIB);
            TestMib(Properties.Resources.ALLIEDTELESYN_MIB);
            TestMib(Properties.Resources.ALVARION_DOT11_WLAN_MIB);
            TestMib(Properties.Resources.APPC_MIB);
            TestMib(Properties.Resources.ARRAYMANAGER_MIB);
            TestMib(Properties.Resources.ARROWPOINT_IPV4_OSPF_MIB);
            TestMib(Properties.Resources.ATM_TC_MIB);
            TestMib(Properties.Resources.BASEBRDD_MIB_MIB);
            TestMib(Properties.Resources.BRIDGE_MIB);
            TestMib(Properties.Resources.CISCO_AAA_SERVER_MIB);
            TestMib(Properties.Resources.CISCO_BULK_FILE_MIB);
            TestMib(Properties.Resources.CISCO_CSG_MIB);
            TestMib(Properties.Resources.DISMAN_EVENT_MIB);
            TestMib(Properties.Resources.DISMAN_EXPRESSION_MIB);
            TestMib(Properties.Resources.DISMAN_NSLOOKUP_MIB);
            TestMib(Properties.Resources.DISMAN_PING_MIB);
            TestMib(Properties.Resources.DISMAN_SCHEDULE_MIB);
            TestMib(Properties.Resources.DISMAN_SCRIPT_MIB);
            TestMib(Properties.Resources.DISMAN_TRACEROUTE_MIB);
            TestMib(Properties.Resources.DMTF_DMI_MIB);
            TestMib(Properties.Resources.empty);
        }
        
        private void TestMib(byte[] bytes)
        {
            //SmiLexer lex = new SmiLexer(new ANTLRInputStream(new MemoryStream(bytes)));
            //CommonTokenStream tokens = new CommonTokenStream(lex);
            //SmiParser parser = new SmiParser(tokens);
            //Assert.AreNotEqual(typeof(CommonErrorNode), parser.statement().Tree.GetType());
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
            //SmiLexer lex = new SmiLexer(new ANTLRStringStream(test));
            //CommonTokenStream tokens = new CommonTokenStream(lex);
            //SmiParser parser = new SmiParser(tokens);
            //CommonTree tree = (CommonTree)parser.statement().Tree;
            //Assert.AreEqual(22, tokens.Size());
            //string moduleName = tree.Children[0].ToString();
            //CommonTreeNodeStream treeNodeStream = new CommonTreeNodeStream(tree);
            //SmiWalker treeWalker = new SmiWalker(treeNodeStream);
            //Lextm.SharpSnmpLib.Mib.Ast.ANTLR.SmiWalker.statement_return state = treeWalker.statement();
            //string expected = "ADSL-LINE-MIB";
            //Assert.AreEqual(expected, moduleName);
        }
    }
}
