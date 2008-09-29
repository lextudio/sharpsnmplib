using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Object tree class.
    /// </summary>
    internal sealed class ObjectTree : IObjectTree
    {
        private IDictionary<string, MibModule> _parsed = new SortedDictionary<string, MibModule>();
        private IDictionary<string, MibModule> _pendings = new Dictionary<string, MibModule>();
        private IDictionary<string, string> _newMibs = new Dictionary<string, string>();
        private IDictionary<string, IDefinition> nameTable;
        private Definition root;
        private string mibDir = Directory.GetCurrentDirectory() + "\\mibs\\";
        
        /// <summary>
        /// Creates an <see cref="ObjectTree"/> instance.
        /// </summary>
        public ObjectTree()
        {
            root = Definition.RootDefinition;
            IDefinition ccitt = Definition.ToDefinition(new OidValueAssignment("SNMPv2-SMI", "ccitt", null, 0), root);
            IDefinition iso = Definition.ToDefinition(new OidValueAssignment("SNMPv2-SMI", "iso", null, 1), root);
            IDefinition joint_iso_ccitt = Definition.ToDefinition(new OidValueAssignment("SNMPv2-SMI", "joint-iso-ccitt", null, 2), root);
            nameTable = new Dictionary<string, IDefinition>()
            {
                {
                    iso.TextualForm, iso
                },
                {
                    ccitt.TextualForm, ccitt
                },
                {
                    joint_iso_ccitt.TextualForm, joint_iso_ccitt
                }
            };
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

        internal IDefinition Find(string module, string name)
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
                if (key.Split(new string[] {"::"}, StringSplitOptions.None)[1] == name)
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
                result = temp[numerical[i]];
                temp = result;
                i++;
            }
            while (i < numerical.Length);
            return result;
        }
        
        private bool ParseModule(MibModule module)
        {
            if (!MibModule.AllDependentsAvailable(module, _parsed))
            {
                return false;
            }
            
            if (_parsed.ContainsKey(module.Name))
            {
                return true;
            }
            
            _parsed.Add(module.Name, module);
            foreach (IEntity node in module.Entities)
            {
                IDefinition result = root.Add(node);
                if (result == null && node.Parent.Contains("."))
                {
                    string[] content = node.Parent.Split('.');
                    IDefinition subroot = Find(content[0]);
                    uint value = uint.Parse(content[1]);
                    IDefinition unknown;
                    try 
                    {
                        unknown = subroot[value];
                    }
                    catch (ArgumentOutOfRangeException) 
                    {
                        IEntity prefixNode = new OidValueAssignment(module.Name, subroot.Name + "Prefix", subroot.Name, value);
                        unknown = root.Add(prefixNode);
                    }
                    
                    AddToTable(unknown);
                    node.Parent = unknown.Name;
                    result = root.Add(node);
                }                

                AddToTable(result);
            }
            
            return true;
        }

        private void AddToTable(IDefinition result)
        {
            if (result != null && !nameTable.ContainsKey(result.TextualForm)) {
                nameTable.Add(result.TextualForm, result);
            }
        }
        
        private int ParsePendings()
        {
            int previous;
            int current = _pendings.Count;
            while (current != 0)
            {
                previous = current;
                IList<string> parsed = new List<string>();
                foreach (MibModule pending in _pendings.Values)
                {
                    bool succeeded = ParseModule(pending);
                    if (succeeded)
                    {
                        parsed.Add(pending.Name);
                        
                        // Console.WriteLine("LoadFile(new StreamReader(new MemoryStream(Resource." + pending.Name + ")));");
                    }
                }
                
                foreach (string file in parsed)
                {
                    _pendings.Remove(file);
                }
                
                current = _pendings.Count;
                if (current == previous)
                {
                    // cannot parse more
                    break;
                }
            }
            
            return current;
        }

        internal static IList<MibModule> ParseFile(string file, TextReader stream)
        {
            Lexer lexer = new Lexer();
            lexer.Parse(file, stream);
            MibDocument doc = new MibDocument(lexer);
            return doc.Modules;
        }

        internal void Parse(string file, TextReader stream)
        {
            IList<MibModule> modules = ParseFile(file, stream);
            foreach (MibModule module in modules)
            {
                if (_pendings.ContainsKey(module.Name))
                {
                    _pendings.Remove(module.Name); // always add new module
                }
                
                _pendings.Add(module.Name, module);

                if (file != null && !_newMibs.ContainsKey(module.Name))
                {
                    _newMibs.Add(module.Name, file);
                }
            }

            if (file != null)
            {
                if (!Directory.Exists(mibDir))
                {
                    Directory.CreateDirectory(mibDir);
                }
                
                if (!File.Exists(mibDir + System.IO.Path.GetFileName(file)))
                {
                    File.Copy(file, mibDir + System.IO.Path.GetFileName(file));
                }
            }

            ParsePendings();
        }

        internal void RemoveMib(string mib, string group)
        {
            string file = string.Empty;
            _newMibs.TryGetValue(mib, out file);

            if (File.Exists(file))
            {
                _newMibs.Remove(mib);
                File.Delete(file);
            }

            if (group == "lvgLoaded")
            {
                _parsed.Remove(mib);
            }
            else
            {
                _pendings.Remove(mib);
            }
        }

        /// <summary>
        /// Loaded MIB modules.
        /// </summary>
        public ICollection<string> LoadedModules
        {
            get
            {
                return _parsed.Keys;
            }
        }
        
        /// <summary>
        /// Pending MIB modules.
        /// </summary>
        public ICollection<string> PendingModules
        {
            get
            {
                return _pendings.Keys;
            }
        }

        /// <summary>
        /// Compiled MIB modules.
        /// </summary>
        public ICollection<string> NewModules
        {
            get
            {
                return _newMibs.Keys;
            }
        }
    }
}