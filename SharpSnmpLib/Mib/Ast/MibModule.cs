using System.Collections.Generic;
using System.Diagnostics;
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
        private IList<IEntity> _entities;
        private IList<ObjectTypeMacro> _objects;

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

        public IList<IEntity> Entities  
        {
            get
            {
                if (_entities != null)
                {
                    return _entities;
                }

                var entities = new List<IEntity>();
                foreach (var construct in Constructs)
                {
                    var assignment = construct as ValueAssignment;
                    if (assignment == null)
                    {
                        continue;
                    }

                    var item = assignment.SmiType as IEntity;
                    if (item == null)
                    {
                        continue;
                    }

                    item.Name = assignment.Name;
                    var seq = assignment.SmiValue as SequenceValue;
                    if (seq != null && seq.Values.Count > 0)
                    {
                        var number = (NumberLiteralValue) seq.Values[0].Value;
                        Debug.Assert(number.Value != null, "number.Value != null");
                        item.Value = number.Value.Value;
                        item.Parent = seq.Values[0].Name;
                    }

                    var v = assignment.SmiValue as IdComponentList;
                    if (v != null && v.IdComponents.Count > 1)
                    {
                        item.Value = v.IdComponents[v.IdComponents.Count - 1].Number;
                        var parent = v.IdComponents[v.IdComponents.Count - 2];
                        if (string.IsNullOrEmpty(parent.Name))
                        {
                            if (v.IdComponents.Count == 2 && parent.Number == 0)
                            {
                                // IMPORTANT: fix for zeroDotZero.
                                item.Parent = "ccitt";
                            }
                            else
                            {
                                item.Parent = parent.Number.ToString();
                            }
                        }
                        else
                        {
                            item.Parent = parent.Name;
                        }
                    }

                    entities.Add(item);
                }

                return (_entities = entities);
            }
        }

        public IList<ObjectTypeMacro> Objects
        {
            get { return _objects ?? (_objects = Entities.OfType<ObjectTypeMacro>().ToList()); }
        }

        public void Add(IConstruct construct)
        {
            _constructs.Add(construct);
        }
    }
}