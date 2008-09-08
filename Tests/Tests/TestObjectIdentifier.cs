using System;
using System.IO;
using NUnit.Framework;
#pragma warning disable 1591
namespace Lextm.SharpSnmpLib.Tests
{
    [TestFixture]
    public class TestObjectIdentifier
    {
        [Test]
        public void TestConstructor()
        {
            ObjectIdentifier oid = new ObjectIdentifier(new byte[] { 0x2B, 0x06, 0x99, 0x37 });
            Assert.AreEqual(new uint[] { 1, 3, 6, 3255 }, oid.ToNumerical());            
        }
        
        [Test]
        public void TestConstructor2()
        {
            ObjectIdentifier oid = new ObjectIdentifier(new byte[] { 0x2B, 0x06, 0x01, 0x04, 0x01, 0x90, 0x72, 0x87, 0x68, 0x02 });
            Assert.AreEqual(new uint[] { 1, 3, 6, 1, 4, 1, 2162, 1000, 2 }, oid.ToNumerical());
        }

        [Test]
        public void TestToBytes()
        {
        	uint[] expected = new uint[] {1,3,6,1,4,1,2162,1000,2};
        	ObjectIdentifier oid = new ObjectIdentifier(expected);
        	Assert.AreEqual(new byte[] { 0x06, 0x0A, 0x2B, 0x06, 0x01, 0x04, 0x01, 0x90, 0x72, 0x87, 0x68, 0x02 }, oid.ToBytes());
        }        
        
        [Test]
        public void TestToBytes2()
        {
            uint[] expected = new uint[] {0, 0};
         	ObjectIdentifier oid = new ObjectIdentifier(expected);
         	Assert.AreEqual(new byte[] {0x06, 0x01, 0x00}, oid.ToBytes());
        } 
        
        [Test] 
        public void TestToBytes3()
        {
            uint[] expected = new uint[] {1, 3, 6, 3255};
         	ObjectIdentifier oid = new ObjectIdentifier(expected);
         	Assert.AreEqual(new byte[] {0x06, 0x04, 0x2B, 0x06, 0x99, 0x37}, oid.ToBytes());            
        }
    }
}
#pragma warning restore 1591

