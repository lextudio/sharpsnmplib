using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Security
{
    public sealed class TsmPrivacyProvider : IPrivacyProvider
    {
        private static IPrivacyProvider _DefaultInstance;

        /// <summary>
        /// Default privacy provider with default authentication provider.
        /// </summary>
        public static IPrivacyProvider DefaultPair => _DefaultInstance ?? (_DefaultInstance = new DefaultPrivacyProvider(DefaultAuthenticationProvider.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPrivacyProvider"/> class.
        /// </summary>
        /// <param name="authentication">Authentication provider.</param>
        public TsmPrivacyProvider(IAuthenticationProvider authentication) => this.AuthenticationProvider = authentication;

        #region IPrivacyProvider Members

        /// <summary>
        /// Corresponding <see cref="IAuthenticationProvider"/>.
        /// </summary>
        public IAuthenticationProvider AuthenticationProvider { get; private set; }

        [Obsolete("Use EngineIds instead.")]
        public OctetString EngineId { get; set; }

        /// <summary>
        /// Engine IDs.
        /// </summary>
        /// <remarks>This is an optional field, and only used by TRAP v2 authentication.</remarks>
        public ICollection<OctetString> EngineIds { get; set; }

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public ISnmpData Decrypt(ISnmpData data, SecurityParameters parameters)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (data.TypeCode != SnmpType.Sequence)
            {
                var newException = new DecryptionException("Default decryption failed");
                throw newException;
            }

            return data;
        }

        /// <summary>
        /// Encrypts the specified scope.
        /// </summary>
        /// <param name="data">The scope data.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public ISnmpData Encrypt(ISnmpData data, SecurityParameters parameters)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (data.TypeCode == SnmpType.Sequence || data is ISnmpPdu)
            {
                return data;
            }

            throw new ArgumentException("Invaild data type.", nameof(data));
        }

        /// <summary>
        /// Gets the salt.
        /// </summary>
        /// <value>The salt.</value>
        public OctetString Salt => OctetString.Empty;

        /// <summary>
        /// Passwords to key.
        /// </summary>
        /// <param name="secret">The secret.</param>
        /// <param name="engineId">The engine identifier.</param>
        /// <returns></returns>
        public byte[] PasswordToKey(byte[] secret, byte[] engineId) => this.AuthenticationProvider.PasswordToKey(secret, engineId);

        #endregion

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() => "Tsm privacy provider";
    }
}
