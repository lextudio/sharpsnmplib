using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    public class ChoiceType : ISmiType
    {
        public IList<ISmiType> ElementTypes { get; set; }

        public ChoiceType(IList<ISmiType> elementTypes)
        {
            ElementTypes = elementTypes;
        }

        public string Name { get; set; }
    }
}