using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    public class EnumType : ISmiType
    {
        public IList<ISmiValue> Values { get; set; }

        public EnumType(IList<ISmiValue> values)
        {
            Values = values;
        }

        public string Name { get; set; }
    }
}