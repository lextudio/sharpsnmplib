using DotNetSnmp.Asn1.SyntaxObjects;
using DotNetSnmp.Common.Definitions;

namespace Lextm.SharpSnmpLib;

public static class CompatibilityExtensions
{
    public static int ToInt32(this Integer32 value)
    {
        return value.Value;
    }

    public static int ToInt32(this ErrorCode value)
    {
        return (int)value;
    }
}
