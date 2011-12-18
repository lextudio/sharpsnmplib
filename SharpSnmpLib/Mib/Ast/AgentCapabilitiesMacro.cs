using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class AgentCapabilitiesMacro : ISmiType
    {
        public EntityStatus Status;
        public string Description;
        public string Reference;
        public IList<AgentCapabilitiesModule> Modules;

        public AgentCapabilitiesMacro(string productRelease)
        {
            
        }

        public string Name { get; set; }
    }
}