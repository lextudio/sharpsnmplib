using System.Security.Cryptography;

namespace DotNetSnmp.Protocol.V3.Security.Authentication
{
    /// <summary>
    /// Provides authentication for SNMP v3 messages using HMAC-SHA384.
    /// </summary>
    /// <remarks>
    /// This authentication provider implements the usmHMAC256SHA384AuthProtocol.
    /// It uses a 48-byte digest size with 32 bytes used for authentication parameters.
    /// </remarks>
    public class SHA384AuthenticationProvider : AuthenticationProviderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SHA384AuthenticationProvider"/> class.
        /// </summary>
        /// <param name="passcode">The password or passphrase to be used for authentication.</param>
        public SHA384AuthenticationProvider(ReadOnlyMemory<byte> passcode)
            : base(48, 32, HashAlgorithmName.SHA384, passcode, bytes => new HMACSHA384(bytes))
        {
        }
    }
}
