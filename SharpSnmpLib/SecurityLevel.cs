using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Security level.
    /// </summary>
    [Flags]
    public enum SecurityLevel
    {
        /// <summary>
        /// Without authentication and without privacy.
        /// </summary>
        None = 0, 
        /// <summary>
        /// Authentication flag.
        /// </summary>
        Authentication = 1, 
        /// <summary>
        /// Privacy flag.
        /// </summary>
        Privacy = 2,
        /// <summary>
        /// Reportable flag.
        /// </summary>
        Reportable = 4
    }
}
