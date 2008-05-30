/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/17
 * Time: 17:43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using Lextm.SharpSnmpLib.Mib;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using System.IO;
#pragma warning disable 1591
namespace Lextm.SharpSnmpLib.Tests
{
	[TestFixture]
	public class TestMibDocument
	{		
		[Test]
		public void TestModule()
		{
			Lexer lexer = new Lexer();
        	MemoryStream m = new MemoryStream(Resource.SNMPv2_SMI);
        	using (StreamReader reader = new StreamReader(m))
        	{
        		lexer.Parse(reader);
        		reader.Close();
        	}
			MibDocument file = new MibDocument(lexer);
			Assert.AreEqual(1, file.Modules.Count);
			Assert.AreEqual("SNMPv2-SMI", file.Modules[0].Name);
			Assert.AreEqual(16, file.Modules[0].EntityNodes.Count);
			IEntity node = file.Modules[0].EntityNodes[0];
			Assert.AreEqual("org", node.Name);
			Assert.AreEqual(3, node.Value);
			Assert.AreEqual("iso", node.Parent.ToString());
		}
		
		[Test]
		public void TestModule2()
		{
			Lexer lexer = new Lexer();
        	MemoryStream m = new MemoryStream(Resource.SNMPv2_MIB);
        	using (StreamReader reader = new StreamReader(m))
        	{
        		lexer.Parse(reader);
        		reader.Close();
        	}
			MibDocument file = new MibDocument(lexer);
			Assert.AreEqual("SNMPv2-MIB", file.Modules[0].Name);
			Assert.AreEqual(3, file.Modules[0].Dependents.Count);
			Assert.AreEqual(70, file.Modules[0].EntityNodes.Count);
			IEntity node = file.Modules[0].EntityNodes[69];
			Assert.AreEqual("snmpObsoleteGroup", node.Name);
			Assert.AreEqual(10, node.Value);
			Assert.AreEqual("snmpMIBGroups", node.Parent.ToString());
		}		
		[Test]
		public void Testv2TM()
		{
			Lexer lexer = new Lexer();
        	MemoryStream m = new MemoryStream(Resource.SNMPv2_TM);
        	using (StreamReader reader = new StreamReader(m))
        	{
        		lexer.Parse(reader);
        		reader.Close();
        	}
			MibDocument file = new MibDocument(lexer);
			Assert.AreEqual("SNMPv2-TM", file.Modules[0].Name);
			Assert.AreEqual(2, file.Modules[0].Dependents.Count);
			Assert.AreEqual(8, file.Modules[0].EntityNodes.Count);
			IEntity node = file.Modules[0].EntityNodes[7];
			Assert.AreEqual("rfc1157Domain", node.Name);
			Assert.AreEqual(1, node.Value);
			Assert.AreEqual("rfc1157Proxy", node.Parent.ToString());
		}
	}
}
#pragma warning restore 1591