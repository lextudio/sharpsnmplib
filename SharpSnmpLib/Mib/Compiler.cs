/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/9/30
 * Time: 15:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Description of Compiler.
    /// </summary>
    internal sealed class Compiler
    {
        private IList<MibModule> _modules = new List<MibModule>();
        
        public IList<MibModule> Modules {
            get { return _modules; }
        }
        
        public IList<MibModule> CompileFolder(string folder, string pattern)
        {
            if (folder == null)
            {
                throw new ArgumentNullException("folder");
            }

            if (folder.Length == 0)
            {
                throw new ArgumentException("folder cannot be empty");
            }

            if (!Directory.Exists(folder))
            {
                throw new ArgumentException("folder does not exist: " + folder);
            }

            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }

            if (pattern.Length == 0)
            {
                throw new ArgumentException("pattern cannot be empty");
            }

            foreach (string file in Directory.GetFiles(folder, pattern))
            {
                Compile(file);
            }
            
            return Modules;
        }
        /// <summary>
        /// Loads a MIB file.
        /// </summary>
        /// <param name="fileName">File name</param>
        public IList<MibModule> Compile(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            if (fileName.Length == 0)
            {
                throw new ArgumentException("fileName cannot be empty");
            }

            if (!File.Exists(fileName))
            {
                throw new ArgumentException("file does not exist: " + fileName);
            }

            return Compile(fileName, File.OpenText(fileName));
        }
        
        internal IList<MibModule> Compile(string file, TextReader stream)
        {
            try
            {
                var result = CompileToModules(file, stream);
                foreach (MibModule module in result)
                {
                    _modules.Add(module);
                }
                return result;
            }
            finally
            {
                stream.Close();
            }            
        }

        internal IList<MibModule> Compile(TextReader stream)
        {
            return Compile(null, stream);
        }
        
        private static IList<MibModule> CompileToModules(string file, TextReader stream)
        {
            Lexer lexer = new Lexer();
            lexer.Parse(file, stream);
            MibDocument doc = new MibDocument(lexer);
            return doc.Modules;
        }
    }
}
