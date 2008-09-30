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
    /// The SEQUENCE type represents a list of specified types. This is roughtly analogous to a <code>struct</code> in C.
    /// </summary>
    internal sealed class Sequence : ITypeAssignment
    {
        /// <summary>
        /// Creates a <see cref="Sequence"/> instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="name"></param>
        /// <param name="lexer"></param>
        public Sequence(string module, string name, Lexer lexer)
        {
            Symbol temp;
            // parse between ( )
            temp = lexer.NextNonEOLSymbol; 
            int bracketSection = 0;
            ConstructHelper.Expect(temp, Symbol.OpenBracket);
            bracketSection++;
            while (bracketSection > 0)
            {
                temp = lexer.NextNonEOLSymbol;
                if (temp == Symbol.OpenBracket)
                {
                    bracketSection++;
                }
                else if (temp == Symbol.CloseBracket)
                {
                    bracketSection--;
                }
            }
        }
    }
}