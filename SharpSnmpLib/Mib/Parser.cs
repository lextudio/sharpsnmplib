/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/10/2
 * Time: 17:32
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Description of Parser.
    /// </summary>
    public sealed class Parser
    {
        private Assembler _assembler;
        
        /// <summary>
        /// Creates a <see cref="Parser"/>.
        /// </summary>
        /// <param name="assembler"></param>
        public Parser(Assembler assembler)
        {
            _assembler = assembler;
        }
        
        /// <summary>
        /// Parses MIB documents to module files (*.module).
        /// </summary>
        /// <param name="files"></param>
        public void ParseToModules(IEnumerable<string> files)
        {
            if (files == null)
            {
                throw new ArgumentNullException("files");
            }

            Console.WriteLine("loading existing modules started");            
            Console.WriteLine("loading existing modules ended");
            List<MibModule> modules = new List<MibModule>();
            foreach (string file in files)
            {
                foreach (MibModule module in Compiler.Compile(file))
                {
                    if (_assembler.Tree.LoadedModules.Contains(module.Name) || _assembler.Tree.PendingModules.Contains(module.Name))
                    {
                        Console.WriteLine(module.Name + " ignored");
                        continue;
                    }
                    
                    modules.Add(module);
                }
                
                Console.WriteLine(file + " compiled");
            }
            
            Console.WriteLine("loading new modules started");
            _assembler.RealTree.Import(modules);
            _assembler.RealTree.Refresh();
            Console.WriteLine("loading new modules ended");
            foreach (MibModule module in modules)
            {
                if (_assembler.Tree.PendingModules.Contains(module.Name))
                {
                    Console.WriteLine(module.Name + " pending");
                }
                else
                {                    
                    PersistModuleToFile(_assembler.Folder, module, _assembler.Tree);
                    Console.WriteLine(module.Name + " parsed");
                }
            }
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
