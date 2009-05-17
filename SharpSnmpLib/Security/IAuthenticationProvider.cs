

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Authentication provider interface.
    /// </summary>
    public interface IAuthenticationProvider
    {
        /// <summary>
        /// Gets the clean digest.
        /// </summary>
        /// <value>The clean digest.</value>
        OctetString CleanDigest { get; }

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        OctetString ComputeHash(ISnmpMessage message);

        /// <summary>
        /// Converts password to key.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="engineId">The engine id.</param>
        /// <returns></returns>
        byte[] PasswordToKey(byte[] password, byte[] engineId);
    }
}
