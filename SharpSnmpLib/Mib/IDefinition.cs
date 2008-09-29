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

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Definition interface.
    /// </summary>
    [CLSCompliant(false)]
    public interface IDefinition
    {
        /// <summary>
        /// Value.
        /// </summary>
        uint Value
        {
            get;
        }
        
        /// <summary>
        /// Children definitions.
        /// </summary>
        IEnumerable<IDefinition> Children
        {
            get;
        }
        
        /// <summary>
        /// Returns the textual form.
        /// </summary>
        string TextualForm
        {
            get;
        }

        /// <summary>
        /// Indexer.
        /// </summary>
        IDefinition this[uint index]
        {
            get;
        }

        /// <summary>
        /// Module name.
        /// </summary>
        string ModuleName
        {
            get;
        }
        
        /// <summary>
        /// Name.
        /// </summary>
        string Name
        {
            get;
        }
        
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
    }
}
