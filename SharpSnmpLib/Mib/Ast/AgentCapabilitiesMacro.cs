using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class AgentCapabilitiesMacro : ISmiType, IEntity
    {
        public EntityStatus Status;
        public string Description;
        public string Reference;
        public IList<AgentCapabilitiesModule> Modules;

        public AgentCapabilitiesMacro(string productRelease)
        {
            
        }

        public long Value { get; set; }
        public string Parent { get; set; }
        public string Name { get; set; }
    }
}