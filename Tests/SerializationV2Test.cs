// removed: DotNetSnmp.Asn1.Serialization
using Lextm.SharpSnmpLib;
// removed: DotNetSnmp.Common.Definitions
// removed: DotNetSnmp.Protocol.V1
// removed: DotNetSnmp.Protocol.V2
using System.Diagnostics.CodeAnalysis;
using System.Formats.Asn1;
using System.Text;
using Xunit;

namespace DotNetSnmp.Test
{
    /// <summary>
    /// Test steps summary:
    ///   1. read a textual hex dump produced from an net-snmp command (e.g snmpget)
    ///      with the -d (dump) flag set
    ///   2. test that the decoded message matches the expected (known) values
    ///   3. encode the decoded message and compare to the original serialized BER bytes
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SerializationV2Test
    {
        [Fact]
        public void ShouldThrowDecodeExceptionOnWrongSnmpVersion()
        {
            var dump = @"Sending 43 bytes to UDP: [127.0.0.1]:161->[0.0.0.0]:0
                0000: 30 29 02 01  00 04 06 70  75 62 6C 69  63 A0 1C 02    0).....public...
                0016: 04 0C BB 47  10 02 01 00  02 01 00 30  0E 30 0C 06    ...G.......0.0..
                0032: 08 2B 06 01  02 01 01 01  00 05 00                    .+.........";

            var messageBytes = Dump.BytesFromDumpString(dump);

            var reader = new AsnReader(messageBytes, AsnEncodingRules.BER);

            Assert.Throws<SnmpDecodeException>(() =>
            {
                _ = SnmpV2Message.ReadFrom(reader);
            });
        }

        [Fact]
        public void DecodeEncodeV2GetRequestMessage()
        {
            var dump = @"Sending 43 bytes to UDP: [127.0.0.1]:161->[0.0.0.0]:0
                0000: 30 29 02 01  01 04 06 70  75 62 6C 69  63 A0 1C 02    0).....public...
                0016: 04 16 E3 3F  8E 02 01 00  02 01 00 30  0E 30 0C 06    ...?.......0.0..
                0032: 08 2B 06 01  02 01 01 01  00 05 00                    .+.........";

            var messageBytes = Dump.BytesFromDumpString(dump);

            var reader = new AsnReader(messageBytes, AsnEncodingRules.BER);

            var message = SnmpV2Message.ReadFrom(reader);

            Assert.NotNull(message);

            Assert.Equal(
                VersionCode.V2,
                message.ProtocolVersion);

            Assert.Equal("public", message.Community);

            Assert.NotNull(message.Pdu);

            Assert.IsType<GetRequestPdu>(message.Pdu);

            Assert.NotNull(message.Pdu.VariableBindings);

            var varbind = message!.Pdu.VariableBindings!.FirstOrDefault();

            Assert.Equal("1.3.6.1.2.1.1.1.0", varbind.Id);

            Assert.IsType<Null>(varbind.Data);

            var writer = new AsnWriter(AsnEncodingRules.BER);

            message.WriteTo(writer);

            var encoded = writer.Encode();

            Assert.Equal(messageBytes, encoded);
        }

        [Fact]
        public void DecodeEncodeV2GetResponseMessage()
        {
            var dump = @"Received 63 byte packet from UDP: [127.0.0.1]:161->[0.0.0.0]:51823
                0000: 30 3D 02 01  01 04 06 70  75 62 6C 69  63 A2 30 02    0=.....public.0.
                0016: 04 16 E3 3F  8E 02 01 00  02 01 00 30  22 30 20 06    ...?.......0""0.
                0032: 08 2B 06 01  02 01 01 01  00 04 14 4E  65 74 53 6E    .+.........NetSn
                0048: 6D 70 54 65  73 74 43 6F  6E 74 61 69  6E 65 72       mpTestContainer";

            var messageBytes = Dump.BytesFromDumpString(dump);

            var reader = new AsnReader(messageBytes, AsnEncodingRules.BER);

            var message = SnmpV2Message.ReadFrom(reader);

            Assert.NotNull(message);

            Assert.Equal(
                VersionCode.V2,
                message.ProtocolVersion);

            Assert.Equal("public", message.Community);

            Assert.NotNull(message.Pdu);

            Assert.IsType<ResponsePdu>(message.Pdu);

            Assert.NotNull(message.Pdu.VariableBindings);

            var varbind = message!.Pdu.VariableBindings!.FirstOrDefault();

            Assert.Equal("1.3.6.1.2.1.1.1.0", varbind.Id);

            Assert.IsType<OctetString>(varbind.Data);

            var octetString = (OctetString)varbind.Data;

            Assert.Equal(
                "NetSnmpTestContainer",
                Encoding.ASCII.GetString(octetString.Octets));

            var writer = new AsnWriter(AsnEncodingRules.BER);

            message.WriteTo(writer);

            var encoded = writer.Encode();

            Assert.Equal(messageBytes, encoded);
        }

        [Fact]
        public void DecodeEncodeV2GetNextRequestMessage()
        {
            var dump = @"Sending 43 bytes to UDP: [127.0.0.1]:161->[0.0.0.0]:0
                0000: 30 29 02 01  01 04 06 70  75 62 6C 69  63 A1 1C 02    0).....public...
                0016: 04 15 2E E1  C9 02 01 00  02 01 00 30  0E 30 0C 06    ...........0.0..
                0032: 08 2B 06 01  02 01 01 01  00 05 00                    .+.........";

            var messageBytes = Dump.BytesFromDumpString(dump);

            var reader = new AsnReader(messageBytes, AsnEncodingRules.BER);

            var message = SnmpV2Message.ReadFrom(reader);

            Assert.NotNull(message);

            Assert.Equal(
                VersionCode.V2,
                message.ProtocolVersion);

            Assert.Equal("public", message.Community);

            Assert.NotNull(message.Pdu);

            Assert.IsType<GetNextRequestPdu>(message.Pdu);

            Assert.NotNull(message.Pdu.VariableBindings);

            var varbind = message!.Pdu.VariableBindings!.FirstOrDefault();

            Assert.Equal("1.3.6.1.2.1.1.1.0", varbind.Id);

            Assert.IsType<Null>(varbind.Data);

            var writer = new AsnWriter(AsnEncodingRules.BER);

            message.WriteTo(writer);

            var encoded = writer.Encode();

            Assert.Equal(messageBytes, encoded);
        }

        [Fact]
        public void DecodeEncodeV2GetNextResponseMessage()
        {
            var dump = @"Received 53 byte packet from UDP: [127.0.0.1]:161->[0.0.0.0]:45929
                0000: 30 33 02 01  01 04 06 70  75 62 6C 69  63 A2 26 02    03.....public.&.
                0016: 04 15 2E E1  C9 02 01 00  02 01 00 30  18 30 16 06    ...........0.0..
                0032: 08 2B 06 01  02 01 01 02  00 06 0A 2B  06 01 04 01    .+.........+....
                0048: BF 08 03 02  0A                                       .....";

            var messageBytes = Dump.BytesFromDumpString(dump);

            var reader = new AsnReader(messageBytes, AsnEncodingRules.BER);

            var message = SnmpV2Message.ReadFrom(reader);

            Assert.NotNull(message);

            Assert.Equal(
                VersionCode.V2,
                message.ProtocolVersion);

            Assert.Equal("public", message.Community);

            Assert.NotNull(message.Pdu);

            Assert.IsType<ResponsePdu>(message.Pdu);

            Assert.NotNull(message.Pdu.VariableBindings);

            var varbind = message!.Pdu.VariableBindings!.FirstOrDefault();

            Assert.Equal("1.3.6.1.2.1.1.2.0", varbind.Id);

            Assert.IsType<ObjectIdentifier>(varbind.Data);

            Assert.Equal("1.3.6.1.4.1.8072.3.2.10", ((ObjectIdentifier)varbind.Data).Oid);

            var writer = new AsnWriter(AsnEncodingRules.BER);

            message.WriteTo(writer);

            var encoded = writer.Encode();

            Assert.Equal(messageBytes, encoded);
        }
    }
}
