using System.Security.Cryptography;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Provides authentication for SNMP v3 messages using HMAC-SHA512.
    /// </summary>
    /// <remarks>
    /// This authentication provider implements the usmHMAC384SHA512AuthProtocol.
    /// It uses a 64-byte digest size with 48 bytes used for authentication parameters.
    /// </remarks>
    public class SHA512AuthenticationProvider : AuthenticationProviderBase
    {
        /// <summary>
        /// Initializes a new instance of SHA512AuthenticationProvider.
        /// </summary>
        /// <param name="passcode">The password or passphrase to be used for authentication.</param>
        public SHA512AuthenticationProvider(ReadOnlyMemory<byte> passcode)
            : base(64, 48, HashAlgorithmName.SHA512, passcode, bytes => new HMACSHA512(bytes))
        {
        }

        /// <summary>
        /// Initializes a new instance of SHA512AuthenticationProvider with an OctetString passphrase.
        /// </summary>
        public SHA512AuthenticationProvider(OctetString passphrase)
            : this(passphrase.Octets)
        {
        }
    }
}
