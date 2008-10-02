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

            Lextm.Diagnostics.LoggingService.EnterMethod();
            Lextm.Diagnostics.Stopwatch watch = new Lextm.Diagnostics.Stopwatch();
            watch.Start();


            var modules = new List<MibModule>();
            foreach (string file in Directory.GetFiles(folder, pattern))
            {
                modules.AddRange(Compile(file));
            }
                        
            Lextm.Diagnostics.LoggingService.Debug(modules.Count + " modules parsed after " + watch.Value.ToString() + "-ms");
            watch.Stop();
            Lextm.Diagnostics.LoggingService.LeaveMethod();
            return modules;
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
                return CompileToModules(file, stream);
            }
            finally
            {
                stream.Close();
            }            
        }

        internal IList<MibModule> Compile(TextReader stream)
        {
            return Compile(string.Empty, stream);
        }
        
        private static IList<MibModule> CompileToModules(string file, TextReader stream)
        {
            Lextm.Diagnostics.Stopwatch watch = new Lextm.Diagnostics.Stopwatch();
            watch.Start();
            Lexer lexer = new Lexer();
            lexer.Parse(file, stream);
            MibDocument doc = new MibDocument(lexer);
            Lextm.Diagnostics.LoggingService.Debug(watch.Value.ToString() + "-ms used to parse " + file);
            watch.Stop();
            return doc.Modules;
        }
    }
}
