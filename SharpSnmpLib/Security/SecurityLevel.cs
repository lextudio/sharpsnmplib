using System;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Security level.
    /// </summary>
    [Flags]
    [Serializable]
    public enum Levels
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
