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
        [Test]
        public void TestGetTextualFrom()
        {
            uint[] oid = new uint[] {1};
            string result = ObjectRegistry.Instance.GetTextualFrom(oid);
            Assert.AreEqual("SNMPv2-SMI::iso", result);
        }
        [Test]
        public void TestGetTextualForm()
        {
            uint[] oid2 = new uint[] {1,3,6,1,2,1,10};
            string result2 = ObjectRegistry.Instance.GetTextualFrom(oid2);
            Assert.AreEqual("SNMPv2-SMI::transmission", result2);
        }    
        [Test]
        public void TestSNMPv2MIBTextual()
        {
            uint[] oid = new uint[] {1,3,6,1,2,1,1};
            string result = ObjectRegistry.Instance.GetTextualFrom(oid);
            Assert.AreEqual("SNMPv2-MIB::system", result);
        }
        [Test]
        public void TestSNMPv2TMTextual()
        {
            uint[] old = ObjectRegistry.Instance.GetNumericalFrom("SNMPv2-SMI::snmpDomains");
            string result = ObjectRegistry.Instance.GetTextualFrom(Definition.AppendTo(old, 1));
            Assert.AreEqual("SNMPv2-TM::snmpUDPDomain", result);
        }
        [Test]
        public void TestGetNumericalFrom()
        {
            uint[] expected = new uint[] {1};
            string textual = "SNMPv2-SMI::iso";
            uint[] result = ObjectRegistry.Instance.GetNumericalFrom(textual);
            Assert.AreEqual(expected, result);            
        }
        [Test]
        public void TestGetNumericalForm()
        {
            uint[] expected = new uint[] {1,3,6,1,2,1,10};
            string textual = "SNMPv2-SMI::transmission";
            uint[] result = ObjectRegistry.Instance.GetNumericalFrom(textual);
            Assert.AreEqual(expected, result);    
        }
        [Test]
        public void TestRFC1155_SMI()
        {
            string textual = "RFC1155-SMI::private";
            uint[] expected = new uint[] {1,3,6,1,4};
            Assert.AreEqual(expected, ObjectRegistry.Instance.GetNumericalFrom(textual));
            
            Assert.AreEqual("SNMPv2-SMI::private", ObjectRegistry.Instance.GetTextualFrom(expected));
        }
        [Test]
        public void TestZeroDotZero()
        {
            Assert.AreEqual(new uint[] {0}, ObjectRegistry.Instance.GetNumericalFrom("SNMPv2-SMI::ccitt"));
            string textual = "SNMPv2-SMI::zeroDotZero";
            uint[] expected = new uint[] {0, 0};
            Assert.AreEqual(textual, ObjectRegistry.Instance.GetTextualFrom(expected));

            Assert.AreEqual(expected, ObjectRegistry.Instance.GetNumericalFrom(textual));            
        }
        [Test]
        public void TestSNMPv2MIBNumerical()
        {
            uint[] expected = new uint[] {1,3,6,1,2,1,1};
            string textual = "SNMPv2-MIB::system";
            uint[] result = ObjectRegistry.Instance.GetNumericalFrom(textual);
            Assert.AreEqual(expected, result);
        }
        [Test]
        public void TestSNMPv2TMNumerical()
        {
            uint[] expected = new uint[] {1,3,6,1,6,1,1};
            string textual = "SNMPv2-TM::snmpUDPDomain";
            uint[] result = ObjectRegistry.Instance.GetNumericalFrom(textual);
            Assert.AreEqual(expected, result);
        }
        [Test]
        public void TestsysORTable()
        {
            string name = "SNMPv2-MIB::sysORTable";
            uint[] id = ObjectRegistry.Instance.GetNumericalFrom(name);
            Assert.IsTrue(ObjectRegistry.Instance.IsTableId(id));
        }
        [Test]
        public void TestsysORTable0()
        {
            uint[] expected = new uint[] {1,3,6,1,2,1,1,9,0};
            string name = "SNMPv2-MIB::sysORTable.0";
            uint[] id = ObjectRegistry.Instance.GetNumericalFrom(name);
            Assert.AreEqual(expected, id);
        }
        [Test]
        public void TestsysORTable0Reverse()
        {
            uint[] id = new uint[] {1,3,6,1,2,1,1,9,0};
            string expected = "SNMPv2-MIB::sysORTable.0";
            string value = ObjectRegistry.Instance.GetTextualFrom(id);
            Assert.AreEqual(expected, value);
        }    
        [Test]
        public void TestsnmpMIB()
        {
            string name = "SNMPv2-MIB::snmpMIB";
            uint[] id = ObjectRegistry.Instance.GetNumericalFrom(name);
            Assert.IsFalse(ObjectRegistry.Instance.IsTableId(id));
        }
    }
}
#pragma warning restore 1591

