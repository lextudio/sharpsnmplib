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
	public class Symbol : IEquatable<Symbol>
	{
		string _text;
		int _row;
		int _column;
		
		Symbol(string text) : this(text, -1, -1) {}
		
		/// <summary>
		/// Creates a <see cref="Symbol"/>.
		/// </summary>
		/// <param name="text">Text</param>
		/// <param name="row">Row number</param>
		/// <param name="column">column number</param>
		public Symbol(string text, int row, int column)
		{
			_text = text;
			_row = row;
			_column = column;
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
		
		
		static Symbol definitions = new Symbol("DEFINITIONS");
		
		internal static Symbol Definitions
		{
			get
			{
				return definitions;
			}
		}
		
		static Symbol begin = new Symbol("BEGIN");
		
		internal static Symbol Begin
		{
			get
			{
				return begin;
			}
		}
		
		static Symbol object1 = new Symbol("OBJECT");
		
		internal static Symbol Object
		{
			get
			{
				return object1;
			}
		}
		
		static Symbol identifier = new Symbol("IDENTIFIER");
		
		internal static Symbol Identifier
		{
			get
			{
				return identifier;
			}
		}
		
		static Symbol assign = new Symbol("::=");
		
		internal static Symbol Assign
		{
			get
			{
				return assign;
			}
		}
		
		static Symbol openBracket = new Symbol("{");
		
		internal static Symbol OpenBracket {
			get { return openBracket; }
		}

		static Symbol closeBracket = new Symbol("}");
		
		internal static Symbol CloseBracket {
			get { return closeBracket; }
		}
		
		static Symbol comment = new Symbol("--");
		
		internal static Symbol Comment {
			get { return comment; }
		}
		
		static Symbol imports = new Symbol("IMPORTS");
		
		internal static Symbol Imports {
			get { return imports; }
		}
		
		static Symbol semicolon = new Symbol(";");
		
		internal static Symbol Semicolon
		{
			get { return semicolon; }
		}
		
		static Symbol @from = new Symbol("FROM");
		
		internal static Symbol From {
			get { return @from; }
		}

        static Symbol module_Identity = new Symbol("MODULE-IDENTITY");

        internal static Symbol Module_Identity
        {
            get { return module_Identity; }
        }

        static Symbol object_Type = new Symbol("OBJECT-TYPE");

        internal static Symbol Object_Type
        {
            get
            {
                return object_Type;
            }
        }
        
        static Symbol object_Group = new Symbol("OBJECT-GROUP");
        
		internal static Symbol Object_Group {
			get { return object_Group; }
		}
        
        static Symbol notification_Group = new Symbol("NOTIFICATION-GROUP");
        
		internal static Symbol Notification_Group {
			get { return notification_Group; }
		}

        static Symbol module_Compliance = new Symbol("MODULE-COMPLIANCE");
        
		internal static Symbol Module_Compliance {
			get { return module_Compliance; }
		}
        
        static Symbol sequence = new Symbol("SEQUENCE");
        
        internal static Symbol Sequence 
        {
        	get { return sequence; }
        }

        static Symbol notification_Type = new Symbol("NOTIFICATION-TYPE");

        internal static Symbol Notification_Type
        {
            get { return notification_Type; }
        }
        
        static Symbol eol = new Symbol(Environment.NewLine);
        
		internal static Symbol EOL {
			get { return eol; }
		}

        static Symbol object_Identity = new Symbol("OBJECT-IDENTITY");

        internal static Symbol Object_Identity
        {
            get { return object_Identity; }
        }
        
        static Symbol end = new Symbol("END");
        
        internal static Symbol End
        {
        	get { return end; }
        }

        static Symbol macro = new Symbol("MACRO");

        internal static Symbol Macro
        {
            get { return macro; }
        }

        static Symbol choice = new Symbol("CHOICE");

        internal static Symbol Choice
        {
            get { return choice; }
        }
	}
}
