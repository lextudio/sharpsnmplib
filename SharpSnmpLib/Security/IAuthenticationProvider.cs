
using System;
using System.Collections.Generic;
using System.Text;
using System;

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
    }
}
