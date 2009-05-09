using System;
using System.Collections.Generic;
using System.Text;

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

        private static object root = new object();
        private static IAuthenticationProvider _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static IAuthenticationProvider Instance
        {
            get
            {
                lock (root)
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
        public OctetString ComputeHash(GetRequestMessage message)
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

        #endregion
    }
}
