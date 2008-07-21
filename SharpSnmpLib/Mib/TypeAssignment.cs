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
    internal sealed class TypeAssignment : ITypeAssignment
    {
        private string _module;
        private string _name;
        private Symbol _last;
        private Symbol _left;
        
        /// <summary>
        /// Creates an <see cref="TypeAssignment"/>.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="name"></param>
        /// <param name="lexer"></param>
        /// <param name="last"></param>
        public TypeAssignment(string module, string name, Symbol last, Lexer lexer)
        {
            _module = module;
            _name = name;
            _last = last;
            
            Symbol temp;
            Symbol veryPrevious = null;
            Symbol previous = last;
            string message;
            while ((temp = lexer.NextSymbol) != null)
            {
                if (veryPrevious == Symbol.EOL && previous == Symbol.EOL && ConstructHelper.IsValidIdentifier(temp.ToString(), out message))
                {
                    _left = temp;
                    return;
                }
                    
                veryPrevious = previous;
                previous = temp;
            }
            
            ConstructHelper.Validate(previous, temp == null, "end of file reached");
        }

        public Symbol Left
        {
            get
            {
                return _left;
            }
        }
    }
}