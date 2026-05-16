using System.Security.Cryptography;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Provides authentication for SNMP v3 messages using HMAC-SHA1.
    /// </summary>
    /// <remarks>
    /// This authentication provider implements the usmHMACSHAAuthProtocol which is defined in RFC 3414.
    /// It uses a 20-byte digest size with 12 bytes used for authentication parameters.
    /// </remarks>
    public class SHA1AuthenticationProvider : AuthenticationProviderBase
    {
        /// <summary>
        /// Initializes a new instance of SHA1AuthenticationProvider.
        /// </summary>
        /// <param name="passcode">The password or passphrase to be used for authentication.</param>
        public SHA1AuthenticationProvider(ReadOnlyMemory<byte> passcode)
            : base(20, 12, HashAlgorithmName.SHA1, passcode, bytes => new HMACSHA1(bytes))
        {
        }

        /// <summary>
        /// Initializes a new instance of SHA1AuthenticationProvider with an OctetString passphrase.
        /// </summary>
        public SHA1AuthenticationProvider(OctetString passphrase)
            : this(passphrase.Octets)
        {
        }
        /// <summary>Returns a string representation.</summary>
        public override string ToString() => GetType().Name;
    }
}
