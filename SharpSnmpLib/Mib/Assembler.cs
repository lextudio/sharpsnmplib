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
        private readonly ObjectTree _tree = new ObjectTree();
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
            _folder = folder;
            if (!Directory.Exists(_folder))
            {
                Directory.CreateDirectory(_folder);
            }

            string[] files = Directory.GetFiles(_folder, "*.module");
            _tree.ImportFiles(files);
        }     
    }
}
