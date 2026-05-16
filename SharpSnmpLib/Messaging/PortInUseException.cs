using System.Net;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Exception raised when an endpoint is already in use.
/// </summary>
public sealed class PortInUseException : SnmpException
{
    /// <summary>
    /// Initializes a new instance of <see cref="PortInUseException"/>.
    /// </summary>
    public PortInUseException()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="PortInUseException"/>.
    /// </summary>
    public PortInUseException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="PortInUseException"/>.
    /// </summary>
    public PortInUseException(string message, Exception inner)
        : base(message, inner)
    {
    }

    /// <summary>Gets details (legacy compatibility).</summary>
    protected string Details => Message;

    /// <summary>
    /// Gets or sets endpoint already in use.
    /// </summary>
    public IPEndPoint? Endpoint { get; set; }
}
