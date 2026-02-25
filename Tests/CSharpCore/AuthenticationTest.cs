using DotNetSnmp.Common.Definitions;
using DotNetSnmp.Protocol.V3;
using DotNetSnmp.Protocol.V3.Security;
using DotNetSnmp.Protocol.V3.Security.Authentication;
using DotNetSnmp.Test.Helpers.XUnit.Project.Attributes;
using DotNetSnmp.Utils;
using System.Diagnostics.CodeAnalysis;
using System.Formats.Asn1;
using Xunit;
using static DotNetSnmp.Test.TestUtils;

namespace DotNetSnmp.Test
{
    [ExcludeFromCodeCoverage]
    [TestCaseOrderer(typeof(XUnit.Project.Orderers.PriorityOrderer))]
    public class AuthenticationTest
    {
        [Fact, TestPriority(0)]
        public void EncodeDecodeDiscoveryMessage()
        {
            var dump = @"Sending 63 bytes to UDP: [127.0.0.1]:161->[0.0.0.0]:0
                0000: 30 3D 02 01  03 30 10 02  04 2E 7D 44  EC 02 02 05    0=...0....}D....
                0016: C0 04 01 04  02 01 03 04  10 30 0E 04  00 02 01 00    .........0......
                0032: 02 01 00 04  00 04 00 04  00 30 14 04  00 04 00 A0    .........0......
                0048: 0E 02 04 63  D4 A9 15 02  01 00 02 01  00 30 00       ...c.........0.";

            var messageBytes = Dump.BytesFromDumpString(dump);

            var reader = new AsnReader(messageBytes, AsnEncodingRules.BER);

            var message = SnmpV3Message.ReadFrom(reader);

            Assert.NotNull(message);

            Assert.Equal(
                VersionCode.V3,
                message.ProtocolVersion);

            Assert.Equal(MsgFlag.Reportable, message.Header.MsgFlags);

            var usmSecParams = message.SecurityParameters;

            Assert.NotNull(usmSecParams);

            Assert.Empty(usmSecParams.EngineId.ToArray());

            Assert.Equal(0, usmSecParams.EngineBoots);

            Assert.Equal(SecurityModel.Usm, message.Header.MsgSecurityModel);
        }

        [Fact, TestPriority(1)]
        public void EncodeDecodeReportResponseMessage()
        {
            var dump = @"Received 127 byte packet from UDP: [127.0.0.1]:161->[0.0.0.0]:57524
                0000: 30 7D 02 01  03 30 11 02  04 51 06 5D  16 02 03 00    0}...0...Q.]....
                0016: FF E3 04 01  00 02 01 03  04 27 30 25  04 17 80 00    .........'0%....
                0032: 1F 88 04 38  30 30 30 30  30 30 32 30  31 30 39 38    ...8000000201098
                0048: 34 30 33 30  31 02 01 01  02 01 17 04  00 04 00 04    40301...........
                0064: 00 30 3C 04  17 80 00 1F  88 04 38 30  30 30 30 30    .0<.......800000
                0080: 30 32 30 31  30 39 38 34  30 33 30 31  04 00 A8 1F    020109840301....
                0096: 02 04 4E ED  B6 35 02 01  00 02 01 00  30 11 30 0F    ..N..5......0.0.
                0112: 06 0A 2B 06  01 06 03 0F  01 01 04 00  41 01 01       ..+.........A..";

            var messageBytes = Dump.BytesFromDumpString(dump);

            var reader = new AsnReader(messageBytes, AsnEncodingRules.BER);

            var message = SnmpV3Message.ReadFrom(reader);

            Assert.NotNull(message);

            Assert.Equal(
                VersionCode.V3,
                message.ProtocolVersion);

            Assert.Equal(MsgFlag.NoAuthNoPriv, message.Header.MsgFlags);

            var usmSecParams = message.SecurityParameters;

            Assert.NotNull(usmSecParams);

            Assert.Equal(SecurityModel.Usm, message.Header.MsgSecurityModel);

            var _engineId = usmSecParams.EngineId.ToArray();

            Assert.NotEmpty(_engineId);

            Assert.Equal(_engineId, Convert.FromBase64String(EngineIdBase64));

            Assert.Equal(1, usmSecParams.EngineBoots);

            Assert.True(usmSecParams.EngineId.Span.SequenceEqual(
                Convert.FromBase64String(EngineIdBase64)));
        }


        private void TestOutgoingAuthNoPriv(
            AuthenticationProtocol authProto,
            string dump)
        {
            var messageBytes = Dump.BytesFromDumpString(dump);

            var reader = new AsnReader(messageBytes, AsnEncodingRules.BER);

            var message = SnmpV3Message.ReadFrom(reader);

            Assert.NotNull(message);

            Assert.Equal(
                VersionCode.V3,
                message.ProtocolVersion);

            Assert.True(message.Header.MsgFlags.HasFlag(MsgFlag.Auth));

            Assert.False(message.Header.MsgFlags.HasFlag(MsgFlag.Priv));

            var usmSecParams = message.SecurityParameters;

            Assert.NotNull(usmSecParams);

            var _engineId = usmSecParams.EngineId.ToArray();

            Assert.NotEmpty(_engineId);

            Assert.Equal(_engineId, Convert.FromBase64String(EngineIdBase64));

            var authSerivice = GetAuthKey(authProto);

            var authParamsLen = authSerivice.TruncatedDigestSize;

            Assert.Equal(
                authParamsLen,
                message.SecurityParameters.AuthParams.Length);

            var digestBuffer = new byte[authSerivice.DigestSize];
            authSerivice.AuthenticateOutgoingMsg(message, digestBuffer);
            Assert.True(
                message.SecurityParameters.AuthParams.Length > 0);
        }

        private void TestIncomingAuthNoPriv(
            AuthenticationProtocol authProto,
            string dump)
        {
            var messageBytes = Dump.BytesFromDumpString(dump);

            var reader = new AsnReader(messageBytes, AsnEncodingRules.BER);

            var message = SnmpV3Message.ReadFrom(reader);

            Assert.NotNull(message);

            Assert.Equal(
                VersionCode.V3,
                message.ProtocolVersion);

            Assert.True(message.Header.MsgFlags.HasFlag(MsgFlag.Auth));

            Assert.False(message.Header.MsgFlags.HasFlag(MsgFlag.Priv));

            var usmSecParams = message.SecurityParameters;

            Assert.NotNull(usmSecParams);

            var _engineId = usmSecParams.EngineId.ToArray();

            Assert.NotEmpty(_engineId);

            Assert.Equal(_engineId, Convert.FromBase64String(EngineIdBase64));

            var authService = GetAuthKey(authProto);

            var authParamsLen = authService.TruncatedDigestSize;

            Assert.Equal(authParamsLen, message.SecurityParameters.AuthParams.Length);

            var authenticated = authService.AuthenticateIncomingMsg(message);

            Assert.True(authenticated);
        }

        [Fact, TestPriority(2)]
        public void EncodeDecodeGetRequest_AuthNoPriv_MD5()
        {
            var dump = @"Sending 146 bytes to UDP: [127.0.0.1]:161->[0.0.0.0]:0
                0000: 30 81 8F 02  01 03 30 10  02 04 51 06  5D 15 02 02    0.....0...Q.]...
                0016: 05 C0 04 01  05 02 01 03  04 3D 30 3B  04 17 80 00    .........=0;....
                0032: 1F 88 04 38  30 30 30 30  30 30 32 30  31 30 39 38    ...8000000201098
                0048: 34 30 33 30  31 02 01 01  02 01 17 04  0A 75 73 72    40301........usr
                0064: 5F 76 33 5F  4D 44 35 04  0C 44 4F CF  E5 51 A7 EF    _v3_MD5..DO..Q..
                0080: 91 3E FB A3  AD 04 00 30  39 04 17 80  00 1F 88 04    .>.....09.......
                0096: 38 30 30 30  30 30 30 32  30 31 30 39  38 34 30 33    8000000201098403
                0112: 30 31 04 00  A0 1C 02 04  4E ED B6 34  02 01 00 02    01......N..4....
                0128: 01 00 30 0E  30 0C 06 08  2B 06 01 02  01 01 03 00    ..0.0...+.......
                0144: 05 00                                                 ..";

            TestOutgoingAuthNoPriv(
                AuthenticationProtocol.Md5,
                dump);
        }

        [Fact, TestPriority(3)]
        public void EncodeDecodeGetResponse_AuthNoPriv_MD5()
        {
            var dump = @"Received 149 byte packet from UDP: [127.0.0.1]:161->[0.0.0.0]:38596
                0000: 30 81 92 02  01 03 30 11  02 04 27 8D  F9 98 02 03    0.....0...'.....
                0016: 00 FF E3 04  01 01 02 01  03 04 3D 30  3B 04 17 80    ..........=0;...
                0032: 00 1F 88 04  38 30 30 30  30 30 30 32  30 31 30 39    ....800000020109
                0048: 38 34 30 33  30 31 02 01  01 02 01 0A  04 0A 75 73    840301........us
                0064: 72 5F 76 33  5F 4D 44 35  04 0C F6 36  DF 50 9F 03    r_v3_MD5...6.P..
                0080: FA C3 BA A9  B9 57 04 00  30 3B 04 17  80 00 1F 88    .....W..0;......
                0096: 04 38 30 30  30 30 30 30  32 30 31 30  39 38 34 30    .800000020109840
                0112: 33 30 31 04  00 A2 1E 02  04 48 AA 65  4A 02 01 00    301......H.eJ...
                0128: 02 01 00 30  10 30 0E 06  08 2B 06 01  02 01 01 03    ...0.0...+......
                0144: 00 43 02 03  C5                                       .C...";

            TestIncomingAuthNoPriv(
                AuthenticationProtocol.Md5,
                dump);
        }

        [Fact, TestPriority(4)]
        public void EncodeDecodeGetRequet_AuthNoPriv_SHA()
        {
            var dump = @"Sending 146 bytes to UDP: [127.0.0.1]:161->[0.0.0.0]:0
                0000: 30 81 8F 02  01 03 30 10  02 04 55 73  8D B2 02 02    0.....0...Us....
                0016: 05 C0 04 01  05 02 01 03  04 3D 30 3B  04 17 80 00    .........=0;....
                0032: 1F 88 04 38  30 30 30 30  30 30 32 30  31 30 39 38    ...8000000201098
                0048: 34 30 33 30  31 02 01 12  02 01 04 04  0A 75 73 72    40301........usr
                0064: 5F 76 33 5F  53 48 41 04  0C C2 6D B3  62 72 CA C7    _v3_SHA...m.br..
                0080: D2 C8 BD E7  A3 04 00 30  39 04 17 80  00 1F 88 04    .......09.......
                0096: 38 30 30 30  30 30 30 32  30 31 30 39  38 34 30 33    8000000201098403
                0112: 30 31 04 00  A0 1C 02 04  21 D0 6A 25  02 01 00 02    01......!.j%....
                0128: 01 00 30 0E  30 0C 06 08  2B 06 01 02  01 01 03 00    ..0.0...+.......
                0144: 05 00 ";

            TestOutgoingAuthNoPriv(
                 AuthenticationProtocol.Sha1,
                 dump);
        }

        [Fact, TestPriority(5)]
        public void EncodeDecodeGetResponse_AuthNoPriv_SHA()
        {
            var dump = @"Received 148 byte packet from UDP: [127.0.0.1]:161->[0.0.0.0]:40858
                0000: 30 81 91 02  01 03 30 10  02 04 55 73  8D B2 02 02    0.....0...Us....
                0016: 05 C0 04 01  01 02 01 03  04 3D 30 3B  04 17 80 00    .........=0;....
                0032: 1F 88 04 38  30 30 30 30  30 30 32 30  31 30 39 38    ...8000000201098
                0048: 34 30 33 30  31 02 01 12  02 01 04 04  0A 75 73 72    40301........usr
                0064: 5F 76 33 5F  53 48 41 04  0C F6 B6 79  03 3D 52 C4    _v3_SHA....y.=R.
                0080: 1A 40 D6 EF  9E 04 00 30  3B 04 17 80  00 1F 88 04    .@.....0;.......
                0096: 38 30 30 30  30 30 30 32  30 31 30 39  38 34 30 33    8000000201098403
                0112: 30 31 04 00  A2 1E 02 04  21 D0 6A 25  02 01 00 02    01......!.j%....
                0128: 01 00 30 10  30 0E 06 08  2B 06 01 02  01 01 03 00    ..0.0...+.......
                0144: 43 02 01 80 ";

            TestIncomingAuthNoPriv(
                 AuthenticationProtocol.Sha1,
                 dump);
        }

        [Fact, TestPriority(6)]
        public void EncodeDecodeGetRequet_AuthNoPriv_SHA256()
        {
            var dump = @"Sending 162 bytes to UDP: [127.0.0.1]:161->[0.0.0.0]:0
                0000: 30 81 9F 02  01 03 30 10  02 04 4C EF  12 0D 02 02    0.....0...L.....
                0016: 05 C0 04 01  05 02 01 03  04 4D 30 4B  04 17 80 00    .........M0K....
                0032: 1F 88 04 38  30 30 30 30  30 30 32 30  31 30 39 38    ...8000000201098
                0048: 34 30 33 30  31 02 01 12  02 02 03 AC  04 0D 75 73    40301.........us
                0064: 72 5F 76 33  5F 53 48 41  32 35 36 04  18 BB 96 E3    r_v3_SHA256.....
                0080: 59 DD EA 5A  43 80 67 E9  A9 92 F5 6D  32 BD 40 3A    Y..ZC.g....m2.@:
                0096: 4B 2E 09 A7  9B 04 00 30  39 04 17 80  00 1F 88 04    K......09.......
                0112: 38 30 30 30  30 30 30 32  30 31 30 39  38 34 30 33    8000000201098403
                0128: 30 31 04 00  A0 1C 02 04  6D 93 9F BB  02 01 00 02    01......m.......
                0144: 01 00 30 0E  30 0C 06 08  2B 06 01 02  01 01 03 00    ..0.0...+.......
                0160: 05 00                                                 ..";

            TestOutgoingAuthNoPriv(
                 AuthenticationProtocol.Sha256,
                 dump);
        }

        [Fact, TestPriority(7)]
        public void EncodeDecodeGetResponse_AuthNoPriv_SHA256()
        {
            var dump = @"Received 165 byte packet from UDP: [127.0.0.1]:161->[0.0.0.0]:44990
                0000: 30 81 A2 02  01 03 30 10  02 04 4C EF  12 0D 02 02    0.....0...L.....
                0016: 05 C0 04 01  01 02 01 03  04 4D 30 4B  04 17 80 00    .........M0K....
                0032: 1F 88 04 38  30 30 30 30  30 30 32 30  31 30 39 38    ...8000000201098
                0048: 34 30 33 30  31 02 01 12  02 02 03 AC  04 0D 75 73    40301.........us
                0064: 72 5F 76 33  5F 53 48 41  32 35 36 04  18 CB E3 A8    r_v3_SHA256.....
                0080: 68 32 C1 39  32 F7 11 D3  AC 63 56 C7  36 12 3D BE    h2.92....cV.6.=.
                0096: DC DD EB C4  41 04 00 30  3C 04 17 80  00 1F 88 04    ....A..0<.......
                0112: 38 30 30 30  30 30 30 32  30 31 30 39  38 34 30 33    8000000201098403
                0128: 30 31 04 00  A2 1F 02 04  6D 93 9F BB  02 01 00 02    01......m.......
                0144: 01 00 30 11  30 0F 06 08  2B 06 01 02  01 01 03 00    ..0.0...+.......
                0160: 43 03 01 6F  58                                       C..oX";

            TestIncomingAuthNoPriv(
                 AuthenticationProtocol.Sha256,
                 dump);
        }

        [Fact, TestPriority(8)]
        public void EncodeDecodeGetRequet_AuthNoPriv_SHA384()
        {
            var dump = @"Sending 170 bytes to UDP: [127.0.0.1]:161->[0.0.0.0]:0
                0000: 30 81 A7 02  01 03 30 10  02 04 1B E8  12 D7 02 02    0.....0.........
                0016: 05 C0 04 01  05 02 01 03  04 55 30 53  04 17 80 00    .........U0S....
                0032: 1F 88 04 38  30 30 30 30  30 30 32 30  31 30 39 38    ...8000000201098
                0048: 34 30 33 30  31 02 01 12  02 02 0D 55  04 0D 75 73    40301......U..us
                0064: 72 5F 76 33  5F 53 48 41  33 38 34 04  20 E8 F3 B6    r_v3_SHA384.....
                0080: 3D 1D 4C 9C  33 2F 46 EE  B2 42 C6 1F  76 4A 39 56    =.L.3/F..B..vJ9V
                0096: 6D 18 72 C8  90 51 EB EA  E0 D5 1E FD  14 04 00 30    m.r..Q.........0
                0112: 39 04 17 80  00 1F 88 04  38 30 30 30  30 30 30 32    9.......80000002
                0128: 30 31 30 39  38 34 30 33  30 31 04 00  A0 1C 02 04    0109840301......
                0144: 45 4B 70 68  02 01 00 02  01 00 30 0E  30 0C 06 08    EKph......0.0...
                0160: 2B 06 01 02  01 01 03 00  05 00                       +.........";

            TestOutgoingAuthNoPriv(
                 AuthenticationProtocol.Sha384,
                 dump);
        }

        [Fact, TestPriority(9)]
        public void EncodeDecodeGetResponse_AuthNoPriv_SHA384()
        {
            var dump = @"Received 173 byte packet from UDP: [127.0.0.1]:161->[0.0.0.0]:56250
                0000: 30 81 AA 02  01 03 30 10  02 04 1B E8  12 D7 02 02    0.....0.........
                0016: 05 C0 04 01  01 02 01 03  04 55 30 53  04 17 80 00    .........U0S....
                0032: 1F 88 04 38  30 30 30 30  30 30 32 30  31 30 39 38    ...8000000201098
                0048: 34 30 33 30  31 02 01 12  02 02 0D 55  04 0D 75 73    40301......U..us
                0064: 72 5F 76 33  5F 53 48 41  33 38 34 04  20 E6 87 B5    r_v3_SHA384.....
                0080: 8D B4 FB A5  56 D8 83 65  24 D1 FA AC  DB FE 98 66    ....V..e$......f
                0096: D3 94 0B 4F  F4 F2 9A 46  01 EA 5B CD  A9 04 00 30    ...O...F..[....0
                0112: 3C 04 17 80  00 1F 88 04  38 30 30 30  30 30 30 32    <.......80000002
                0128: 30 31 30 39  38 34 30 33  30 31 04 00  A2 1F 02 04    0109840301......
                0144: 45 4B 70 68  02 01 00 02  01 00 30 11  30 0F 06 08    EKph......0.0...
                0160: 2B 06 01 02  01 01 03 00  43 03 05 35  1F             +.......C..5.";

            TestIncomingAuthNoPriv(
                 AuthenticationProtocol.Sha384,
                 dump);
        }

        [Fact, TestPriority(10)]
        public void EncodeDecodeGetRequet_AuthNoPriv_SHA512()
        {
            var dump = @"Sending 185 bytes to UDP: [127.0.0.1]:161->[0.0.0.0]:0
                0000: 30 81 B6 02  01 03 30 10  02 04 50 67  61 3C 02 02    0.....0...Pga<..
                0016: 05 C0 04 01  05 02 01 03  04 65 30 63  04 17 80 00    .........e0c....
                0032: 1F 88 04 38  30 30 30 30  30 30 32 30  31 30 39 38    ...8000000201098
                0048: 34 30 33 30  31 02 01 12  02 02 0D EE  04 0D 75 73    40301.........us
                0064: 72 5F 76 33  5F 53 48 41  35 31 32 04  30 99 5B 1B    r_v3_SHA512.0.[.
                0080: 30 51 90 49  32 6F 58 3B  3C 82 29 8C  6D 98 17 DE    0Q.I2oX;<.).m...
                0096: F1 29 D8 74  DA 74 DD 80  72 B0 34 DC  85 9D 03 2C    .).t.t..r.4....,
                0112: 49 50 AB 32  62 08 86 6E  F4 E7 74 59  29 04 00 30    IP.2b..n..tY)..0
                0128: 38 04 17 80  00 1F 88 04  38 30 30 30  30 30 30 32    8.......80000002
                0144: 30 31 30 39  38 34 30 33  30 31 04 00  A0 1B 02 03    0109840301......
                0160: 7A 22 53 02  01 00 02 01  00 30 0E 30  0C 06 08 2B    z""S......0.0...+
                0176: 06 01 02 01  01 03 00 05  00                          .........";

            TestOutgoingAuthNoPriv(
                 AuthenticationProtocol.Sha512,
                 dump);
        }

        [Fact, TestPriority(11)]
        public void EncodeDecodeGetResponse_AuthNoPriv_SHA512()
        {
            var dump = @"Received 188 byte packet from UDP: [127.0.0.1]:161->[0.0.0.0]:42946
                0000: 30 81 B9 02  01 03 30 10  02 04 50 67  61 3C 02 02    0.....0...Pga<..
                0016: 05 C0 04 01  01 02 01 03  04 65 30 63  04 17 80 00    .........e0c....
                0032: 1F 88 04 38  30 30 30 30  30 30 32 30  31 30 39 38    ...8000000201098
                0048: 34 30 33 30  31 02 01 12  02 02 0D EE  04 0D 75 73    40301.........us
                0064: 72 5F 76 33  5F 53 48 41  35 31 32 04  30 80 0A F2    r_v3_SHA512.0...
                0080: 8D 50 6C 5E  38 9B 5D 25  62 94 73 A1  9E EB F4 07    .Pl^8.]%b.s.....
                0096: 74 D4 9B 59  B7 AC C5 9F  4F BC 0D DE  0B 37 76 DB    t..Y....O....7v.
                0112: 68 DE 56 A9  34 73 37 E6  44 60 F8 D9  E1 04 00 30    h.V.4s7.D`.....0
                0128: 3B 04 17 80  00 1F 88 04  38 30 30 30  30 30 30 32    ;.......80000002
                0144: 30 31 30 39  38 34 30 33  30 31 04 00  A2 1E 02 03    0109840301......
                0160: 7A 22 53 02  01 00 02 01  00 30 11 30  0F 06 08 2B    z""S......0.0...+
                0176: 06 01 02 01  01 03 00 43  03 05 70 DC                 .......C..p.";

            TestIncomingAuthNoPriv(
                 AuthenticationProtocol.Sha512,
                 dump);
        }
    }
}
