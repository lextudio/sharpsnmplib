using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
	/// <summary>
	/// Object tree class.
	/// </summary>
	public sealed class ObjectTree
	{
		IDictionary<string, MibModule> _parsed = new Dictionary<string, MibModule>();
		IList<MibModule> _pending = new List<MibModule>();
		IDictionary<string, IDefinition> nameTable;
		Definition root;
		Lexer _lexer;
		/// <summary>
		/// Creates an <see cref="ObjectTree"/> instance.
		/// </summary>
		public ObjectTree()
		{
			_lexer = new Lexer();
			root = Definition.RootDefinition;
			IDefinition ccitt = Definition.ToDefinition(new OidValueAssignment("SNMPv2-SMI", "ccitt", null, 0), null);
			IDefinition iso = Definition.ToDefinition(new OidValueAssignment("SNMPv2-SMI", "iso", null, 1), null);
			IDefinition joint_iso_ccitt = Definition.ToDefinition(new OidValueAssignment("SNMPv2-SMI", "joint-iso-ccitt", null, 2), null);
			root.Add(ccitt);
			root.Add(iso);
			root.Add(joint_iso_ccitt);
			nameTable = new Dictionary<string, IDefinition>() { { iso.TextualForm, iso },
				{ccitt.TextualForm, ccitt}, {joint_iso_ccitt.TextualForm, joint_iso_ccitt} };
		}
		/// <summary>
		/// Root definition.
		/// </summary>
		[CLSCompliant(false)]
        public IDefinition Root
        {
            get
            {
                return root;
            }
        }

		internal IDefinition Find(string module, string name)
		{
			string full = module + "::" + name;
			if (nameTable.ContainsKey(full))
			{
				return nameTable[full];
			}
			return null;
		}

		internal IDefinition Find(uint[] numerical)
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
			IDefinition result;
			IDefinition temp = root;
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
				IDefinition result = root.Add(node);
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
