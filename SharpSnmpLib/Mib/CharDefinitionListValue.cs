using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    public class CharDefinitionListValue : ISmiValue
    {
        private readonly IList<CharDefinition> _definitions = new List<CharDefinition>(); 
        public void Add(CharDefinition definition)
        {
            _definitions.Add(definition);
        }
    }
}