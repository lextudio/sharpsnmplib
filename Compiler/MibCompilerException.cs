using System;

namespace Lextm.SharpSnmpLib.Compiler
{
    [Serializable]
    public class MibCompilerException : Exception
    {
        public MibCompilerException(string message) : base(message) { }
    }
}