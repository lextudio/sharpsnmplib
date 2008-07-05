/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/19
 * Time: 20:10
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace Lextm.SharpSnmpLib.Mib
{
	/// <summary>
	/// Entity interface.
	/// </summary>
	interface IEntity : IConstruct
	{
		/// <summary>
		/// Module name.
		/// </summary>
        string Module
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
        /// Parent name.
        /// </summary>
        string Parent
        {
            get;
        }
        /// <summary>
        /// Value.
        /// </summary>
        int Value
        {
            get;            
        }
    }
}


