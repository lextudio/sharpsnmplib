using DotNetSnmp.Protocol.V3.Security.Authentication;

namespace DotNetSnmp.Protocol.V3.Security.Privacy
{
    /// <summary>
    /// Implementation of AES-192 privacy protocol for SNMPv3.
    /// </summary>
    public class AES192PrivacyProvider : AESPrivacyProviderBase
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
    }
}
