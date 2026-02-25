using System.Net;

namespace DotNetSnmp.Client
{
    /// <summary>
    /// Exception thrown when an SNMP operation times out.
    /// </summary>
    public class SnmpTimeoutException : Exception
    {
        /// <summary>
        /// Gets target Endpoint.
        /// </summary>
        public IPEndPoint? TargetEndpoint { get; }

        /// <summary>
        /// Gets timeout.
        /// </summary>
        public int Timeout { get; }

        /// <summary>
        /// Initializes a new instance of SnmpTimeoutException.
        /// </summary>
        /// <param name="message">The error message.</param>
        public SnmpTimeoutException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of SnmpTimeoutException.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="targetEndpoint">The target endpoint that timed out.</param>
        public SnmpTimeoutException(string message, IPEndPoint targetEndpoint)
            : base(message)
        {
            TargetEndpoint = targetEndpoint;
        }

        /// <summary>
        /// Initializes a new instance of SnmpTimeoutException.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="targetEndpoint">The target endpoint that timed out.</param>
        /// <param name="timeout">The timeout duration that was exceeded (in milliseconds).</param>
        public SnmpTimeoutException(string message, IPEndPoint targetEndpoint, int timeout)
            : base(message)
        {
            TargetEndpoint = targetEndpoint;
            Timeout = timeout;
        }

        /// <summary>
        /// Initializes a new instance of SnmpTimeoutException.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SnmpTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of SnmpTimeoutException.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="targetEndpoint">The target endpoint that timed out.</param>
        /// <param name="innerException">The inner exception.</param>
        public SnmpTimeoutException(string message, IPEndPoint targetEndpoint, Exception innerException)
            : base(message, innerException)
        {
            TargetEndpoint = targetEndpoint;
        }
    }
}
