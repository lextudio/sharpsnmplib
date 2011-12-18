using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class IntegerType : ISmiType
    {
        public IList<ISmiValue> NamedNumberList { get; set; }

        public string Name { get; set; }

        public Constraint Constraint { get; set; }
    }
}