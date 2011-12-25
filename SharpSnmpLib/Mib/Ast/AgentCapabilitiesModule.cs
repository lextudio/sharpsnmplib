using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class AgentCapabilitiesModule {
        private readonly IList<ISmiValue> _includes = new List<ISmiValue>();
        public ISmiValue Value;
        public IList<Variantion> Variations = new List<Variantion>();

        public AgentCapabilitiesModule(string name)
        {
            
        }

        public IList<ISmiValue> Includes
        {
            get {
                return _includes;
            }
        }
    }
}