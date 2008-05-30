using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
    class MacroNode : IAsn
    {
        public MacroNode(string module, IList<Symbol> header, Lexer lexer)
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
