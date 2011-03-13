namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// MAX ACCESS modifier.
    /// </summary>
    public enum MaxAccess
    {
        /// <summary>
        /// Not accessible.
        /// </summary>
        NotAccessible,

        /// <summary>
        /// Accessible for nofiy.
        /// </summary>
        AccessibleForNotify,

        /// <summary>
        /// Read-only.
        /// </summary>
        ReadOnly,

        /// <summary>
        /// Read-write.
        /// </summary>
        ReadWrite,

        /// <summary>
        /// Read-create.
        /// </summary>
        ReadCreate
    }
}
