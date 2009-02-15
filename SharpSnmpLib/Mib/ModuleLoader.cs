/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2009/2/8
 * Time: 8:45
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace Lextm.SharpSnmpLib.Mib
{
	/// <summary>
	/// Description of ModuleLoader.
	/// </summary>
	internal class ModuleLoader
	{
		private List<Definition> nodes;
		private List<string> dependents;
		private MibModule module;
		
		public ModuleLoader(TextReader reader, string moduleName)
		{
			nodes = new List<Definition>();
			dependents = new List<string>();

			string line;
			while ((line = reader.ReadLine()) != null)
			{
				if (line.StartsWith("#", StringComparison.Ordinal))
				{
					dependents.AddRange(ParseDependents(line));
					continue;
				}

				nodes.Add(ParseLine(line, moduleName));
			}

			module = new MibModule(moduleName, dependents);
		}
		
		public MibModule Module
		{
			get { return module; }
		}
		
		public IEnumerable<Definition> Nodes
		{
			get { return nodes; }
		}

		private static IEnumerable<string> ParseDependents(string line)
		{
			return line.Substring(1).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
		}

		private static Definition ParseLine(string line, string module)
		{
			string[] content = line.Split(',');
			/* 0: id
			 * 1: type
			 * 2: name
			 * 3: parent name
			 */
			uint[] id = ObjectIdentifier.Convert(content[0]);
			return new Definition(id, content[2], content[3], module, content[1]);
		}
	}
}
