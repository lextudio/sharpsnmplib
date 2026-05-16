using Lextm.SharpSnmpLib.Security;
using System.Security.Cryptography;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Implementation of AES-128 privacy protocol for SNMPv3.
    /// </summary>
    public class AESPrivacyProvider : AESPrivacyProviderBase
    {
        private const int KeyLength = 16; // 128 bits = 16 bytes

        /// <summary>
        /// Initializes a new instance of AESPrivacyProvider.
        /// </summary>
        public AESPrivacyProvider(
            IAuthenticationProvider authenticationService,
            ReadOnlyMemory<byte> passcode)
            : base(authenticationService, passcode, KeyLength)
        {
        }

        /// <summary>
        /// Initializes a new instance of AESPrivacyProvider with v12-compatible signature.
        /// </summary>
        public AESPrivacyProvider(OctetString? passphrase, IAuthenticationProvider authenticationProvider)
            : this(
                authenticationProvider ?? throw new ArgumentNullException(nameof(authenticationProvider)),
                (passphrase ?? throw new ArgumentNullException(nameof(passphrase))).Octets)
        {
        }

        /// <summary>
        /// Gets a value indicating whether AES is supported on current runtime.
        /// </summary>
        public static bool IsSupported
        {
            get
            {
                try { using var _ = Aes.Create(); return true; }
                catch { return false; }
            }
        }
    }
}
