using System;
using System.ComponentModel;
using System.Globalization;

namespace Lextm.SharpSnmpLib
{
	#if (!CF)
	/// <summary>
	/// The <see cref="TypeConverter"/> dedicated for the <see cref="ObjectIdentifier"/> class.
	/// </summary>
	public class ObjectIdentifierConverter : TypeConverter
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
				return true;
	
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
				return true;
	
			return base.CanConvertTo(context, destinationType);
		}
	
		/// <summary>
		/// Converts the given object to the type of this converter, using the specified context and culture information.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="culture"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				try
				{
					uint[] oidVal = ObjectIdentifier.ParseString((string)value, null);
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
			if (destinationType == typeof(string) && value is ObjectIdentifier)
			{
				ObjectIdentifier oid = (ObjectIdentifier)value;
				return oid.GetTextual(null);
			}
	
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
	#endif
}
