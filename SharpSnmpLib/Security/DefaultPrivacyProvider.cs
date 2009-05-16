
namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Default privacy provider.
    /// </summary>
    public sealed class DefaultPrivacyProvider : IPrivacyProvider
    {
        private DefaultPrivacyProvider()
        {
        }

        private static IPrivacyProvider _instance;
        private static object root = new object();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static IPrivacyProvider Instance
        {
            get
            {
                lock (root)
                {
                    if (_instance == null)
                    {
                        _instance = new DefaultPrivacyProvider();
                    }
                }

                return _instance;
            }
        }

        #region IPrivacyProvider Members

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public Scope Decrypt(ISnmpData data, SecurityParameters parameters)
        {
            return new Scope((Sequence)data);
        }

        /// <summary>
        /// Encrypts the specified scope.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public ISnmpData Encrypt(Scope scope, SecurityParameters parameters)
        {
            return scope.ToSequence();
        }

        /// <summary>
        /// Gets the salt.
        /// </summary>
        /// <value>The salt.</value>
        public byte[] Salt
        {
            get { return new byte[0]; }
        }

        #endregion
    }
}
