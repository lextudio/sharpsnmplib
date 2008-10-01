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
    /// Description of MibInventory.
    /// </summary>
    public class MibInventory
    {
        private readonly string folder = Path.Combine(Directory.GetCurrentDirectory(), "mibs");
        private IList<string> _existing = new List<string>();
        private ObjectRegistry _registry;
        private IDictionary<string, string> _moduleTable = new Dictionary<string, string>();
        
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
            Compiler compiler = new Compiler();
            compiler.Compile(file);
            foreach (MibModule module in compiler.Modules)
            {
                if (!_moduleTable.ContainsKey(module.Name)) 
                {
                    _moduleTable.Add(module.Name, file);
                }                
            }
            _registry.Import(compiler.Modules);
        }
        
        public bool Contains(string module)
        {
            return _moduleTable.ContainsKey(module);
        }
        
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
        
        public void AddFiles(IEnumerable<string> files)
        {
            foreach (string file in files)
            {
                AddFileInner(file);
            }
            _registry.Refresh();
        }
        
        public void AddFile(string orig)
        {
            AddFileInner(orig);
            _registry.Refresh();
        }

        void AddFileInner(string orig)
        {
            if (orig == null) {
                throw new ArgumentNullException("orig");
            }

            if (orig.Length == 0) {
                throw new ArgumentNullException("orig cannot be empty", "orig");
            }

            if (!_existing.Contains(orig)) {
                _existing.Add(orig);
            }

            if (!Directory.Exists(folder)) {
                Directory.CreateDirectory(folder);
            }

            string fileName = Path.Combine(folder, Path.GetFileName(orig));
            if (!File.Exists(fileName)) {
                File.Copy(orig, fileName);
            }

            Import(orig);
        }
        /// <summary>
        /// Removes a MIB file.
        /// </summary>
        /// <param name="file">File</param>
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
