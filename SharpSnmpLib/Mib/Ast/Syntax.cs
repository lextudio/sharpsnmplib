using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class Syntax {
        public ISmiType Subtype;
        public IList<NamedBit> SubtypeNamedBits = new List<NamedBit>();
    }
}