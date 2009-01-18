/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/6/28
 * Time: 12:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using System.Windows.Forms;

namespace Lextm.SharpSnmpLib.Browser
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal static class Program
	{
	    private static IMediator _mediator;
	    
	    internal static IMediator Mediator
	    {
	        get
	        {
	            return _mediator;
	        }
	    }
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{         
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			MainForm main = new MainForm();
			_mediator = main;
			Application.Run(main);
		}
		
	}
}
