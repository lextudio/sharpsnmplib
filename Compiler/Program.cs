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
using Lextm.Common;
using Lextm.SharpSnmpLib.Compiler;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Lextm.SharpSnmpLib
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "args")]
        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var controller = new SingleInstanceController(typeof(MainForm));
            controller.MainFormCreated += delegate
            {
                Container = new UnityContainer().LoadConfiguration("compiler");
                ToolStripManager.Renderer = new Office2007Renderer.Office2007Renderer();
            };
            controller.Run(args);
        }
    }
}
