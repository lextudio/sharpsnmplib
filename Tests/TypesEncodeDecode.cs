// removed: DotNetSnmp.Asn1.Serialization
using Lextm.SharpSnmpLib;
using System.Diagnostics.CodeAnalysis;
using System.Formats.Asn1;
using System.Text;
using Xunit;

namespace DotNetSnmp.Test
{
    [ExcludeFromCodeCoverage]
    public class TypesEncodeDecode
    {
        private static AsnReader GetReader(string berHexString)
        {
            return new AsnReader(
                Convert.FromHexString(berHexString),
                AsnEncodingRules.BER);
        }

        private static string ToHexString(ISnmpData asns)
        {
            var writer = new AsnWriter(AsnEncodingRules.BER);
            asns.WriteTo(writer);
            return Convert.ToHexString(writer.Encode());
        }

        [Fact]
        public void EncodeDecode_ObjectIdentifier()
        {
            var hexBer = "060B2B0601040183A468010163";
            var reader = GetReader(hexBer);
            var oid = ObjectIdentifier.ReadFrom(reader);
            Assert.Equal("1.3.6.1.4.1.53864.1.1.99", oid.Oid);
            Assert.Equal(hexBer, ToHexString(oid));
        }

        [Fact]
        public void EncodeDecode_Integer32()
        {
            var hexBer = "02012A";
            var reader = GetReader(hexBer); // 42
            var i32 = Integer32.ReadFrom(reader);
            Assert.Equal(42, i32.Value);
            Assert.Equal(hexBer, ToHexString(i32));
        }

        [Fact]
        public void EncodeDecode_IpAddress()
        {
            var hexBer = "4004C0A8ED18";
            var reader = GetReader(hexBer);
            var ipAddress = IP.ReadFrom(reader);
            Assert.Equal(new byte[4] { 192, 168, 237, 24 }, ipAddress.AddressBytes);
            Assert.Equal(hexBer, ToHexString(ipAddress));
        }

        [Fact]
        public void EncodeDecode_Counter64()
        {
            var hexBer = "460900FFFFFFFFFFFFFFFE";
            var reader = GetReader(hexBer);
            var counter64 = Counter64.ReadFrom(reader);
            Assert.Equal(18446744073709551614, counter64.Value);
            Assert.Equal(hexBer, ToHexString(counter64));

            hexBer = "460100";
            reader = GetReader(hexBer);
            var zeroCounter = Counter64.ReadFrom(reader);
            Assert.Equal((ulong)0, zeroCounter.Value);
            Assert.Equal(hexBer, ToHexString(zeroCounter));
        }

        // [Fact]
        // public void EncodeDecode_OpaqueInteger64()
        // {
        //     var hexBer = "440B9F7A087FFFFFFFFFFFFFFF";
        //     var reader = GetReader(hexBer); // 9223372036854775807 
        //     var opaqueInt64 = OpaqueInteger64.ReadFrom(reader);
        //     Assert.Equal(9223372036854775807, opaqueInt64.Value);
        //     Assert.Equal(hexBer, ToHexString(opaqueInt64));
        // }

        [Fact]
        public void EncodeDecode_OctetString_Ascii()
        {
            var hexBer = "04224C6966652C2074686520556E6976657273652C20616E642045766572797468696E67";
            var reader = GetReader(hexBer);
            var octetString = OctetString.ReadFrom(reader);
            Assert.Equal(
                "Life, the Universe, and Everything",
                Encoding.ASCII.GetString(octetString.Octets));
            Assert.Equal(hexBer, ToHexString(octetString));
        }

        [Fact]
        public void EncodeDecode_TimeTicks()
        {
            var hexBer = "430306034C"; // Timeticks: (394060) 1:05:40.60
            var reader = GetReader(hexBer);
            var timeTicks = TimeTicks.ReadFrom(reader);
            Assert.Equal((uint)394060, timeTicks.Value);
            Assert.Equal(hexBer, ToHexString(timeTicks));
            Assert.Equal(TimeSpan.FromMilliseconds(timeTicks.Value * 10).ToString(), timeTicks.ToString());
        }
    }
}
