using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class ModuleIdentityMacro : ISmiType
    {
        public Categories Categories;
        public string LastUpdate;
        public string Organization;
        public string ContactInfo;
        public IList<Revision> Revisions = new List<Revision>();
        public string Description;
        public string Name { get; set; }
        
    }
}