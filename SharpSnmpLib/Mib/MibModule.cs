using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lextm.SharpSnmpLib.Mib
{
    public class MibModule : IModule
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
        private List<IEntity> _objects;
        private IList<string> _dependents;
        private readonly Dictionary<string, ITypeAssignment> _typeAssignments = new Dictionary<string, ITypeAssignment>();

        public MibModule(string moduleName, List<string> dependents)
        {
            Name = moduleName;
            _dependents = dependents;
        }

        public MibModule()
        {
        }

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
                    item.ModuleName = Name;
                    var seq = assignment.SmiValue as SequenceValue;
                    if (seq != null && seq.Values.Count > 0)
                    {
                        var number = (NumberLiteralValue) seq.Values[0].Value;
                        item.Value = (uint) number.Value.Value;
                        item.Parent = seq.Values[0].Name;
                        entities.Add(item);
                        continue;
                    }

                    var v = assignment.SmiValue as IdComponentList;
                    if (v != null )
                    {
                        if (v.IdComponents.Count >= 2)
                        {
                            int start = 0;
                            var parent = new StringBuilder();
                            if (v.DefinedValue != null)
                            {
                                parent.AppendFormat("{0}.", v.DefinedValue.Value);
                            }
                            while (start < v.IdComponents.Count - 1)
                            {
                                if (string.IsNullOrEmpty(v.IdComponents[start].Name))
                                {
                                    parent.AppendFormat("{0}.", v.IdComponents[start].Number);
                                }
                                else
                                {
                                    parent.AppendFormat("{0}({1}).", v.IdComponents[start].Name, v.IdComponents[start].Number);
                                }

                                start++;
                            }

                            parent.Length--;
                            item.Parent = parent.ToString();
                            if (item.Parent == "0")
                            {
                                // IMPORTANT: fix for 0.0
                                item.Parent = "ccitt";
                            }

                            item.Value = (uint)v.IdComponents[v.IdComponents.Count - 1].Number;
                            entities.Add(item);
                        }
                    }
                }

                return (_entities = entities);
            }
        }

        public IDictionary<string, ITypeAssignment> Types
        {
            get { return _typeAssignments; }
        }

        public IList<string> Dependents
        {
            get {
                return _dependents ?? (_dependents = Imports.Clauses.Select(import => import.Module).ToList());
            }
        }

        public IList<IEntity> Objects
        {
            get { return (_objects ?? (_objects = Entities.OfType<ObjectTypeMacro>().OfType<IEntity>().ToList())); }
        }

        public void Add(IConstruct construct)
        {
            _constructs.Add(construct);
        }

        internal static bool AllDependentsAvailable(MibModule module, IDictionary<string, MibModule> modules)
        {
            return module.Dependents.All(dependent => DependentFound(dependent, modules));
        }

        const string Pattern = "-V[0-9]+$";

        private static bool DependentFound(string dependent, IDictionary<string, MibModule> modules)
        {
            if (!Regex.IsMatch(dependent, Pattern))
            {
                return modules.ContainsKey(dependent);
            }

            if (modules.ContainsKey(dependent))
            {
                return true;
            }

            string dependentNonVersion = Regex.Replace(dependent, Pattern, string.Empty);
            return modules.ContainsKey(dependentNonVersion);
        }
    }
}