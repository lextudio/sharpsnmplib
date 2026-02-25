using DotNetSnmp.Protocol.V3.Security.Authentication;
using System.Text;

namespace DotNetSnmp.Test
{
    internal static class TestUtils
    {
        internal const string TestPassword = "Password1";

        internal const string EngineIdBase64 = "gAAfiAQ4MDAwMDAwMjAxMDk4NDAzMDE=";

        internal static IAuthenticationProvider GetAuthKey(AuthenticationProtocol protocol = AuthenticationProtocol.Md5)
        {
            switch (protocol)
            {
                case AuthenticationProtocol.Md5:
                    return new MD5AuthenticationProvider(TestPassword.GetBytesMemoryOrDefault(Encoding.UTF8));
                case AuthenticationProtocol.Sha1:
                    return new SHA1AuthenticationProvider(TestPassword.GetBytesMemoryOrDefault(Encoding.UTF8));
                case AuthenticationProtocol.Sha256:
                    return new SHA256AuthenticationProvider(TestPassword.GetBytesMemoryOrDefault(Encoding.UTF8));
                case AuthenticationProtocol.Sha384:
                    return new SHA384AuthenticationProvider(TestPassword.GetBytesMemoryOrDefault(Encoding.UTF8));
                case AuthenticationProtocol.Sha512:
                    return new SHA512AuthenticationProvider(TestPassword.GetBytesMemoryOrDefault(Encoding.UTF8));
                default:
                    throw new NotSupportedException($"Unsupported authentication protocol: {protocol}");
            }
        }
    }
}
