/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/6/28
 * Time: 12:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Lextm.SharpSnmpLib.Browser
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal static class Program
	{
	    internal static IUnityContainer Container { get; private set; }

	    /// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
            if (args.Length > 0)
            {
                return;
            }

            Container = new UnityContainer();
	        Container.LoadConfiguration("browser");

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
		    Application.Run(new MainForm());
		}
	}
}
