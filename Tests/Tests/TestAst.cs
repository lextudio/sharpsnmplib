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
            TestMib(Resources.ACTONA_ACTASTOR_MIB);
            TestMib(Resources.ADSL_LINE_MIB);
            TestMib(Resources.ADSL_TC_MIB);
            TestMib(Resources.AGENTX_MIB);
            TestMib(Resources.AIRPORT_BASESTATION_3_MIB);
            TestMib(Resources.ALLIEDTELESYN_MIB);
            TestMib(Resources.ALVARION_DOT11_WLAN_MIB);
            TestMib(Resources.APPC_MIB);
            TestMib(Resources.ARRAYMANAGER_MIB);
            TestMib(Resources.ARROWPOINT_IPV4_OSPF_MIB);
            TestMib(Resources.ATM_TC_MIB);
            TestMib(Resources.BASEBRDD_MIB_MIB);
            TestMib(Resources.BRIDGE_MIB);
            TestMib(Resources.CISCO_AAA_SERVER_MIB);
            TestMib(Resources.CISCO_BULK_FILE_MIB);
            TestMib(Resources.CISCO_CSG_MIB);
            TestMib(Resources.DISMAN_EVENT_MIB);
            TestMib(Resources.DISMAN_EXPRESSION_MIB);
            TestMib(Resources.DISMAN_NSLOOKUP_MIB);
            TestMib(Resources.DISMAN_PING_MIB);
            TestMib(Resources.DISMAN_SCHEDULE_MIB);
            TestMib(Resources.DISMAN_SCRIPT_MIB);
            TestMib(Resources.DISMAN_TRACEROUTE_MIB);
            TestMib(Resources.DMTF_DMI_MIB);
            TestMib(Resources.empty);
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
//            string test = "ADSL-LINE-MIB DEFINITIONS ::= BEGIN" + Environment.NewLine +
//                "IMPORTS" + Environment.NewLine +
//                "MODULE-IDENTITY, OBJECT-TYPE," + Environment.NewLine +
//                "Counter32, Gauge32, Integer32," + Environment.NewLine +
//                "NOTIFICATION-TYPE," + Environment.NewLine +
//                "transmission           FROM SNMPv2-SMI;" + Environment.NewLine +
//                "END";
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
