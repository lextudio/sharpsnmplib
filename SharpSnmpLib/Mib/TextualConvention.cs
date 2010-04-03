namespace Lextm.SharpSnmpLib.Mib
{
    internal sealed class TextualConvention : IConstruct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "module")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "name")]
        public TextualConvention(string module, string name, Lexer lexer)
        {
            Symbol temp;
            while ((temp = lexer.NextSymbol) != Symbol.Syntax)
            {
            }
            
            temp = lexer.NextNonEOLSymbol;            
            if (temp == Symbol.Bits)
            {
                // parse between { }
                temp = lexer.NextNonEOLSymbol;
                if (temp == Symbol.OpenBracket)
                {
                    while ((temp = lexer.NextNonEOLSymbol) != Symbol.CloseBracket)
                    {
                    }
                    
                    return;
                }
                
                // parse between ( )
                if (temp == Symbol.OpenParentheses)
                {
                    while ((temp = lexer.NextNonEOLSymbol) != Symbol.CloseParentheses)
                    {
                    }
                    
                    return;
                }                
            }
            else if (temp == Symbol.Integer)
            {
                // parse between { }
                temp = lexer.NextNonEOLSymbol;
                if (temp == Symbol.OpenBracket)
                {
                    while ((temp = lexer.NextNonEOLSymbol) != Symbol.CloseBracket)
                    {
                    }
                    
                    return;
                }
                
                // parse between ( )
                if (temp == Symbol.OpenParentheses)
                {
                    while ((temp = lexer.NextNonEOLSymbol) != Symbol.CloseParentheses)
                    {
                    }
                    
                    return;
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
                
                ConstructHelper.Validate(previous, true, "end of file reached");
            }
        }
    }
}