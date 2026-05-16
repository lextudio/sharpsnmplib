namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Exception raised when message parsing fails.
/// </summary>
public sealed class MessageFactoryException : SnmpException
{
    private byte[]? _bytes;

    /// <summary>
    /// Initializes a new instance of <see cref="MessageFactoryException"/>.
    /// </summary>
    public MessageFactoryException()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="MessageFactoryException"/>.
    /// </summary>
    public MessageFactoryException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="MessageFactoryException"/>.
    /// </summary>
    public MessageFactoryException(string message, Exception inner)
        : base(message, inner)
    {
    }

    /// <summary>
    /// Gets parsed bytes.
    /// </summary>
    public byte[]? GetBytes()
    {
        return _bytes;
    }

    /// <summary>
    /// Sets parsed bytes.
    /// </summary>
    public void SetBytes(byte[] value)
    {
        _bytes = value;
    }
}
