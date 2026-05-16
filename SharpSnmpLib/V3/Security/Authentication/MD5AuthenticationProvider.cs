using System.Security.Cryptography;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Provides authentication for SNMP v3 messages using HMAC-MD5.
    /// </summary>
    /// <remarks>
    /// This authentication provider implements the usmHMACMD5AuthProtocol which is defined in RFC 3414.
    /// It uses a 16-byte digest size with 12 bytes used for authentication parameters.
    /// </remarks>
    public class MD5AuthenticationProvider : AuthenticationProviderBase
    {
        /// <summary>
        /// Initializes a new instance of MD5AuthenticationProvider.
        /// </summary>
        /// <param name="passcode">The password or passphrase to be used for authentication.</param>
        public MD5AuthenticationProvider(ReadOnlyMemory<byte> passcode)
            : base(16, 12, HashAlgorithmName.MD5, passcode, bytes => new HMACMD5(bytes))
        {
        }

        /// <summary>
        /// Initializes a new instance of MD5AuthenticationProvider with an OctetString passphrase.
        /// </summary>
        public MD5AuthenticationProvider(OctetString passphrase)
            : this(passphrase.Octets)
        {
        }
    }
}
