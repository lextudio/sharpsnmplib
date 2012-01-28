using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    public class ModuleComplianceMacro : ISmiType, IEntity
    {
        public EntityStatus Status { get; set; }
        public string Description { get; set; }
        public string Reference;
        public IList<ModuleCompliance> Modules = new List<ModuleCompliance>();

        public ModuleComplianceMacro(EntityStatus status, string description)
        {
            Status = status;
            Description = description;
        }

        [CLSCompliant(false)]
        public uint Value { get; set; }
        public string Parent { get; set; }
        public string Name { get; set; }
        public string ModuleName { get; set; }
    }
}