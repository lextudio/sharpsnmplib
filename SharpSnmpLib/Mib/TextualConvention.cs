using System;

namespace Lextm.SharpSnmpLib.Mib
{
    internal sealed class TextualConvention : IConstruct
    {
        public enum StatusEnum
        {
            current,
            deprecated,
            obsolete
        }

        private string _name;
        private string _displayHint;
        private StatusEnum _status;
        private string _description;
        private string _reference;
        private ITypeAssignment _syntax;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "module")]
        public TextualConvention(string module, string name, Lexer lexer)
        {
            _name = name;

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
                _status = (StatusEnum)Enum.Parse(typeof(StatusEnum), lexer.NextNonEOLSymbol.ToString());
                temp = lexer.NextNonEOLSymbol;
            }
            catch (ArgumentException)
            {
                temp.Validate(true, "Invalid status");
            }

            temp.Expect(Symbol.Description);
            _description = lexer.NextNonEOLSymbol.ToString().Trim(new char[] { '"' });
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
             * Therefore the possible values are (grouped by underlying type):
             *      INTEGER, Integer32
             *      OCTET STRING, Opaque
             *      OBJECT IDENTIFIER
             *      IpAddress
             *      Counter64
             *      Unsigned32, Counter32, Gauge32, TimeTicks
             *      BITS
             * With appropriate sub-typing.
             */

            temp = lexer.NextNonEOLSymbol;
            if (temp == Symbol.Bits)
            {
                _syntax = new BitsType(module, string.Empty, lexer);
            }
            else if (temp == Symbol.Integer || temp == Symbol.Integer32)
            {
                _syntax = new IntegerType(module, string.Empty, lexer);
            }
            else if (temp == Symbol.Octet)
            {
                temp = lexer.NextSymbol;
                temp.Expect(Symbol.String);
                _syntax = new OctetStringType(module, string.Empty, lexer);
            }
            else if (temp == Symbol.Opaque)
            {
                _syntax = new OctetStringType(module, string.Empty, lexer);
            }
            else if (temp == Symbol.IpAddress)
            {
                SkipSyntax(lexer);
            }
            else if (temp == Symbol.Counter64)
            {
                SkipSyntax(lexer);
            }
            else if (temp == Symbol.Unsigned32 || temp == Symbol.Counter32 || temp == Symbol.Gauge32 || temp == Symbol.TimeTicks)
            {
                SkipSyntax(lexer);
            }
            else if (temp == Symbol.Object)
            {
                temp = lexer.NextSymbol;
                temp.Expect(Symbol.Identifier);
                SkipSyntax(lexer);
            }
            else
            {
                SkipSyntax(lexer);
            }
        }

        // TODO: Remove every call of this and replace with appropriate parsing.
        private static void SkipSyntax(Lexer lexer)
        {
            Symbol temp = null;
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
            return;
        }

        public string Name
        {
            get { return _name; }
        }

        public string DisplayHint
        {
            get { return _displayHint; }
        }

        public StatusEnum Status
        {
            get { return _status; }
        }

        public string Description
        {
            get { return _description; }
        }

        public string Reference
        {
            get { return _reference; }
        }

        public ITypeAssignment Syntax
        {
            get { return _syntax; }
        }
    }
}