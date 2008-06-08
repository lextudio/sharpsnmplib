using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
			Definition iso = Definition.ToDefinition(new OidValueAssignment("SNMPv2-SMI", "iso", null, 1), null);
			root.Add(iso);
			nameTable = new Dictionary<string, Definition>() { { iso.TextualForm, iso } };
		}

		internal Definition Find(string module, string name)
		{
			string full = module + "::" + name;
			if (nameTable.ContainsKey(full))
			{
				return nameTable[full];
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
		IList<MibModule> _pending = new List<MibModule>();
		
		bool ParseModule(MibModule module)
		{
			if (!MibModule.AllDependentsAvailable(module, _modules)) {
				return false;
			}
			_modules.Add(module.Name, module);
			foreach (IEntity node in module.Entities)
			{
				Definition result = root.Add(node);
				if (result != null)
				{
					nameTable.Add(result.TextualForm, result);
				}
			}
			return true;
		}
		
		int ParsePendings()
		{
			int previous;
			int current = _pending.Count;
			while (current != 0)
			{
				previous = current;
				for (int i = 0; i < _pending.Count; i++)//MibModule module in _pending)
				{
					bool succeeded = ParseModule(_pending[i]);
					if (succeeded) {
						_pending.RemoveAt(i);
					}
				}
				current = _pending.Count;
				if (current == previous) {
					// cannot parse more
					break;
				}
			}
			return current;
		}

		internal int Parse(TextReader stream)
		{
			_lexer.Parse(stream);
			MibDocument file = new MibDocument(_lexer);
			IList<MibModule> modules = file.Modules;
			foreach (MibModule module in modules)
			{
				_pending.Add(module);
			}
			ParsePendings();
			return _lexer.SymbolCount;
		}
	}
}
