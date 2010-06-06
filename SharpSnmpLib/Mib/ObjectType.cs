using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
    internal sealed class ObjectType : IEntity
    {
        private readonly string _module;
        private string _parent;
        private readonly uint _value;
        private readonly string _name;
		private readonly IDictionary<string, string> _properties;

        public ObjectType(string module, IList<Symbol> header, Lexer lexer)
        {
            _module = module;
            _name = header[0].ToString();
			_properties = ParseProperties(header);
            lexer.ParseOidValue(out _parent, out _value);
        }

		private IDictionary<string, string> ParseProperties(IList<Symbol> header)
		{
			IDictionary<string, string> result = new Dictionary<string, string>();
			StringBuilder data = new StringBuilder();
			string previous = string.Empty;
			foreach (Symbol sym in header)
			{
				if (IsProperty(sym))
				{
					result.Add(previous, data.ToString());
					previous = sym.ToString();
					data.Length = 0;
					continue;
				}
				
				data.Append(sym.ToString());
			}
			
			result.Add(previous, data.ToString());
			return result;
		}

		private bool IsProperty(Symbol sym)
		{
			string s = sym.ToString();
			return  s == "SYNTAX" || s == "MAX-ACCESS" || s == "STATUS" || s == "DESCRIPTION";
		}

        public string ModuleName
        {
            get { return _module; }
        }

        public string Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public uint Value
        {
            get { return _value; }
        }

        public string Name
        {
            get { return _name; }
        }
		
		public string Description
		{			
			get { return _properties.ContainsKey("DESCRIPTION") ? _properties["DESCRIPTION"] : string.Empty; }
		}
    }
}