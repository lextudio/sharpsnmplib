using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Lextm.SharpSnmpLib.Mib
{
	class ObjectTree
	{
		IDictionary<string, Definition> nameTable;
		Definition root;
		Lexer _lexer;
		
		public ObjectTree()
		{
			_lexer = new Lexer();
			root = Definition.RootDefinition;
			Definition iso = Definition.ToDefinition(new ObjectIdentifierNode("SNMPv2-SMI", "iso", null, 1), null);
			root.Add(iso);
			nameTable = new Dictionary<string, Definition>() { { "iso", iso } };
		}

		internal Definition Find(string module, string name)
		{
			if (nameTable.ContainsKey(name))
			{
				if (nameTable[name].Module == module)
				{
					return nameTable[name];
				}
			}
			return null;
		}

		internal Definition Find(uint[] numerical)
		{
			if (numerical == null)
			{
				throw new ArgumentNullException("numerical");
			}
			if (numerical.Length == 0)
			{
				throw new ArgumentException("numerical cannot be empty");
			}
			int i = 0;
			Definition result;
			Definition temp = root;
			do
			{
				result = temp[(int)numerical[i]];
				temp = result;
				i++;
			}
			while (i < numerical.Length);
			return result;
		}
		
		IDictionary<string, MibModule> _modules = new Dictionary<string, MibModule>();

		internal int Parse(TextReader stream)
		{
			_lexer.Parse(stream);
			MibDocument file = new MibDocument(_lexer);
			IList<MibModule> modules = file.Modules;
			foreach (MibModule module in modules)
			{
				if (!MibModule.AllDependentsAvailable(module, _modules)) {
					return -1;
				}
				_modules.Add(module.Name, module);
				foreach (IEntity node in module.EntityNodes)
				{
					Definition result = root.Add(node);
					if (result != null)
					{
						nameTable.Add(result.Name, result);
					}
				}
			}
			return _lexer.SymbolCount;
		}
	}
}
