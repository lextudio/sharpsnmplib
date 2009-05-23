namespace Lextm.SharpSnmpLib
{
    using System;
    
    /// <summary>
    /// Protocol version code.
    /// </summary>
    [Serializable]
    public enum VersionCode
    {
        /// <summary>
        /// SNMP v1.
        /// </summary>
        V1 = 0,
        
        /// <summary>
        /// SNMP v2 classic.
        /// </summary>
        V2 = 1,
        
        /// <summary>
        /// SNMP v3.
        /// </summary>
        V3 = 3
    }
}