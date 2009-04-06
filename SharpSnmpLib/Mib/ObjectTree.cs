using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Object tree class.
    /// </summary>
    internal sealed class ObjectTree : IObjectTree
    {
        private readonly IDictionary<string, MibModule> _loaded = new Dictionary<string, MibModule>();
        private readonly IDictionary<string, MibModule> _pendings = new Dictionary<string, MibModule>();
        private readonly IDictionary<string, IDefinition> nameTable;
        private readonly Definition root;
        
        /// <summary>
        /// Creates an <see cref="ObjectTree"/> instance.
        /// </summary>
        public ObjectTree()
        {
            root = Definition.RootDefinition;
            IDefinition ccitt = new Definition(new OidValueAssignment("SNMPV2-SMI", "ccitt", null, 0), root);
            IDefinition iso = new Definition(new OidValueAssignment("SNMPV2-SMI", "iso", null, 1), root);
            IDefinition joint_iso_ccitt = new Definition(new OidValueAssignment("SNMPV2-SMI", "joint-iso-ccitt", null, 2), root);
            nameTable = new Dictionary<string, IDefinition>();
            nameTable.Add(iso.TextualForm, iso);
            nameTable.Add(ccitt.TextualForm, ccitt);
            nameTable.Add(joint_iso_ccitt.TextualForm, joint_iso_ccitt);
        }
        
        public ObjectTree(ICollection<ModuleLoader> loaders) : this()
        {
            if (loaders == null)
            {
                throw new ArgumentNullException("loaders");
            }
            
            TraceSource source = new TraceSource("Library");
            source.TraceInformation(loaders.Count + " module files found");
            source.Flush();
            source.Close();

            List<Definition> defines = new List<Definition>();
            foreach (ModuleLoader loader in loaders)
            {
                Import(loader.Module);
                defines.AddRange(loader.Nodes);
            }
            
            AddNodes(defines);
            Refresh();
        }
        
        public ObjectTree(IEnumerable<string> files) : this(PrepareFiles(files))
        {
        }
        
        private static ICollection<ModuleLoader> PrepareFiles(IEnumerable<string> files)
        {
            if (files == null)
            {
                throw new ArgumentNullException("files");
            }

            IList<ModuleLoader> result = new List<ModuleLoader>();
            foreach (string file in files)
            {
                string moduleName = Path.GetFileNameWithoutExtension(file);
                using (StreamReader reader = new StreamReader(file))
                {
                    result.Add(new ModuleLoader(reader, moduleName));
                    reader.Close();
                }
            }

            return result;
        }
        
        /// <summary>
        /// Root definition.
        /// </summary>
        public IDefinition Root
        {
            get
            {
                return root;
            }
        }

        public IDefinition Find(string moduleName, string name)
        {
            string full = moduleName + "::" + name;
            if (nameTable.ContainsKey(full))
            {
                return nameTable[full];
            }
            
            return null;
        }
        
        internal IDefinition Find(string name)
        {
            foreach (string key in nameTable.Keys)
            {
                if (string.CompareOrdinal(key.Split(new string[] { "::" }, StringSplitOptions.None)[1], name) == 0)
                {
                    return nameTable[key];
                }
            }
            
            return null;
        }

        public IDefinition Find(uint[] numerical)
        {
            if (numerical == null)
            {
                throw new ArgumentNullException("numerical");
            }
            
            if (numerical.Length == 0)
            {
                throw new ArgumentException("numerical cannot be empty");
            }
            
            IDefinition result = root;
            for (int i = 0; i < numerical.Length; i++)
            {
                result = result.GetChildAt(numerical[i]);
            }

            return result;
        }
        
        internal bool CanParse(MibModule module)
        {
            if (!MibModule.AllDependentsAvailable(module, _loaded))
            {
                return false;
            }
            
            bool exists = _loaded.ContainsKey(module.Name); // FIXME: don't parse the same module twice now.
            if (!exists)
            {
                _loaded.Add(module.Name, module);
            }

            return true;
        }

        internal void Parse(MibModule module)
        {
            TraceSource source = new TraceSource("Library");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            AddNodes(module);
            source.TraceInformation(watch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture) + "-ms used to assemble " + module.Name);
            watch.Stop();
            source.Flush();
            source.Close();
        }

        private IDefinition CreateSelf(IEntity node)
        {
            /* algorithm 2: slower, dropped
            IDefinition parent = Find(node.Parent);
            if (parent == null)
            {
                return null;
            }

            return ((Definition)parent).Add(node);
            // */
            
            // * algorithm 1
            return root.Add(node);
            
            // */
        }

        private void AddNodes(MibModule module)
        {
            List<IEntity> pendingNodes = new List<IEntity>();
            
            // parse all direct nodes.
            foreach (IEntity node in module.Entities)
            {
                if (node.Parent.Contains("."))
                {
                    pendingNodes.Add(node);
                    continue;
                }
                
                IDefinition result = CreateSelf(node);
                if (result == null)
                {
                    pendingNodes.Add(node);
                    continue;
                }

                AddToTable(result);
            }

            // parse indirect nodes.
            int current = pendingNodes.Count;
            while (current != 0)
            {
                List<IEntity> parsed = new List<IEntity>();
                int previous = current;
                foreach (IEntity node in pendingNodes)
                {
                    if (node.Parent.Contains("."))
                    {
                        if (!FirstNodeExists(node))
                        {
                            // wait till first node available.
                            continue;
                        }

                        // create all place holders.
                        IDefinition unknown = CreateExtraNodes(module.Name, node.Parent);
                        if (unknown != null)
                        {
                            node.Parent = unknown.Name;
                            AddToTable(CreateSelf(node));
                        }
                    }
                    else
                    {
                        IDefinition result = CreateSelf(node);
                        if (result == null)
                        {
                            // wait for parent
                            continue;
                        }

                        AddToTable(result);
                    }
                    
                    parsed.Add(node);
                }
                
                foreach (IEntity en in parsed)
                {
                    pendingNodes.Remove(en);
                }

                current = pendingNodes.Count;
                if (previous == current)
                {
                    break;
                }
            }
        }

        private bool FirstNodeExists(IEntity node)
        {
            string first = StringUtility.ExtractName(node.Parent.Split('.')[0]);
            IDefinition firstNode;
            try
            {
                firstNode = Find(first);
            }
            catch (ArgumentOutOfRangeException)
            {
                firstNode = null;
            }
            
            return firstNode != null;
        }

        private IDefinition CreateExtraNodes(string module, string longParent)
        {
            string[] content = longParent.Split('.');
            IDefinition node = Find(StringUtility.ExtractName(content[0]));
            uint[] rootId = node.GetNumericalForm();
            uint[] all = new uint[content.Length + rootId.Length - 1];
            for (int j = rootId.Length - 1; j >= 0; j--)
            {
                all[j] = rootId[j];
            }
            
            // change all to numerical
            for (int i = 1; i < content.Length; i++)
            {
                uint value;
                bool numberFound = uint.TryParse(content[i], out value);
                int currentCursor = rootId.Length + i - 1;
                if (numberFound)
                {
                    all[currentCursor] = value;
                    try
                    {
                        node = Find(ExtractParent(all, currentCursor + 1));
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        node = null;
                    }

                    if (node != null)
                    {
                        continue;
                    }

                    IDefinition subroot = Find(ExtractParent(all, currentCursor));
                    
                    // if not, create Prefix node.
                    IEntity prefix = new OidValueAssignment(module, subroot.Name + "_" + value.ToString(CultureInfo.InvariantCulture), subroot.Name, value);
                    node = CreateSelf(prefix);
                    AddToTable(node);
                }
                else
                {
                    string self = content[i];
                    string parent = content[i - 1];
                    IEntity extra = new OidValueAssignment(module, StringUtility.ExtractName(self), StringUtility.ExtractName(parent), StringUtility.ExtractValue(self));
                    node = CreateSelf(extra);
                    if (node != null)
                    {
                        AddToTable(node);
                        all[currentCursor] = node.Value;
                    }
                    else
                    {
                        TraceSource source = new TraceSource("Library");
                        source.TraceInformation("ignored " + longParent + " in module " + module);
                        source.Flush();
                        source.Close();
                    }
                }
            }

            return node;
        }
        
        internal static uint[] ExtractParent(uint[] input, int length)
        {
            uint[] result = new uint[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = input[i];
            }
            
            return result;
        }
        
        private void AddToTable(IDefinition result)
        {
            if (result != null && !nameTable.ContainsKey(result.TextualForm))
            {
                nameTable.Add(result.TextualForm, result);
            }
        }

        public void Refresh()
        {
            TraceSource source = new TraceSource("Library");
            source.TraceInformation("loading modules started");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            int current = _pendings.Count;
            while (current != 0)
            {
                int previous = current;
                IList<string> parsed = new List<string>();
                foreach (MibModule pending in _pendings.Values)
                {
                    bool succeeded = CanParse(pending);
                    if (succeeded)
                    {
                        Parse(pending);
                        parsed.Add(pending.Name);
                    }
                }

                foreach (string file in parsed)
                {
                    _pendings.Remove(file);
                }

                current = _pendings.Count;
                source.TraceInformation(current.ToString(CultureInfo.InvariantCulture) + " pending after " + watch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture) + "-ms");
                if (current == previous)
                {
                    // cannot parse more
                    break;
                }
            }
            
            watch.Stop();
            
            foreach (string loaded in _loaded.Keys)
            {   
                source.TraceInformation(loaded + " is parsed");
            }
            
            foreach (MibModule module in _pendings.Values)
            {
                StringBuilder builder = new StringBuilder(module.Name);
                builder.Append(" is pending. Missing dependencies: ");
                foreach (string depend in module.Dependents)
                {
                    if (!LoadedModules.Contains(depend))
                    {
                        builder.Append(depend);
                        builder.Append(' ');
                    }
                }
                
                source.TraceInformation(builder.ToString());
            }
            
            source.TraceInformation("loading modules ended");
            source.Flush();
            source.Close();
        }

        public void Import(IEnumerable<MibModule> modules)
        {
            if (modules == null)
            {
                throw new ArgumentNullException("modules");
            }

            TraceSource source = new TraceSource("Library");
            foreach (MibModule module in modules)
            {
                if (LoadedModules.Contains(module.Name) || PendingModules.Contains(module.Name))
                {
                    source.TraceInformation(module.Name + " ignored");
                    continue;
                }

                _pendings.Add(module.Name, module);
            }

            source.Flush();
            source.Close();
        }
        
        internal void Import(MibModule module)
        {
            if (module == null)
            {
                throw new ArgumentNullException("module");
            }

            TraceSource source = new TraceSource("Library");
            if (LoadedModules.Contains(module.Name) || PendingModules.Contains(module.Name))
            {
                source.TraceInformation(module.Name + " ignored");
            }
            else
            {
                _pendings.Add(module.Name, module);
            }
            
            source.Flush();
            source.Close();
        }

        /// <summary>
        /// Loaded MIB modules.
        /// </summary>
        public ICollection<string> LoadedModules
        {
            get { return _loaded.Keys; }
        }
        
        /// <summary>
        /// Pending MIB modules.
        /// </summary>
        public ICollection<string> PendingModules
        {
            get { return _pendings.Keys; }
        }

        private void AddNodes(IEnumerable<Definition> nodes)
        {
            List<Definition> pendings = new List<Definition>(nodes);
            int current = pendings.Count;
            while (current != 0)
            {
                int previous = current;
                List<Definition> parsed = new List<Definition>();
                foreach (Definition node in pendings)
                {
                    IDefinition def;
                    try
                    {
                        def = Find(Definition.GetParent(node));
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        def = null;
                    }

                    if (def == null)
                    {
                        continue;
                    }

                    node.DetermineType(def);
                    ((Definition)def).Append(node);
                    AddToTable(node);
                    parsed.Add(node);
                }

                foreach (Definition d in parsed)
                {
                    pendings.Remove(d);
                }

                current = pendings.Count;
                if (current == previous)
                {
                    break;
                }
            }
        }
        
        #region IObjectTree Members

        public void Remove(string moduleName)
        {
            if (_loaded.ContainsKey(moduleName))
            {
                _loaded.Remove(moduleName);
            }

            if (_pendings.ContainsKey(moduleName))
            {
                _pendings.Remove(moduleName);
            }

            // TODO: remove all those nodes
        }

        #endregion
    }
}
