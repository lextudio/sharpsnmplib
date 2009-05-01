/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 5/1/2009
 * Time: 10:40 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Description of IModule.
    /// </summary>
    public interface IModule
    {
        string Name
        {
            get;
        }
        
        IList<IEntity> Objects
        {
            get;
        }
        
        IList<IEntity> Entities
        {
            get;
        }
        
        IList<string> Dependents
        {
            get;
        }
    }
}
