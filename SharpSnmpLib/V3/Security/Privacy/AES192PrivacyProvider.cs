using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Implementation of AES-192 privacy protocol for SNMPv3.
    /// </summary>
    public sealed class AES192PrivacyProvider : AESPrivacyProviderBase
    {
        private const int KeyLength = 24; // 192 bits = 24 bytes

        /// <summary>
        /// Initializes a new instance of AES192PrivacyProvider.
        /// </summary>
        public AES192PrivacyProvider(
            IAuthenticationProvider authenticationService,
            ReadOnlyMemory<byte> passcode)
            : base(authenticationService, passcode, KeyLength)
        {
        }

        /// <summary>
        /// Initializes a new instance of AES192PrivacyProvider with v12-compatible signature.
        /// </summary>
        public AES192PrivacyProvider(OctetString? passphrase, IAuthenticationProvider authenticationProvider)
            : this(
                authenticationProvider ?? throw new ArgumentNullException(nameof(authenticationProvider)),
                (passphrase ?? throw new ArgumentNullException(nameof(passphrase))).Octets)
        {
        }
    }
}
