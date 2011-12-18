using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class Variantion {
        public Syntax Syntax;
        public Syntax WriteSyntax;
        public Access Access;
        public IList<ISmiValue> CreationRequires;
        public IList<string> DefaultValueIdentifiers;
        public ISmiValue DefaultValue;
        public string Description;

        public Variantion(ISmiValue variationValue)
        {
            
        }
    }
}