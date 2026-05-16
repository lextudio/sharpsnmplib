using System.Net;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Exception raised when an SNMP operation times out.
/// </summary>
public sealed class TimeoutException : SnmpException
{
    /// <summary>
    /// Initializes a new instance of <see cref="TimeoutException"/>.
    /// </summary>
    public TimeoutException()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="TimeoutException"/>.
    /// </summary>
    public TimeoutException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="TimeoutException"/>.
    /// </summary>
    public TimeoutException(string message, Exception inner)
        : base(message, inner)
    {
    }

    /// <summary>Gets details (legacy compatibility).</summary>
    protected string Details => Message;

    /// <summary>
    /// Gets or sets timed-out target address.
    /// </summary>
    public IPAddress? Agent { get; set; }

    /// <summary>
    /// Gets or sets timeout in milliseconds.
    /// </summary>
    public int Timeout { get; set; }

    /// <summary>Creates an OperationException (legacy compat, same as OperationException.Create).</summary>
    public static OperationException Create(string message, IPAddress agent)
        => OperationException.Create(message, agent);

    /// <summary>
    /// Creates a timeout exception populated with target and timeout values.
    /// </summary>
    public static TimeoutException Create(IPAddress agent, int timeout)
    {
        return new TimeoutException($"Request timed out after {timeout}-ms.")
        {
            Agent = agent,
            Timeout = timeout
        };
    }
}
