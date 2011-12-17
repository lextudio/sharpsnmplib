using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class Import
    {
        public IList<string> Symbols { get; set; }

        public string Module { get; set; }
    }
}