/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/21
 * Time: 19:34
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
	/// <summary>
	/// Description of NotificationGroupNode.
	/// </summary>
	public class NotificationGroupNode : IEntity
	{
        string _module;
        Symbol _parent;
        int _value;
        string _name;

        public NotificationGroupNode(string module, IList<Symbol> header, Lexer lexer)
        {
            _module = module;
            _name = header[0].ToString();
            Symbol temp = lexer.NextSymbol;
            if (temp != Symbol.OpenBracket)
            {
                throw SharpMibException.Create(temp);
            }
            _parent = lexer.NextSymbol;
            temp = lexer.NextSymbol;
            bool succeeded = int.TryParse(temp.ToString(), out _value);
            if (!succeeded)
            {
                throw SharpMibException.Create(temp);
            }
            temp = lexer.NextSymbol;
            if (temp != Symbol.CloseBracket)
            {
                throw SharpMibException.Create(temp);
            }
        }


        public string Module
        {
            get { return _module; }
        }

        public string Parent
        {
            get { return _parent.ToString(); }
        }

        public int Value
        {
            get { return _value; }
        }

        public string Name
        {
            get { return _name; }
        }
	}
}
