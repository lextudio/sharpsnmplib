namespace Lextm.SharpSnmpLib.Security
{
    public enum AuthenticationProtocol
    {
        /// <summary>
        /// usmNoAuthProtocol
        /// </summary>
        None = 0,

        /// <summary>
        /// usmHMACMD5AuthProtocol
        /// </summary>
        Md5,

        /// <summary>
        /// usmHMACSHAAuthProtocol 
        /// </summary>
        Sha1,

        /// <summary>
        /// usmHMAC192SHA256AuthProtocol
        /// </summary>
        Sha256,

        /// <summary>
        /// usmHMAC256SHA384AuthProtocol
        /// </summary>
        Sha384,

        /// <summary>
        /// usmHMAC384SHA512AuthProtocol
        /// </summary>
        Sha512
    }

    public static class AuthenticationProtocolExtensions
    {
        public static int TruncatedDigestSize(
            this AuthenticationProtocol authProto) => authProto switch
            {
                AuthenticationProtocol.Md5 => 12,
                AuthenticationProtocol.Sha1 => 12,
                AuthenticationProtocol.Sha256 => 24,
                AuthenticationProtocol.Sha384 => 32,
                AuthenticationProtocol.Sha512 => 48,
                _ => throw new NotImplementedException()
            };
    }
}
