using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Compiler
{
    [Serializable]
    public class MibCompilerException : Exception
    {
        public MibCompilerException(string message) : base(message) { }
    }
}