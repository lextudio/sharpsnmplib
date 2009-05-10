
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
        /// <param name="scope">The scope.</param>
        /// <returns></returns>
        ISnmpData Encrypt(Scope scope, SecurityParameters parameters);

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        Scope Decrypt(ISnmpData data, SecurityParameters parameters);
    }
}
