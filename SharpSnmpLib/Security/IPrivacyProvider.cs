using System;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Privacy provider interface.
    /// </summary>
    public interface IPrivacyProvider
    {
        /// <summary>
        /// Encrypts the specified scope.
        /// </summary>
        /// <param name="data">The scope data.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        ISnmpData Encrypt(ISnmpData data, SecurityParameters parameters);

        /// <summary>
        /// Gets the salt.
        /// </summary>
        /// <value>The salt.</value>
        OctetString Salt { get; }

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        ISnmpData Decrypt(ISnmpData data, SecurityParameters parameters);
    }
}
