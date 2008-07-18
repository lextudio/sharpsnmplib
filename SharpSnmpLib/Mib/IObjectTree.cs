using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
