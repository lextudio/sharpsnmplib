using System.Formats.Asn1;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;
using Xunit;

namespace DotNetSnmp.Test
{
    /// <summary>
    /// End-to-end coverage for privacy protocols that need the password-to-key algorithm to be
    /// chained, i.e. where the auth digest is shorter than the privacy key. The MD5 (16 octet)
    /// combinations with 3DES and AES-256 are the ones affected by #705; AES-192 and the
    /// SHA-1 combinations are included to guard against regressing them.
    /// </summary>
    public class PrivacyKeyChainingRoundTripTest
    {
        private static readonly byte[] _engineId = Convert.FromHexString("80004FB805636C6F75644DAB22CC");

        private static OctetString Auth() => new("authkey1");

        private static OctetString Priv() => new("privkey1");

        public static TheoryData<string, Func<IPrivacyProvider>> Cases => new()
        {
            { "3DES+MD5", () => new TripleDESPrivacyProvider(new MD5AuthenticationProvider(Auth()), Priv().Octets) },
            { "3DES+SHA1", () => new TripleDESPrivacyProvider(new SHA1AuthenticationProvider(Auth()), Priv().Octets) },
            { "AES128+MD5", () => new AESPrivacyProvider(new MD5AuthenticationProvider(Auth()), Priv().Octets) },
            { "AES192+MD5", () => new AES192PrivacyProvider(new MD5AuthenticationProvider(Auth()), Priv().Octets) },
            { "AES256+MD5", () => new AES256PrivacyProvider(new MD5AuthenticationProvider(Auth()), Priv().Octets) },
            { "AES256+SHA1", () => new AES256PrivacyProvider(new SHA1AuthenticationProvider(Auth()), Priv().Octets) },
            { "DES+MD5", () => new DESPrivacyProvider(new MD5AuthenticationProvider(Auth()), Priv().Octets) },
        };

        private static SnmpV3Message CreateMessage()
        {
            var scope = new Scope(
                new OctetString(_engineId),
                OctetString.Empty,
                new GetRequestPdu(0x3A25, new List<Variable>
                {
                    new(new ObjectIdentifier("1.3.6.1.2.1.1.3.0")),
                    new(new ObjectIdentifier("1.3.6.1.2.1.1.1.0"), new OctetString("Test system")),
                }));

            return new SnmpV3Message
            {
                Header = new HeaderData
                {
                    MsgId = 0x1639993C,
                    MsgMaxSize = 0xFFE3,
                    MsgFlags = MsgFlag.Auth | MsgFlag.Priv | MsgFlag.Reportable,
                    MsgSecurityModel = SecurityModel.Usm,
                },
                SecurityParameters = new UsmSecurityParameters
                {
                    EngineId = _engineId,
                    EngineBoots = 1,
                    EngineTime = 0x35,
                    SecurityName = new OctetString("usr-md5-3des"),
                    AuthParams = new byte[12],
                    PrivParams = new byte[8],
                },
                Scope = scope,
            };
        }

        [Theory]
        [MemberData(nameof(Cases))]
        public void EncryptMessageThenDecryptMessage_RestoresScope(string name, Func<IPrivacyProvider> factory)
        {
            var privacy = factory();
            var message = CreateMessage();
            var expected = ((Scope)message.Scope!).GetData(VersionCode.V3);

            privacy.EncryptMessage(message);

            Assert.Null(message.Scope);
            Assert.False(message.EncryptedScopedPdu.IsEmpty);

            // A wrong pre-IV or key yields an AsnContentException or mismatched contents here.
            privacy.DecryptMessage(message);

            Assert.NotNull(message.Scope);
            Assert.Equal(
                Convert.ToHexString(expected.ToBytes()),
                Convert.ToHexString(((Scope)message.Scope!).GetData(VersionCode.V3).ToBytes()));
        }

        [Theory]
        [MemberData(nameof(Cases))]
        public void DecryptMessageRejectsTamperedScope(string name, Func<IPrivacyProvider> factory)
        {
            // Confirms the derived key really is load-bearing: corrupting one cipher block
            // must not still parse back into the original scope.
            var privacy = factory();
            var message = CreateMessage();

            privacy.EncryptMessage(message);

            var ct = message.EncryptedScopedPdu.ToArray();
            ct[0] ^= 0xFF;
            message.EncryptedScopedPdu = ct;

            try
            {
                privacy.DecryptMessage(message);
            }
            catch (AsnContentException)
            {
                return; // expected: garbage does not parse
            }

            Assert.NotEqual(
                Convert.ToHexString(CreateMessage().Scope!.GetData(VersionCode.V3).ToBytes()),
                Convert.ToHexString(((Scope)message.Scope!).GetData(VersionCode.V3).ToBytes()));
        }
    }
}
