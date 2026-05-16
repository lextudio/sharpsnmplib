using System.Runtime.Serialization;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Security level.
    /// </summary>
    [Flags]
    [DataContract]
    public enum Levels : byte
    {
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
