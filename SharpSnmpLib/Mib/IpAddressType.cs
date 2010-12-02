using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
    class IpAddressType : ITypeAssignment
    {
        private string _module;
        private string _name;

        public IpAddressType(string module, string name, Lexer lexer)
        {
            _module = module;
            _name = name;
        }
    }
}
