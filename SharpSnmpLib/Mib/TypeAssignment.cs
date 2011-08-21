/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/18
 * Time: 13:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.Collections.Generic;
namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Alias.
    /// </summary>
    internal sealed class TypeAssignment : TypeAssignmentBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private string _module;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private string _name;
        private readonly Symbol _left;
        private string _value;
        
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
            _value = last.ToString();
            
            Symbol temp;
            Symbol veryPrevious = null;
            Symbol previous = last;
            while ((temp = lexer.NextSymbol) != null)
            {
                if (veryPrevious == Symbol.EOL && previous == Symbol.EOL && temp.ValidateType())
                {
                    _left = temp;
                    return;
                }
                    
                veryPrevious = previous;
                previous = temp;
            }
            
            previous.Validate(true, "end of file reached");
        }

        public TypeAssignment(string module, string name, IEnumerator<Symbol> enumerator, ref Symbol temp)
        {
            _module = module;
            _name = name;
            _value = temp.ToString();

            temp = enumerator.NextNonEOLSymbol();

            if (temp == Symbol.OpenBracket)
            {
                DecodeEnumerations(enumerator);
                temp = enumerator.NextNonEOLSymbol();
            }
            else if (temp == Symbol.OpenParentheses)
            {
                DecodeRanges(enumerator);
                temp = enumerator.NextNonEOLSymbol();
            }
        }

        public Symbol Left
        {
            get
            {
                return _left;
            }
        }

        public override string Name
        {
            get { return _name; }
        }

        public string Value
        {
            get { return _value; }
        }
    }
}