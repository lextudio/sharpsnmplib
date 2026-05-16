namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Defines values for MessageProcessingResult.
    /// </summary>
    public enum MessageProcessingResult
    {
        /// <summary>
        /// Represents the Success value.
        /// </summary>
        Success,
        /// <summary>
        /// Represents the UnsupportedSecurityModel value.
        /// </summary>
        UnsupportedSecurityModel,
        /// <summary>
        /// Represents the DecryptionError value.
        /// </summary>
        DecryptionError,
        /// <summary>
        /// Represents the BadEncoding value.
        /// </summary>
        BadEncoding,
        /// <summary>
        /// Represents the InternalError value.
        /// </summary>
        InternalError,
        /// <summary>
        /// Represents this member.
        /// </summary>
        AuthenticationError
    }
}
