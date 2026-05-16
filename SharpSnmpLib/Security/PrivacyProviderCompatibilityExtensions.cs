using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;
using Lextm.SharpSnmpLib.Messaging;
using System.Formats.Asn1;
using System.Security.Cryptography;

namespace Lextm.SharpSnmpLib.Security;

/// <summary>
/// Legacy extension helpers for privacy providers.
/// </summary>
public static class PrivacyProviderCompatibilityExtensions
{
    /// <summary>
    /// Converts provider settings to legacy security levels.
    /// </summary>
    public static Levels ToSecurityLevel(this IPrivacyProvider privacy)
    {
        if (privacy == null)
        {
            throw new ArgumentNullException(nameof(privacy));
        }

        if (privacy.AuthenticationProvider == DefaultAuthenticationProvider.Instance)
        {
            return 0;
        }

        if (privacy is DefaultPrivacyProvider)
        {
            return Levels.Authentication;
        }

        return Levels.Authentication | Levels.Privacy;
    }

    /// <summary>
    /// Encrypts scope data using legacy compatibility signature.
    /// </summary>
    public static ISnmpData Encrypt(this IPrivacyProvider privacy, ISnmpData data, SecurityParameters parameters)
    {
        if (privacy == null)
        {
            throw new ArgumentNullException(nameof(privacy));
        }

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        if (privacy is DefaultPrivacyProvider)
        {
            if (data.TypeCode == SnmpType.Sequence || data is IScope)
            {
                return data;
            }

            throw new ArgumentException("Invalid data type.", nameof(data));
        }

        if (data.TypeCode != SnmpType.Sequence && data is not IScope)
        {
            throw new ArgumentException("Invalid data type.", nameof(data));
        }

        var plain = data.Encode();
        var privacyParameters = GetPrivacyParameters(parameters);

        if (privacy is DESPrivacyProvider des)
        {
            var key = DeriveLocalizedKey(des.Passphrase, des.AuthenticationProvider, parameters.EngineId, 16);
            var padded = PadForDes(plain);
            return new OctetString(DESPrivacyProvider.Encrypt(padded, key, privacyParameters));
        }

        if (privacy is AESPrivacyProvider aes)
        {
            var key = DeriveLocalizedKey(aes.Passphrase, aes.AuthenticationProvider, parameters.EngineId, 16);
            return new OctetString(EncryptAesCore(plain, key, parameters.EngineBoots.Value, parameters.EngineTime.Value, privacyParameters, 16));
        }

        if (privacy is AES192PrivacyProvider aes192)
        {
            var key = DeriveLocalizedKey(aes192.Passphrase, aes192.AuthenticationProvider, parameters.EngineId, 24);
            return new OctetString(EncryptAesCore(plain, key, parameters.EngineBoots.Value, parameters.EngineTime.Value, privacyParameters, 24));
        }

        if (privacy is AES256PrivacyProvider aes256)
        {
            var key = DeriveLocalizedKey(aes256.Passphrase, aes256.AuthenticationProvider, parameters.EngineId, 32);
            return new OctetString(EncryptAesCore(plain, key, parameters.EngineBoots.Value, parameters.EngineTime.Value, privacyParameters, 32));
        }

        var scope = TryReadScope(data);
        var message = CreateCompatibilityMessage(privacy, parameters, scope);
        privacy.EncryptMessage(message);
        return new OctetString(message.EncryptedScopedPdu.ToArray());
    }

    /// <summary>
    /// Decrypts scope data using legacy compatibility signature.
    /// </summary>
    public static ISnmpData Decrypt(this IPrivacyProvider privacy, ISnmpData data, SecurityParameters parameters)
    {
        if (privacy == null)
        {
            throw new ArgumentNullException(nameof(privacy));
        }

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        if (privacy is DefaultPrivacyProvider)
        {
            if (data.TypeCode == SnmpType.Sequence || data is IScope)
            {
                return data;
            }

            throw new ArgumentException("Default decryption failed", nameof(data));
        }

        var code = data.TypeCode;
        if (code != SnmpType.OctetString)
        {
            throw new ArgumentException($"Cannot decrypt the scope data: {code}.", nameof(data));
        }

        if (data is not OctetString encrypted)
        {
            throw new ArgumentException($"Cannot decrypt the scope data: {code}.", nameof(data));
        }

        var privacyParameters = GetPrivacyParameters(parameters);

        if (privacy is DESPrivacyProvider des)
        {
            var key = DeriveLocalizedKey(des.Passphrase, des.AuthenticationProvider, parameters.EngineId, 16);
            return DataFactory.CreateSnmpData(DESPrivacyProvider.Decrypt(encrypted.Octets, key, privacyParameters));
        }

        if (privacy is AESPrivacyProvider aes)
        {
            var key = DeriveLocalizedKey(aes.Passphrase, aes.AuthenticationProvider, parameters.EngineId, 16);
            var decrypted = DecryptAesCore(encrypted.Octets, key, parameters.EngineBoots.Value, parameters.EngineTime.Value, privacyParameters, 16);
            return DataFactory.CreateSnmpData(decrypted);
        }

        if (privacy is AES192PrivacyProvider aes192)
        {
            var key = DeriveLocalizedKey(aes192.Passphrase, aes192.AuthenticationProvider, parameters.EngineId, 24);
            var decrypted = DecryptAesCore(encrypted.Octets, key, parameters.EngineBoots.Value, parameters.EngineTime.Value, privacyParameters, 24);
            return DataFactory.CreateSnmpData(decrypted);
        }

        if (privacy is AES256PrivacyProvider aes256)
        {
            var key = DeriveLocalizedKey(aes256.Passphrase, aes256.AuthenticationProvider, parameters.EngineId, 32);
            var decrypted = DecryptAesCore(encrypted.Octets, key, parameters.EngineBoots.Value, parameters.EngineTime.Value, privacyParameters, 32);
            return DataFactory.CreateSnmpData(decrypted);
        }

        var message = CreateCompatibilityMessage(privacy, parameters, null);
        message.EncryptedScopedPdu = encrypted.Octets;
        privacy.DecryptMessage(message);

        return message.Scope as ISnmpData
            ?? throw new ArgumentException("Cannot decrypt the scope data: Unknown.", nameof(data));
    }

    private static byte[] PadForDes(byte[] source)
    {
        var remainder = source.Length % 8;
        if (remainder == 0)
        {
            return source;
        }

        var count = 8 - remainder;
        var result = new byte[source.Length + count];
        Buffer.BlockCopy(source, 0, result, 0, source.Length);
        for (var i = source.Length; i < result.Length; i++)
        {
            result[i] = 1;
        }

        return result;
    }

    private static byte[] GetPrivacyParameters(SecurityParameters parameters)
    {
        if (parameters.PrivacyParameters == null)
        {
            throw new ArgumentException("Invalid security parameters", nameof(parameters));
        }

        return parameters.PrivacyParameters.Value.Octets;
    }

    private static byte[] DeriveLocalizedKey(
        ReadOnlyMemory<byte> passphrase,
        IAuthenticationProvider authenticationProvider,
        OctetString engineId,
        int keyBytes)
    {
        var digest = new byte[authenticationProvider.DigestSize];
        authenticationProvider.PasswordToKey(passphrase, engineId.Octets, digest);

        if (digest.Length >= keyBytes)
        {
            return GetKeySlice(digest, keyBytes);
        }

        var expanded = new byte[keyBytes];
        TripleDESPrivacyProvider.ExtendShortKey(digest, keyBytes, authenticationProvider, engineId.Octets, expanded);
        return expanded;
    }

    /// <summary>
    /// Encrypts raw bytes using legacy AES helper signature.
    /// </summary>
    public static byte[] Encrypt(this AESPrivacyProvider privacy, byte[] unencryptedData, byte[] key, int engineBoots, int engineTime, byte[] privacyParameters)
    {
        if (privacy == null)
        {
            throw new ArgumentNullException(nameof(privacy));
        }

        return EncryptAesCore(unencryptedData, key, engineBoots, engineTime, privacyParameters, 16);
    }

    /// <summary>
    /// Decrypts raw bytes using legacy AES helper signature.
    /// </summary>
    public static byte[] Decrypt(this AESPrivacyProvider privacy, byte[] encryptedData, byte[] key, int engineBoots, int engineTime, byte[] privacyParameters)
    {
        if (privacy == null)
        {
            throw new ArgumentNullException(nameof(privacy));
        }

        return DecryptAesCore(encryptedData, key, engineBoots, engineTime, privacyParameters, 16);
    }

    /// <summary>
    /// Encrypts raw bytes using legacy AES192 helper signature.
    /// </summary>
    public static byte[] Encrypt(this AES192PrivacyProvider privacy, byte[] unencryptedData, byte[] key, int engineBoots, int engineTime, byte[] privacyParameters)
    {
        if (privacy == null)
        {
            throw new ArgumentNullException(nameof(privacy));
        }

        return EncryptAesCore(unencryptedData, key, engineBoots, engineTime, privacyParameters, 24);
    }

    /// <summary>
    /// Decrypts raw bytes using legacy AES192 helper signature.
    /// </summary>
    public static byte[] Decrypt(this AES192PrivacyProvider privacy, byte[] encryptedData, byte[] key, int engineBoots, int engineTime, byte[] privacyParameters)
    {
        if (privacy == null)
        {
            throw new ArgumentNullException(nameof(privacy));
        }

        return DecryptAesCore(encryptedData, key, engineBoots, engineTime, privacyParameters, 24);
    }

    /// <summary>
    /// Encrypts raw bytes using legacy AES256 helper signature.
    /// </summary>
    public static byte[] Encrypt(this AES256PrivacyProvider privacy, byte[] unencryptedData, byte[] key, int engineBoots, int engineTime, byte[] privacyParameters)
    {
        if (privacy == null)
        {
            throw new ArgumentNullException(nameof(privacy));
        }

        return EncryptAesCore(unencryptedData, key, engineBoots, engineTime, privacyParameters, 32);
    }

    /// <summary>
    /// Decrypts raw bytes using legacy AES256 helper signature.
    /// </summary>
    public static byte[] Decrypt(this AES256PrivacyProvider privacy, byte[] encryptedData, byte[] key, int engineBoots, int engineTime, byte[] privacyParameters)
    {
        if (privacy == null)
        {
            throw new ArgumentNullException(nameof(privacy));
        }

        return DecryptAesCore(encryptedData, key, engineBoots, engineTime, privacyParameters, 32);
    }

    private static SnmpV3Message CreateCompatibilityMessage(IPrivacyProvider privacy, SecurityParameters parameters, Scope? scope)
    {
        var usm = parameters.ToUsmSecurityParameters();
        if (usm.PrivParams.Length != privacy.PrivacyParametersLength)
        {
            usm.PrivParams = new byte[privacy.PrivacyParametersLength];
        }

        return new SnmpV3Message
        {
            Header = new HeaderData
            {
                MsgId = 0,
                MsgMaxSize = Messenger.MaxMessageSize,
                MsgFlags = MsgFlag.Auth | MsgFlag.Priv,
                MsgSecurityModel = SecurityModel.Usm
            },
            SecurityParameters = usm,
            Scope = scope
        };
    }

    private static Scope TryReadScope(ISnmpData data)
    {
        if (data is Scope scope)
        {
            return scope;
        }

        try
        {
            var reader = new AsnReader(data.Encode(), AsnEncodingRules.BER);
            return Scope.ReadFrom(reader);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Invalid data type.", nameof(data), ex);
        }
    }

    private static byte[] EncryptAesCore(byte[] unencryptedData, byte[] key, int engineBoots, int engineTime, byte[] privacyParameters, int keyBytes)
    {
        if (!AESPrivacyProvider.IsSupported)
        {
            throw new PlatformNotSupportedException();
        }

        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (key.Length < keyBytes)
        {
            throw new ArgumentOutOfRangeException(nameof(key), "Invalid key length.");
        }

        if (unencryptedData == null)
        {
            throw new ArgumentNullException(nameof(unencryptedData));
        }

        if (privacyParameters == null)
        {
            throw new ArgumentNullException(nameof(privacyParameters));
        }

        if (privacyParameters.Length < 8)
        {
            throw new ArgumentOutOfRangeException(nameof(privacyParameters), "Privacy parameters argument has to be 8 bytes long.");
        }

        var iv = BuildAesIv(engineBoots, engineTime, privacyParameters);
        var normalizedKey = GetKeySlice(key, keyBytes);

        using var aes = Aes.Create();
        aes.Key = normalizedKey;

        var length = (unencryptedData.Length % 16 == 0)
            ? unencryptedData.Length
            : ((unencryptedData.Length / 16) + 1) * 16;

        var encrypted = new byte[length];
        var written = aes.EncryptCfb(unencryptedData.AsSpan(), iv.AsSpan(), encrypted.AsSpan(), PaddingMode.Zeros, 128);

        if (written == unencryptedData.Length)
        {
            return encrypted.AsSpan(0, written).ToArray();
        }

        return encrypted.AsSpan(0, unencryptedData.Length).ToArray();
    }

    private static byte[] DecryptAesCore(byte[] encryptedData, byte[] key, int engineBoots, int engineTime, byte[] privacyParameters, int keyBytes)
    {
        if (!AESPrivacyProvider.IsSupported)
        {
            throw new PlatformNotSupportedException();
        }

        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (encryptedData == null)
        {
            throw new ArgumentNullException(nameof(encryptedData));
        }

        if (key.Length < keyBytes)
        {
            throw new ArgumentOutOfRangeException(nameof(key), "Invalid key length.");
        }

        if (privacyParameters == null)
        {
            throw new ArgumentNullException(nameof(privacyParameters));
        }

        if (privacyParameters.Length < 8)
        {
            throw new ArgumentOutOfRangeException(nameof(privacyParameters), "Privacy parameters argument has to be 8 bytes long.");
        }

        var iv = BuildAesIv(engineBoots, engineTime, privacyParameters);
        var normalizedKey = GetKeySlice(key, keyBytes);

        using var aes = Aes.Create();
        aes.Key = normalizedKey;

        if ((encryptedData.Length % 16) != 0)
        {
            var div = encryptedData.Length / 16;
            var newLength = (div + 1) * 16;
            var decryptBuffer = new byte[newLength];
            Buffer.BlockCopy(encryptedData, 0, decryptBuffer, 0, encryptedData.Length);
            var output = new byte[newLength];
            aes.DecryptCfb(decryptBuffer.AsSpan(), iv.AsSpan(), output.AsSpan(), PaddingMode.Zeros, 128);
            return output.AsSpan(0, encryptedData.Length).ToArray();
        }

        return aes.DecryptCfb(encryptedData, iv, PaddingMode.Zeros, 128);
    }

    private static byte[] BuildAesIv(int engineBoots, int engineTime, IReadOnlyList<byte> privacyParameters)
    {
        var iv = new byte[16];
        var bootsBytes = BitConverter.GetBytes(engineBoots);
        iv[0] = bootsBytes[3];
        iv[1] = bootsBytes[2];
        iv[2] = bootsBytes[1];
        iv[3] = bootsBytes[0];

        var timeBytes = BitConverter.GetBytes(engineTime);
        iv[4] = timeBytes[3];
        iv[5] = timeBytes[2];
        iv[6] = timeBytes[1];
        iv[7] = timeBytes[0];

        for (var i = 0; i < 8; i++)
        {
            iv[8 + i] = privacyParameters[i];
        }

        return iv;
    }

    private static byte[] GetKeySlice(IReadOnlyList<byte> source, int keyBytes)
    {
        var result = new byte[keyBytes];
        for (var i = 0; i < keyBytes; i++)
        {
            result[i] = source[i];
        }

        return result;
    }
}
