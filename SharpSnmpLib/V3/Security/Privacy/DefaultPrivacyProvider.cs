using System.Buffers;
using System.Collections.Generic;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Security;

/// <summary>
/// Default privacy provider.
/// </summary>
public class DefaultPrivacyProvider : IPrivacyProvider
{
    /// <inheritdoc/>
    public int PrivacyParametersLength => throw new NotImplementedException();

    /// <inheritdoc/>
    public int EngineTime => throw new NotImplementedException();

    /// <inheritdoc/>
    public int EngineBoots => throw new NotImplementedException();

    /// <inheritdoc/>
    public IAuthenticationProvider AuthenticationProvider { get; }

    /// <summary>
    /// Initializes a new instance of DefaultPrivacyProvider.
    /// </summary>
    public DefaultPrivacyProvider()
    : this(DefaultAuthenticationProvider.Instance)
    {
    }

    /// <summary>
    /// Initializes a new instance of DefaultPrivacyProvider.
    /// </summary>
    public DefaultPrivacyProvider(IAuthenticationProvider authenticationProvider)
    {
        AuthenticationProvider = authenticationProvider;
    }

    /// <summary>
    /// Decrypts a scoped PDU payload. The default provider performs no decryption.
    /// </summary>
    public void DecryptScopedPdu(in ReadOnlyMemory<byte> encryptedPdu, in UsmSecurityParameters parameters, Span<byte> decryptedPdu)
    {
        // No decryption needed for NoPrivacyService
    }

    /// <summary>
    /// Encrypts a scoped PDU payload. The default provider performs no encryption.
    /// </summary>
    public int EncryptScopedPdu(in ReadOnlyMemory<byte> scopedPdu, in UsmSecurityParameters parameters, Span<byte> encryptedScopedPdu)
    {
        return 0;
    }

    /// <inheritdoc/>
    public void DecryptMessage(SnmpV3Message message)
    {
        // No decryption needed for NoPrivacyService
    }

    /// <inheritdoc/>
    public void EncryptMessage(SnmpV3Message message)
    {
        // No encryption needed for NoPrivacyService
    }

    /// <summary>
    /// Allocates a temporary buffer for compatibility with legacy call paths.
    /// </summary>
    public byte[] PrepareBuffer(int encodedLength)
    {
        return ArrayPool<byte>.Shared.Rent(encodedLength);
    }

    private static readonly IPrivacyProvider _defaultPair =
        new DefaultPrivacyProvider(DefaultAuthenticationProvider.Instance);

    /// <summary>
    /// Gets the default privacy/auth pair (no privacy, no authentication).
    /// </summary>
    public static IPrivacyProvider DefaultPair => _defaultPair;

    /// <summary>
    /// Gets or sets known engine IDs (legacy compatibility member).
    /// </summary>
    public ICollection<OctetString>? EngineIds { get; set; }
}
