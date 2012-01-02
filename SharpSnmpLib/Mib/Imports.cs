using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    public class Imports
    {
        private readonly IList<Import> _imports = new List<Import>();

        public IList<Import> Clauses
        {
            get { return _imports; }
        }

        public void Add(Import import)
        {
            _imports.Add(import);
        }
    }
}