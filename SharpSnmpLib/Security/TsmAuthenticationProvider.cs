using System;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Tsm Authentication Provider
    /// </summary>
    public sealed class TsmAuthenticationProvider : IAuthenticationProvider
    {
        private TsmAuthenticationProvider()
        {

        }

        private static readonly IAuthenticationProvider _Instance = new TsmAuthenticationProvider();
        
        /// <summary>
        /// instance of TsmAuthenticationProvider
        /// </summary>
        public static IAuthenticationProvider Instance => _Instance;

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="header">The header.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="data">The scope data.</param>
        /// <param name="privacy">The privacy provider.</param>
        /// <param name="length">The length bytes.</param>
        /// <returns></returns>
        public OctetString ComputeHash(VersionCode version, ISegment header, SecurityParameters parameters, ISnmpData data, IPrivacyProvider privacy, byte[]? length)
        {
            if (header == null)
            {
                throw new ArgumentNullException(nameof(header));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (privacy == null)
            {
                throw new ArgumentNullException(nameof(privacy));
            }

            return OctetString.Empty;
        }

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <returns></returns>
        public static OctetString ComputeHash(byte[] buffer, OctetString engineId)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (engineId == null)
            {
                throw new ArgumentNullException(nameof(engineId));
            }

            return OctetString.Empty;
        }

        /// <summary>
        /// Gets the clean digest.
        /// </summary>
        /// <value>The clean digest.</value>
        public OctetString CleanDigest => OctetString.Empty;

        /// <summary>
        /// Converts password to key.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="engineId"></param>
        /// <returns></returns>
        public byte[] PasswordToKey(byte[] password, byte[] engineId)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (engineId == null)
            {
                throw new ArgumentNullException(nameof(engineId));
            }

            // IMPORTANT: this function is not used.
            return [];
        }

        /// <summary>
        /// Gets the length of the digest.
        /// </summary>
        /// <value>The length of the digest.</value>
        public int DigestLength => 0;

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString() => "TSM authentication provider";
    }
}
