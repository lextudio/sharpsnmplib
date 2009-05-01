using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib
{
    [Flags]
    public enum SecurityLevel
    {
        /// <summary>
        /// Without authentication and without privacy.
        /// </summary>
        None = 0x00, 
        /// <summary>
        /// Authentication flag.
        /// </summary>
        Authentication = 0x01, 
        /// <summary>
        /// Privacy flag.
        /// </summary>
        Privacy = 0x10,
        /// <summary>
        /// Reportable flag.
        /// </summary>
        Reportable = 0x100
    }
}
