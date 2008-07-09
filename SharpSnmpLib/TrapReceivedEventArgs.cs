/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/23
 * Time: 19:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Lextm.SharpSnmpLib
{
	/// <summary>
	/// Provides data for a trap received event.
	/// </summary>
	public sealed class TrapReceivedEventArgs : EventArgs
	{	  
		TrapV1Message _trap;
		/// <summary>
		/// Creates a <see cref="TrapReceivedEventArgs"/> 
		/// </summary>
		/// <param name="trap">Trap message</param>
	    public TrapReceivedEventArgs(TrapV1Message trap)
	    {
	        _trap = trap;
	    }
		/// <summary>
		/// Trap message.
		/// </summary>
	    public TrapV1Message Trap
	    {
	        get
	        {
	            return _trap;
	        }
	    }
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="TrapReceivedEventArgs"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Trap received event args: trap message: " + _trap;
        }
	}
}
