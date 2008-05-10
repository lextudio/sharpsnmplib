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
	public class TrapReceivedEventArgs : EventArgs
	{	  
		TrapMessage _trap;
		/// <summary>
		/// Creates a <see cref="TrapReceivedEventArgs"/> 
		/// </summary>
		/// <param name="trap">Trap message</param>
	    public TrapReceivedEventArgs(TrapMessage trap)
	    {
	        _trap = trap;
	    }
		/// <summary>
		/// Trap message.
		/// </summary>
	    public TrapMessage Trap
	    {
	        get
	        {
	            return _trap;
	        }
	    }
	}
}
