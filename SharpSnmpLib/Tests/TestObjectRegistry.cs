/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/16
 * Time: 21:10
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using System.IO;
using Lextm.SharpSnmpLib.Mib;
#pragma warning disable 1591
namespace Lextm.SharpSnmpLib.Tests
{
	[TestFixture]
	public class TestObjectRegistry
	{
        ObjectRegistry registry;
        [SetUp]
        public void SetUp()
        {
            registry = new ObjectRegistry();
//            registry.LoadFolder(Environment.GetFolderPath(
//            	Environment.SpecialFolder.System), "*.mib");
			MemoryStream m = new MemoryStream(Resource.SNMPv2_SMI);
			registry.LoadFile(new StreamReader(m));
			m = new MemoryStream(Resource.SNMPv2_CONF);
			registry.LoadFile(new StreamReader(m));
			m = new MemoryStream(Resource.SNMPv2_TC);
			registry.LoadFile(new StreamReader(m));
			m = new MemoryStream(Resource.SNMPv2_MIB);
			registry.LoadFile(new StreamReader(m));
			m = new MemoryStream(Resource.SNMPv2_TM);
			registry.LoadFile(new StreamReader(m));
			
        }
		[Test]
		public void TestGetTextualFrom()
		{
			uint[] oid = new uint[] {1};
			string result = registry.GetTextualFrom(oid);
            Assert.AreEqual("SNMPv2-SMI::iso", result);
		}
		[Test]
		public void TestGetTextualForm()
		{
            uint[] oid2 = new uint[] {1,3,6,1,2,1,10};
			string result2 = registry.GetTextualFrom(oid2);
            Assert.AreEqual("SNMPv2-SMI::transmission", result2);
		}	
		[Test]
		public void TestSNMPv2MIBTextual()
		{
			uint[] oid = new uint[] {1,3,6,1,2,1,1};
			string result = registry.GetTextualFrom(oid);
			Assert.AreEqual("SNMPv2-MIB::system", result);
		}
		[Test]
		public void TestSNMPv2TMTextual()
		{
			uint[] old = registry.GetNumericalFrom("SNMPv2-SMI::snmpDomains");
			string result = registry.GetTextualFrom(Definition.GetChildId(old, 1));
			Assert.AreEqual("SNMPv2-TM::snmpUDPDomain", result);
		}
		[Test]
		public void TestGetNumericalFrom()
		{
			uint[] expected = new uint[] {1};
            string textual = "SNMPv2-SMI::iso";
			uint[] result = registry.GetNumericalFrom(textual);
			Assert.AreEqual(expected, result);			
		}
		[Test]
		public void TestGetNumericalForm()
		{
			uint[] expected = new uint[] {1,3,6,1,2,1,10};
            string textual = "SNMPv2-SMI::transmission";
			uint[] result = registry.GetNumericalFrom(textual);
			Assert.AreEqual(expected, result);	
		}
		[Test]
		public void TestSNMPv2MIBNumerical()
		{
			uint[] expected = new uint[] {1,3,6,1,2,1,1};
			string textual = "SNMPv2-MIB::system";
			uint[] result = registry.GetNumericalFrom(textual);
			Assert.AreEqual(expected, result);
		}
		[Test]
		public void TestSNMPv2TMNumerical()
		{
			uint[] expected = new uint[] {1,3,6,1,6,1,1};
			string textual = "SNMPv2-TM::snmpUDPDomain";
			uint[] result = registry.GetNumericalFrom(textual);
			Assert.AreEqual(expected, result);
		}
	}
}
#pragma warning restore 1591