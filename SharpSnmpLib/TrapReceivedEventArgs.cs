/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/23
 * Time: 19:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace SharpSnmpLib
{
	public class TrapReceivedEventArgs : EventArgs
	{
	    public TrapReceivedEventArgs(TrapMessage trap)
	    {
	        _trap = trap;
	    }
	
	    public TrapMessage Trap
	    {
	        get
	        {
	            return _trap;
	        }
	    }
	
	    TrapMessage _trap;
	}
}
