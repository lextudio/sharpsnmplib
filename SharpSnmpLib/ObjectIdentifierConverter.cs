using System;
using System.ComponentModel;
using System.Globalization;

namespace Lextm.SharpSnmpLib
{
    #if (!CF)
    /// <summary>
    /// The <see cref="TypeConverter"/> dedicated for the <see cref="ObjectIdentifier"/> class.
    /// </summary>
    public sealed class ObjectIdentifierConverter : TypeConverter
    {
        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
    
            return base.CanConvertFrom(context, sourceType);
        }
    
        /// <summary>
        /// Returns whether this converter can convert the object to the specified type.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
    
            return base.CanConvertTo(context, destinationType);
        }
    
        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string s = value as string;
            if (s != null)
            {
                try
                {
                    uint[] oidVal = ObjectIdentifier.Convert(s);
                    return new ObjectIdentifier(oidVal);
                }
                catch
                {
                }
            }
    
            return base.ConvertFrom(context, culture, value);
        }
    
        /// <summary>
        /// Converts the given value object to the specified type, using the arguments.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            ObjectIdentifier oid = value as ObjectIdentifier;
            if (destinationType == typeof(string) && oid != null)
            {
                return oid.ToString(); // GetTextual(null);
            }
    
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    #endif
}
