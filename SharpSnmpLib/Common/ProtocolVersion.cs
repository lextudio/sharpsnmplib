using System.Runtime.Serialization;

namespace DotNetSnmp.Common.Definitions
{
    /// <summary>
    /// Protocol version code.
    /// </summary>
    [DataContract]
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
        /// SNMP v2u is obsolete.
        /// </summary>
        [Obsolete("This version of SNMP is obsolete and replaced by v3.")]
        V2U = 2,

        /// <summary>
        /// SNMP v3.
        /// </summary>
        V3 = 3
    }
}
