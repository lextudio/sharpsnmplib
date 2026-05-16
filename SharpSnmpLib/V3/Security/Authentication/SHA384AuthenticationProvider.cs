using System.Security.Cryptography;

namespace Lextm.SharpSnmpLib.Security
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
        /// Initializes a new instance of SHA384AuthenticationProvider.
        /// </summary>
        /// <param name="passcode">The password or passphrase to be used for authentication.</param>
        public SHA384AuthenticationProvider(ReadOnlyMemory<byte> passcode)
            : base(48, 32, HashAlgorithmName.SHA384, passcode, bytes => new HMACSHA384(bytes))
        {
        }

        /// <summary>
        /// Initializes a new instance of SHA384AuthenticationProvider with an OctetString passphrase.
        /// </summary>
        public SHA384AuthenticationProvider(OctetString passphrase)
            : this(passphrase.Octets)
        {
        }
    }
}
