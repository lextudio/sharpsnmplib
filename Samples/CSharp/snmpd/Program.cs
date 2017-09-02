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
using Lextm.SharpSnmpLib.Objects;
using Lextm.SharpSnmpLib.Pipeline;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// Class with program entry point.
    /// </summary>
    internal static class Program
    {
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
            
            ToolStripManager.Renderer = new Office2007Renderer.Office2007Renderer();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }   
    }
}
