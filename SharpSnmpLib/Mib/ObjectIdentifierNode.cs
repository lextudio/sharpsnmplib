/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/17
 * Time: 20:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
	/// <summary>
	/// Object identifier node.
	/// </summary>
	public class ObjectIdentifierNode : IEntity
	{
		string _module;
		string _name;
		string _parent;
		int _value;
		/// <summary>
		/// Creates an <see cref="ObjectIdentifierNode"/>.
		/// </summary>
		/// <param name="module"></param>
		/// <param name="name"></param>
		/// <param name="parent"></param>
		/// <param name="value"></param>
        public ObjectIdentifierNode(string module, string name, string parent, int value)
        {
            _module = module;
            _name = name;
            _parent = parent;
            _value = value;
        }
		/// <summary>
		/// Creates a <see cref="ObjectIdentifierNode"/>.
		/// </summary>
		/// <param name="module">Module name</param>
		/// <param name="name">Name</param>
		/// <param name="lexer">Lexer</param>
		public ObjectIdentifierNode(string module, string name, Lexer lexer)
		{			
			_module = module;
			_name = name;
			Symbol temp = lexer.NextSymbol;
			if (temp != Symbol.OpenBracket) {
				throw SharpMibException.Create(temp);
			}// {			
			temp = lexer.NextSymbol;
			bool isNumerical = int.TryParse(temp.ToString(), out _value);
			if (isNumerical) {
				_parent = null;
			}
			else
			{
				_parent = temp.ToString();
				temp = lexer.NextSymbol;
				isNumerical = int.TryParse(temp.ToString(), out _value);
				if (!isNumerical) {
					throw SharpMibException.Create(temp);
				}
			}
			temp = lexer.NextSymbol;
			if (temp != Symbol.CloseBracket) {
				throw SharpMibException.Create(temp);
			}// }
		}
		/// <summary>
		/// Module name.
		/// </summary>
		public string Module
		{
			get
			{
				return _module;
			}
		}
		/// <summary>
		/// Name.
		/// </summary>
		public string Name
		{
			get 
			{
				return _name;
			}
		}
		/// <summary>
		/// Parent name.
		/// </summary>
		public string Parent
		{
			get
			{
				return _parent;
			}
		}
		/// <summary>
		/// Value.
		/// </summary>
		public int Value
		{
			get
			{
				return _value;
			}
		}
    }
}
