using DotNetSnmp.Asn1;
using DotNetSnmp.Asn1.Serialization;
using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;
using System.Globalization;
using System.Text;

namespace Lextm.SharpSnmpLib;

/// <summary>
/// Provides compatibility helpers for legacy SharpSnmpLib APIs.
/// </summary>
public static class CompatibilityExtensions
{
    /// <summary>
    /// Converts an <see cref="Integer32"/> value to <see cref="int"/>.
    /// </summary>
    public static int ToInt32(this Integer32 value)
    {
        return value.Value;
    }

    /// <summary>
    /// Converts an <see cref="ErrorCode"/> value to <see cref="int"/>.
    /// </summary>
    public static int ToInt32(this ErrorCode value)
    {
        return (int)value;
    }

    /// <summary>
    /// Converts an <see cref="ErrorCode"/> value to itself (legacy helper signature).
    /// </summary>
    public static ErrorCode ToErrorCode(this ErrorCode value)
    {
        return value;
    }

    /// <summary>
    /// Converts an <see cref="Integer32"/> value to <see cref="ErrorCode"/> (legacy helper signature).
    /// </summary>
    public static ErrorCode ToErrorCode(this Integer32 value)
    {
        if (!TryToErrorCode(value, out var code))
        {
            throw new InvalidCastException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "Integer32 value {0} cannot be converted to a known ErrorCode.",
                    value.Value));
        }

        return code;
    }

    /// <summary>
    /// Tries to convert an <see cref="Integer32"/> value to <see cref="ErrorCode"/>.
    /// </summary>
    public static bool TryToErrorCode(this Integer32 value, out ErrorCode code)
    {
        var raw = value.Value;
        if (raw < byte.MinValue || raw > byte.MaxValue)
        {
            code = default;
            return false;
        }

        code = (ErrorCode)(byte)raw;
        if (!Enum.IsDefined(code))
        {
            code = default;
            return false;
        }

        return true;
    }

    /// <summary>
    /// Returns raw octets from <see cref="OctetString"/>.
    /// </summary>
    public static byte[] GetRaw(this OctetString value)
    {
        return value.Octets ?? Array.Empty<byte>();
    }

    /// <summary>
    /// Returns the octets as uppercase hexadecimal text.
    /// </summary>
    public static string ToHexString(this OctetString value)
    {
        var raw = value.Octets;
        if (raw.Length == 0)
        {
            return string.Empty;
        }

        var result = new StringBuilder(raw.Length * 2);
        foreach (var b in raw)
        {
            result.Append(b.ToString("X2", CultureInfo.InvariantCulture));
        }

        return result.ToString();
    }

    /// <summary>
    /// Serializes ASN.1 data to BER bytes (legacy helper signature).
    /// </summary>
    public static byte[] ToBytes(this IAsnSerializable value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return value.Encode();
    }
}
