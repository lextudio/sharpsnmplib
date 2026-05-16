namespace Lextm.SharpSnmpLib
{
    // Enum representing the security models used in SNMP (Simple Network Management Protocol).
    // Each value corresponds to a specific security model.
    /// <summary>
    /// Defines values for SecurityModel.
    /// </summary>
    public enum SecurityModel : byte
    {
        // User-based Security Model (USM) - Value 3
        /// <summary>
        /// Represents the Usm value.
        /// </summary>
        Usm = 3,

        // Transport Security Model (TSM) - Value 4
        /// <summary>
        /// Represents the Tsm value.
        /// </summary>
        Tsm = 4,
    }
}
