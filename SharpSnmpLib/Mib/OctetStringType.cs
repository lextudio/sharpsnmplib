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
        private IList<ValueRange> _size;

        public OctetStringType(string module, string name, Lexer lexer)
        {
            _module = module;
            _name = name;
            _size = new List<ValueRange>();

            Symbol temp = lexer.NextSymbol;
            if (temp == Symbol.EOL)
            {
                return;
            }

            temp.Expect(Symbol.OpenParentheses);
            lexer.NextSymbol.Expect(Symbol.Size);
            lexer.NextSymbol.Expect(Symbol.OpenParentheses);

            while (temp != Symbol.CloseParentheses)
            {
                Symbol value1 = lexer.NextSymbol;
                Symbol value2 = null;

                temp = lexer.NextSymbol;
                if (temp == Symbol.DoubleDot)
                {
                    value2 = lexer.NextSymbol;
                    temp = lexer.NextSymbol;
                }

                var range = new ValueRange(value1, value2);
                value1.Validate(range.Start < 0, "invalid sub-type");
                _size.Add(range);
            }

            lexer.NextSymbol.Expect(Symbol.CloseParentheses);
        }


        public bool Contains(int p)
        {
            foreach (ValueRange range in _size)
            {
                if (range.Contains(p))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
