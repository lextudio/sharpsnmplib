using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Lextm.SharpSnmpLib.Mib.Ast.ANTLR;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class MibDocument
    {
        private readonly IList<MibModule> _modules = new List<MibModule>();

        public IList<MibModule> Modules
        {
            get { return _modules; }
        }

        public void Add(MibModule module)
        {
            _modules.Add(module);
        }
    }
}
