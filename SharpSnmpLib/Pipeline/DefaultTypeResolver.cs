using System;
using System.Reflection;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Default type resolver to return default type.
    /// </summary>
    public class DefaultTypeResolver : ITypeResolver
    {
        /// <inheritdoc />
        public Type Load(string assembly, string name)
        {
            // IMPORTANT: .NET standard 1.3 does not support this scenario so simply return a default type.
            return typeof(NullMessageHandler);
        }

        /// <inheritdoc />
        public Assembly[] GetAssemblies()
        {
#if NETSTANDARD1_3
            return Array.Empty<Assembly>();
#else
            return new Assembly[0];
#endif
        }
    }
}
