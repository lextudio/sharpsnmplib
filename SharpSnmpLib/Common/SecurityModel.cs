namespace DotNetSnmp.Common.Definitions
{
    // Enum representing the security models used in SNMP (Simple Network Management Protocol).
    // Each value corresponds to a specific security model.
    public enum SecurityModel : byte
    {
        // User-based Security Model (USM) - Value 3
        Usm = 3,

        // Transport Security Model (TSM) - Value 4
        Tsm = 4,
    }
}
