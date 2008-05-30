/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/21
 * Time: 19:43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace Lextm.SharpSnmpLib.Mib
{
	/// <summary>
	/// Description of SequenceNode.
	/// </summary>
	public class SequenceNode : IAsn
    {
		public SequenceNode(string module, Symbol name, Lexer lexer)
		{
			Symbol temp;
            Symbol last = null;
            while ((temp = lexer.NextSymbol) != null)
            {
                if (temp == Symbol.CloseBracket)
                {
                    break;
                }
                last = temp;
            }
            if (temp == null)
            {
                throw SharpMibException.Create(last);
            }
		}
	}
}
