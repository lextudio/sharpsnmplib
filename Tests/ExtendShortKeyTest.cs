using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;
using Xunit;

namespace DotNetSnmp.Test
{
    /// <summary>
    /// Tests for <see cref="TripleDESPrivacyProvider.ExtendShortKey"/> key extension,
    /// per draft-reeder-snmpv3-usm-3desede-00 Appendix B.1.
    /// </summary>
    public class ExtendShortKeyTest
    {
        private static readonly byte[] _engineId = Convert.FromHexString("000000000000000000000002");

        private static string Hex(ReadOnlySpan<byte> bytes) => Convert.ToHexString(bytes);

        private static byte[] HexBytes(string spaced) => Convert.FromHexString(spaced.Replace(" ", ""));

        [Fact]
        public void ExtendShortKey_MD5_MatchesDraftTestVector()
        {
            // draft-reeder-snmpv3-usm-3desede-00 Appendix B.1, MD5 case
            var passcode = new OctetString("maplesyrup");
            byte[] expectedKey = HexBytes(
                "52 6f 5e ed 9f cc e2 6f 89 64 c2 93 07 87 d8 2b 79 ef f4 4a " +
                "90 65 0e e0 a3 a4 0a bf ac 5a cc 12");

            IAuthenticationProvider auth = new MD5AuthenticationProvider(passcode);
            const int keyLength = 32;

            Span<byte> initialKey = stackalloc byte[auth.DigestSize];
            auth.PasswordToKey(passcode.Octets, _engineId, initialKey);

            Span<byte> localizedKey = stackalloc byte[keyLength];
            TripleDESPrivacyProvider.ExtendShortKey(
                initialKey, keyLength, auth, _engineId, localizedKey);

            Assert.Equal(Hex(expectedKey), Hex(localizedKey));
        }

        [Fact]
        public void ExtendShortKey_MD5_PreIVMatchesDraftTestVector()
        {
            // The 8 octets at offset 24..31 form the pre-IV; if extension is wrong,
            // sender and receiver derive different pre-IVs and decryption fails.
            var passcode = new OctetString("maplesyrup");
            byte[] expectedKey = HexBytes(
                "52 6f 5e ed 9f cc e2 6f 89 64 c2 93 07 87 d8 2b 79 ef f4 4a " +
                "90 65 0e e0 a3 a4 0a bf ac 5a cc 12");

            IAuthenticationProvider auth = new MD5AuthenticationProvider(passcode);
            const int keyLength = 32;

            Span<byte> initialKey = stackalloc byte[auth.DigestSize];
            auth.PasswordToKey(passcode.Octets, _engineId, initialKey);

            Span<byte> localizedKey = stackalloc byte[keyLength];
            TripleDESPrivacyProvider.ExtendShortKey(
                initialKey, keyLength, auth, _engineId, localizedKey);

            Assert.Equal(
                Hex(expectedKey.AsSpan(24, 8)),
                Hex(localizedKey.Slice(24, 8)));
        }

        [Fact]
        public void ExtendShortKey_SHA1_PreIVMatchesDraftTestVector()
        {
            // draft-reeder-snmpv3-usm-3desede-00 Appendix B.1, SHA-1 case.
            // SHA-1 works only because (32 - 20) < 20.
            var passcode = new OctetString("maplesyrup");
            byte[] expectedKey = HexBytes(
                "66 95 fe bc 92 88 e3 62 82 23 5f c7 15 1f 12 84 97 b3 8f " +
                "3f 9b 8b 6d 78 93 6b a6 e7 d1 9d fd 9c d2 d5 06 55 47 74 3f b5");

            IAuthenticationProvider auth = new SHA1AuthenticationProvider(passcode);
            const int keyLength = 32;

            Span<byte> initialKey = stackalloc byte[auth.DigestSize];
            auth.PasswordToKey(passcode.Octets, _engineId, initialKey);

            Span<byte> localizedKey = stackalloc byte[keyLength];
            TripleDESPrivacyProvider.ExtendShortKey(
                initialKey, keyLength, auth, _engineId, localizedKey);

            Assert.Equal(
                Hex(expectedKey.AsSpan(24, 8)),
                Hex(localizedKey.Slice(24, 8)));
        }

        [Fact]
        public void ExtendShortKey_MD5_Key32_IsChainedNotTruncated()
        {
            // Regression test for #705: when DigestSize does not evenly divide the gap,
            // the previous code advanced by TruncatedDigestSize, re-ran password-to-key and
            // duplicated the leading octets of the second key block. Assert the tail is
            // exactly K2 with no repetition.
            var passcode = new OctetString("maplesyrup");
            IAuthenticationProvider auth = new MD5AuthenticationProvider(passcode);
            const int keyLength = 32;

            Span<byte> initialKey = stackalloc byte[auth.DigestSize];
            auth.PasswordToKey(passcode.Octets, _engineId, initialKey);

            Span<byte> localizedKey = stackalloc byte[keyLength];
            TripleDESPrivacyProvider.ExtendShortKey(
                initialKey, keyLength, auth, _engineId, localizedKey);

            Span<byte> k2 = stackalloc byte[auth.DigestSize];
            auth.PasswordToKey(initialKey.ToArray(), _engineId, k2);

            Assert.Equal(Hex(k2), Hex(localizedKey.Slice(auth.DigestSize, keyLength - auth.DigestSize)));
        }

        [Fact]
        public void ExtendShortKey_MD5_Key24_ForAES192_UnaffectedByTruncation()
        {
            // AES-192 needs only 8 more octets, which is below TruncatedDigestSize (12),
            // so this path was never broken. Guards against regressing it.
            var passcode = new OctetString("maplesyrup");
            IAuthenticationProvider auth = new MD5AuthenticationProvider(passcode);
            const int keyLength = 24;

            Span<byte> initialKey = stackalloc byte[auth.DigestSize];
            auth.PasswordToKey(passcode.Octets, _engineId, initialKey);

            Span<byte> localizedKey = stackalloc byte[keyLength];
            TripleDESPrivacyProvider.ExtendShortKey(
                initialKey, keyLength, auth, _engineId, localizedKey);

            Span<byte> k2 = stackalloc byte[auth.DigestSize];
            auth.PasswordToKey(initialKey.ToArray(), _engineId, k2);

            Assert.Equal(Hex(k2[..8]), Hex(localizedKey.Slice(auth.DigestSize, 8)));
        }

        [Fact]
        public void ExtendShortKey_MD5_Key32_ForAES256_IsChained()
        {
            // AES-256 with MD5 hits the same duplicated-tail bug: 32 - 16 == 16, which is
            // greater than TruncatedDigestSize (12).
            var passcode = new OctetString("maplesyrup");
            IAuthenticationProvider auth = new MD5AuthenticationProvider(passcode);
            const int keyLength = 32;

            Span<byte> initialKey = stackalloc byte[auth.DigestSize];
            auth.PasswordToKey(passcode.Octets, _engineId, initialKey);

            Span<byte> localizedKey = stackalloc byte[keyLength];
            TripleDESPrivacyProvider.ExtendShortKey(
                initialKey, keyLength, auth, _engineId, localizedKey);

            Span<byte> k2 = stackalloc byte[auth.DigestSize];
            auth.PasswordToKey(initialKey.ToArray(), _engineId, k2);

            Assert.Equal(Hex(k2), Hex(localizedKey.Slice(auth.DigestSize, 16)));
        }
    }
}
