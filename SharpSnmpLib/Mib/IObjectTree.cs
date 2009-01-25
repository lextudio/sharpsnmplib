using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Object tree interface.
    /// </summary>
    [CLSCompliant(false)]
    public interface IObjectTree
    {
        /// <summary>
        /// Root definition.
        /// </summary>
        IDefinition Root
        {
            get;
        }
        
        /// <summary>
        /// Loaded MIB modules.
        /// </summary>
        ICollection<string> LoadedModules
        {
            get;
        }
        
        /// <summary>
        /// Pending MIB modules.
        /// </summary>
        ICollection<string> PendingModules
        {
            get;
        }

        /// <summary>
        /// Finds an <see cref="IDefinition"/>.
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        IDefinition Find(string moduleName, string name);

        /// <summary>
        /// Removes a module.
        /// </summary>
        /// <param name="moduleName"></param>
        void Remove(string moduleName);
    }
}