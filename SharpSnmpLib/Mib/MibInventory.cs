/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/9/30
 * Time: 15:50
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// MIB inventory.
    /// </summary>
    public class MibInventory
    {
        private readonly string folder = Path.Combine(Directory.GetCurrentDirectory(), "mibs");
        private IList<string> _existing = new List<string>();
        private ObjectRegistry _registry;
        private IDictionary<string, string> _moduleTable = new Dictionary<string, string>();
        
        /// <summary>
        /// Creates a <see cref="MibInventory"/> object.
        /// </summary>
        /// <param name="registry">Registry to use this inventory.</param>
        public MibInventory(ObjectRegistry registry)
        {
            _registry = registry;
            
            // User loaded MIBS
            if (Directory.Exists(folder))
            {
                var files = Directory.GetFiles(folder, "*.*");
                foreach (string file in files)
                {
                    Import(file);
                }
                
                _registry.Refresh();
            }
        }

        private void Import(string file)
        {
            var modules = Compiler.Compile(file);
            foreach (MibModule module in modules)
            {
                if (!_moduleTable.ContainsKey(module.Name)) 
                {
                    _moduleTable.Add(module.Name, file);
                }                
            }
            
            _registry.Import(modules);
        }
        
        /// <summary>
        /// Returns a value that indicates if this module is in the inventory.
        /// </summary>
        /// <param name="module">Module name.</param>
        /// <returns></returns>
        public bool Contains(string module)
        {
            return _moduleTable.ContainsKey(module);
        }
        
        /// <summary>
        /// Returns the real file name for this module.
        /// </summary>
        /// <param name="module">Module name.</param>
        public string this[string module]
        {
            get
            {
                if (!Contains(module))
                {
                    throw new ArgumentException("no such module: " + module, "module");
                }
                
                return _moduleTable[module];
            }
        }
        
        /// <summary>
        /// Adds a few files to the inventory.
        /// </summary>
        /// <param name="files">Files.</param>
        public void AddFiles(IEnumerable<string> files)
        {
            foreach (string file in files)
            {
                AddFileInner(file);
            }
            
            _registry.Refresh();
        }
        
        /// <summary>
        /// Adds a file to the inventory.
        /// </summary>
        /// <param name="file">File name.</param>
        public void AddFile(string file)
        {
            AddFileInner(file);
            _registry.Refresh();
        }

        private void AddFileInner(string orig)
        {
            if (orig == null) 
            {
                throw new ArgumentNullException("orig");
            }

            if (orig.Length == 0) 
            {
                throw new ArgumentException("orig cannot be empty", "orig");
            }

            if (!_existing.Contains(orig)) 
            {
                _existing.Add(orig);
            }

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string fileName = Path.Combine(folder, Path.GetFileName(orig));
            if (!File.Exists(fileName)) 
            {
                File.Copy(orig, fileName);
            }

            Import(orig);
        }
        
        /// <summary>
        /// Removes a MIB file.
        /// </summary>
        /// <param name="file">File name.</param>
        public void RemoveFile(string file)
        {
            if (File.Exists(file))
            {
                _existing.Remove(file);
                File.Delete(file);
            }
            
            // TODO: We also need to figure out how to remove the mibs we just took out!
        }
    }
}
