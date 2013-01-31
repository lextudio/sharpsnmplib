// Definition interface.
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

/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/6/29
 * Time: 15:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Definition interface.
    /// </summary>
    [CLSCompliant(false)]
    public interface IDefinition : IEntity
    {      
        /// <summary>
        /// Children definitions.
        /// </summary>
        IEnumerable<IDefinition> Children
        {
            get;
        }
        
        /// <summary>
        /// Parent definition.
        /// </summary>
        IDefinition ParentDefinition
        {
            get;
        }

        /// <summary>
        /// Indexer.
        /// </summary>
        IDefinition GetChildAt(uint index);

        /// <summary>
        /// Gets the numerical form.
        /// </summary>
        /// <returns></returns>
        uint[] GetNumericalForm();

        /// <summary>
        /// Type.
        /// </summary>
        DefinitionType Type
        {
            get;
        }

        /// <summary>
        /// The base entity of this definition/
        /// </summary>
        IEntity Entity
        {
            get;
        }
    }
}
