using Lextm.SharpSnmpLib.Mib;
using NUnit.Framework;
#pragma warning disable 1591,0618
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

        [Test]
        public void TestGreaterThan()
        {
            Assert.Greater(new ObjectIdentifier("1.1"), new ObjectIdentifier("0.0"));
            Assert.Greater(new ObjectIdentifier("0.0.0"), new ObjectIdentifier("0.0"));
            Assert.AreEqual(new ObjectIdentifier("0.0"), new ObjectIdentifier("0.0"));
            Assert.AreEqual((ObjectIdentifier)null, (ObjectIdentifier)null);
        }

        [Test]
        public void TestConversion()
        {	  	
            new ObjectIdentifier(".1.3.6.1.2.1.1.1.0");
        }

        [Test]
        public void TestToString()
        {
            Assert.AreEqual("iso.org.dod.internet.mgmt.mib-2.transmission",
                SearchResult.GetStringOf(new ObjectIdentifier(new uint[] {1, 3, 6, 1, 2, 1, 10}), DefaultObjectRegistry.Instance));
        }

        [Test]
        public void TestToStringLong()
        {
            Assert.AreEqual("iso.org.dod.internet.mgmt.mib-2.transmission.100",
                SearchResult.GetStringOf(new ObjectIdentifier(new uint[] { 1, 3, 6, 1, 2, 1, 10, 100 }), DefaultObjectRegistry.Instance));
        }
    }
}
#pragma warning restore 1591,0618

