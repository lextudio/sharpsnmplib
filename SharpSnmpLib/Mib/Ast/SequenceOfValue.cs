using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class SequenceOfValue : ISmiValue
    {
        private readonly IList<ISmiValue> _values = new List<ISmiValue>(); 
        public void Add(ISmiValue value)
        {
            _values.Add(value);
        }
    }
}