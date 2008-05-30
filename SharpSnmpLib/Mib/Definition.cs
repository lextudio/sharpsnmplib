using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
	class Definition
	{
		uint[] _id;
		string _name;
		string _module;
		IList<Definition> _children = new List<Definition>();

		Definition()
		{
			//TODO: update later.
			//_children.Add(new Definition(new uint[] {1}, "iso", "Unknown"));
		}

		public Definition(uint[] id, string name, string module)
		{
			_id = id;
			_name = name;
			_module = module;
		}

		internal static Definition RootDefinition
		{
			get
			{
				return new Definition();
			}
		}
		
		public string TextualForm
		{
			get
			{
				return _module + "::" + _name;
			}
		}

		public Definition this[int index]
		{
			get
			{
				foreach (Definition d in _children)//index >= _children.Count)
				{
                    if (d.NumericalForm[d.NumericalForm.Length - 1] == index)
                    {
                        return d;
                    }					
				}
                throw new ArgumentOutOfRangeException("index");
			}
		}

		public string Module
		{
			get
			{
				return _module;
			}
		}
		
		public string Name
		{
			get
			{
				return _name;
			}
		}

		public uint[] NumericalForm
		{
			get
			{
				return _id;
			}
		}
		
		public Definition Add(IEntity node)
		{
			if (_name == node.Parent) {
                Definition def = ToDefinition(node, this);
                Add(def);
				return def;
			}
			foreach (Definition d in _children)
			{
                Definition result = d.Add(node);
				if (result != null) {
					return result;
				}
			}
			return null;
		}

        public void Add(Definition def)
        {
            _children.Add(def);
        }

        internal static Definition ToDefinition(IEntity entity, Definition parent)
        {
            List<uint> id;
            if (parent == null || parent.NumericalForm == null || parent.NumericalForm.Length == 0)
            {
                id = new List<uint>();
            }
            else
            {
                id = new List<uint>(parent.NumericalForm);
            }
            id.Add((uint)entity.Value);
            return new Definition(id.ToArray(), entity.Name.ToString(), entity.Module);
        }
        
        internal static uint[] GetChildId(uint[] parentId, uint value)
        {
        	List<uint> n = new List<uint>(parentId);
			n.Add(value);
			return n.ToArray();
        }
	}
}
