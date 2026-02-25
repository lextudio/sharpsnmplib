namespace DotNetSnmp.Client
{
    public enum MessageProcessingResult
    {
        Success,
        UnsupportedSecurityModel,
        DecryptionError,
        BadEncoding,
        InternalError,
        AuthenticationError
    }
}
