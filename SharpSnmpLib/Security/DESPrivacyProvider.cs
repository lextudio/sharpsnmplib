using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Privacy provider for DES.
    /// </summary>
    public class DESPrivacyProvider : IPrivacyProvider
    {
        #region IPrivacyProvider Members

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return "DES"; }
        }

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public Scope Decrypt(ISnmpData data)
        {
            OctetString octets = (OctetString)data;
            byte[] bytes = octets.GetRaw();
            return new Scope(new Sequence(bytes));
        }

        /// <summary>
        /// Encrypts the specified scope.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <returns></returns>
        public ISnmpData Encrypt(Scope scope)
        {
            return new OctetString(scope.ToSequence().ToBytes());
        }

        #endregion
    }
}
