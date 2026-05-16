using System.ComponentModel;
using System.Globalization;

namespace Lextm.SharpSnmpLib;

/// <summary>TypeConverter for ObjectIdentifier (legacy compatibility).</summary>
public class ObjectIdentifierConverter : TypeConverter
{
    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc/>
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

    /// <inheritdoc/>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string s)
            return new ObjectIdentifier(s);
        return base.ConvertFrom(context, culture, value);
    }

    /// <inheritdoc/>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string) && value is ObjectIdentifier oid)
            return oid.ToString();
        return base.ConvertTo(context, culture, value, destinationType);
    }

    /// <inheritdoc/>
    public override string ToString() => nameof(ObjectIdentifierConverter);
}
