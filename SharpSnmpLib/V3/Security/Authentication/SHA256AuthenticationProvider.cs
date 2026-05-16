using System.Security.Cryptography;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Provides authentication for SNMP v3 messages using HMAC-SHA256.
    /// </summary>
    /// <remarks>
    /// This authentication provider implements the usmHMAC192SHA256AuthProtocol.
    /// It uses a 32-byte digest size with 24 bytes used for authentication parameters.
    /// </remarks>
    public class SHA256AuthenticationProvider : AuthenticationProviderBase
    {
        /// <summary>
        /// Initializes a new instance of SHA256AuthenticationProvider.
        /// </summary>
        /// <param name="passcode">The password or passphrase to be used for authentication.</param>
        public SHA256AuthenticationProvider(ReadOnlyMemory<byte> passcode)
            : base(32, 24, HashAlgorithmName.SHA256, passcode, bytes => new HMACSHA256(bytes))
        {
        }

        /// <summary>
        /// Initializes a new instance of SHA256AuthenticationProvider with an OctetString passphrase.
        /// </summary>
        public SHA256AuthenticationProvider(OctetString passphrase)
            : this(passphrase.Octets)
        {
        }
    }
}
