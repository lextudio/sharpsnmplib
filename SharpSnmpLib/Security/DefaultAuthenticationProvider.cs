using System;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Default authentication provider.
    /// </summary>
    public sealed class DefaultAuthenticationProvider : IAuthenticationProvider
    {
        private DefaultAuthenticationProvider()
        {
        }

        private static readonly object Root = new object();
        private static IAuthenticationProvider _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static IAuthenticationProvider Instance
        {
            get
            {
                lock (Root)
                {
                    if (_instance == null)
                    {
                        _instance = new DefaultAuthenticationProvider();
                    }
                }

                return _instance;
            }
        }

        #region IAuthenticationProvider Members

        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public OctetString ComputeHash(ISnmpMessage message)
        {
            return OctetString.Empty;
        }

        /// <summary>
        /// Gets the clean digest.
        /// </summary>
        /// <value>The clean digest.</value>
        public OctetString CleanDigest
        {
            get { return OctetString.Empty; }
        }

        /// <summary>
        /// Converts password to key.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="engineId"></param>
        /// <returns></returns>
        public byte[] PasswordToKey(byte[] password, byte[] engineId)
        {
            // IMPORTANT: this is not needed in this class.
            return null;
        }

        #endregion
    }
}
