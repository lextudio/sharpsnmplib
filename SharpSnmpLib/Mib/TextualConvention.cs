using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
    internal sealed class TextualConvention : IConstruct
    {
        public TextualConvention(string module, string name, Lexer lexer)
        {
            Symbol temp;
            while ((temp = lexer.NextSymbol) != Symbol.Syntax)
            {
            }
            
            while ((temp = lexer.NextSymbol) == Symbol.EOL)
            {
            }
            
            if (temp == Symbol.Integer)
            {
                // parse between { }
                while ((temp = lexer.NextSymbol) != Symbol.OpenBracket)
                {
                }
                
                while ((temp = lexer.NextSymbol) != Symbol.CloseBracket)
                {
                }
            }
            else if (temp == Symbol.Octet)
            {
                // parse between ( )
                temp = lexer.NextSymbol;
                ConstructHelper.Expect(temp, Symbol.String);
                temp = lexer.NextSymbol;
                if (temp == Symbol.EOL)
                {
                    return;
                }
                
                int parenthesesSection = 0;
                ConstructHelper.Expect(temp, Symbol.OpenParentheses);
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
            else
            {
                Symbol previous = null;
                while ((temp = lexer.NextSymbol) != null)
                {
                    if (previous == Symbol.EOL && temp == Symbol.EOL)
                    {
                        return;
                    }
                    
                    previous = temp;
                }
                
                ConstructHelper.Validate(previous, temp == null, "end of file reached");
            }
        }
    }
}