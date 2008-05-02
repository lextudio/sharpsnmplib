/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/30
 * Time: 20:07
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Diagnostics.CodeAnalysis;

[module: SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Scope = "member", Target = "SharpSnmpLib.Null.#op_Equality(SharpSnmpLib.Null,SharpSnmpLib.Null)")]
[module: SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Scope = "member", Target = "SharpSnmpLib.Null.#op_Inequality(SharpSnmpLib.Null,SharpSnmpLib.Null)")]

namespace SharpSnmpLib
{
	/// <summary>
	/// Description of Null.
	/// </summary>
	public struct Null : ISnmpData, IEquatable<Null>
	{
		public SnmpType TypeCode {
			get {
				return SnmpType.Null;
			}
		}
		
		public byte[] ToBytes()
		{
			return _null;
		}
		
		readonly static byte[] _null = new byte[] {0x05, 0x00};
		
		public override bool Equals(object obj)
		{
			if (obj == null) {
				return false;
			}
			if (object.ReferenceEquals(this, obj)) {
				return true;
			}
			if (GetType() != obj.GetType()) {
				return false;
			}
			return true;
		}
		
		public override int GetHashCode()
		{
			return 0;
		}
		
		public bool Equals(Null other)
		{
			return true;
		}
		
		public static bool operator == (Null left, Null right)
		{
			return true;;
		}

		public static bool operator != (Null left, Null right)
		{
			return false;
		}
	}
}
