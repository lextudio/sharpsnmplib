/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/17
 * Time: 17:43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using Lextm.SharpSnmpLib.Mib;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

#pragma warning disable 1591
namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class TestMibDocument
    {
        [Test]
        public void TestBASEBRDD_MIB_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.BASEBRDD_MIB_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual("BASEBRDD_MIB-MIB", file.Modules[0].Name);
            Assert.AreEqual(5, file.Modules[0].Dependents.Count);
            Assert.AreEqual(607, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[606];
            Assert.AreEqual("dMTFVoltageProbeEvSub", node.Name);
            Assert.AreEqual(7, node.Value);
            Assert.AreEqual("dMTFVoltageProbeTable", node.Parent.ToString());
        }
        
        [Test]
        public void TestATM_TC__MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.ATM_TC_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual("ATM-TC-MIB", file.Modules[0].Name);
            Assert.AreEqual(2, file.Modules[0].Dependents.Count);
            Assert.AreEqual(11, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[10];
            Assert.AreEqual("atmClpTaggingScrCdvt", node.Name);
            Assert.AreEqual(15, node.Value);
            Assert.AreEqual("atmTrafficDescriptorTypes", node.Parent.ToString());
        }
        
        [Test]
        public void TestARROWPOINT_IPV4_OSPF_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.ARROWPOINT_IPV4_OSPF_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual("ARROWPOINT-IPV4-OSPF-MIB", file.Modules[0].Name);
            Assert.AreEqual(5, file.Modules[0].Dependents.Count);
            Assert.AreEqual(50, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[49];
            Assert.AreEqual("apIpv4OspfCompliance", node.Name);
            Assert.AreEqual(1, node.Value);
            Assert.AreEqual("apIpv4OspfCompliances", node.Parent.ToString());
        }
        
        [Test]
        public void TestAPPC_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.APPC_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual("APPC-MIB", file.Modules[0].Name);
            Assert.AreEqual(4, file.Modules[0].Dependents.Count);
            Assert.AreEqual(305, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[304];
            Assert.AreEqual("appcConversationConfGroup", node.Name);
            Assert.AreEqual(10, node.Value);
            Assert.AreEqual("appcGroups", node.Parent.ToString());
        }
        
        [Test]
        public void TestALVARION_DOT11_WLAN_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.ALVARION_DOT11_WLAN_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual("ALVARION-DOT11-WLAN-MIB", file.Modules[0].Name);
            Assert.AreEqual(2, file.Modules[0].Dependents.Count);
            Assert.AreEqual(269, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[268];
            Assert.AreEqual("brzaccVLTrapFtpStatus", node.Name);
            Assert.AreEqual(10, node.Value);
            Assert.AreEqual("brzaccVLTraps", node.Parent.ToString());
        }
        
        [Test]
        public void TestARRAYMANAGER_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.ARRAYMANAGER_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual("ArrayManager-MIB", file.Modules[0].Name);
            Assert.AreEqual(4, file.Modules[0].Dependents.Count);
            Assert.AreEqual(380, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[379];
            Assert.AreEqual("rebuildRateEv", node.Name);
            Assert.AreEqual(222, node.Value);
            Assert.AreEqual("aryMgrEvts", node.Parent.ToString());
        }
        
        [Test]
        public void TestAIRPORT_BASESTATION_3_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.AIRPORT_BASESTATION_3_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual("AIRPORT-BASESTATION-3-MIB", file.Modules[0].Name);
            Assert.AreEqual(3, file.Modules[0].Dependents.Count);
            Assert.AreEqual(47, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[46];
            Assert.AreEqual("physicalInterfaceNumRXError", node.Name);
            Assert.AreEqual(10, node.Value);
            Assert.AreEqual("physicalInterface", node.Parent.ToString());
        }
        
        [Test]
        public void TestALLIEDTELESYN_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.ALLIEDTELESYN_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual("ALLIEDTELESYN-MIB", file.Modules[0].Name);
            Assert.AreEqual(4, file.Modules[0].Dependents.Count);
            Assert.AreEqual(606, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[605];
            Assert.AreEqual("ds3TrapInterval", node.Name);
            Assert.AreEqual(4, node.Value);
            Assert.AreEqual("ds3TrapEntry", node.Parent.ToString());
        }
        
        [Test]
        public void TestADSL_TC_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.ADSL_TC_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual("ADSL-TC-MIB", file.Modules[0].Name);
            Assert.AreEqual(2, file.Modules[0].Dependents.Count);
            Assert.AreEqual(1, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[0];
            Assert.AreEqual("adsltcmib", node.Name);
            Assert.AreEqual(2, node.Value);
            Assert.AreEqual("transmission.94", node.Parent.ToString());
        }
        
        [Test]
        public void TestADSL_LINE_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.ADSL_LINE_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual("ADSL-LINE-MIB", file.Modules[0].Name);
            Assert.AreEqual(6, file.Modules[0].Dependents.Count);
            Assert.AreEqual(275, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[274];
            Assert.AreEqual("adslAturLineProfileControlGroup", node.Name);
            Assert.AreEqual(25, node.Value);
            Assert.AreEqual("adslGroups", node.Parent.ToString());
        }
        
        [Test]
        public void TestACTONA_ACTASTOR_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.ACTONA_ACTASTOR_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual("ACTONA-ACTASTOR-MIB", file.Modules[0].Name);
            Assert.AreEqual(3, file.Modules[0].Dependents.Count);
            Assert.AreEqual(100, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[99];
            Assert.AreEqual("acNotificationInfoGroup", node.Name);
            Assert.AreEqual(7, node.Value);
            Assert.AreEqual("actastorGroups", node.Parent.ToString());
        }
        
        [Test]
        [ExpectedException(typeof(SharpMibException))]
        public void TestException()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.fivevarbinds);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse("temp.txt", reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
        }
        [Test]
        public void TestRFC1155_SMI()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.RFC1155_SMI);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("RFC1155-SMI", file.Modules[0].Name);
            Assert.AreEqual(6, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[0];
            Assert.AreEqual("internet", node.Name);
            Assert.AreEqual(1, node.Value);
            Assert.AreEqual("dod", node.Parent.ToString());
        }
        [Test]
        public void TestRFC1271_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.RFC1271_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("RFC1271-MIB", file.Modules[0].Name);
            Assert.AreEqual(213, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[212];
            Assert.AreEqual("logDescription", node.Name);
            Assert.AreEqual(4, node.Value);
            Assert.AreEqual("logEntry", node.Parent.ToString());
        }
        [Test]
        public void TestRFC1213_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.RFC1213_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("RFC1213-MIB", file.Modules[0].Name);
            Assert.AreEqual(201, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[200];
            Assert.AreEqual("snmpEnableAuthenTraps", node.Name);
            Assert.AreEqual(30, node.Value);
            Assert.AreEqual("snmp", node.Parent.ToString());
        }

        [Test]
        public void TestRFC1213_MIB2()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.RFC1213_MIB2);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("RFC1213-MIB", file.Modules[0].Name);
            Assert.AreEqual(206, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[205];
            Assert.AreEqual("snmpEnableAuthenTraps", node.Name);
            Assert.AreEqual(30, node.Value);
            Assert.AreEqual("snmp", node.Parent.ToString());
        }
        [Test]
        public void TestRFC_1215()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.RFC_1215);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("RFC-1215", file.Modules[0].Name);
            Assert.AreEqual(0, file.Modules[0].Entities.Count);
        }
        [Test]
        public void TestRMON_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.RMON_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("RMON-MIB", file.Modules[0].Name);
            Assert.AreEqual(232, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[231];
            Assert.AreEqual("rmonNotificationGroup", node.Name);
            Assert.AreEqual(11, node.Value);
            Assert.AreEqual("rmonGroups", node.Parent.ToString());
        }
        [Test]
        public void TestSMUX_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.SMUX_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("SMUX-MIB", file.Modules[0].Name);
            Assert.AreEqual(14, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[13];
            Assert.AreEqual("smuxTstatus", node.Name);
            Assert.AreEqual(4, node.Value);
            Assert.AreEqual("smuxTreeEntry", node.Parent.ToString());
        }
        [Test]
        public void TestSNMP_VIEW_BASED_ACM_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.SNMP_VIEW_BASED_ACM_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("SNMP-VIEW-BASED-ACM-MIB", file.Modules[0].Name);
            Assert.AreEqual(38, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[37];
            Assert.AreEqual("vacmBasicGroup", node.Name);
            Assert.AreEqual(1, node.Value);
            Assert.AreEqual("vacmMIBGroups", node.Parent.ToString());
        }
        [Test]
        public void TestTCP_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.TCP_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("TCP-MIB", file.Modules[0].Name);
            Assert.AreEqual(51, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[50];
            Assert.AreEqual("tcpHCGroup", node.Name);
            Assert.AreEqual(5, node.Value);
            Assert.AreEqual("tcpMIBGroups", node.Parent.ToString());
        }
        [Test]
        public void TestTransport_Address_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.TRANSPORT_ADDRESS_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("TRANSPORT-ADDRESS-MIB", file.Modules[0].Name);
            Assert.AreEqual(18, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[17];
            Assert.AreEqual("transportDomainSctpDns", node.Name);
            Assert.AreEqual(16, node.Value);
            Assert.AreEqual("transportDomains", node.Parent.ToString());
        }
        [Test]
        public void TestTunnel_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.TUNNEL_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("TUNNEL-MIB", file.Modules[0].Name);
            Assert.AreEqual(42, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[41];
            Assert.AreEqual("tunnelMIBInetGroup", node.Name);
            Assert.AreEqual(2, node.Value);
            Assert.AreEqual("tunnelMIBGroups", node.Parent.ToString());
        }
        [Test]
        public void TestUCD_DEMO_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.UCD_DEMO_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("UCD-DEMO-MIB", file.Modules[0].Name);
            Assert.AreEqual(7, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[6];
            Assert.AreEqual("ucdDemoPassphrase", node.Name);
            Assert.AreEqual(4, node.Value);
            Assert.AreEqual("ucdDemoPublic", node.Parent.ToString());
        }
        [Test]
        public void TestUCD_DISKIO_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.UCD_DISKIO_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("UCD-DISKIO-MIB", file.Modules[0].Name);
            Assert.AreEqual(14, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[13];
            Assert.AreEqual("diskIONWrittenX", node.Name);
            Assert.AreEqual(13, node.Value);
            Assert.AreEqual("diskIOEntry", node.Parent.ToString());
        }
        [Test]
        public void TestUCD_DLMOD_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.UCD_DLMOD_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("UCD-DLMOD-MIB", file.Modules[0].Name);
            Assert.AreEqual(9, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[8];
            Assert.AreEqual("dlmodStatus", node.Name);
            Assert.AreEqual(5, node.Value);
            Assert.AreEqual("dlmodEntry", node.Parent.ToString());
        }
        [Test]
        public void TestUCD_IPFILTER_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.UCD_IPFILTER_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("UCD-IPFILTER-MIB", file.Modules[0].Name);
            Assert.AreEqual(23, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[22];
            Assert.AreEqual("ipfAccOutBytes", node.Name);
            Assert.AreEqual(4, node.Value);
            Assert.AreEqual("ipfAccOutEntry", node.Parent.ToString());
        }
        [Test]
        public void TestUCD_IPFWACC_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.UCD_IPFWACC_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("UCD-IPFWACC-MIB", file.Modules[0].Name);
            Assert.AreEqual(29, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[28];
            Assert.AreEqual("ipFwAccPort10", node.Name);
            Assert.AreEqual(26, node.Value);
            Assert.AreEqual("ipFwAccEntry", node.Parent.ToString());
        }
        [Test]
        public void TestUCD_SNMP_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.UCD_SNMP_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("UCD-SNMP-MIB", file.Modules[0].Name);
            Assert.AreEqual(158, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[157];
            Assert.AreEqual("logMatchRegExCompilation", node.Name);
            Assert.AreEqual(101, node.Value);
            Assert.AreEqual("logMatchEntry", node.Parent.ToString());
        }
        [Test]
        public void TestUCD_SNMP_MIB_OLD()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.UCD_SNMP_MIB_OLD);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("UCD-SNMP-MIB-OLD", file.Modules[0].Name);
            Assert.AreEqual(35, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[34];
            Assert.AreEqual("loadaveErrMessage", node.Name);
            Assert.AreEqual(101, node.Value);
            Assert.AreEqual("loadaves", node.Parent.ToString());
        }
        [Test]
        public void TestUDP_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.UDP_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("UDP-MIB", file.Modules[0].Name);
            Assert.AreEqual(31, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[30];
            Assert.AreEqual("udpEndpointGroup", node.Name);
            Assert.AreEqual(4, node.Value);
            Assert.AreEqual("udpMIBGroups", node.Parent.ToString());
        }
        [Test]
        public void TestMTA_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.MTA_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("MTA-MIB", file.Modules[0].Name);
            Assert.AreEqual(81, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[80];
            Assert.AreEqual("mtaRFC2789ErrorGroup", node.Name);
            Assert.AreEqual(9, node.Value);
            Assert.AreEqual("mtaGroups", node.Parent.ToString());
        }
        [Test]
        public void TestNet_Snmp_Agent_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.NET_SNMP_AGENT_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("NET-SNMP-AGENT-MIB", file.Modules[0].Name);
            Assert.AreEqual(54, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[53];
            Assert.AreEqual("nsAgentNotifyGroup", node.Name);
            Assert.AreEqual(9, node.Value);
            Assert.AreEqual("netSnmpGroups", node.Parent.ToString());
        }
        [Test]
        public void TestNet_Snmp_Examples_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.NET_SNMP_EXAMPLES_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("NET-SNMP-EXAMPLES-MIB", file.Modules[0].Name);
            Assert.AreEqual(25, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[24];
            Assert.AreEqual("netSnmpExampleNotification", node.Name);
            Assert.AreEqual(1, node.Value);
            Assert.AreEqual("netSnmpExampleNotifications", node.Parent.ToString());
        }
        [Test]
        public void TestNet_Snmp_Extend_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.NET_SNMP_EXTEND_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("NET-SNMP-EXTEND-MIB", file.Modules[0].Name);
            Assert.AreEqual(27, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[26];
            Assert.AreEqual("nsExtendOutputGroup", node.Name);
            Assert.AreEqual(2, node.Value);
            Assert.AreEqual("nsExtendGroups", node.Parent.ToString());
        }
        [Test]
        public void TestNet_Snmp_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.NET_SNMP_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("NET-SNMP-MIB", file.Modules[0].Name);
            Assert.AreEqual(14, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[13];
            Assert.AreEqual("netSnmpGroups", node.Name);
            Assert.AreEqual(2, node.Value);
            Assert.AreEqual("netSnmpConformance", node.Parent.ToString());
        }
        [Test]
        public void TestNet_Snmp_Monitor_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.NET_SNMP_MONITOR_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("NET-SNMP-MONITOR-MIB", file.Modules[0].Name);
            Assert.AreEqual(5, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[4];
            Assert.AreEqual("nsLog", node.Name);
            Assert.AreEqual(24, node.Value);
            Assert.AreEqual("netSnmpObjects", node.Parent.ToString());
        }
        [Test]
        public void TestNet_Snmp_System_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.NET_SNMP_SYSTEM_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("NET-SNMP-SYSTEM-MIB", file.Modules[0].Name);
            Assert.AreEqual(6, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[5];
            Assert.AreEqual("nsDiskIO", node.Name);
            Assert.AreEqual(35, node.Value);
            Assert.AreEqual("netSnmpObjects", node.Parent.ToString());
        }
        [Test]
        public void TestNet_Snmp_TC()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.NET_SNMP_TC);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("NET-SNMP-TC", file.Modules[0].Name);
            Assert.AreEqual(24, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[23];
            Assert.AreEqual("netSnmpCallbackDomain", node.Name);
            Assert.AreEqual(6, node.Value);
            Assert.AreEqual("netSnmpDomains", node.Parent.ToString());
        }
        [Test]
        public void TestNet_Snmp_VACM_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.NET_SNMP_VACM_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("NET-SNMP-VACM-MIB", file.Modules[0].Name);
            Assert.AreEqual(8, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[7];
            Assert.AreEqual("nsVacmStatus", node.Name);
            Assert.AreEqual(5, node.Value);
            Assert.AreEqual("nsVacmAccessEntry", node.Parent.ToString());
        }
        [Test]
        public void TestNetwork_Service_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.NETWORK_SERVICES_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("NETWORK-SERVICES-MIB", file.Modules[0].Name);
            Assert.AreEqual(44, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[43];
            Assert.AreEqual("applUDPProtoID", node.Name);
            Assert.AreEqual(5, node.Value);
            Assert.AreEqual("application", node.Parent.ToString());
        }
        [Test]
        public void TestNotification_Log_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.NOTIFICATION_LOG_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("NOTIFICATION-LOG-MIB", file.Modules[0].Name);
            Assert.AreEqual(55, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[54];
            Assert.AreEqual("notificationLogDateGroup", node.Name);
            Assert.AreEqual(4, node.Value);
            Assert.AreEqual("notificationLogMIBGroups", node.Parent.ToString());
        }
        [Test]
        public void TestIPv6_Flow_Label_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.IPV6_FLOW_LABEL_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("IPV6-FLOW-LABEL-MIB", file.Modules[0].Name);
            Assert.AreEqual(1, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[0];
            Assert.AreEqual("ipv6FlowLabelMIB", node.Name);
            Assert.AreEqual(103, node.Value);
            Assert.AreEqual("mib-2", node.Parent.ToString());
        }
        [Test]
        public void TestIPv6_ICMP_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.IPV6_ICMP_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("IPV6-ICMP-MIB", file.Modules[0].Name);
            Assert.AreEqual(43, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[42];
            Assert.AreEqual("ipv6IcmpGroup", node.Name);
            Assert.AreEqual(1, node.Value);
            Assert.AreEqual("ipv6IcmpGroups", node.Parent.ToString());
        }
        [Test]
        public void TestIPv6_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.IPV6_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("IPV6-MIB", file.Modules[0].Name);
            Assert.AreEqual(91, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[90];
            Assert.AreEqual("ipv6NotificationGroup", node.Name);
            Assert.AreEqual(2, node.Value);
            Assert.AreEqual("ipv6Groups", node.Parent.ToString());
        }
        [Test]
        public void TestIPv6_TC()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.IPV6_TC);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("IPV6-TC", file.Modules[0].Name);
            Assert.AreEqual(0, file.Modules[0].Entities.Count);
        }
        [Test]
        public void TestIPv6_Tcp_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.IPV6_TCP_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("IPV6-TCP-MIB", file.Modules[0].Name);
            Assert.AreEqual(15, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[14];
            Assert.AreEqual("ipv6TcpGroup", node.Name);
            Assert.AreEqual(1, node.Value);
            Assert.AreEqual("ipv6TcpGroups", node.Parent.ToString());
        }
        [Test]
        public void TestIPv6_Udp_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.IPV6_UDP_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("IPV6-UDP-MIB", file.Modules[0].Name);
            Assert.AreEqual(12, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[11];
            Assert.AreEqual("ipv6UdpGroup", node.Name);
            Assert.AreEqual(1, node.Value);
            Assert.AreEqual("ipv6UdpGroups", node.Parent.ToString());
        }
        [Test]
        public void TestLM_Sensors_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.LM_SENSORS_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("LM-SENSORS-MIB", file.Modules[0].Name);
            Assert.AreEqual(22, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[21];
            Assert.AreEqual("lmMiscSensorsValue", node.Name);
            Assert.AreEqual(3, node.Value);
            Assert.AreEqual("lmMiscSensorsEntry", node.Parent.ToString());
        }
        [Test]
        public void TestIP_Forward_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.IP_FORWARD_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("IP-FORWARD-MIB", file.Modules[0].Name);
            Assert.AreEqual(69, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[68];
            Assert.AreEqual("ipForwardMultiPathGroup", node.Name);
            Assert.AreEqual(2, node.Value);
            Assert.AreEqual("ipForwardGroups", node.Parent.ToString());
        }
        [Test]
        public void TestIF_Inverted_Stack_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.IF_INVERTED_STACK_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("IF-INVERTED-STACK-MIB", file.Modules[0].Name);
            Assert.AreEqual(10, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[9];
            Assert.AreEqual("ifInvStackGroup", node.Name);
            Assert.AreEqual(1, node.Value);
            Assert.AreEqual("ifInvGroups", node.Parent.ToString());
        }
        [Test]
        public void TestIANA_RTPROTO_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.IANA_RTPROTO_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("IANA-RTPROTO-MIB", file.Modules[0].Name);
            Assert.AreEqual(1, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[0];
            Assert.AreEqual("ianaRtProtoMIB", node.Name);
            Assert.AreEqual(84, node.Value);
            Assert.AreEqual("mib-2", node.Parent.ToString());
        }
        [Test]
        public void TestIANA_Language_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.IANA_LANGUAGE_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("IANA-LANGUAGE-MIB", file.Modules[0].Name);
            Assert.AreEqual(8, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[7];
            Assert.AreEqual("ianaLangSMSL", node.Name);
            Assert.AreEqual(7, node.Value);
            Assert.AreEqual("ianaLanguages", node.Parent.ToString());
        }
        [Test]
        public void TestIANA_ADDRESS_FAMILY_NUMBERS_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.IANA_ADDRESS_FAMILY_NUMBERS_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("IANA-ADDRESS-FAMILY-NUMBERS-MIB", file.Modules[0].Name);
            Assert.AreEqual(1, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[0];
            Assert.AreEqual("ianaAddressFamilyNumbers", node.Name);
            Assert.AreEqual(72, node.Value);
            Assert.AreEqual("mib-2", node.Parent.ToString());
        }
        [Test]
        public void TestHOST_RESOURCES_TYPE()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.HOST_RESOURCES_TYPES);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("HOST-RESOURCES-TYPES", file.Modules[0].Name);
            Assert.AreEqual(55, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[54];
            Assert.AreEqual("hrFSLinuxExt2", node.Name);
            Assert.AreEqual(23, node.Value);
            Assert.AreEqual("hrFSTypes", node.Parent.ToString());
        }
        [Test]
        public void TestHOST_RESOURCES_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.HOST_RESOURCES_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("HOST-RESOURCES-MIB", file.Modules[0].Name);
            Assert.AreEqual(104, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[103];
            Assert.AreEqual("hrSWInstalledGroup", node.Name);
            Assert.AreEqual(6, node.Value);
            Assert.AreEqual("hrMIBGroups", node.Parent.ToString());
        }
        [Test]
        public void TestHCNUM_TC()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.HCNUM_TC);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("HCNUM-TC", file.Modules[0].Name);
            Assert.AreEqual(1, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[0];
            Assert.AreEqual("hcnumTC", node.Name);
            Assert.AreEqual(78, node.Value);
            Assert.AreEqual("mib-2", node.Parent.ToString());
        }
        [Test]
        public void TestEtherLike_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.EtherLike_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("EtherLike-MIB", file.Modules[0].Name);
            Assert.AreEqual(76, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[75];
            Assert.AreEqual("etherRateControlGroup", node.Name);
            Assert.AreEqual(15, node.Value);
            Assert.AreEqual("etherGroups", node.Parent.ToString());
        }
        [Test]
        public void TestDISKMAN_Event_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.DISMAN_EVENT_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("DISMAN-EVENT-MIB", file.Modules[0].Name);
            Assert.AreEqual(121, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[120];
            Assert.AreEqual("dismanEventNotificationGroup", node.Name);
            Assert.AreEqual(6, node.Value);
            Assert.AreEqual("dismanEventMIBGroups", node.Parent.ToString());
        }
        [Test]
        public void TestDISKMAN_Expression_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.DISMAN_EXPRESSION_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("DISMAN-EXPRESSION-MIB", file.Modules[0].Name);
            Assert.AreEqual(58, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[57];
            Assert.AreEqual("dismanExpressionValueGroup", node.Name);
            Assert.AreEqual(3, node.Value);
            Assert.AreEqual("dismanExpressionMIBGroups", node.Parent.ToString());
        }
        [Test]
        public void TestDISKMAN_NSLookUp_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.DISMAN_NSLOOKUP_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("DISMAN-NSLOOKUP-MIB", file.Modules[0].Name);
            Assert.AreEqual(25, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[24];
            Assert.AreEqual("lookupGroup", node.Name);
            Assert.AreEqual(1, node.Value);
            Assert.AreEqual("lookupGroups", node.Parent.ToString());
        }
        [Test]
        public void TestDISKMAN_Ping_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.DISMAN_PING_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("DISMAN-PING-MIB", file.Modules[0].Name);
            Assert.AreEqual(68, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[67];
            Assert.AreEqual("pingTimeStampGroup", node.Name);
            Assert.AreEqual(2, node.Value);
            Assert.AreEqual("pingGroups", node.Parent.ToString());
        }
        [Test]
        public void TestDISKMAN_Schedule_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.DISMAN_SCHEDULE_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("DISMAN-SCHEDULE-MIB", file.Modules[0].Name);
            Assert.AreEqual(38, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[37];
            Assert.AreEqual("schedGroup", node.Name);
            Assert.AreEqual(1, node.Value);
            Assert.AreEqual("schedGroups", node.Parent.ToString());
        }
        [Test]
        public void TestDISKMAN_Script_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.DISMAN_SCRIPT_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("DISMAN-SCRIPT-MIB", file.Modules[0].Name);
            Assert.AreEqual(94, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[93];
            Assert.AreEqual("smNotificationsGroup", node.Name);
            Assert.AreEqual(6, node.Value);
            Assert.AreEqual("smGroups", node.Parent.ToString());
        }
        [Test]
        public void TestDISKMAN_TRACEROUT_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.DISMAN_TRACEROUTE_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("DISMAN-TRACEROUTE-MIB", file.Modules[0].Name);
            Assert.AreEqual(84, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[83];
            Assert.AreEqual("traceRouteTimeStampGroup", node.Name);
            Assert.AreEqual(2, node.Value);
            Assert.AreEqual("traceRouteGroups", node.Parent.ToString());
        }
        [Test]
        public void TestSNMP_USM_DH_OBJECTS_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.SNMP_USM_DH_OBJECTS_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("SNMP-USM-DH-OBJECTS-MIB", file.Modules[0].Name);
            Assert.AreEqual(24, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[23];
            Assert.AreEqual("usmDHKeyKickstartGroup", node.Name);
            Assert.AreEqual(3, node.Value);
            Assert.AreEqual("usmDHKeyMIBGroups", node.Parent.ToString());
        }
        [Test]
        public void TestSNMP_USM_AES_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.SNMP_USM_AES_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("SNMP-USM-AES-MIB", file.Modules[0].Name);
            Assert.AreEqual(2, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[1];
            Assert.AreEqual("usmAesCfb128Protocol", node.Name);
            Assert.AreEqual(4, node.Value);
            Assert.AreEqual("snmpPrivProtocols", node.Parent.ToString());
        }
        [Test]
        public void TestSNMP_USER_BASED_SM_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.SNMP_USER_BASED_SM_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("SNMP-USER-BASED-SM-MIB", file.Modules[0].Name);
            Assert.AreEqual(36, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[35];
            Assert.AreEqual("usmMIBBasicGroup", node.Name);
            Assert.AreEqual(1, node.Value);
            Assert.AreEqual("usmMIBGroups", node.Parent.ToString());
        }
        [Test]
        public void TestSNMP_MPD_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.SNMP_MPD_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("SNMP-MPD-MIB", file.Modules[0].Name);
            Assert.AreEqual(12, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[11];
            Assert.AreEqual("snmpMPDGroup", node.Name);
            Assert.AreEqual(1, node.Value);
            Assert.AreEqual("snmpMPDMIBGroups", node.Parent.ToString());
        }
        [Test]
        public void TestSNMP_Notification_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.SNMP_NOTIFICATION_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("SNMP-NOTIFICATION-MIB", file.Modules[0].Name);
            Assert.AreEqual(29, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[28];
            Assert.AreEqual("snmpNotifyFilterGroup", node.Name);
            Assert.AreEqual(2, node.Value);
            Assert.AreEqual("snmpNotifyGroups", node.Parent.ToString());
        }
        [Test]
        public void TestSNMP_Proxy_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.SNMP_PROXY_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("SNMP-PROXY-MIB", file.Modules[0].Name);
            Assert.AreEqual(18, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[17];
            Assert.AreEqual("snmpProxyGroup", node.Name);
            Assert.AreEqual(3, node.Value);
            Assert.AreEqual("snmpProxyGroups", node.Parent.ToString());
        }
        [Test]
        public void TestSNMP_Target_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.SNMP_TARGET_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("SNMP-TARGET-MIB", file.Modules[0].Name);
            Assert.AreEqual(32, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[31];
            Assert.AreEqual("snmpTargetCommandResponderGroup", node.Name);
            Assert.AreEqual(3, node.Value);
            Assert.AreEqual("snmpTargetGroups", node.Parent.ToString());
        }
        [Test]
        public void TestAgentX_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.AGENTX_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("AGENTX-MIB", file.Modules[0].Name);
            Assert.AreEqual(41, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[40];
            Assert.AreEqual("agentxMIBGroup", node.Name);
            Assert.AreEqual(1, node.Value);
            Assert.AreEqual("agentxMIBGroups", node.Parent.ToString());
        }
        [Test]
        public void TestSNMP_Community_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.SNMP_COMMUNITY_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("SNMP-COMMUNITY-MIB", file.Modules[0].Name);
            Assert.AreEqual(25, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[24];
            Assert.AreEqual("snmpProxyTrapForwardGroup", node.Name);
            Assert.AreEqual(3, node.Value);
            Assert.AreEqual("snmpCommunityMIBGroups", node.Parent.ToString());
        }
        [Test]
        public void TestSNMP_Framework_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.SNMP_FRAMEWORK_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("SNMP-FRAMEWORK-MIB", file.Modules[0].Name);
            Assert.AreEqual(15, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[14];
            Assert.AreEqual("snmpEngineGroup", node.Name);
            Assert.AreEqual(1, node.Value);
            Assert.AreEqual("snmpFrameworkMIBGroups", node.Parent.ToString());
        }
        
        [Test]
        public void Testv2CONF()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.SNMPv2_CONF);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("SNMPv2-CONF", file.Modules[0].Name);
            Assert.AreEqual(0, file.Modules[0].Entities.Count);
        }
        [Test]
        public void Testv2_TC()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.SNMPv2_TC);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual(1, file.Modules.Count);
            Assert.AreEqual("SNMPv2-TC", file.Modules[0].Name);
            Assert.AreEqual(0, file.Modules[0].Entities.Count);
        }
        
        [Test]
        public void Testv2_SMI()
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
            Assert.AreEqual(16, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[15];
            Assert.AreEqual("zeroDotZero", node.Name);
            Assert.AreEqual(0, node.Value);
            Assert.AreEqual("ccitt", node.Parent.ToString());
        }
        
        [Test]
        public void Testv2_MIB()
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
            Assert.AreEqual(70, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[69];
            Assert.AreEqual("snmpObsoleteGroup", node.Name);
            Assert.AreEqual(10, node.Value);
            Assert.AreEqual("snmpMIBGroups", node.Parent.ToString());
            Assert.AreEqual(47, file.Modules[0].Objects.Count);
        }
        [Test]
        public void TestIANAifType_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.IANAifType_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual("IANAifType-MIB", file.Modules[0].Name);
            Assert.AreEqual(2, file.Modules[0].Dependents.Count);
            Assert.AreEqual(1, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[0];
            Assert.AreEqual("ianaifType", node.Name);
            Assert.AreEqual(30, node.Value);
            Assert.AreEqual("mib-2", node.Parent.ToString());
        }
        [Test]
        public void TestIF_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.IF_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual("IF-MIB", file.Modules[0].Name);
            Assert.AreEqual(5, file.Modules[0].Dependents.Count);
            Assert.AreEqual(91, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[90];
            Assert.AreEqual("ifCompliance2", node.Name);
            Assert.AreEqual(2, node.Value);
            Assert.AreEqual("ifCompliances", node.Parent.ToString());
        }
        [Test]
        public void TestINET_ADDRESS_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.INET_ADDRESS_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual("INET-ADDRESS-MIB", file.Modules[0].Name);
            Assert.AreEqual(2, file.Modules[0].Dependents.Count);
            Assert.AreEqual(1, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[0];
            Assert.AreEqual("inetAddressMIB", node.Name);
            Assert.AreEqual(76, node.Value);
            Assert.AreEqual("mib-2", node.Parent.ToString());
        }
        [Test]
        public void TestIP_MIB()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.IP_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual("IP-MIB", file.Modules[0].Name);
            Assert.AreEqual(5, file.Modules[0].Dependents.Count);
            Assert.AreEqual(293, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[292];
            Assert.AreEqual("icmpGroup", node.Name);
            Assert.AreEqual(2, node.Value);
            Assert.AreEqual("ipMIBGroups", node.Parent.ToString());
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
            Assert.AreEqual(8, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[7];
            Assert.AreEqual("rfc1157Domain", node.Name);
            Assert.AreEqual(1, node.Value);
            Assert.AreEqual("rfc1157Proxy", node.Parent.ToString());
        }

        [Test]
        public void TestBridge()
        {
            Lexer lexer = new Lexer();
            MemoryStream m = new MemoryStream(Resource.BRIDGE_MIB);
            using (StreamReader reader = new StreamReader(m))
            {
                lexer.Parse(reader);
                reader.Close();
            }
            MibDocument file = new MibDocument(lexer);
            Assert.AreEqual("BRIDGE-MIB", file.Modules[0].Name);
            Assert.AreEqual(4, file.Modules[0].Dependents.Count);
            Assert.AreEqual(62, file.Modules[0].Entities.Count);
            IEntity node = file.Modules[0].Entities[61];
            Assert.AreEqual("dot1dStaticStatus", node.Name);
            Assert.AreEqual(4, node.Value);
            Assert.AreEqual("dot1dStaticEntry", node.Parent.ToString());
        }
    }
}
#pragma warning restore 1591

