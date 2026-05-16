using System.Net;
using Lextm.SharpSnmpLib;

namespace Lextm.SharpSnmpLib;

/// <summary>
/// Base exception type of #SNMP.
/// </summary>
public class SnmpException : Exception
{
    /// <summary>
    /// Initializes a new instance of SnmpException.
    /// </summary>
    public SnmpException()
    {
    }

    /// <summary>Gets details (legacy compatibility).</summary>
    protected string Details => Message;

    /// <summary>
    /// Initializes a new instance of SnmpException.
    /// </summary>
    public SnmpException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of SnmpException.
    /// </summary>
    public SnmpException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Represents an operation-level SNMP exception.
/// </summary>
public class OperationException : SnmpException
{
    /// <summary>
    /// Initializes a new instance of <see cref="OperationException"/>.
    /// </summary>
    public OperationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="OperationException"/>.
    /// </summary>
    public OperationException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="OperationException"/>.
    /// </summary>
    public OperationException(string message, Exception inner)
        : base(message, inner)
    {
    }

    /// <summary>
    /// Gets the agent IP address.
    /// </summary>
    public IPAddress? Agent { get; protected set; }

    /// <summary>
    /// Gets details.
    /// </summary>
    protected string Details => Message;

    /// <summary>
    /// Creates an <see cref="OperationException"/>.
    /// </summary>
    public static OperationException Create(string message, IPAddress agent)
    {
        return new OperationException(message) { Agent = agent };
    }
}
