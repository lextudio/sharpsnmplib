using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib
{
    public enum SecurityLevel
    {
        /// <summary>
        /// Without authentication and without privacy.
        /// </summary>
        noAuthNoPriv = 0, 
        /// <summary>
        /// With authentication but without privacy.
        /// </summary>
        authNoPriv, 
        /// <summary>
        /// With authentication and with privacy.
        /// </summary>
        authPriv 
    }
}
