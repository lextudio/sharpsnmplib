using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class ModuleIdentityMacro : ISmiType, IEntity
    {
        public Categories Categories;
        public string LastUpdate;
        public string Organization;
        public string ContactInfo;
        public IList<Revision> Revisions = new List<Revision>();
        public string Description;
        public long Value { get; set; }
        public string Parent { get; set; }
        public string Name { get; set; }
        
    }
}