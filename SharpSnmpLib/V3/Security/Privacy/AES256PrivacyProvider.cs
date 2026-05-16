using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Implementation of AES-256 privacy protocol for SNMPv3.
    /// </summary>
    public sealed class AES256PrivacyProvider : AESPrivacyProviderBase
    {
        private const int KeyLength = 32; // 256 bits = 32 bytes

        /// <summary>
        /// Initializes a new instance of AES256PrivacyProvider.
        /// </summary>
        public AES256PrivacyProvider(
            IAuthenticationProvider authenticationService,
            ReadOnlyMemory<byte> passcode)
            : base(authenticationService, passcode, KeyLength)
        {
        }

        /// <summary>
        /// Initializes a new instance of AES256PrivacyProvider with v12-compatible signature.
        /// </summary>
        public AES256PrivacyProvider(OctetString? passphrase, IAuthenticationProvider authenticationProvider)
            : this(
                authenticationProvider ?? throw new ArgumentNullException(nameof(authenticationProvider)),
                (passphrase ?? throw new ArgumentNullException(nameof(passphrase))).Octets)
        {
        }
    }
}
