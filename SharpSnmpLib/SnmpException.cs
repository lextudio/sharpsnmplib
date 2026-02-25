using System.Net;
using DotNetSnmp.Common.Definitions;

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
/// Represents the ErrorException type.
/// </summary>
public sealed class ErrorException : SnmpException
{
    private ErrorException(string message, IPAddress address, ISnmpMessage response)
        : base($"{message}: receiver={address}; response={response}")
    {
        Address = address;
        Response = response;
    }

    /// <summary>
    /// Gets address.
    /// </summary>
    public IPAddress Address { get; }

    /// <summary>
    /// Gets response.
    /// </summary>
    public ISnmpMessage Response { get; }

    /// <summary>
    /// Creates an <see cref="ErrorException"/> for a failed SNMP response.
    /// </summary>
    public static ErrorException Create(string message, IPAddress address, ISnmpMessage response)
    {
        return new ErrorException(message, address, response);
    }
}
