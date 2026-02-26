using System.Collections.Generic;
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

    /// <summary>
    /// Gets or sets known engine IDs (legacy compatibility member).
    /// </summary>
    public ICollection<OctetString>? EngineIds { get; set; }
}

/// <summary>
/// Represents the DESPrivacyProvider type.
/// </summary>
public sealed class DESPrivacyProvider : DotNetSnmp.Protocol.V3.Security.Privacy.DESPrivacyProvider
{
    private const int LegacyPrivacyParametersLength = 8;
    private const int LegacyMinimumKeyLength = 16;
    internal ReadOnlyMemory<byte> Passphrase { get; }

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
    public DESPrivacyProvider(OctetString? passphrase, IAuthenticationProvider authenticationProvider)
        : this(RequirePassphrase(passphrase), authenticationProvider)
    {
    }

    private DESPrivacyProvider(byte[] passphrase, IAuthenticationProvider authenticationProvider)
        : base(authenticationProvider ?? throw new ArgumentNullException(nameof(authenticationProvider)), passphrase)
    {
        Passphrase = passphrase;
    }

    /// <summary>
    /// Encrypts scoped PDU payload bytes using legacy DES helper signature.
    /// </summary>
    public static byte[] Encrypt(byte[] unencryptedData, byte[] key, byte[] privacyParameters)
    {
        if (!IsSupported)
        {
            throw new PlatformNotSupportedException();
        }

        if (unencryptedData == null)
        {
            throw new ArgumentNullException(nameof(unencryptedData));
        }

        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (privacyParameters == null)
        {
            throw new ArgumentNullException(nameof(privacyParameters));
        }

        if (privacyParameters.Length != LegacyPrivacyParametersLength)
        {
            throw new ArgumentOutOfRangeException(nameof(privacyParameters), "Privacy parameters argument has to be 8 bytes long.");
        }

        if (key.Length < LegacyMinimumKeyLength)
        {
            throw new ArgumentException($"Encryption key length has to 16 bytes or more. Current: {key.Length}.", nameof(key));
        }

        var iv = GetLegacyIv(key, privacyParameters);
        var outKey = GetLegacyKey(key);

        if ((unencryptedData.Length % 8) != 0)
        {
            var tmpBuffer = new byte[8 * ((unencryptedData.Length / 8) + 1)];
            Buffer.BlockCopy(unencryptedData, 0, tmpBuffer, 0, unencryptedData.Length);
            unencryptedData = tmpBuffer;
        }

        using var des = DES.Create();
        des.Key = outKey;
        return des.EncryptCbc(unencryptedData, iv, PaddingMode.None);
    }

    /// <summary>
    /// Decrypts scoped PDU payload bytes using legacy DES helper signature.
    /// </summary>
    public static byte[] Decrypt(byte[] encryptedData, byte[] key, byte[] privacyParameters)
    {
        if (!IsSupported)
        {
            throw new PlatformNotSupportedException();
        }

        if (encryptedData == null)
        {
            throw new ArgumentNullException(nameof(encryptedData));
        }

        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (privacyParameters == null)
        {
            throw new ArgumentNullException(nameof(privacyParameters));
        }

        if (encryptedData.Length == 0)
        {
            throw new ArgumentException("Empty encrypted data.", nameof(encryptedData));
        }

        if ((encryptedData.Length % 8) != 0)
        {
            throw new ArgumentException("Encrypted data buffer has to be divisible by 8.", nameof(encryptedData));
        }

        if (privacyParameters.Length != LegacyPrivacyParametersLength)
        {
            throw new ArgumentOutOfRangeException(nameof(privacyParameters), "Privacy parameters argument has to be 8 bytes long.");
        }

        if (key.Length < LegacyMinimumKeyLength)
        {
            throw new ArgumentOutOfRangeException(nameof(key), "Decryption key has to be at least 16 bytes long.");
        }

        var iv = GetLegacyIv(key, privacyParameters);
        var outKey = GetLegacyKey(key);
        using var des = DES.Create();
        des.Key = outKey;
        return des.DecryptCbc(encryptedData, iv, PaddingMode.Zeros);
    }

    private static byte[] GetLegacyIv(IReadOnlyList<byte> key, IReadOnlyList<byte> privacyParameters)
    {
        var iv = new byte[LegacyPrivacyParametersLength];
        for (var i = 0; i < iv.Length; i++)
        {
            iv[i] = (byte)(key[8 + i] ^ privacyParameters[i]);
        }

        return iv;
    }

    private static byte[] GetLegacyKey(IReadOnlyList<byte> privacyPassword)
    {
        var outKey = new byte[8];
        for (var i = 0; i < outKey.Length; i++)
        {
            outKey[i] = privacyPassword[i];
        }

        return outKey;
    }

    private static byte[] RequirePassphrase(OctetString? passphrase)
    {
        if (passphrase == null)
        {
            throw new ArgumentNullException(nameof(passphrase));
        }

        return passphrase.Value.Octets;
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
    public TripleDESPrivacyProvider(OctetString? passphrase, IAuthenticationProvider authenticationProvider)
        : base(authenticationProvider ?? throw new ArgumentNullException(nameof(authenticationProvider)), (passphrase ?? throw new ArgumentNullException(nameof(passphrase))).Octets)
    {
    }
}

/// <summary>
/// Represents the AESPrivacyProvider type.
/// </summary>
public class AESPrivacyProvider : DotNetSnmp.Protocol.V3.Security.Privacy.AESPrivacyProvider
{
    internal ReadOnlyMemory<byte> Passphrase { get; }

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
    public AESPrivacyProvider(OctetString? passphrase, IAuthenticationProvider authenticationProvider)
        : this(RequirePassphrase(passphrase), authenticationProvider)
    {
    }

    private AESPrivacyProvider(byte[] passphrase, IAuthenticationProvider authenticationProvider)
        : base(authenticationProvider ?? throw new ArgumentNullException(nameof(authenticationProvider)), passphrase)
    {
        Passphrase = passphrase;
    }

    private static byte[] RequirePassphrase(OctetString? passphrase)
    {
        if (passphrase == null)
        {
            throw new ArgumentNullException(nameof(passphrase));
        }

        return passphrase.Value.Octets;
    }
}

/// <summary>
/// Legacy facade for AES privacy provider capability checks.
/// </summary>
[Obsolete("This type is for internal use only and may be removed in a future release.")]
public abstract class AESPrivacyProviderBase
{
    /// <summary>
    /// Gets a value indicating whether AES is supported on current runtime.
    /// </summary>
    public static bool IsSupported => AESPrivacyProvider.IsSupported;
}

/// <summary>
/// Represents the AES192PrivacyProvider type.
/// </summary>
public sealed class AES192PrivacyProvider : DotNetSnmp.Protocol.V3.Security.Privacy.AES192PrivacyProvider
{
    internal ReadOnlyMemory<byte> Passphrase { get; }

    /// <summary>
    /// Initializes a new instance of AES192PrivacyProvider.
    /// </summary>
    public AES192PrivacyProvider(OctetString? passphrase, IAuthenticationProvider authenticationProvider)
        : this(RequirePassphrase(passphrase), authenticationProvider)
    {
    }

    private AES192PrivacyProvider(byte[] passphrase, IAuthenticationProvider authenticationProvider)
        : base(authenticationProvider ?? throw new ArgumentNullException(nameof(authenticationProvider)), passphrase)
    {
        Passphrase = passphrase;
    }

    private static byte[] RequirePassphrase(OctetString? passphrase)
    {
        if (passphrase == null)
        {
            throw new ArgumentNullException(nameof(passphrase));
        }

        return passphrase.Value.Octets;
    }
}

/// <summary>
/// Represents the AES256PrivacyProvider type.
/// </summary>
public sealed class AES256PrivacyProvider : DotNetSnmp.Protocol.V3.Security.Privacy.AES256PrivacyProvider
{
    internal ReadOnlyMemory<byte> Passphrase { get; }

    /// <summary>
    /// Initializes a new instance of AES256PrivacyProvider.
    /// </summary>
    public AES256PrivacyProvider(OctetString? passphrase, IAuthenticationProvider authenticationProvider)
        : this(RequirePassphrase(passphrase), authenticationProvider)
    {
    }

    private AES256PrivacyProvider(byte[] passphrase, IAuthenticationProvider authenticationProvider)
        : base(authenticationProvider ?? throw new ArgumentNullException(nameof(authenticationProvider)), passphrase)
    {
        Passphrase = passphrase;
    }

    private static byte[] RequirePassphrase(OctetString? passphrase)
    {
        if (passphrase == null)
        {
            throw new ArgumentNullException(nameof(passphrase));
        }

        return passphrase.Value.Octets;
    }
}
