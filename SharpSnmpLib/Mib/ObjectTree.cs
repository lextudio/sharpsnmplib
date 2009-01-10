using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Object tree class.
    /// </summary>
    internal sealed class ObjectTree : IObjectTree
    {
        private IDictionary<string, MibModule> _loaded = new SortedDictionary<string, MibModule>();
        private IDictionary<string, MibModule> _pendings = new Dictionary<string, MibModule>();
        private IDictionary<string, IDefinition> nameTable;
        private Definition root;
        
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

        public IDefinition Find(string module, string name)
        {
            string full = module + "::" + name;
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

        internal IDefinition Find(uint[] numerical)
        {
            if (numerical == null)
            {
                throw new ArgumentNullException("numerical");
            }
            
            if (numerical.Length == 0)
            {
                throw new ArgumentException("numerical cannot be empty");
            }
            
            int i = 0;
            IDefinition result;
            IDefinition temp = root;
            do
            {
                result = temp.GetChildAt(numerical[i]);
                temp = result;
                i++;
            }
            while (i < numerical.Length);
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
            int previous;
            while (current != 0)
            {
                List<IEntity> parsed = new List<IEntity>();
                previous = current;
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
                        node.Parent = unknown.Name;
                        AddToTable(CreateSelf(node));
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
            for (int j = 0; j < rootId.Length; j++)
            {
                all[j] = rootId[j];
            }
            
            // change all to numerical
            for (int i = 1; i < content.Length; i++)
            {
                uint value;
                bool isUInt = uint.TryParse(content[i], out value);
                int currentCursor = rootId.Length + i - 1;
                if (isUInt)
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
                    AddToTable(node);
                    all[currentCursor] = node.Value;
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
        
        internal void Refresh()
        {
            TraceSource source = new TraceSource("Library");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            int previous;
            int current = _pendings.Count;
            while (current != 0)
            {
                previous = current;
                IList<string> parsed = new List<string>();
                foreach (MibModule pending in _pendings.Values)
                {
                    bool succeeded = CanParse(pending);
                    if (succeeded)
                    {
                        Parse(pending);
                        parsed.Add(pending.Name);
                    }
                    
                    // Console.WriteLine("LoadFile(new StreamReader(new MemoryStream(Resource." + pending.Name + ")));");
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
            source.Flush();
            source.Close();
        }

        internal void Import(IEnumerable<MibModule> modules)
        {
            if (modules == null)
            {
                throw new ArgumentNullException("modules");
            }

            foreach (MibModule module in modules)
            {
                if (_pendings.ContainsKey(module.Name))
                {
                    // _pendings.Remove(module.Name); // always add new module
                    continue;
                }
                
                _pendings.Add(module.Name, module);
            }
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

        internal int ImportFiles(IEnumerable<string> files)
        {
            List<string> pendings = new List<string>(files);
            Console.WriteLine(pendings.Count + " module files found");
            int current = pendings.Count;
            int previous;
            while (current != 0)
            {
                previous = current;
                List<string> parsed = new List<string>();
                foreach (string file in pendings)
                {
                    List<string> dependents;
                    IEnumerable<Definition> nodes = ExtractNodes(file, out dependents);
                    MibModule module = new MibModule(Path.GetFileNameWithoutExtension(file), dependents);
                    bool canParse = CanParse(module);
                    if (!canParse)
                    {
                        continue;
                    }

                    AddNodes(nodes);
                    parsed.Add(file);
                    Console.WriteLine(file + " loaded");
                }

                foreach (string p in parsed)
                {
                    pendings.Remove(p);
                }

                current = pendings.Count;
                if (current == previous)
                {
                    break;
                }
            }
            
            Console.WriteLine(current + " pending");
            return current;
        }

        private void AddNodes(IEnumerable<Definition> nodes)
        {
            List<Definition> pendings = new List<Definition>(nodes);
            int current = pendings.Count;
            int previous;
            while (current != 0)
            {
                previous = current;
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

        private static IEnumerable<Definition> ExtractNodes(string fileName, out List<string> dependents)
        {
            List<Definition> result = new List<Definition>();
            dependents = new List<string>();
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("#", StringComparison.Ordinal))
                    {
                        dependents.AddRange(ParseDependents(line));
                        continue;
                    }

                    result.Add(ParseLine(line, fileName));
                }
                
                reader.Close();
            }
            
            return result;
        }

        private static IEnumerable<string> ParseDependents(string line)
        {
            return line.Substring(1).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static Definition ParseLine(string line, string module)
        {
            string[] content = line.Split(',');
            /* 0: id
             * 1: type
             * 2: name
             * 3: parent name
             */
            uint[] id = ObjectIdentifier.Convert(content[0]);
            return new Definition(id, content[2], content[3], module, content[1]);
        }

        #region IObjectTree Members

        public void Remove(string module)
        {
            if (_loaded.ContainsKey(module))
            {
                _loaded.Remove(module);
            }

            if (_pendings.ContainsKey(module))
            {
                _pendings.Remove(module);
            }

            // TODO: remove all those nodes
        }

        #endregion
    }
}