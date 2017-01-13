using System;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// .NET standard 1.3 only helper.
    /// </summary>
    public interface ITypeResolver
    {
        /// <summary>
        /// Loads desired type from an assembly.
        /// </summary>
        /// <param name="assembly">Assembly name.</param>
        /// <param name="name">Type name.</param>
        /// <returns>Type metadata.</returns>
        Type Load(string assembly, string name);
    }
}
