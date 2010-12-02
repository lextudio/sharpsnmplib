using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
    internal class BitsType : ITypeAssignment
    {
        private string _module;
        private string _name;
        private IDictionary<int, string> _mapIntToString;
        private IDictionary<string, int> _mapStringToInt;

        public BitsType(string module, string name, Lexer lexer)
        {
            _module = module;
            _name = name;

            _mapIntToString = new Dictionary<int, string>();
            _mapStringToInt = new Dictionary<string, int>();

            int number;
            lexer.NextSymbol.Expect(Symbol.OpenBracket);
            lexer.Restore(Symbol.OpenBracket);
            while (lexer.NextNonEOLSymbol != Symbol.CloseBracket)
            {
                string identifier = lexer.NextNonEOLSymbol.ToString();

                lexer.NextNonEOLSymbol.Expect(Symbol.OpenParentheses);

                Symbol value = lexer.NextNonEOLSymbol;

                if (int.TryParse(value.ToString(), out number))
                {
                    _mapIntToString.Add(number, identifier);
                    _mapStringToInt.Add(identifier, number);
                }
                else
                {
                    // Unsure if this can use "DefinedValue"s
                }

                lexer.NextNonEOLSymbol.Expect(Symbol.CloseParentheses);
            }
        }

        public int? this[string identifier]
        {
            get
            {
                return (int?)_mapStringToInt[identifier];
            }
        }

        public string this[int value]
        {
            get
            {
                return _mapIntToString[value];
            }
        }
    }
}
