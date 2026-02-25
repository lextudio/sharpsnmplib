using DotNetSnmp.Protocol.V3.Security.Authentication;

namespace DotNetSnmp.Protocol.V3.Security.Privacy
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
    }
}
