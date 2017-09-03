/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/30
 * Time: 19:58
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using Xunit;

#pragma warning disable 1591, 0618
namespace Lextm.SharpSnmpLib.Unit
{
    public class DataFactoryTestFixture
    {
        [Fact]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => DataFactory.CreateSnmpData((Stream)null));
            Assert.Throws<ArgumentNullException>(() => DataFactory.CreateSnmpData(null, 0, 0));
            Assert.Throws<ArgumentNullException>(() => DataFactory.CreateSnmpData((byte[])null));
            Assert.Throws<ArgumentNullException>(() => DataFactory.CreateSnmpData(0, null));
        }
        
        [Fact]
        public void TestCreateObjectIdentifier()
        {
            byte[] expected = new byte[] {0x06, 0x0A, 0x2B, 0x06, 0x01, 0x04, 0x01, 0x90, 0x72, 0x87, 0x68, 0x02};
            ISnmpData data = DataFactory.CreateSnmpData(expected);
            Assert.Equal(SnmpType.ObjectIdentifier, data.TypeCode);
            ObjectIdentifier o = (ObjectIdentifier)data;
            Assert.Equal(new uint[] { 1, 3, 6, 1, 4, 1, 2162, 1000, 2 }, o.ToNumerical());
        }
        
        [Fact]
        public void TestCreateObjectIdentifier2()
        {
            byte[] expected = new Byte[] {0x06, 0x01, 0x00};
            ISnmpData data = DataFactory.CreateSnmpData(expected);
            Assert.Equal(SnmpType.ObjectIdentifier, data.TypeCode);
            ObjectIdentifier o = (ObjectIdentifier)data;
            Assert.Equal(new uint[] {0, 0}, o.ToNumerical());
        }
        
        [Fact]
        public void TestCreateNull()
        {
            byte[] expected = new byte[] {0x05, 0x00};
            ISnmpData data = DataFactory.CreateSnmpData(expected);
            Assert.Equal(SnmpType.Null, data.TypeCode);
            Null n = (Null)data;
            Assert.Equal(expected, n.ToBytes());
        }
        
        [Fact]
        public void TestCreateInteger()
        {
            byte[] expected = new byte[] {0x02, 0x01, 0x00};
            ISnmpData data = DataFactory.CreateSnmpData(expected);
            Assert.Equal(SnmpType.Integer32, data.TypeCode);
            Integer32 i = (Integer32)data;
            Assert.Equal(0, i.ToInt32());
        }
        
        [Fact]
        public void TestCreateOctetString()
        {
            byte[] expected = new byte[] {0x04, 0x06, 0x70, 0x75, 0x62, 0x6C, 0x69, 0x63};
            ISnmpData data = DataFactory.CreateSnmpData(expected);
            Assert.Equal(SnmpType.OctetString, data.TypeCode);
            Assert.Equal("public", data.ToString());
        }
        
        [Fact]
        public void TestCreateIP()
        {
            byte[] expected = new byte[] { 0x40, 0x04, 0x7F, 0x00, 0x00, 0x01};
            ISnmpData data = DataFactory.CreateSnmpData(expected);
            Assert.Equal(SnmpType.IPAddress, data.TypeCode);
            IP a = (IP)data;
            Assert.Equal("127.0.0.1", a.ToString());
        }
        
        [Fact]
        public void TestTimeticks()
        {
            byte[] expected = new byte[] { 0x43, 0x02, 0x3F, 0xE0 };
            ISnmpData data = DataFactory.CreateSnmpData(expected);
            Assert.Equal(SnmpType.TimeTicks, data.TypeCode);
            TimeTicks t = (TimeTicks)data;
            Assert.Equal(16352U, t.ToUInt32());
        }
        
        [Fact]
        public void TestVarbind()
        {
            byte[] expected = new byte[] {0x30, 0x17,
                0x06, 0x0B, 0x2B, 0x06, 0x01, 0x04, 0x01, 0x90, 0x72, 0x87, 0x69, 0x15, 0x00,
                0x04, 0x08, 0x54, 0x72, 0x61, 0x70, 0x54, 0x65, 0x73, 0x74};
            ISnmpData data = DataFactory.CreateSnmpData(expected);
            Assert.Equal(SnmpType.Sequence, data.TypeCode);
            Sequence a = (Sequence)data;
            Assert.Equal(2, a.Length);
            
            ISnmpData oid = a[0];
            ISnmpData name = a[1];
            Assert.Equal(SnmpType.ObjectIdentifier, oid.TypeCode);
            Assert.Equal(SnmpType.OctetString, name.TypeCode);
            ObjectIdentifier o = (ObjectIdentifier)oid;
            Assert.Equal(new uint[] {1,3,6,1,4,1,2162,1001,21,0}, o.ToNumerical());
            OctetString s = (OctetString)name;
            Assert.Equal("TrapTest", s.ToString());
        }
        
        [Fact]
        public void TestVarbindSection()
        {
            byte[] expected = new byte[] {0x30, 0x19,
                0x30, 0x17,
                0x06, 0x0B, 0x2B, 0x06, 0x01, 0x04, 0x01, 0x90, 0x72, 0x87, 0x69, 0x15, 0x00,
                0x04, 0x08, 0x54, 0x72, 0x61, 0x70, 0x54, 0x65, 0x73, 0x74};
            ISnmpData data = DataFactory.CreateSnmpData(expected);
            Assert.Equal(SnmpType.Sequence, data.TypeCode);
            
            Sequence a = (Sequence)data;
            Assert.Equal(1, a.Length);
            ISnmpData varbind = a[0];
            Assert.Equal(SnmpType.Sequence, varbind.TypeCode);
        }
        
        [Fact]
        public void TestTrapv1Pdu()
        {
            byte[] expected = new byte[] {0xA4, 0x37,
                0x06, 0x0A, 0x2B, 0x06, 0x01, 0x04, 0x01, 0x90, 0x72, 0x87, 0x68, 0x02,
                0x40, 0x04, 0x7F, 0x00, 0x00, 0x01,
                0x02, 0x01, 0x06,
                0x02, 0x01, 0x0C,
                0x43, 0x02, 0x3F, 0xE0,
                0x30, 0x19,
                0x30, 0x17,
                0x06, 0x0B, 0x2B, 0x06, 0x01, 0x04, 0x01, 0x90, 0x72, 0x87, 0x69, 0x15, 0x00,
                0x04, 0x08, 0x54, 0x72, 0x61, 0x70, 0x54, 0x65, 0x73, 0x74};
            ISnmpData data = DataFactory.CreateSnmpData(expected);
            Assert.Equal(SnmpType.TrapV1Pdu, data.TypeCode);
            
            TrapV1Pdu t = (TrapV1Pdu)data;
            Assert.Equal(new uint[] {1,3,6,1,4,1,2162,1000,2}, t.Enterprise.ToNumerical());
            Assert.Equal("127.0.0.1", t.AgentAddress.ToIPAddress().ToString());
            Assert.Equal(GenericCode.EnterpriseSpecific, t.Generic);
            Assert.Equal(12, t.Specific);
            Assert.Equal(16352U, t.TimeStamp.ToUInt32());
            Assert.Equal(1, t.Variables.Count);
        }
        
        [Fact]
        public void TestTrapPacket()
        {
            byte[] expected = new byte[] {
                0x30, 0x44,
                0x02, 0x01, 0x00,
                0x04, 0x06, 0x70, 0x75, 0x62, 0x6C, 0x69, 0x63,
                0xA4, 0x37,
                0x06, 0x0A, 0x2B, 0x06, 0x01, 0x04, 0x01, 0x90, 0x72, 0x87, 0x68, 0x02,
                0x40, 0x04, 0x7F, 0x00, 0x00, 0x01,
                0x02, 0x01, 0x06,
                0x02, 0x01, 0x0C,
                0x43, 0x02, 0x3F, 0xE0,
                0x30, 0x19,
                0x30, 0x17,
                0x06, 0x0B, 0x2B, 0x06, 0x01, 0x04, 0x01, 0x90, 0x72, 0x87, 0x69, 0x15, 0x00,
                0x04, 0x08, 0x54, 0x72, 0x61, 0x70, 0x54, 0x65, 0x73, 0x74};
            ISnmpData data = DataFactory.CreateSnmpData(expected);
            Assert.Equal(SnmpType.Sequence, data.TypeCode);
            
            Sequence t = (Sequence)data;
            Assert.Equal(3, t.Length);
            ISnmpData version = t[0];
            Assert.Equal(SnmpType.Integer32, version.TypeCode);
            Assert.Equal(1, 1 + ((Integer32)version).ToInt32());
            ISnmpData community = t[1];
            Assert.Equal(SnmpType.OctetString, community.TypeCode);
            Assert.Equal("public", ((OctetString)community).ToString());
            ISnmpData pdu = t[2];
            Assert.Equal(SnmpType.TrapV1Pdu, pdu.TypeCode);
        }

        [Fact]
        public void TestInformDiscovery()
        {
            //#7245
            var pdu = new InformRequestPdu(12);
            var bytes = pdu.ToBytes();
            var data = DataFactory.CreateSnmpData(bytes);
            Assert.Equal(SnmpType.InformRequestPdu, data.TypeCode);
            var newPdu = (InformRequestPdu)data;
            Assert.Null(newPdu.Enterprise);
            Assert.Equal(0U, newPdu.TimeStamp);
        }
    }
}
#pragma warning restore 1591,0618

