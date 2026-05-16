using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Security
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
        /// Gets privacy salt (legacy compatibility member).
        /// </summary>
        OctetString Salt => new(Array.Empty<byte>());

        /// <summary>
        /// Gets known engine ids (legacy compatibility member).
        /// </summary>
        ICollection<OctetString>? EngineIds => null;

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
