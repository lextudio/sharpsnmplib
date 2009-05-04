/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/16
 * Time: 21:10
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
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
            ObjectIdentifier table = new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 1, 9 });
            ObjectIdentifier entry = new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 1, 9, 1 });
            ObjectIdentifier unknown = new ObjectIdentifier(new uint[] { 1, 3, 6, 8, 18579, 111111});
            Assert.IsTrue(DefaultObjectRegistry.Instance.ValidateTable(table));
            Assert.IsFalse(DefaultObjectRegistry.Instance.ValidateTable(entry));
            Assert.IsFalse(DefaultObjectRegistry.Instance.ValidateTable(unknown));
        }

        [Test]
        public void TestGetTextualFrom()
        {
            uint[] oid = new uint[] {1};
            string result = DefaultObjectRegistry.Instance.Translate(oid);
            Assert.AreEqual("SNMPV2-SMI::iso", result);
        }
        [Test]
        public void TestGetTextualForm()
        {
            uint[] oid2 = new uint[] {1,3,6,1,2,1,10};
            string result2 = DefaultObjectRegistry.Instance.Translate(oid2);
            Assert.AreEqual("SNMPV2-SMI::transmission", result2);
        }    
        [Test]
        public void TestSNMPv2MIBTextual()
        {
            uint[] oid = new uint[] {1,3,6,1,2,1,1};
            string result = DefaultObjectRegistry.Instance.Translate(oid);
            Assert.AreEqual("SNMPV2-MIB::system", result);
        }
        [Test]
        public void TestSNMPv2TMTextual()
        {
            uint[] old = DefaultObjectRegistry.Instance.Translate("SNMPV2-SMI::snmpDomains");
            string result = DefaultObjectRegistry.Instance.Translate(Definition.AppendTo(old, 1));
            Assert.AreEqual("SNMPV2-TM::snmpUDPDomain", result);
        }
        [Test]
        public void TestGetNumericalFrom()
        {
            uint[] expected = new uint[] {1};
            const string textual = "SNMPV2-SMI::iso";
            uint[] result = DefaultObjectRegistry.Instance.Translate(textual);
            Assert.AreEqual(expected, result);            
        }
        [Test]
        public void TestGetNumericalForm()
        {
            uint[] expected = new uint[] {1,3,6,1,2,1,10};
            const string textual = "SNMPV2-SMI::transmission";
            uint[] result = DefaultObjectRegistry.Instance.Translate(textual);
            Assert.AreEqual(expected, result);    
        }
        
      //  [Test]
        public void TestRFC1155_SMI()
        {
            const string textual = "RFC1155-SMI::private";
            uint[] expected = new uint[] {1,3,6,1,4};
            Assert.AreEqual(expected, DefaultObjectRegistry.Instance.Translate(textual));
            
            Assert.AreEqual("SNMPV2-SMI::private", DefaultObjectRegistry.Instance.Translate(expected));
        }
        [Test]
        public void TestZeroDotZero()
        {
            Assert.AreEqual(new uint[] {0}, DefaultObjectRegistry.Instance.Translate("SNMPV2-SMI::ccitt"));
            const string textual = "SNMPV2-SMI::zeroDotZero";
            uint[] expected = new uint[] {0, 0};
            Assert.AreEqual(textual, DefaultObjectRegistry.Instance.Translate(expected));

            Assert.AreEqual(expected, DefaultObjectRegistry.Instance.Translate(textual));            
        }
        [Test]
        public void TestSNMPv2MIBNumerical()
        {
            uint[] expected = new uint[] {1,3,6,1,2,1,1};
            const string textual = "SNMPV2-MIB::system";
            uint[] result = DefaultObjectRegistry.Instance.Translate(textual);
            Assert.AreEqual(expected, result);
        }
        [Test]
        public void TestSNMPv2TMNumerical()
        {
            uint[] expected = new uint[] {1,3,6,1,6,1,1};
            const string textual = "SNMPV2-TM::snmpUDPDomain";
            uint[] result = DefaultObjectRegistry.Instance.Translate(textual);
            Assert.AreEqual(expected, result);
        }
        [Test]
        public void TestsysORTable()
        {
            const string name = "SNMPV2-MIB::sysORTable";
            uint[] id = DefaultObjectRegistry.Instance.Translate(name);
            Assert.IsTrue(DefaultObjectRegistry.Instance.IsTableId(id));
        }
        [Test]
        public void TestsysORTable0()
        {
            uint[] expected = new uint[] {1,3,6,1,2,1,1,9,0};
            const string name = "SNMPV2-MIB::sysORTable.0";
            uint[] id = DefaultObjectRegistry.Instance.Translate(name);
            Assert.AreEqual(expected, id);
        }
        [Test]
        public void TestsysORTable0Reverse()
        {
            uint[] id = new uint[] {1,3,6,1,2,1,1,9,0};
            const string expected = "SNMPV2-MIB::sysORTable.0";
            string value = DefaultObjectRegistry.Instance.Translate(id);
            Assert.AreEqual(expected, value);
        }    
        [Test]
        public void TestsnmpMIB()
        {
            const string name = "SNMPV2-MIB::snmpMIB";
            uint[] id = DefaultObjectRegistry.Instance.Translate(name);
            Assert.IsFalse(DefaultObjectRegistry.Instance.IsTableId(id));
        }
        
        [Test]
        public void TestActona()
        {
            const string name = "ACTONA-ACTASTOR-MIB::actona";
            IList<IModule> modules = Parser.Compile(new StreamReader(new MemoryStream(Resources.ACTONA_ACTASTOR_MIB)));
            DefaultObjectRegistry.Instance.Import(modules);
            DefaultObjectRegistry.Instance.Refresh();
            uint[] id = DefaultObjectRegistry.Instance.Translate(name);
            
            Assert.AreEqual(new uint[] {1, 3, 6, 1, 4, 1, 17471}, id);
            Assert.AreEqual("ACTONA-ACTASTOR-MIB::actona", DefaultObjectRegistry.Instance.Translate(id));
        }
        
       // [Test]
        public void TestSYMMIB_MIB_MIB()
        {
            const string name = "SYMMIB_MIB-MIB::symbios_3_1";
            IList<IModule> modules = Parser.Compile(new StreamReader(new MemoryStream(Resources.SYMMIB_MIB_MIB)));
            DefaultObjectRegistry.Instance.Import(modules);
            modules = Parser.Compile(new StreamReader(new MemoryStream(Resources.DMTF_DMI_MIB)));
            DefaultObjectRegistry.Instance.Import(modules);
            DefaultObjectRegistry.Instance.Refresh();
            uint[] id = DefaultObjectRegistry.Instance.Translate(name);
            
            Assert.AreEqual(new uint[] {1, 3, 6, 1, 4, 1, 1123, 3, 1}, id);
            Assert.AreEqual(name, DefaultObjectRegistry.Instance.Translate(id));
        }
        
       //[Test]
        public void TestIEEE802dot11_MIB()
        {
            const string name = "IEEE802DOT11-MIB::dot11SMTnotification";
            IList<IModule> modules = Parser.Compile(new StreamReader(new MemoryStream(Resources.IEEE802DOT11_MIB)));
            DefaultObjectRegistry.Instance.Import(modules);
            DefaultObjectRegistry.Instance.Refresh();
            uint[] id = DefaultObjectRegistry.Instance.Translate(name);
            
            Assert.AreEqual(new uint[] {1, 2, 840, 10036, 1, 6}, id);
            Assert.AreEqual("IEEE802DOT11-MIB::dot11SMTnotification", DefaultObjectRegistry.Instance.Translate(id));
        
            const string name1 = "IEEE802DOT11-MIB::dot11Disassociate";
            uint[] id1 = new uint[] {1, 2, 840, 10036, 1, 6, 0, 1};
            Assert.AreEqual(id1, DefaultObjectRegistry.Instance.Translate(name1));
            Assert.AreEqual(name1, DefaultObjectRegistry.Instance.Translate(id1));
        }
    }
}
#pragma warning restore 1591

