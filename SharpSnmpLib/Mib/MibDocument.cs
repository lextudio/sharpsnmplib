using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    public class MibDocument
    {
        private readonly IList<MibModule> _modules = new List<MibModule>();

        public IList<MibModule> Modules
        {
            get { return _modules; }
        }

        public void Add(MibModule module)
        {
            _modules.Add(module);
        }
    }
}
