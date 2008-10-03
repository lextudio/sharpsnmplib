/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/10/2
 * Time: 18:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// MIB assembler.
    /// </summary>
    public class Assembler
    {
        private ObjectTree _tree = new ObjectTree();
        
        internal ObjectTree Tree {
            get { return _tree; }
        }

        internal ICollection<string> Modules
        {
            get { return _tree.LoadedModules; }
        }
        
        /// <summary>
        /// Creates an instance of <see cref="Assembler"/>.
        /// </summary>
        /// <param name="folder">Folder.</param>
        public Assembler(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            
            var pendings = new List<string>(Directory.GetFiles(folder, "*.module"));
            Console.WriteLine(pendings.Count + " module files found");
            int current = pendings.Count;
            int previous;
            while (current != 0)
            {
                previous = current;
                var parsed = new List<string>();
                foreach (var file in pendings)
                {
                    List<string> dependents;
                    var nodes = ExtractNodes(file, out dependents);
                    MibModule module = new MibModule(Path.GetFileNameWithoutExtension(file), dependents);
                    bool canParse = _tree.ParseModule(module);
                    if (canParse)
                    {
                        parsed.Add(file);
                    }
                    else
                    {
                        continue;
                    }
                    
                    AddNodes(nodes);
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
        }

        void AddNodes(IEnumerable<IDefinition> nodes)
        {
            var pendings = new List<IDefinition>(nodes);
            int current = pendings.Count;
            int previous;
            while (current != 0) {
                previous = current;
                var parsed = new List<IDefinition>();
                foreach (var node in pendings) {
                    IDefinition def;
                    try {
                        def = _tree.Find(Definition.GetParent(node));
                    }
                    catch (ArgumentOutOfRangeException) {
                        def = null;
                    }

                    if (def == null) {
                        continue;
                    }

                    ((Definition)def).Append(node);
                    parsed.Add(node);
                }

                foreach (var d in parsed) {
                    pendings.Remove(d);
                }

                current = pendings.Count;
                if (current == previous) {
                    break;
                }
            }
        }
        
        private IEnumerable<IDefinition> ExtractNodes(string fileName, out List<string> dependents)
        {
            var result = new List<IDefinition>();
            dependents = new List<string>();
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("#"))
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
        
        private IEnumerable<string> ParseDependents(string line)
        {
            return line.Substring(1).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
        
        private IDefinition ParseLine(string line, string module)
        {
            string[] content = line.Split(',');
            /* 0: id
             * 1: type
             * 2: name
             * 3: parent name
             */
            uint[] id = ObjectIdentifier.Convert(content[0]);
            return new Definition(id, content[2], content[3], module);
        }
    }
}
