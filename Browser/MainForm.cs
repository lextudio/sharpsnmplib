/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/6/28
 * Time: 12:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using WeifenLuo.WinFormsUI.Docking;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Browser
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	internal partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			if (!Helper.IsRunningOnMono())
			{
				Icon = Properties.Resources.internet_web_browser;
				actAbout.Image = Properties.Resources.help_browser;
			}
			
            DockContent agent = Program.Container.Resolve<DockContent>("AgentProfile");
            agent.Show(dockPanel1, DockState.DockLeft);

            DockContent notification = Program.Container.Resolve<DockContent>("Notification");
            notification.Show(dockPanel1, DockState.DockBottom);
            
		    DockContent output = Program.Container.Resolve<DockContent>("Output");
            output.Show(dockPanel1, DockState.DockBottom);

		    DockContent tree = Program.Container.Resolve<DockContent>("MibTree");
            tree.Show(dockPanel1, DockState.Document);

            DockContent modules = Program.Container.Resolve<DockContent>("ModuleList");
            modules.Show(dockPanel1, DockState.DockRight);
		}

        private void ActExitExecute(object sender, EventArgs e)
        {
            Close();
        }
		
		private void ActAboutExecute(object sender, EventArgs e)
		{
			Help.ShowHelp(this, "http://sharpsnmplib.codeplex.com");
		}

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text = string.Format(CultureInfo.CurrentUICulture, "{0} (Version: {1})", Text, Assembly.GetExecutingAssembly().GetName().Version);
        }
    }
}
