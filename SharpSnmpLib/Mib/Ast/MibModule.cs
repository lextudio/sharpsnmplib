using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class MibModule
    {
        private readonly IList<IEntity> _assignments = new List<IEntity>();
        public string Name { get; set; }
        public bool AllExported { get; set; }

        private Exports _exports;
        public Exports Exports
        {
            get { return _exports ?? (_exports = new Exports()) ; }
            set { _exports = value; }
        }

        private Imports _imports;
        public Imports Imports
        {
            get { return _imports ?? (_imports = new Imports()); }
            set { _imports = value; }
        }

        public void AddAssignment(IEntity assignment)
        {
            _assignments.Add(assignment);
        }
    }
}