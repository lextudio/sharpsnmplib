using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class TextualConventionMacro : ISmiType
    {
        public IList<NamedBit> SyntaxNamedBits = new List<NamedBit>();
        public EntityStatus Status;
        public string Description;
        public string Reference;
        public ISmiType Syntax;
        public string DisplayHint { get; set; }
        public string Name { get; set; }
    }
}