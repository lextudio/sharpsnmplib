using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class ModuleComplianceMacro : ISmiType
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

        public string Name { get; set; }
    }
}