using System.Collections.Generic;
using System.Linq;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class MibModule
    {
        private readonly IList<IConstruct> _constructs = new List<IConstruct>();
        public string Name { get; set; }
        public bool AllExported { get; set; }

        private Exports _exports;
        public Exports Exports
        {
            get { return _exports ?? (_exports = new Exports()) ; }
            set { _exports = value; }
        }

        private Imports _imports;
        private IList<ValueAssignment> _entities;

        public Imports Imports
        {
            get { return _imports ?? (_imports = new Imports()); }
            set { _imports = value; }
        }

        public IList<IConstruct> Constructs
        {
            get {
                return _constructs;
            }
        }

        public IList<ValueAssignment> Entities  
        {
            get { return _entities ?? (_entities = Constructs.OfType<ValueAssignment>().ToList()); }
        }

        public void Add(IConstruct construct)
        {
            _constructs.Add(construct);
        }
    }
}