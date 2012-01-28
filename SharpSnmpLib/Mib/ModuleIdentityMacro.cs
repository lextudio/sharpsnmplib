using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    public class ModuleIdentityMacro : ISmiType, IEntity
    {
        public Categories Categories;
        public string LastUpdate;
        public string Organization;
        public string ContactInfo;
        public IList<Revision> Revisions = new List<Revision>();
        public string Description;
        [CLSCompliant(false)]
        public uint Value { get; set; }
        public string Parent { get; set; }
        public string Name { get; set; }
        public string ModuleName { get; set; }
    }
}