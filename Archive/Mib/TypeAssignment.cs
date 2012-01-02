/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/18
 * Time: 13:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Alias.
    /// </summary>
    internal sealed class TypeAssignment : ITypeAssignment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private string _module;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private string _name;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private Symbol _last;
        private readonly Symbol _left;
        
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

        public Symbol Left
        {
            get
            {
                return _left;
            }
        }
    }
}