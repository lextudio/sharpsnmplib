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
        public void TestValidateTable()
        {
            ObjectIdentifier table = new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 2, 2 });
            ObjectIdentifier entry = new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 2, 2, 1 });
            ObjectIdentifier unknown = new ObjectIdentifier(new uint[] { 1, 3, 6, 8, 18579, 111111});
            Assert.IsTrue(ObjectRegistry.ValidateTable(table));
            Assert.IsFalse(ObjectRegistry.ValidateTable(entry));
            Assert.IsTrue(ObjectRegistry.ValidateTable(unknown));
        }
        [Test]
        public void TestGetTextualFrom()
        {
            uint[] oid = new uint[] {1};
            string result = ObjectRegistry.Instance.Translate(oid);
            Assert.AreEqual("SNMPV2-SMI::iso", result);
        }
        [Test]
        public void TestGetTextualForm()
        {
            uint[] oid2 = new uint[] {1,3,6,1,2,1,10};
            string result2 = ObjectRegistry.Instance.Translate(oid2);
            Assert.AreEqual("SNMPV2-SMI::transmission", result2);
        }    
        [Test]
        public void TestSNMPv2MIBTextual()
        {
            uint[] oid = new uint[] {1,3,6,1,2,1,1};
            string result = ObjectRegistry.Instance.Translate(oid);
            Assert.AreEqual("SNMPV2-MIB::system", result);
        }
        [Test]
        public void TestSNMPv2TMTextual()
        {
            uint[] old = ObjectRegistry.Instance.Translate("SNMPV2-SMI::snmpDomains");
            string result = ObjectRegistry.Instance.Translate(Definition.AppendTo(old, 1));
            Assert.AreEqual("SNMPV2-TM::snmpUDPDomain", result);
        }
        [Test]
        public void TestGetNumericalFrom()
        {
            uint[] expected = new uint[] {1};
            string textual = "SNMPV2-SMI::iso";
            uint[] result = ObjectRegistry.Instance.Translate(textual);
            Assert.AreEqual(expected, result);            
        }
        [Test]
        public void TestGetNumericalForm()
        {
            uint[] expected = new uint[] {1,3,6,1,2,1,10};
            string textual = "SNMPV2-SMI::transmission";
            uint[] result = ObjectRegistry.Instance.Translate(textual);
            Assert.AreEqual(expected, result);    
        }
        
        [Test]
        public void TestRFC1155_SMI()
        {
            string textual = "RFC1155-SMI::private";
            uint[] expected = new uint[] {1,3,6,1,4};
            Assert.AreEqual(expected, ObjectRegistry.Instance.Translate(textual));
            
            Assert.AreEqual("SNMPV2-SMI::private", ObjectRegistry.Instance.Translate(expected));
        }
        [Test]
        public void TestZeroDotZero()
        {
            Assert.AreEqual(new uint[] {0}, ObjectRegistry.Instance.Translate("SNMPV2-SMI::ccitt"));
            string textual = "SNMPV2-SMI::zeroDotZero";
            uint[] expected = new uint[] {0, 0};
            Assert.AreEqual(textual, ObjectRegistry.Instance.Translate(expected));

            Assert.AreEqual(expected, ObjectRegistry.Instance.Translate(textual));            
        }
        [Test]
        public void TestSNMPv2MIBNumerical()
        {
            uint[] expected = new uint[] {1,3,6,1,2,1,1};
            string textual = "SNMPV2-MIB::system";
            uint[] result = ObjectRegistry.Instance.Translate(textual);
            Assert.AreEqual(expected, result);
        }
        [Test]
        public void TestSNMPv2TMNumerical()
        {
            uint[] expected = new uint[] {1,3,6,1,6,1,1};
            string textual = "SNMPV2-TM::snmpUDPDomain";
            uint[] result = ObjectRegistry.Instance.Translate(textual);
            Assert.AreEqual(expected, result);
        }
        [Test]
        public void TestsysORTable()
        {
            string name = "SNMPV2-MIB::sysORTable";
            uint[] id = ObjectRegistry.Instance.Translate(name);
            Assert.IsTrue(ObjectRegistry.Instance.IsTableId(id));
        }
        [Test]
        public void TestsysORTable0()
        {
            uint[] expected = new uint[] {1,3,6,1,2,1,1,9,0};
            string name = "SNMPV2-MIB::sysORTable.0";
            uint[] id = ObjectRegistry.Instance.Translate(name);
            Assert.AreEqual(expected, id);
        }
        [Test]
        public void TestsysORTable0Reverse()
        {
            uint[] id = new uint[] {1,3,6,1,2,1,1,9,0};
            string expected = "SNMPV2-MIB::sysORTable.0";
            string value = ObjectRegistry.Instance.Translate(id);
            Assert.AreEqual(expected, value);
        }    
        [Test]
        public void TestsnmpMIB()
        {
            string name = "SNMPV2-MIB::snmpMIB";
            uint[] id = ObjectRegistry.Instance.Translate(name);
            Assert.IsFalse(ObjectRegistry.Instance.IsTableId(id));
        }
        
        [Test]
        public void TestActona()
        {
            string name = "ACTONA-ACTASTOR-MIB::actona";
            var modules = Compiler.Compile(new StreamReader(new MemoryStream(Resource.ACTONA_ACTASTOR_MIB)));
            ObjectRegistry.Instance.Import(modules);
            ObjectRegistry.Instance.Refresh();
            uint[] id = ObjectRegistry.Instance.Translate(name);
            
            Assert.AreEqual(new uint[] {1, 3, 6, 1, 4, 1, 17471}, id);
            Assert.AreEqual("ACTONA-ACTASTOR-MIB::actona", ObjectRegistry.Instance.Translate(id));
        }
        
        [Test]
        public void TestSYMMIB_MIB_MIB()
        {
            string name = "SYMMIB_MIB-MIB::symbios_3_1";
            var modules = Compiler.Compile(new StreamReader(new MemoryStream(Resource.SYMMIB_MIB_MIB)));
            ObjectRegistry.Instance.Import(modules);
            modules = Compiler.Compile(new StreamReader(new MemoryStream(Resource.DMTF_DMI_MIB)));
            ObjectRegistry.Instance.Import(modules);
            ObjectRegistry.Instance.Refresh();
            uint[] id = ObjectRegistry.Instance.Translate(name);
            
            Assert.AreEqual(new uint[] {1, 3, 6, 1, 4, 1, 1123, 3, 1}, id);
            Assert.AreEqual(name, ObjectRegistry.Instance.Translate(id));
        }
        
        [Test]
        public void TestIEEE802dot11_MIB()
        {
            string name = "IEEE802DOT11-MIB::dot11SMTnotification";
            var modules = Compiler.Compile(new StreamReader(new MemoryStream(Resource.IEEE802DOT11_MIB)));
            ObjectRegistry.Instance.Import(modules);
            ObjectRegistry.Instance.Refresh();
            uint[] id = ObjectRegistry.Instance.Translate(name);
            
            Assert.AreEqual(new uint[] {1, 2, 840, 10036, 1, 6}, id);
            Assert.AreEqual("IEEE802DOT11-MIB::dot11SMTnotification", ObjectRegistry.Instance.Translate(id));
        
            string name1 = "IEEE802DOT11-MIB::dot11Disassociate";
            uint[] id1 = new uint[] {1, 2, 840, 10036, 1, 6, 0, 1};
            Assert.AreEqual(id1, ObjectRegistry.Instance.Translate(name1));
            Assert.AreEqual(name1, ObjectRegistry.Instance.Translate(id1));
        }
    }
}
#pragma warning restore 1591

