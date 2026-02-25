using System.Net;
using DotNetSnmp.Common.Definitions;

namespace Lextm.SharpSnmpLib;

public class SnmpException : Exception
{
    public SnmpException()
    {
    }

    public SnmpException(string message)
        : base(message)
    {
    }

    public SnmpException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

public sealed class ErrorException : SnmpException
{
    private ErrorException(string message, IPAddress address, ISnmpMessage response)
        : base($"{message}: receiver={address}; response={response}")
    {
        Address = address;
        Response = response;
    }

    public IPAddress Address { get; }

    public ISnmpMessage Response { get; }

    public static ErrorException Create(string message, IPAddress address, ISnmpMessage response)
    {
        return new ErrorException(message, address, response);
    }
}
