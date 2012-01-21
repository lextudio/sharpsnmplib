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
using System.Globalization;
using System.IO;
using System.Linq;
using Antlr.Runtime;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Description of Parser.
    /// </summary>
    public sealed class Parser
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger("Lextm.SharpSnmpLib.Mib");

        /// <summary>
        /// Parses MIB documents to module files (*.module).
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public IEnumerable<IModule> ParseToModules(IEnumerable<string> files, out IEnumerable<CompilerError> errors)
        { 
            if (files == null)
            {
                throw new ArgumentNullException("files");
            }

            var list = new List<CompilerError>();
            var modules = new List<IModule>();
            foreach (string file in files)
            {
                modules.AddRange(Compile(file, list));
                Logger.Info(file + " compiled");
            }
            
            errors = list;
            return modules;
        }

        /// <summary>
        /// Loads a MIB file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="errors">Errors.</param>
        public static IEnumerable<IModule> Compile(string fileName, List<CompilerError> errors)
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

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            var lexer = new SmiLexer(new ANTLRFileStream(fileName));
            var tokens = new CommonTokenStream(lexer);
            var parser = new SmiParser(tokens);
            try
            {
                var doc = parser.GetDocument(fileName);
                Logger.Info(watch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture) + "-ms used to parse " +
                            fileName);
                watch.Stop();
                errors.AddRange(parser.Errors);
                return doc.Modules.OfType<IModule>();
            }
            catch (RecognitionException ex)
            {
                errors.Add(new CompilerError(ex) {FileName = fileName});
                return new IModule[0];
            }
        }

        /// <summary>
        /// Loads a MIB file.
        /// </summary>
        public static IEnumerable<IModule> Compile(Stream stream, List<CompilerError> errors)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            var lexer = new SmiLexer(new ANTLRInputStream(stream));
            var tokens = new CommonTokenStream(lexer);
            var parser = new SmiParser(tokens);
            try
            {
                var doc = parser.GetDocument(string.Empty);
                Logger.Info(watch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture) + "-ms used to parse");
                watch.Stop();
                errors.AddRange(parser.Errors);
                return doc.Modules.OfType<IModule>().ToList();
            }
            catch (RecognitionException ex)
            {
                errors.Add(new CompilerError(ex));
                return new IModule[0];
            }
        }
    }
}
