using System.Globalization;
using System.Text;
using DotNetSnmp.Utils;

namespace Lextm.SharpSnmpLib;

/// <summary>
/// Helper utility that performs data conversions from/to bytes.
/// </summary>
public static class ByteTool
{
    /// <summary>
    /// Converts decimal.
    /// </summary>
    [Obsolete("Use Convert(this string str) instead.")]
    public static byte[] ConvertDecimal(string description)
    {
        if (description == null)
        {
            throw new ArgumentNullException(nameof(description));
        }

        var result = new List<byte>();
        var content = description.Trim().Split(' ');
        foreach (var part in content)
        {
            if (part.Length == 0)
            {
                continue;
            }

            if (!byte.TryParse(part, out byte value))
            {
                throw new ArgumentException("Invalid decimal string.", nameof(description));
            }

            result.Add(value);
        }

        return result.ToArray();
    }

    /// <summary>
    /// Converts a hexadecimal character sequence into bytes.
    /// </summary>
    [Obsolete("Use Convert(this string str) instead.")]
    public static byte[] Convert(this IEnumerable<char> description)
    {
        if (description == null)
        {
            throw new ArgumentNullException(nameof(description));
        }

        var result = new List<byte>();
        var buffer = new StringBuilder(2);
        foreach (var c in description)
        {
            if (char.IsWhiteSpace(c))
            {
                continue;
            }

            if (!char.IsLetterOrDigit(c))
            {
                throw new ArgumentException("Illegal character found.", nameof(description));
            }

            buffer.Append(c);
            if (buffer.Length != 2)
            {
                continue;
            }

            if (!byte.TryParse(buffer.ToString(), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out byte value))
            {
                throw new ArgumentException("Invalid byte string.", nameof(description));
            }

            result.Add(value);
            buffer.Length = 0;
        }

        if (buffer.Length != 0)
        {
            throw new ArgumentException("Not a complete byte string.", nameof(description));
        }

        return result.ToArray();
    }

    /// <summary>
    /// Converts a hexadecimal string into bytes.
    /// </summary>
    public static byte[] Convert(this string str)
    {
        return Dump.BytesFromHexString(str);
    }

    /// <summary>
    /// Converts bytes into an uppercase hexadecimal string without separators.
    /// </summary>
    public static string Convert(this byte[] bytes)
    {
        return Dump.BytesToHexString(bytes);
    }
}
