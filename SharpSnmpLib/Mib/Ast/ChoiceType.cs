using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class ChoiceType : ISmiType
    {
        public IList<ElementType> ElementTypes { get; set; }

        public ChoiceType(IList<ElementType> elementTypes)
        {
            ElementTypes = elementTypes;
        }

        public string Name { get; set; }
    }
}