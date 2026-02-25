namespace DotNetSnmp.Asn1.Serialization
{
    /// <summary>
    /// The exception that is thrown when ASN.1 payload data cannot be decoded as valid SNMP data.
    /// </summary>
    public class SnmpDecodeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SnmpDecodeException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the decode failure.</param>
        public SnmpDecodeException(string? message) : base(message)
        {
        }
    }
}
