using System;

namespace Lextm.SharpSnmpLib.Mib
{
    internal sealed class TextualConvention : IConstruct
    {
        private enum Status
        {
            current,
            deprecated,
            obsolete
        }

        private string _displayHint;
        private Status _status;
        private string _description;
        private string _reference;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "module")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "name")]
        public TextualConvention(string module, string name, Lexer lexer)
        {
            Symbol temp = lexer.NextNonEOLSymbol;
            
            if (temp == Symbol.DisplayHint)
            {
                // TODO: this needs decoding to a useful format.
                _displayHint = lexer.NextNonEOLSymbol.ToString();
                temp = lexer.NextNonEOLSymbol;
            }

            temp.Expect(Symbol.Status);
            try
            {
                _status = (Status)Enum.Parse(typeof(Status), lexer.NextNonEOLSymbol.ToString());
                temp = lexer.NextNonEOLSymbol;
            }
            catch (ArgumentException)
            {
                temp.Validate(true, "Invalid status");
            }

            temp.Expect(Symbol.Description);
            _description = lexer.NextNonEOLSymbol.ToString();
            temp = lexer.NextNonEOLSymbol;

            if (temp == Symbol.Reference)
            {
                _reference = lexer.NextNonEOLSymbol.ToString();
                temp = lexer.NextNonEOLSymbol;
            }

            temp.Expect(Symbol.Syntax);

            /* 
             * RFC2579 definition:
             *       Syntax ::=   -- Must be one of the following:
             *                    -- a base type (or its refinement), or
             *                    -- a BITS pseudo-type
             *               type
             *             | "BITS" "{" NamedBits "}"
             *
             * From section 3.5:
             *      The data structure must be one of the alternatives defined
             *      in the ObjectSyntax CHOICE or the BITS construct.  Note
             *      that this means that the SYNTAX clause of a Textual
             *      Convention can not refer to a previously defined Textual
             *      Convention.
             *      
             *      The SYNTAX clause of a TEXTUAL CONVENTION macro may be
             *      sub-typed in the same way as the SYNTAX clause of an
             *      OBJECT-TYPE macro.
             * 
             * Therefore the possible values are:
             *      INTEGER
             *      OCTET STRING
             *      OBJECT IDENTIFIER
             *      Integer32
             *      IpAddress
             *      Counter32
             *      TimeTicks
             *      Opaque
             *      Counter64
             *      Unsigned32
             *      Gauge32
             *      BITS
             * With appropriate sub-typing.
             */

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
                temp.Expect(Symbol.String);
                temp = lexer.NextSymbol;
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

                if (previous != null)
                {
                    previous.Validate(true, "end of file reached");
                }
            }
        }
    }
}