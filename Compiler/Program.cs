/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/6/28
 * Time: 12:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.Configuration;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Windows.Forms;
using Microsoft.Practices.Unity;

namespace Lextm.SharpSnmpLib.Compiler
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		private static IUnityContainer container;

		internal static IUnityContainer Container
		{
			get { return container; }
		}
		
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			container = new UnityContainer();
			UnityConfigurationSection section
				= (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
			section.Containers.Default.Configure(Container);

		    Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

            MainForm main = Container.Resolve<MainForm>();
            Application.Run(main);
		}
	}
}
