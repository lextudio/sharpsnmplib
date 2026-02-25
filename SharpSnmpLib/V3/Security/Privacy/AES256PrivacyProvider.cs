using DotNetSnmp.Protocol.V3.Security.Authentication;

namespace DotNetSnmp.Protocol.V3.Security.Privacy
{
    /// <summary>
    /// Implementation of AES-256 privacy protocol for SNMPv3.
    /// </summary>
    public class AES256PrivacyProvider : AESPrivacyProviderBase
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
    }
}
