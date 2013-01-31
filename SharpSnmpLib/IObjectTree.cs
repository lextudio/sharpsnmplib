// Object tree interface.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib
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

        /// <summary>
        /// Imports the specified enumerable.
        /// </summary>
        /// <param name="modules">The modules.</param>
        void Import(IEnumerable<IModule> modules);

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Searches for the specified OID.
        /// </summary>
        /// <param name="id">The OID.</param>
        /// <returns></returns>
        /// <remarks>This method performs best matching.</remarks>
        SearchResult Search(uint[] id);

        // TODO:
        ///// <summary>
        ///// Decodes a variable using the loaded definitions to the best type.
        ///// 
        ///// Depending on the variable and loaded MIBs can return:
        /////     * Double
        /////     * Int32
        /////     * UInt32
        /////     * UInt64
        ///// </summary>
        ///// <param name="variable">The variable to decode the value of.</param>
        ///// <returns>The best result based on the loaded MIBs.</returns>
        //object Decode(Variable variable);
    }
}