using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class TrapTypeMacro : ISmiType
    {
        public ISmiValue Enterprise;
        public IList<ISmiValue> Variables;
        public ISmiValue Description;
        public ISmiValue Reference;
        public string Name { get; set; }
    }
}