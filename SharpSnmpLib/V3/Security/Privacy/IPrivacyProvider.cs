using DotNetSnmp.Protocol.V3.Security.Authentication;

namespace DotNetSnmp.Protocol.V3.Security.Privacy
{
    /// <summary>
    /// Privacy provider interface.
    /// </summary>
    public interface IPrivacyProvider
    {
        /// <summary>
        /// Gets privacy Parameters Length.
        /// </summary>
        public int PrivacyParametersLength { get; }
        /// <summary>
        /// Gets engine Time.
        /// </summary>
        int EngineTime { get; }
        /// <summary>
        /// Gets engine Boots.
        /// </summary>
        int EngineBoots { get; }
        /// <summary>
        /// Corresponding <see cref="IAuthenticationProvider"/>.
        /// </summary>
        IAuthenticationProvider AuthenticationProvider { get; }

        /// <summary>
        /// Encrypts a v3 message scope according to the configured privacy protocol.
        /// </summary>
        void EncryptMessage(SnmpV3Message message);

        /// <summary>
        /// Decrypts a v3 message scope according to the configured privacy protocol.
        /// </summary>
        void DecryptMessage(SnmpV3Message message);
    }
}
