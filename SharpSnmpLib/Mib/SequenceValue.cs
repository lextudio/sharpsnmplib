using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    public class SequenceValue : ISmiValue
    {
        private readonly IList<NamedValue> _values = new List<NamedValue>();

        public IList<NamedValue> Values
        {
            get { return _values; }
        }

        public void Add(NamedValue namedValue)
        {
            _values.Add(namedValue);
        }
    }
}