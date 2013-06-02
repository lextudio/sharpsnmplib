// Object registry interface.
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
    /// Object registry interface.
    /// </summary>
    [CLSCompliant(false)]
    [Obsolete("Please use Pro edition")]
    public interface IObjectRegistry
    {
        /// <summary>
        /// This event occurs when new documents are loaded.
        /// </summary>
        event EventHandler<EventArgs> OnChanged;

        /// <summary>
        /// Object tree.
        /// </summary>        
        IObjectTree Tree { get; }

        /// <summary>
        /// Creates a variable.
        /// </summary>
        /// <param name="textual">The textual ID.</param>
        /// <returns></returns>
        Variable CreateVariable(string textual);

        /// <summary>
        /// Creates a variable.
        /// </summary>
        /// <param name="textual">The textual ID.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        Variable CreateVariable(string textual, ISnmpData data);

        /// <summary>
        /// Gets numercial form from textual form.
        /// </summary>
        /// <param name="textual">Textual</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        uint[] Translate(string textual);

        /// <summary>
        /// Gets numerical form from textual form.
        /// </summary>
        /// <param name="moduleName">Module name.</param>
        /// <param name="name">Object name.</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        uint[] Translate(string moduleName, string name);

        /// <summary>
        /// Gets textual form from numerical form.
        /// </summary>
        /// <param name="numerical">Numerical form</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        string Translate(uint[] numerical);

        /// <summary>
        /// Validates the table.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns></returns>
        bool ValidateTable(ObjectIdentifier identifier);

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Imports the specified modules.
        /// </summary>
        /// <param name="modules">The modules.</param>
        void Import(IEnumerable<IModule> modules);

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