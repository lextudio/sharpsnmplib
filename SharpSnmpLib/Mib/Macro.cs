using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    internal sealed class Macro : ITypeAssignment
    {
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "temp")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "header")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "module")]
        public Macro(string module, IList<Symbol> header, Lexer lexer)
        {
            Symbol temp;
            while ((temp = lexer.NextSymbol) != Symbol.Begin)
            {                
            }
            
            while ((temp = lexer.NextSymbol) != Symbol.End)
            {
            }
        }
    }
}