using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
    internal class OctetStringType : ITypeAssignment
    {
        private string _module;
        private string _name;

        public OctetStringType(string module, string name, Lexer lexer)
        {
            _module = module;
            _name = name;
            Symbol temp = lexer.NextSymbol;
            if (temp == Symbol.EOL)
            {
                return;
            }

            int parenthesesSection = 0;
            temp.Expect(Symbol.OpenParentheses);
            parenthesesSection++;
            while (parenthesesSection > 0)
            {
                temp = lexer.NextSymbol;
                if (temp == Symbol.OpenParentheses)
                {
                    parenthesesSection++;
                }
                else if (temp == Symbol.CloseParentheses)
                {
                    parenthesesSection--;
                }
            }
        }
    }
}
