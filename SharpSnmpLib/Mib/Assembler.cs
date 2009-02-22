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
using System.Diagnostics;
using System.IO;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// MIB assembler.
    /// </summary>
    public class Assembler
    {
        private readonly ObjectTree _tree;
        private readonly string _folder;

        /// <summary>
        /// Folder.
        /// </summary>
        public string Folder
        {
            get { return _folder; }
        }
        
        /// <summary>
        /// Tree.
        /// </summary>
        [CLSCompliant(false)]
        public IObjectTree Tree
        {
            get { return _tree; }
        }

        internal ObjectTree RealTree
        {
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
            _folder = Path.GetFullPath(folder);
            if (!Directory.Exists(_folder))
            {
                Directory.CreateDirectory(_folder);
            }

            string[] files = Directory.GetFiles(_folder, "*.module");
            _tree = new ObjectTree(files);
        }

        /// <summary>
        /// Assemblers modules.
        /// </summary>
        /// <param name="modules">Modules.</param>
        public void Assemble(IEnumerable<MibModule> modules)
        {
            TraceSource source = new TraceSource("Library");
            source.TraceInformation("loading new modules started");
            RealTree.Import(modules);
            RealTree.Refresh();

            foreach (MibModule module in modules)
            {
                if (Tree.PendingModules.Contains(module.Name))
                {
                    continue;
                }
                
                source.TraceInformation(module.Name + " is parsed");
                PersistModuleToFile(Folder, module, Tree);
            }
            
            foreach (string module in Tree.PendingModules)
            {
                source.TraceInformation(module + " is pending");
            }
            
            source.TraceInformation("loading new modules ended");
            source.Flush();
            source.Close();
        }

        internal static void PersistModuleToFile(string folder, MibModule module, IObjectTree tree)
        {
            string fileName = Path.Combine(folder, module.Name + ".module");
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.Write("#");
                foreach (string dependent in module.Dependents)
                {
                    writer.Write(dependent);
                    writer.Write(',');
                }
                
                writer.WriteLine();
                foreach (IEntity entity in module.Entities)
                {
                    IDefinition node = tree.Find(module.Name, entity.Name);
                    if (node == null)
                    {
                        continue;
                    }
                    
                    uint[] id = node.GetNumericalForm();
                    /* 0: id
                     * 1: type
                     * 2: name
                     * 3: parent name
                     */
                    writer.WriteLine(ObjectIdentifier.Convert(id) + "," + entity.GetType() + "," + entity.Name + "," + entity.Parent);
                }
                
                writer.Close();
            }
        }
    }
}
