
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
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name
        {
            get;
        }

        OctetString CleanDigest { get; }

        OctetString ComputeHash(GetRequestMessage message);
    }
}
