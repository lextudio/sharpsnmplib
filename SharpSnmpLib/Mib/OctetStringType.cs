using System.Collections.Generic;
using System.Linq;

namespace Lextm.SharpSnmpLib.Mib
{
    internal class OctetStringType : AbstractTypeAssignment
    {
        private string _module;
        private readonly string _name;
        private readonly IList<ValueRange> _size;

        public OctetStringType(string module, string name, Lexer lexer)
        {
            _module = module;
            _name = name;
            _size = new List<ValueRange>();

            var temp = lexer.NextSymbol;
            if (temp == Symbol.OpenParentheses)
            {
                _size = DecodeRanges(lexer);
            }

        }

        public OctetStringType(string module, string name, IEnumerator<Symbol> enumerator, ref Symbol temp)
        {
            _module = module;
            _name = name;
            _size = new List<ValueRange>();

            temp = enumerator.NextSymbol();
            if (temp != Symbol.OpenParentheses)
            {
                return;
            }

            _size = DecodeRanges(enumerator);
            temp = enumerator.NextNonEOLSymbol();
        }

        public override string Name
        {
            get { return _name; }
        }

        public bool Contains(int p)
        {
            return _size.Any(range => range.Contains(p));
        }
    }
}
