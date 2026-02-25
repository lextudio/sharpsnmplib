using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;

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
}
