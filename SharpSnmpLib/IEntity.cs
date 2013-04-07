// Entity interface.
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
 * Date: 2008/5/19
 * Time: 20:10
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Entity interface.
    /// </summary>
    [CLSCompliant(false)]
    public interface IEntity : IConstruct
    {
        /// <summary>
        /// Parent name.
        /// </summary>
        string Parent { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        [CLSCompliant(false)]
        uint Value { get; set; }

        /// <summary>
        /// Gets or sets the parent module.
        /// </summary>
        /// <value>
        /// The parent module.
        /// </value>
        string ParentModule { get; set; }

        /// <summary>
        /// Validates this entity.
        /// </summary>
        /// <param name="knownConstructs">Known constructs.</param>
        /// <param name="fileName">File name.</param>
        /// <returns></returns>
        bool Validate(List<IConstruct> knownConstructs, string fileName);
    }
}