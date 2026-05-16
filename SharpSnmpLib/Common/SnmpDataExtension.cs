namespace Lextm.SharpSnmpLib;

/// <summary>Extension methods for ISnmpData (legacy compatibility).</summary>
public static class SnmpDataExtension
{
    /// <summary>Converts an ISnmpData to its byte representation.</summary>
    public static byte[] ToBytes(this ISnmpData data)
    {
        return AsnSerializableExtensions.Encode(data);
    }
}
