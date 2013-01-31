// Module interface.
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
 * Date: 5/1/2009
 * Time: 10:40 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// MIB Module interface.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Module name.
        /// </summary>
        string Name
        {
            get;
        }
        
        /// <summary>
        /// Objects.
        /// </summary>
        IList<IEntity> Objects
        {
            get;
        }
        
        /// <summary>
        /// Entities.
        /// </summary>
        IList<IEntity> Entities
        {
            get;
        }

        /// <summary>
        /// Known types.
        /// </summary>
        IDictionary<string, ITypeAssignment> Types
        {
            get;
        }
        
        /// <summary>
        /// Modules that this module dependent on.
        /// </summary>
        IList<string> Dependents
        {
            get;
        }
    }
}
