using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    public class Exports
    {
        private readonly IList<string> _symbols = new List<string>();

        public bool AllExported { get; set; }

        public IList<string> Symbols
        {
            get { return _symbols; }
        }

        public void Add(string symbol)
        {
            _symbols.Add(symbol);
        }
    }
}