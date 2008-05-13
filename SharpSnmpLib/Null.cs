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

namespace Lextm.SharpSnmpLib
{
	/// <summary>
	/// Null type.
	/// </summary>
	public struct Null : ISnmpData, IEquatable<Null>
	{
		/// <summary>
		/// Type code.
		/// </summary>
		public SnmpType TypeCode {
			get {
				return SnmpType.Null;
			}
		}
		/// <summary>
		/// Converts to byte format.
		/// </summary>
		/// <returns></returns>
		public byte[] ToBytes()
		{
			return _null;
		}
		
		readonly static byte[] _null = new byte[] {0x05, 0x00};
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="Null"/>. 
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="Null"/>. </param>
        /// <returns><value>true</value> if the specified <see cref="Object"/> is equal to the current <see cref="Null"/>; otherwise, <value>false</value>.
		/// </returns>		
		public override bool Equals(object obj)
		{
			if (null == obj) {
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
		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>A hash code for the current <see cref="Null"/>.</returns>
		public override int GetHashCode()
		{
			return 0;
		}
		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns><value>true</value> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <value>false</value>.
		///</returns>
		public bool Equals(Null other)
		{
			return (other != null);
		}
        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <param name="left">Left <see cref="Null"/> object</param>
        /// <param name="right">Right <see cref="Null"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are equal, <c>false</c> otherwise.</returns>
		public static bool operator == (Null left, Null right)
		{
			return true;;
		}
        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <param name="left">Left <see cref="Null"/> object</param>
        /// <param name="right">Right <see cref="Null"/> object</param>
        /// <returns>
        /// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
        public static bool operator !=(Null left, Null right)
        {
            return false;
        }
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="Null"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Null";
        }
	}
}
