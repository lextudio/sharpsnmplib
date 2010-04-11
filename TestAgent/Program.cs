/*
 * Created by SharpDevelop.
 * User: lexli
 * Date: 2008-12-14
 * Time: 14:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// Class with program entry point.
    /// </summary>
    internal sealed class Program
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
            Container.LoadConfiguration("agent");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }   
		
		public static bool IsRunningOnMono()
  		{
    		return Type.GetType ("Mono.Runtime") != null;
  		}
    }
}
