using System.Security.Cryptography;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Protocol.V3.Security.Authentication;

namespace Lextm.SharpSnmpLib.Security;

/// <summary>
/// Legacy facade for default authentication provider singleton.
/// </summary>
public static class DefaultAuthenticationProvider
{
    public static IAuthenticationProvider Instance => DotNetSnmp.Protocol.V3.Security.Authentication.DefaultAuthenticationProvider.Instance;
}

public sealed class MD5AuthenticationProvider : DotNetSnmp.Protocol.V3.Security.Authentication.MD5AuthenticationProvider
{
    public MD5AuthenticationProvider(OctetString passphrase)
        : base(passphrase.Octets)
    {
    }
}

public sealed class SHA1AuthenticationProvider : DotNetSnmp.Protocol.V3.Security.Authentication.SHA1AuthenticationProvider
{
    public SHA1AuthenticationProvider(OctetString passphrase)
        : base(passphrase.Octets)
    {
    }
}

public sealed class SHA256AuthenticationProvider : DotNetSnmp.Protocol.V3.Security.Authentication.SHA256AuthenticationProvider
{
    public SHA256AuthenticationProvider(OctetString passphrase)
        : base(passphrase.Octets)
    {
    }
}

public sealed class SHA384AuthenticationProvider : DotNetSnmp.Protocol.V3.Security.Authentication.SHA384AuthenticationProvider
{
    public SHA384AuthenticationProvider(OctetString passphrase)
        : base(passphrase.Octets)
    {
    }
}

public sealed class SHA512AuthenticationProvider : DotNetSnmp.Protocol.V3.Security.Authentication.SHA512AuthenticationProvider
{
    public SHA512AuthenticationProvider(OctetString passphrase)
        : base(passphrase.Octets)
    {
    }
}

public class DefaultPrivacyProvider : DotNetSnmp.Protocol.V3.Security.Privacy.DefaultPrivacyProvider
{
    private static readonly DotNetSnmp.Protocol.V3.Security.Privacy.IPrivacyProvider _defaultPair =
        new DefaultPrivacyProvider(DotNetSnmp.Protocol.V3.Security.Authentication.DefaultAuthenticationProvider.Instance);

    public static DotNetSnmp.Protocol.V3.Security.Privacy.IPrivacyProvider DefaultPair => _defaultPair;

    public DefaultPrivacyProvider()
        : base()
    {
    }

    public DefaultPrivacyProvider(IAuthenticationProvider authenticationProvider)
        : base(authenticationProvider)
    {
    }
}

public sealed class DESPrivacyProvider : DotNetSnmp.Protocol.V3.Security.Privacy.DESPrivacyProvider
{
    public static bool IsSupported
    {
        get
        {
            try
            {
                using var _ = DES.Create();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public DESPrivacyProvider(OctetString passphrase, IAuthenticationProvider authenticationProvider)
        : base(authenticationProvider, passphrase.Octets)
    {
    }
}

public sealed class TripleDESPrivacyProvider : DotNetSnmp.Protocol.V3.Security.Privacy.TripleDESPrivacyProvider
{
    public TripleDESPrivacyProvider(OctetString passphrase, IAuthenticationProvider authenticationProvider)
        : base(authenticationProvider, passphrase.Octets)
    {
    }
}

public class AESPrivacyProvider : DotNetSnmp.Protocol.V3.Security.Privacy.AESPrivacyProvider
{
    public static bool IsSupported
    {
        get
        {
            try
            {
                using var _ = Aes.Create();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public AESPrivacyProvider(OctetString passphrase, IAuthenticationProvider authenticationProvider)
        : base(authenticationProvider, passphrase.Octets)
    {
    }
}

public sealed class AES192PrivacyProvider : DotNetSnmp.Protocol.V3.Security.Privacy.AES192PrivacyProvider
{
    public AES192PrivacyProvider(OctetString passphrase, IAuthenticationProvider authenticationProvider)
        : base(authenticationProvider, passphrase.Octets)
    {
    }
}

public sealed class AES256PrivacyProvider : DotNetSnmp.Protocol.V3.Security.Privacy.AES256PrivacyProvider
{
    public AES256PrivacyProvider(OctetString passphrase, IAuthenticationProvider authenticationProvider)
        : base(authenticationProvider, passphrase.Octets)
    {
    }
}
