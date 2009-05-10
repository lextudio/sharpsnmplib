/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 5/1/2009
 * Time: 10:40 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
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
        /// Modules that this module dependent on.
        /// </summary>
        IList<string> Dependents
        {
            get;
        }
    }
}
