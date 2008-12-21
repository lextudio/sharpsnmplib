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
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Description of Compiler.
    /// </summary>
    internal static class Compiler
    {
        public static IList<MibModule> CompileFolder(string folder, string pattern)
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
            #if (!CF)
            TraceSource source = new TraceSource("Library");
            #endif
            Stopwatch watch = new Stopwatch();
            watch.Start();

            List<MibModule> modules = new List<MibModule>();
            foreach (string file in Directory.GetFiles(folder, pattern))
            {
                modules.AddRange(Compile(file));
            }
            #if (!CF)
            source.TraceInformation(modules.Count.ToString(CultureInfo.InvariantCulture) + " modules parsed after " + watch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture) + "-ms");
            #endif
            watch.Stop();
            #if (!CF)
            source.Flush();
            source.Close();
            #endif
            return modules;
        }
        
        /// <summary>
        /// Loads a MIB file.
        /// </summary>
        /// <param name="fileName">File name</param>
        public static IList<MibModule> Compile(string fileName)
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
        
        internal static IList<MibModule> Compile(string file, TextReader stream)
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

        internal static IList<MibModule> Compile(TextReader stream)
        {
            return Compile(string.Empty, stream);
        }
        
        private static IList<MibModule> CompileToModules(string file, TextReader stream)
        {
            #if (!CF)
            TraceSource source = new TraceSource("Library");
            #endif
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Lexer lexer = new Lexer();
            lexer.Parse(file, stream);
            MibDocument doc = new MibDocument(lexer);
            #if (!CF)
            source.TraceInformation(watch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture) + "-ms used to parse " + file);
            #endif
            watch.Stop();
            #if (!CF)
            source.Flush();
            source.Close();
            #endif
            return doc.Modules;
        }
    }
}
