/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/18
 * Time: 13:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace Lextm.SharpSnmpLib.Mib
{
	/// <summary>
	/// Alias.
	/// </summary>
	public class AliasNode : IAsn
	{
		string _module;
		Symbol _name;
		Symbol _last;
		/// <summary>
		/// Creates an <see cref="AliasNode"/>.
		/// </summary>
		/// <param name="module"></param>
		/// <param name="name"></param>
		/// <param name="lexer"></param>
		/// <param name="last"></param>
		public AliasNode(string module, Symbol name, Symbol last, Lexer lexer)
		{
            //TODO:
			_module = module;
			_name = name;
			_last = last;
			
            Symbol temp;
            Symbol previous = last;
            do
            {
                temp = lexer.NextSymbol;
                if (temp == Symbol.Choice)
                {
                    while ((temp = lexer.NextSymbol) != Symbol.OpenBracket)
                    {
                    }
                    while ((temp = lexer.NextSymbol) != Symbol.CloseBracket)
                    {
                    }
                    return;
                }
                if (last == Symbol.EOL && temp == Symbol.EOL)
                {
                    return;
                }
                last = temp;
            } while (temp != null);
            throw SharpMibException.Create(temp);
		}
	}
}
