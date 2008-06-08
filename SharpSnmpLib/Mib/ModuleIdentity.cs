using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
    sealed class ModuleIdentity : IEntity
    {
        string _module;
        string _parent;
        int _value;
        string _name;

        public ModuleIdentity(string module, IList<Symbol> header, Lexer lexer)
        {
            _module = module;
            _name = header[0].ToString();
            ConstructHelper.ParseOidValue(lexer, out _parent, out _value);
        }

        public string Module
        {
            get { return _module; }
        }

        public string Parent
        {
            get { return _parent; }
        }

        public int Value
        {
            get { return _value; }
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
