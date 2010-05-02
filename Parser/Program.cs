/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/10/2
 * Time: 17:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.IO;
using Lextm.SharpSnmpLib.Mib;

namespace Lextm.SharpSnmpLib.Parser
{
    class Program
    {
        public static void Main(string[] args)
        {
            List<string> files = new List<string>();
            string folder = null;
            string pattern = null;
            string root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modules");
            const string folderSwitch = "/folder:";
            const string patternSwitch = "/pattern:";
            const string rootSwitch = "/root:";
            foreach (string arg in args)
            {
                if (arg.StartsWith(folderSwitch))
                {
                    folder = arg.Substring(folderSwitch.Length);
                }
                else if (arg.StartsWith(patternSwitch))
                {
                    pattern = arg.Substring(patternSwitch.Length);
                }
                else if (arg.StartsWith(rootSwitch))
                {
                    root = arg.Substring(rootSwitch.Length);
                }
                else
                {
                    files.Add(arg);
                }
            }
            
            if (folder != null && pattern != null) 
            {
                files.AddRange(Directory.GetFiles(folder, pattern));
            }
            
            Console.WriteLine(files.Count + " files found");
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            
            Mib.Parser parser = new Mib.Parser();
            IEnumerable<MibException> errors;
            IEnumerable<IModule> modules = parser.ParseToModules(files, out errors);
            Assembler assembler = new Assembler(root);
            assembler.Assemble(modules);
            Console.WriteLine("total time " + watch.ElapsedMilliseconds);
            watch.Stop();
            Console.WriteLine("Press any key to exit");
            Console.Read();
        }       
    }
}