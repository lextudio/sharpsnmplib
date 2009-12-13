using System;

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
        private static readonly object Root = new object();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static IPrivacyProvider Instance
        {
            get
            {
                lock (Root)
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
        public ISnmpData Decrypt(ISnmpData data, SecurityParameters parameters)
        {
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
                throw new ArgumentNullException("data");
            }
            
            if (data.TypeCode == SnmpType.Sequence || data is ISnmpPdu)
            {
                return data;                
            }
            
            throw new ArgumentException("unencrypted data is expected.", "data");
        }

        /// <summary>
        /// Gets the salt.
        /// </summary>
        /// <value>The salt.</value>
        public OctetString Salt
        {
            get { return OctetString.Empty; }
        }

        #endregion
    }
}
