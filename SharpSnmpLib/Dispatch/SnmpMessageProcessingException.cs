namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Exception thrown when SNMP message processing fails.
    /// </summary>
    public class SnmpMessageProcessingException : Exception
    {
        /// <summary>
        /// Gets processing Result.
        /// </summary>
        public MessageProcessingResult ProcessingResult { get; }

        /// <summary>
        /// Initializes a new instance of SnmpMessageProcessingException.
        /// </summary>
        /// <param name="message">The error message.</param>
        public SnmpMessageProcessingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of SnmpMessageProcessingException.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="processingResult">The message processing result that caused the exception.</param>
        public SnmpMessageProcessingException(string message, MessageProcessingResult processingResult)
            : base(message)
        {
            ProcessingResult = processingResult;
        }

        /// <summary>
        /// Initializes a new instance of SnmpMessageProcessingException.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SnmpMessageProcessingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of SnmpMessageProcessingException.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="processingResult">The message processing result that caused the exception.</param>
        /// <param name="innerException">The inner exception.</param>
        public SnmpMessageProcessingException(string message, MessageProcessingResult processingResult, Exception innerException)
            : base(message, innerException)
        {
            ProcessingResult = processingResult;
        }
    }
}
