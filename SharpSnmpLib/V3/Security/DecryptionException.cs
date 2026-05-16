namespace Lextm.SharpSnmpLib.Security;

/// <summary>
/// Exception thrown when decryption fails.
/// </summary>
public class DecryptionException : SnmpException
{
    /// <summary>Initializes a new instance.</summary>
    public DecryptionException() { }

    /// <summary>Initializes a new instance.</summary>
    public DecryptionException(string message) : base(message) { }

    /// <summary>Initializes a new instance.</summary>
    public DecryptionException(string message, Exception inner) : base(message, inner) { }

    private byte[]? _bytes;

    /// <summary>Gets associated bytes (legacy compatibility).</summary>
    public byte[] GetBytes() => _bytes ?? [];

    /// <summary>Sets associated bytes (legacy compatibility).</summary>
    public void SetBytes(byte[] value) => _bytes = value;

    /// <inheritdoc/>
    public override string ToString() => Message;
}
