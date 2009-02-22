/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/7/30
 * Time: 19:19
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Lextm.SharpSnmpLib
{
    using System;
    
    /// <summary>
    /// Walk mode.
    /// </summary>
    [Serializable]
    public enum WalkMode
    {
        /// <summary>
        /// Default mode walk to the end of MIB view.
        /// </summary>
        Default = 0,
        
        /// <summary>
        /// In this mode, walk within subtree.
        /// </summary>
        WithinSubtree = 1
    }
}
