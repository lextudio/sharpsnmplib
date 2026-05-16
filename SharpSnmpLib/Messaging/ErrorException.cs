using System.Net;
using Lextm.SharpSnmpLib;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Represents the ErrorException type.
/// </summary>
public sealed class ErrorException : SnmpException
{
    /// <summary>
    /// Initializes a new instance of <see cref="ErrorException"/>.
    /// </summary>
    public ErrorException()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ErrorException"/>.
    /// </summary>
    public ErrorException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ErrorException"/>.
    /// </summary>
    public ErrorException(string message, Exception inner)
        : base(message, inner)
    {
    }

    private ErrorException(string message, IPAddress address, ISnmpMessage response)
        : base($"{message}: receiver={address}; response={response}")
    {
        Address = address;
        Response = response;
    }

    /// <summary>
    /// Gets address.
    /// </summary>
    public IPAddress? Address { get; set; }

    /// <summary>
    /// Gets agent (legacy alias for Address).
    /// </summary>
    public IPAddress? Agent
    {
        get => Address;
        set => Address = value;
    }

    /// <summary>
    /// Gets response.
    /// </summary>
    public ISnmpMessage? Response { get; }

    /// <summary>
    /// Gets body (legacy alias for Response).
    /// </summary>
    public ISnmpMessage? Body => Response;

    /// <summary>
    /// Gets details (legacy compatibility).
    /// </summary>
    protected string Details => Message;

    /// <summary>
    /// Creates an <see cref="ErrorException"/> for a failed SNMP response.
    /// </summary>
    public static ErrorException Create(string message, IPAddress address, ISnmpMessage response)
    {
        return new ErrorException(message, address, response);
    }

    /// <summary>
    /// Creates an <see cref="OperationException"/> for a failed operation without a response.
    /// </summary>
    public static OperationException Create(string message, IPAddress agent)
    {
        return OperationException.Create(message, agent);
    }
}
