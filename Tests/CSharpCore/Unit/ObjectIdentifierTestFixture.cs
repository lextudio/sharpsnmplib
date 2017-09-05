using System;
using System.IO;
using Xunit;

#pragma warning disable 1591, 0618
namespace Lextm.SharpSnmpLib.Unit
{
    public class ObjectIdentifierTestFixture
    {
        [Fact]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObjectIdentifier((uint[])null));
            Assert.Throws<ArgumentException>(() => new ObjectIdentifier(new uint[] {1}));
            Assert.Throws<ArgumentException>(() => new ObjectIdentifier(new uint[] {5, 8}));
            Assert.Throws<ArgumentException>(() => new ObjectIdentifier(new uint[] {1, 80}));
            Assert.Throws<ArgumentNullException>(() => new ObjectIdentifier(new Tuple<int, byte[]>(0, new byte[] { 0 }), null));
            Assert.Throws<ArgumentException>(() => new ObjectIdentifier(new Tuple<int, byte[]>(0, new byte[] { 0 }), new MemoryStream()));
// ReSharper disable RedundantCast
            Assert.Throws<ArgumentNullException>(() => new ObjectIdentifier(new uint[] {1, 3}).CompareTo((ObjectIdentifier)null));
// ReSharper restore RedundantCast
            Assert.Throws<ArgumentNullException>(() => ObjectIdentifier.Convert((uint[])null));
            Assert.Throws<ArgumentNullException>(() => ObjectIdentifier.Convert((string)null));
            var oid = new ObjectIdentifier(new uint[] { 1, 3 });
            Assert.Throws<ArgumentException>(() => oid.CompareTo(80));
            Assert.Throws<ArgumentNullException>(() => ObjectIdentifier.Create(null, 0));
            Assert.Throws<ArgumentNullException>(() => oid.AppendBytesTo(null));
        }

        [Fact]
        public void TestConstructor()
        {
            ObjectIdentifier oid = new ObjectIdentifier(new byte[] { 0x2B, 0x06, 0x99, 0x37 });
            Assert.Equal(new uint[] { 1, 3, 6, 3255 }, oid.ToNumerical());
            var o = ObjectIdentifier.Create(new uint[] {1, 3, 6}, 3255);
            Assert.Equal(oid, o);
        }
        
        [Fact]
        public void TestConstructor2()
        {
            ObjectIdentifier oid = new ObjectIdentifier(new byte[] { 0x2B, 0x06, 0x01, 0x04, 0x01, 0x90, 0x72, 0x87, 0x68, 0x02 });
            Assert.Equal(new uint[] { 1, 3, 6, 1, 4, 1, 2162, 1000, 2 }, oid.ToNumerical());
        }

        [Fact]
        public void TestToBytes()
        {
            uint[] expected = new uint[] {1,3,6,1,4,1,2162,1000,2};
            ObjectIdentifier oid = new ObjectIdentifier(expected);
            Assert.Equal(new byte[] { 0x06, 0x0A, 0x2B, 0x06, 0x01, 0x04, 0x01, 0x90, 0x72, 0x87, 0x68, 0x02 }, oid.ToBytes());
        }
        
        [Fact]
        public void TestToBytes2()
        {
            uint[] expected = new uint[] {0, 0};
            ObjectIdentifier oid = new ObjectIdentifier(expected);
            Assert.Equal(new byte[] {0x06, 0x01, 0x00}, oid.ToBytes());
        }
        
        [Fact]
        public void TestToBytes3()
        {
            uint[] expected = new uint[] {1, 3, 6, 3255};
            ObjectIdentifier oid = new ObjectIdentifier(expected);
            Assert.Equal(new byte[] {0x06, 0x04, 0x2B, 0x06, 0x99, 0x37}, oid.ToBytes());
        }

        [Fact]
        public void TestGreaterThan()
        {
            Assert.True(new ObjectIdentifier("1.1") > new ObjectIdentifier("0.0"));
            Assert.True(new ObjectIdentifier("0.0.0") > new ObjectIdentifier("0.0"));
            Assert.True(new ObjectIdentifier("0.0") < new ObjectIdentifier("1.1"));
            Assert.True(new ObjectIdentifier("0.0").Compare(new ObjectIdentifier("1.1")) < 0);
        }

        [Fact]
        public void TestConversion()
        {
            var o = new ObjectIdentifier("1.3.6.1.2.1.1.1.0");
            Assert.Equal("1.3.6.1.2.1.1.1.0", o.ToString());
        }
        
        [Fact]
        public void TestUuid()
        {
            var exception = Assert.Throws<ArgumentException>(() => new ObjectIdentifier("2.25.329800735698586629295641978511506172918"));
            Assert.Equal($"Parameter 329800735698586629295641978511506172918 is out of 32 bit unsigned integer range.{Environment.NewLine}Parameter name: dotted", exception.Message);
        }

        [Fact]
        public void TestToString()
        {
            var transmission = new ObjectIdentifier(new uint[] {1, 3, 6, 1, 2, 1, 10});
            Assert.Equal("1.3.6.1.2.1.10", transmission.ToString());
        }

        [Fact]
        public void TestEqual()
        {
            var left = new ObjectIdentifier("1.3.6.3");
            var right = new ObjectIdentifier("1.3.6.3");
            Assert.Equal(left, right);
            Assert.True(left.Equals(right));
            Assert.True(left != null);
// ReSharper disable RedundantCast
// ReSharper disable EqualExpressionComparison
            Assert.True((ObjectIdentifier)null == (ObjectIdentifier)null);
// ReSharper restore EqualExpressionComparison
// ReSharper restore RedundantCast
        }
    }
}
#pragma warning restore 1591,0618

