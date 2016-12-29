using System;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Default type resolver to return default type.
    /// </summary>
    public class DefaultTypeResolver : ITypeResolver
    {
        public Type Load(string assembly, string name)
        {
            // IMPORTANT: .NET standard 1.3 does not support this scenario so simply return a default type.
            return typeof(NullMessageHandler);
        }
    }
}