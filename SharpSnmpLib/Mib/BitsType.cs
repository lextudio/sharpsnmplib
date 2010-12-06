using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
    internal class BitsType : AbstractTypeAssignment
    {
        private string _module;
        private string _name;
        private IDictionary<string, int> _map;

        public BitsType(string module, string name, Lexer lexer)
        {
            _module = module;
            _name = name;
            lexer.NextNonEOLSymbol.Expect(Symbol.OpenBracket);
            _map = DecodeEnumerations(lexer);
        }

        public BitsType(string module, string name, IEnumerator<Symbol> enumerator)
        {
            _module = module;
            _name = name;
            enumerator.NextNonEOLSymbol().Expect(Symbol.OpenBracket);
            _map = DecodeEnumerations(enumerator);
        }

        public int? this[string identifier]
        {
            get
            {
                return (int?)_map[identifier];
            }
        }

        public override string Name
        {
            get { return _name; }
        }
    }
}
