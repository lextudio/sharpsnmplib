using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    public class SetType : ISmiType
    {
        public string Name { get; set; }

        public IList<ISmiType> ElementTypeList { get; set; }
    }
}