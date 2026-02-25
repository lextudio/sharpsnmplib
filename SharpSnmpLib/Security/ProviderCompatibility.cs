using System.Security.Cryptography;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Protocol.V3.Security.Authentication;

namespace Lextm.SharpSnmpLib.Security;

/// <summary>
/// Legacy facade for default authentication provider singleton.
/// </summary>
public static class DefaultAuthenticationProvider
{
    /// <summary>
    /// Represents instance.
    /// </summary>
    public static IAuthenticationProvider Instance => DotNetSnmp.Protocol.V3.Security.Authentication.DefaultAuthenticationProvider.Instance;
}

/// <summary>
/// Represents the MD5AuthenticationProvider type.
/// </summary>
public sealed class MD5AuthenticationProvider : DotNetSnmp.Protocol.V3.Security.Authentication.MD5AuthenticationProvider
{
    /// <summary>
    /// Initializes a new instance of MD5AuthenticationProvider.
    /// </summary>
    public MD5AuthenticationProvider(OctetString passphrase)
        : base(passphrase.Octets)
    {
    }
}

/// <summary>
/// Represents the SHA1AuthenticationProvider type.
/// </summary>
public sealed class SHA1AuthenticationProvider : DotNetSnmp.Protocol.V3.Security.Authentication.SHA1AuthenticationProvider
{
    /// <summary>
    /// Initializes a new instance of SHA1AuthenticationProvider.
    /// </summary>
    public SHA1AuthenticationProvider(OctetString passphrase)
        : base(passphrase.Octets)
    {
    }
}

/// <summary>
/// Represents the SHA256AuthenticationProvider type.
/// </summary>
public sealed class SHA256AuthenticationProvider : DotNetSnmp.Protocol.V3.Security.Authentication.SHA256AuthenticationProvider
{
    /// <summary>
    /// Initializes a new instance of SHA256AuthenticationProvider.
    /// </summary>
    public SHA256AuthenticationProvider(OctetString passphrase)
        : base(passphrase.Octets)
    {
    }
}

/// <summary>
/// Represents the SHA384AuthenticationProvider type.
/// </summary>
public sealed class SHA384AuthenticationProvider : DotNetSnmp.Protocol.V3.Security.Authentication.SHA384AuthenticationProvider
{
    /// <summary>
    /// Initializes a new instance of SHA384AuthenticationProvider.
    /// </summary>
    public SHA384AuthenticationProvider(OctetString passphrase)
        : base(passphrase.Octets)
    {
    }
}

/// <summary>
/// Represents the SHA512AuthenticationProvider type.
/// </summary>
public sealed class SHA512AuthenticationProvider : DotNetSnmp.Protocol.V3.Security.Authentication.SHA512AuthenticationProvider
{
    /// <summary>
    /// Initializes a new instance of SHA512AuthenticationProvider.
    /// </summary>
    public SHA512AuthenticationProvider(OctetString passphrase)
        : base(passphrase.Octets)
    {
    }
}

/// <summary>
/// Represents the DefaultPrivacyProvider type.
/// </summary>
public class DefaultPrivacyProvider : DotNetSnmp.Protocol.V3.Security.Privacy.DefaultPrivacyProvider
{
    private static readonly DotNetSnmp.Protocol.V3.Security.Privacy.IPrivacyProvider _defaultPair =
        new DefaultPrivacyProvider(DotNetSnmp.Protocol.V3.Security.Authentication.DefaultAuthenticationProvider.Instance);

    /// <summary>
    /// Stores default Pair.
    /// </summary>
    public static DotNetSnmp.Protocol.V3.Security.Privacy.IPrivacyProvider DefaultPair => _defaultPair;

    /// <summary>
    /// Initializes a new instance of DefaultPrivacyProvider.
    /// </summary>
    public DefaultPrivacyProvider()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of DefaultPrivacyProvider.
    /// </summary>
    public DefaultPrivacyProvider(IAuthenticationProvider authenticationProvider)
        : base(authenticationProvider)
    {
    }
}

/// <summary>
/// Represents the DESPrivacyProvider type.
/// </summary>
public sealed class DESPrivacyProvider : DotNetSnmp.Protocol.V3.Security.Privacy.DESPrivacyProvider
{
    /// <summary>
    /// Represents this member.
    /// </summary>
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

    /// <summary>
    /// Initializes a new instance of DESPrivacyProvider.
    /// </summary>
    public DESPrivacyProvider(OctetString passphrase, IAuthenticationProvider authenticationProvider)
        : base(authenticationProvider, passphrase.Octets)
    {
    }
}

/// <summary>
/// Represents the TripleDESPrivacyProvider type.
/// </summary>
public sealed class TripleDESPrivacyProvider : DotNetSnmp.Protocol.V3.Security.Privacy.TripleDESPrivacyProvider
{
    /// <summary>
    /// Initializes a new instance of TripleDESPrivacyProvider.
    /// </summary>
    public TripleDESPrivacyProvider(OctetString passphrase, IAuthenticationProvider authenticationProvider)
        : base(authenticationProvider, passphrase.Octets)
    {
    }
}

/// <summary>
/// Represents the AESPrivacyProvider type.
/// </summary>
public class AESPrivacyProvider : DotNetSnmp.Protocol.V3.Security.Privacy.AESPrivacyProvider
{
    /// <summary>
    /// Represents this member.
    /// </summary>
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

    /// <summary>
    /// Initializes a new instance of AESPrivacyProvider.
    /// </summary>
    public AESPrivacyProvider(OctetString passphrase, IAuthenticationProvider authenticationProvider)
        : base(authenticationProvider, passphrase.Octets)
    {
    }
}

/// <summary>
/// Represents the AES192PrivacyProvider type.
/// </summary>
public sealed class AES192PrivacyProvider : DotNetSnmp.Protocol.V3.Security.Privacy.AES192PrivacyProvider
{
    /// <summary>
    /// Initializes a new instance of AES192PrivacyProvider.
    /// </summary>
    public AES192PrivacyProvider(OctetString passphrase, IAuthenticationProvider authenticationProvider)
        : base(authenticationProvider, passphrase.Octets)
    {
    }
}

/// <summary>
/// Represents the AES256PrivacyProvider type.
/// </summary>
public sealed class AES256PrivacyProvider : DotNetSnmp.Protocol.V3.Security.Privacy.AES256PrivacyProvider
{
    /// <summary>
    /// Initializes a new instance of AES256PrivacyProvider.
    /// </summary>
    public AES256PrivacyProvider(OctetString passphrase, IAuthenticationProvider authenticationProvider)
        : base(authenticationProvider, passphrase.Octets)
    {
    }
}
