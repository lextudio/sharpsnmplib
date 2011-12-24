using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class SequenceType : ISmiType
    {
        public string Name { get; set; }

        public IList<ISmiType> ElementTypeList { get; set; }
    }
}