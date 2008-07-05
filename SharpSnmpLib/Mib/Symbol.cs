/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/17
 * Time: 17:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
	/// <summary>
	/// Description of Symbol.
	/// </summary>
	public sealed class Symbol : IEquatable<Symbol>
	{
		string _text;
		int _row;
		int _column;
		string _file;
		
		Symbol(string text) : this(null, text, -1, -1) {}
		
		/// <summary>
		/// Creates a <see cref="Symbol"/>.
		/// </summary>
		/// <param name="file">File</param>
		/// <param name="text">Text</param>
		/// <param name="row">Row number</param>
		/// <param name="column">column number</param>
		public Symbol(string file, string text, int row, int column)
		{
			_file = file;
			_text = text;
			_row = row;
			_column = column;
		}
		/// <summary>
		/// File.
		/// </summary>
		public string File
		{
			get
			{
				return _file;
			}
		}
		/// <summary>
		/// Row number.
		/// </summary>
		public int Row
		{
			get
			{
				return _row;
			}
		}
		/// <summary>
		/// Column number.
		/// </summary>
		public int Column
		{
			get
			{
				return _column;
			}
		}		
		/// <summary>
		/// Returns a <see cref="String"/> that represents this <see cref="Symbol"/>.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return _text;
		}
		/// <summary>
		/// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="Symbol"/>.
		/// </summary>
		/// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="Symbol"/>. </param>
		/// <returns><value>true</value> if the specified <see cref="Object"/> is equal to the current <see cref="Symbol"/>; otherwise, <value>false</value>.
		/// </returns>
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
			return Equals((Symbol)obj);
		}
		
		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>A hash code for the current <see cref="Symbol"/>.</returns>
		public override int GetHashCode()
		{
			return _text.GetHashCode();
		}

		/// <summary>
		/// The equality operator.
		/// </summary>
		/// <param name="left">Left <see cref="Symbol"/> object</param>
		/// <param name="right">Right <see cref="Symbol"/> object</param>
		/// <returns>
		/// Returns <c>true</c> if the values of its operands are equal, <c>false</c> otherwise.</returns>
		public static bool operator ==(Symbol left, Symbol right)
		{
			return Equals(left, right);
		}
		/// <summary>
		/// Determines whether the specified <see cref="Symbol"/> is equal to the current <see cref="Symbol"/>.
		/// </summary>
		/// <param name="left">Left <see cref="Symbol"/> object</param>
		/// <param name="right">Right <see cref="Symbol"/> object</param>
		/// <returns>
		/// Returns <c>true</c> if the values of its operands are equal, <c>false</c> otherwise.</returns>
		public static bool Equals(Symbol left, Symbol right)
		{
			if (((object)left) == ((object)right))
				return true;

			if ((object)left == null || (object)right == null)
				return false;

			return left._text.Equals(right._text);
		}
		/// <summary>
		/// The inequality operator.
		/// </summary>
		/// <param name="left">Left <see cref="Symbol"/> object</param>
		/// <param name="right">Right <see cref="Symbol"/> object</param>
		/// <returns>
		/// Returns <c>true</c> if the values of its operands are not equal, <c>false</c> otherwise.</returns>
		public static bool operator !=(Symbol left, Symbol right)
		{
			return !(left == right);
		}

		#region IEquatable<Symbol> Members
		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns><value>true</value> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <value>false</value>.
		///</returns>
		public bool Equals(Symbol other)
		{
			return Equals(this, other);
		}

		#endregion
		
		
		internal static Symbol Definitions = new Symbol("DEFINITIONS");		
		internal static Symbol Begin = new Symbol("BEGIN");		
		internal static Symbol Object = new Symbol("OBJECT");		
		internal static Symbol Identifier = new Symbol("IDENTIFIER");
		internal static Symbol Assign = new Symbol("::=");
		internal static Symbol OpenBracket = new Symbol("{");
		internal static Symbol CloseBracket = new Symbol("}");
		internal static Symbol Comment = new Symbol("--");
		internal static Symbol Imports = new Symbol("IMPORTS");
		internal static Symbol Semicolon = new Symbol(";");
		internal static Symbol From = new Symbol("FROM");
        internal static Symbol Module_Identity = new Symbol("MODULE-IDENTITY");
        internal static Symbol Object_Type = new Symbol("OBJECT-TYPE");
        internal static Symbol Object_Group = new Symbol("OBJECT-GROUP");
        internal static Symbol Notification_Group = new Symbol("NOTIFICATION-GROUP");
        internal static Symbol Module_Compliance = new Symbol("MODULE-COMPLIANCE");
        internal static Symbol Sequence = new Symbol("SEQUENCE");
        internal static Symbol Notification_Type = new Symbol("NOTIFICATION-TYPE");
        internal static Symbol EOL = new Symbol(Environment.NewLine);
        internal static Symbol Object_Identity = new Symbol("OBJECT-IDENTITY");
        internal static Symbol End = new Symbol("END");
        internal static Symbol Macro = new Symbol("MACRO");
        internal static Symbol Choice = new Symbol("CHOICE");
        internal static Symbol Trap_Type = new Symbol("TRAP_TYPE");
        internal static Symbol Agent_Capabilities = new Symbol("AGENT-CAPABILITIES");
        internal static Symbol Comma = new Symbol(",");
        internal static Symbol Textual_Convention = new Symbol("TEXTUAL-CONVENTION");
        internal static Symbol Syntax = new Symbol("SYNTAX");
        internal static Symbol Integer = new Symbol("INTEGER");
        internal static Symbol Octet = new Symbol("OCTET");
        internal static Symbol String = new Symbol("STRING");
        internal static Symbol OpenParentheses = new Symbol("(");
        internal static Symbol CloseParentheses = new Symbol(")");
        internal static Symbol Exports = new Symbol("EXPORTS");
	}
}


