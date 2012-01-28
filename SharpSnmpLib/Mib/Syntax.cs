using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    public class Syntax {
        public ISmiType Subtype;
        public IList<NamedBit> SubtypeNamedBits = new List<NamedBit>();
    }
}