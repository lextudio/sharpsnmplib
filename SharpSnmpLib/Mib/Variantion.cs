using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    public class Variantion {
        public Syntax Syntax;
        public Syntax WriteSyntax;
        public Access Access;
        public IList<ISmiValue> CreationRequires = new List<ISmiValue>();
        public IList<string> DefaultValueIdentifiers = new List<string>();
        public ISmiValue DefaultValue;
        public string Description;

        public Variantion(ISmiValue variationValue)
        {
            
        }
    }
}