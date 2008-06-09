using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
	sealed class ObjectTree
	{
		IDictionary<string, MibModule> _parsed = new Dictionary<string, MibModule>();
		IList<MibModule> _pending = new List<MibModule>();
		IDictionary<string, Definition> nameTable;
		Definition root;
		Lexer _lexer;
		
		public ObjectTree()
		{
			_lexer = new Lexer();
			root = Definition.RootDefinition;
			Definition ccitt = Definition.ToDefinition(new OidValueAssignment("SNMPv2-SMI", "ccitt", null, 0), null);
			Definition iso = Definition.ToDefinition(new OidValueAssignment("SNMPv2-SMI", "iso", null, 1), null);
			Definition joint_iso_ccitt = Definition.ToDefinition(new OidValueAssignment("SNMPv2-SMI", "joint-iso-ccitt", null, 2), null);
			root.Add(iso);
			root.Add(ccitt);
			root.Add(joint_iso_ccitt);
			nameTable = new Dictionary<string, Definition>() { { iso.TextualForm, iso },
				{ccitt.TextualForm, ccitt}, {joint_iso_ccitt.TextualForm, joint_iso_ccitt} };
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
		
		bool ParseModule(MibModule module)
		{
			if (!MibModule.AllDependentsAvailable(module, _parsed)) {
				return false;
			}
			if (_parsed.ContainsKey(module.Name)) {
				return true;
			}
			_parsed.Add(module.Name, module);
			foreach (IEntity node in module.Entities)
			{
				Definition result = root.Add(node);
				if (result != null && !nameTable.ContainsKey(result.TextualForm))
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
				for (int i = 0; i < _pending.Count; i++)
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

		internal int Parse(string file, TextReader stream)
		{
			_lexer.Parse(file, stream);
			MibDocument doc = new MibDocument(_lexer);
			IList<MibModule> modules = doc.Modules;
			foreach (MibModule module in modules)
			{
				_pending.Add(module);
			}
			ParsePendings();
			return _lexer.SymbolCount;
		}
	}
}
