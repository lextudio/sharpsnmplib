using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    public class OperationMacro : ISmiType
    {
        public ISmiType ResultType;
        public IList<TypeOrValue> ErrorList;
        public IList<TypeOrValue> LinkedOperationList;
        public string ResultIdentifier { get; set; }

        public string ArgumentIdentifier { get; set; }

        public ISmiType ArgumentType { get; set; }

        public string Name { get; set; }
    }
}