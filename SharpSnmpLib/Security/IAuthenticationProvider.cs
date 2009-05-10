

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
        OctetString ComputeHash(GetRequestMessage message);

        /// <summary>
        /// Converts password to key.
        /// </summary>
        /// <param name="_phrase">The password phrase.</param>
        /// <param name="octetString">The engine ID.</param>
        /// <returns></returns>
        byte[] PasswordToKey(byte[] password, byte[] engineId);
    }
}
