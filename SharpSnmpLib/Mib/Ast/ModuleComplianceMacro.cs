using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class ModuleComplianceMacro : ISmiType
    {
        public string Reference;
        public IList<ModuleCompliance> Modules;

        public ModuleComplianceMacro(EntityStatus status, string description)
        {
            
        }

        public string Name { get; set; }
    }
}