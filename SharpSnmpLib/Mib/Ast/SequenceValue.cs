using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class SequenceValue : ISmiValue
    {
        private readonly IList<NamedValue> _values = new List<NamedValue>(); 
        public void Add(NamedValue namedValue)
        {
            _values.Add(namedValue);
        }
    }
}