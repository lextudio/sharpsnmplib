using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    public class BitStringType : ISmiType
    {
        public string Name { get; set; }

        public IList<ISmiValue> NamedNumberList { get; set; }

        public Constraint Constraint { get; set; }
    }
}