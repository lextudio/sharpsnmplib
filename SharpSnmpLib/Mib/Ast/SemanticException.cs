using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class SemanticException : SharpSnmpException
    {
        public SemanticException(string message) : base(message)
		{
		}

		public SemanticException(string message, Exception inner)
			: base(message, inner)
		{
		}
    }
}
